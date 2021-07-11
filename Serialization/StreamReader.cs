using CER.JSON.DocumentObjectModel;
using System.Collections.Generic;
using System.Text;

using InvalidOperationException = System.InvalidOperationException;

namespace CER.JSON.Stream
{
	/// <summary>
	/// A parser that reads JSON as a stream, maintaining the minimum of information as it goes. Therefore, it can be used to read very large documents while still only using a small amount of memory.
	/// </summary>
	public class StreamReader
	{
		/// <summary>
		/// Create a reader from the given text stream, which should be positioned before the JSON data.
		/// </summary>
		/// <param name="text"></param>
		/// <exception cref="System.ArgumentNullException">The given text reader is null.</exception>
		public StreamReader(System.IO.TextReader text)
		{
			_text = text ?? throw new System.ArgumentNullException(nameof(text));
			_stack = new List<bool>();
			_state = State.StartValue;
			_line = 1;
			_lineCharacter = 0;
			Type = Type.Invalid;
		}

		readonly System.IO.TextReader _text;
		readonly IList<bool> _stack;
		State _state;
		int? _buffer;
		bool _hasError;
		ulong _line;
		ulong _lineCharacter;
		Type _type;
		String _stringValue;
		Number _numberValue;
		bool _booleanValue;
		string _whitespaceValue;

		/// <summary>
		/// The type of data that the reader is currently positioned at.
		/// </summary>
		/// <exception cref="System.InvalidOperationException">The reader previously encountered an error.</exception>
		public Type Type
		{
			get => !_hasError ? _type : throw new InvalidOperationException();
			private set => _type = value;
		}

		/// <summary>
		/// The string value that the reader is currently positioned at. Only use if Type == Type.String.
		/// </summary>
		/// <exception cref="System.InvalidOperationException">The reader is not currently positioned at a string.</exception>
		public String StringValue
		{
			get
			{
				if (Type != Type.String)
				{
					throw new InvalidOperationException();
				}
				return new String(_stringValue.JSON, true);
			}
			private set => _stringValue = value;
		}

		/// <summary>
		/// The number value that the reader is currently positioned at. Only use if Type == Type.Number.
		/// </summary>
		/// <exception cref="System.InvalidOperationException">The reader is not currently positioned at a number.</exception>
		public Number NumberValue
		{
			get
			{
				if (Type != Type.Number)
				{
					throw new InvalidOperationException();
				}
				return new Number(_numberValue.JSON);
			}
			private set => _numberValue = value;
		}

		/// <summary>
		/// The boolean value that the reader is currently positioned at. Only use if Type == Type.Boolean.
		/// </summary>
		/// <exception cref="System.InvalidOperationException">The reader is not currently positioned at a boolean.</exception>
		public bool BooleanValue
		{
			get
			{
				if (Type != Type.Boolean)
				{
					throw new InvalidOperationException();
				}
				return _booleanValue;
			}
		}

		/// <summary>
		/// The whitespace value that the reader is currently positioned at. Only use if Type == Type.Whitespace.
		/// </summary>
		/// <exception cref="System.InvalidOperationException">The reader is not currently positioned at whitespace.</exception>
		public Whitespace WhitespaceValue
		{
			get
			{
				if (Type != Type.Whitespace)
				{
					throw new InvalidOperationException();
				}
				return new Whitespace(_whitespaceValue);
			}
		}

		/// <exception cref="System.InvalidOperationException">reader is in an invalid state from a previous exception.</exception>
		/// <exception cref="CER.JSON.Stream.InvalidTextException">The underlying text stream is not valid JSON.</exception>
		/// <exception cref="System.ObjectDisposedException">The underlying stream has been closed.</exception>
		/// <exception cref="System.IO.IOException">An I/O error occurs.</exception>
		public bool Read()
		{
			if (_hasError)
			{
				throw new InvalidOperationException();
			}

			try
			{
				StringValue = null;
				NumberValue = null;
				_booleanValue = false;
				_whitespaceValue = null;

				char character;
				if (!TryPeek(out character))
				{
					if (_state != State.EndValue || _stack.Count > 0)
					{
						throw new System.IO.InvalidDataException(string.Format("Line {0} character {1}: Unexpected end of file.", _line, _lineCharacter));
					}
					Type = Type.Invalid;
					return false;
				}

				if (TryReadWhitespace(out _whitespaceValue))
				{
					Type = Type.Whitespace;
					return true;
				}

				switch (character)
				{
					case '{':
						_ = Advance();
						if (_state != State.StartValue)
						{
							throw new System.IO.InvalidDataException(string.Format("Line {0} character {1}: Unexpected object start.", _line, _lineCharacter));
						}
						_stack.Add(true);
						Type = Type.BeginObject;
						_state = State.StartKey;
						return true;
					case '}':
						_ = Advance();
						if (_state != State.StartKey &&
							_state != State.EndValue)
						{
							throw new System.IO.InvalidDataException(string.Format("Line {0} character {1}: Unexpected object end.", _line, _lineCharacter));
						}
						if (_stack.Count < 1 || !_stack[_stack.Count - 1])
						{
							throw new System.IO.InvalidDataException(string.Format("Line {0} character {1}: No object to end.", _line, _lineCharacter));
						}
						_stack.RemoveAt(_stack.Count - 1);
						Type = Type.EndObject;
						_state = State.EndValue;
						return true;
					case '[':
						_ = Advance();
						if (_state != State.StartValue)
						{
							throw new System.IO.InvalidDataException(string.Format("Line {0} character {1}: Unexpected array start.", _line, _lineCharacter));
						}
						_stack.Add(false);
						Type = Type.BeginArray;
						_state = State.StartValue;
						return true;
					case ']':
						_ = Advance();
						if (_state != State.StartValue &&
							_state != State.EndValue)
						{
							throw new System.IO.InvalidDataException(string.Format("Line {0} character {1}: Unexpected array end.", _line, _lineCharacter));
						}
						if (_stack.Count < 1 || _stack[_stack.Count - 1])
						{
							throw new System.IO.InvalidDataException(string.Format("Line {0} character {1}: No array to end.", _line, _lineCharacter));
						}
						_stack.RemoveAt(_stack.Count - 1);
						Type = Type.EndArray;
						_state = State.EndValue;
						return true;
					case ',':
						_ = Advance();
						if (_state != State.EndValue || _stack.Count < 1)
						{
							throw new System.IO.InvalidDataException(string.Format("Line {0} character {1}: Unexpected comma.", _line, _lineCharacter));
						}
						Type = Type.ListSeparator;
						if (_stack[_stack.Count - 1])
						{
							_state = State.StartKey;
						}
						else
						{
							_state = State.StartValue;
						}
						return true;
					case ':':
						_ = Advance();
						if (_state != State.EndKey || _stack.Count < 1 || !_stack[_stack.Count - 1])
						{
							throw new System.IO.InvalidDataException(string.Format("Line {0} character {1}: Unexpected colon.", _line, _lineCharacter));
						}
						Type = Type.KeyValueSeparator;
						_state = State.StartValue;
						return true;
					case 'n':
						if (_state != State.StartValue)
						{
							throw new System.IO.InvalidDataException(string.Format("Line {0} character {1}: Unexpected character {2}.", _line, _lineCharacter, character));
						}
						ReadConstant("null");
						Type = Type.Null;
						_state = State.EndValue;
						return true;
					case 't':
						if (_state != State.StartValue)
						{
							throw new System.IO.InvalidDataException(string.Format("Line {0} character {1}: Unexpected character {2}.", _line, _lineCharacter, character));
						}
						ReadConstant("true");
						Type = Type.Boolean;
						_state = State.EndValue;
						_booleanValue = true;
						return true;
					case 'f':
						if (_state != State.StartValue)
						{
							throw new System.IO.InvalidDataException(string.Format("Line {0} character {1}: Unexpected character {2}.", _line, _lineCharacter, character));
						}
						ReadConstant("false");
						Type = Type.Boolean;
						_state = State.EndValue;
						_booleanValue = false;
						return true;
					case '"':
						switch (_state)
						{
							case State.StartValue:
								_state = State.EndValue;
								break;
							case State.StartKey:
								_state = State.EndKey;
								break;
							default:
								throw new System.IO.InvalidDataException(string.Format("Line {0} character {1}: Unexpected character {2}.", _line, _lineCharacter, character));
						}
						Type = Type.String;
						StringValue = ReadString();
						return true;
					default:
						if (_state != State.StartValue)
						{
							throw new System.IO.InvalidDataException(string.Format("Line {0} character {1}: Unexpected character {2}.", _line, _lineCharacter, character));
						}
						NumberValue = ReadNumber();
						Type = Type.Number;
						_state = State.EndValue;
						return true;
				}
			}
			catch (System.IO.InvalidDataException)
			{
				_hasError = true;
				throw;
			}
		}

		static bool IsDecimalDigit(char c)
		{
			switch (c)
			{
				case '0':
				case '1':
				case '2':
				case '3':
				case '4':
				case '5':
				case '6':
				case '7':
				case '8':
				case '9':
					return true;
				default:
					return false;
			}
		}

		static bool IsHexDigit(char c)
		{
			if (IsDecimalDigit(c))
			{
				return true;
			}
			switch (c)
			{
				case 'A':
				case 'B':
				case 'C':
				case 'D':
				case 'E':
				case 'F':
				case 'a':
				case 'b':
				case 'c':
				case 'd':
				case 'e':
				case 'f':
					return true;
				default:
					return false;
			}
		}

		bool TryReadWhitespace(out string result)
		{
			result = null;
			char character;
			while (TryPeek(out character) && Whitespace.IsLegal(character))
			{
				if (result == null)
				{
					result = string.Empty;
				}
				result += character;
				_ = Advance();
			}
			return result != null;
		}

		String ReadString()
		{
			_ = Advance();
			StringBuilder stringRepresentation = new StringBuilder();
			char c;
			bool escaped = false;
			while (TryPeek(out c))
			{
				_ = Advance();

				if ((ushort)c < 32)
				{
					throw new InvalidTextException(_line, _lineCharacter, "Control character in string.");
				}

				if (escaped)
				{
					_ = stringRepresentation.Append(c);
					escaped = false;
					switch (c)
					{
						case '"':
						case '\\':
						case '/':
						case 'b':
						case 'f':
						case 'n':
						case 'r':
						case 't':
							break;
						case 'u':
							for (int i = 0; i < 4; i++)
							{
								if (!TryPeek(out c))
								{
									throw new System.IO.InvalidDataException(string.Format("Line {0} character {1}: Unexpected end of file in string.", _line, _lineCharacter));
								}
								if (!IsHexDigit(c))
								{
									throw new System.IO.InvalidDataException(string.Format("Line {0} character {1}: Invalid unicode hexadecimal {2}.", _line, _lineCharacter, c));
								}
								_ = stringRepresentation.Append(c);
								_ = Advance();
							}
							break;
						default:
							throw new System.IO.InvalidDataException(string.Format("Line {0} character {1}: Invalid escape sequence {2}.", _line, _lineCharacter, c));
					}
				}
				else
				{
					switch (c)
					{
						case '\\':
							escaped = true;
							_ = stringRepresentation.Append(c);
							break;
						case '"':
							return new String(stringRepresentation.ToString(), true);
						default:
							_ = stringRepresentation.Append(c);
							break;
					}
				}
			}
			throw new System.IO.InvalidDataException(string.Format("Line {0} character {1}: Unexpected end of file in string.", _line, _lineCharacter));
		}

		Number ReadNumber()
		{
			StringBuilder stringRepresentation = new StringBuilder();
			char c;

			if (!TryPeek(out c))
			{
				throw new System.IO.InvalidDataException(string.Format("Line {0} character {1}: Unexpected end of file.", _line, _lineCharacter));
			}
			if (c == '-')
			{
				_ = stringRepresentation.Append(c);
				_ = Advance();
			}

			if (!TryPeek(out c))
			{
				throw new System.IO.InvalidDataException(string.Format("Line {0} character {1}: Unexpected end of file.", _line, _lineCharacter));
			}
			if (c == '0')
			{
				_ = stringRepresentation.Append(c);
				_ = Advance();
			}
			else if (IsDecimalDigit(c))
			{
				while (TryPeek(out c) && IsDecimalDigit(c))
				{
					_ = stringRepresentation.Append(c);
					_ = Advance();
				}
			}
			else
			{
				throw new System.IO.InvalidDataException(string.Format("Line {0} character {1}: Unexpected {2}.", _line, _lineCharacter, c));
			}

			if (TryPeek(out c) && c == '.')
			{
				_ = stringRepresentation.Append(c);
				_ = Advance();

				bool hasDigitAfterDecimal = false;
				while (TryPeek(out c) && IsDecimalDigit(c))
				{
					_ = stringRepresentation.Append(c);
					_ = Advance();
					hasDigitAfterDecimal = true;
				}
				if (!hasDigitAfterDecimal)
				{
					throw new System.IO.InvalidDataException(string.Format("Line {0} character {1}: Numbers must have at least one digit after the decimal point.", _line, _lineCharacter));
				}
			}

			if (TryPeek(out c) &&
				(c == 'e' || c == 'E'))
			{
				_ = stringRepresentation.Append(c);
				_ = Advance();

				if (TryPeek(out c) &&
					(c == '+' || c == '-'))
				{
					_ = stringRepresentation.Append(c);
					_ = Advance();
				}

				bool hasDigitInExponent = false;
				while (TryPeek(out c) && IsDecimalDigit(c))
				{
					_ = stringRepresentation.Append(c);
					_ = Advance();
				}
				if (!hasDigitInExponent)
				{
					throw new System.IO.InvalidDataException(string.Format("Line {0} character {1}: Numbers must have at least one digit in an exponent.", _line, _lineCharacter));
				}
			}

			return new Number(stringRepresentation.ToString());
		}

		void ReadConstant(string constant)
		{
			foreach (char expected in constant)
			{
				char actual = Advance();
				if (expected != actual)
				{
					throw new System.IO.InvalidDataException(string.Format("Line {0} character {1}: Invalid character {2} in constant {3}: expected {4}.", _line, _lineCharacter, actual, constant, expected));
				}
			}
		}

		bool TryPeek(out char character)
		{
			if (!_buffer.HasValue)
			{
				_buffer = _text.Read();
			}

			if (_buffer.Value < 0)
			{
				character = '\0';
				return false;
			}
			else
			{
				character = (char)_buffer.Value;
				return true;
			}
		}

		char Advance()
		{
			int value;
			if (_buffer.HasValue)
			{
				value = _buffer.Value;
				_buffer = null;
			}
			else
			{
				value = _text.Read();
				if (value < 0)
				{
					throw new System.IO.EndOfStreamException();
				}
			}
			if ((char)value == '\n')
			{
				_line += 1;
				_lineCharacter = 0;
			}
			else
			{
				_lineCharacter += 1;
			}
			return (char)value;
		}

		enum State
		{
			StartValue,
			EndValue,
			StartKey,
			EndKey,
		}
	}
}
