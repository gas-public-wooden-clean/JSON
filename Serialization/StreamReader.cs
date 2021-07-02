using CER.JSON.DocumentObjectModel;
using System.Collections.Generic;
using System.Text;

using InvalidOperationException = System.InvalidOperationException;

namespace CER.JSON.Stream
{
	public class StreamReader
	{
		public StreamReader(System.IO.TextReader text)
		{
			_text = text;
			_stack = new List<bool>();
			_state = State.StartValue;
			_line = 1;
			_lineCharacter = 0;
			Type = Type.Invalid;
		}

		private System.IO.TextReader _text;
		private IList<bool> _stack;
		private State _state;
		private int? _buffer;
		private bool _hasError;
		private ulong _line;
		private ulong _lineCharacter;
		private Type _type;
		private String _stringValue;
		private Number _numberValue;
		private bool _booleanValue;
		private string _whitespaceValue;

		public Type Type
		{
			get
			{
				if (_hasError)
				{
					throw new InvalidOperationException();
				}
				return _type;
			}
			private set
			{
				_type = value;
			}
		}

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
			private set
			{
				_stringValue = value;
			}
		}

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
			private set
			{
				_numberValue = value;
			}
		}

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

		private static bool IsDecimalDigit(char c)
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

		private static bool IsHexDigit(char c)
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
						Advance();
						if (_state != State.StartValue)
						{
							throw new System.IO.InvalidDataException(string.Format("Line {0} character {1}: Unexpected object start.", _line, _lineCharacter));
						}
						_stack.Add(true);
						Type = Type.BeginObject;
						_state = State.StartKey;
						return true;
					case '}':
						Advance();
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
						Advance();
						if (_state != State.StartValue)
						{
							throw new System.IO.InvalidDataException(string.Format("Line {0} character {1}: Unexpected array start.", _line, _lineCharacter));
						}
						_stack.Add(false);
						Type = Type.BeginArray;
						_state = State.StartValue;
						return true;
					case ']':
						Advance();
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
						Advance();
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
						Advance();
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

		private bool TryReadWhitespace(out string result)
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
				Advance();
			}
			return result != null;
		}

		private String ReadString()
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

		private Number ReadNumber()
		{
			StringBuilder stringRepresentation = new StringBuilder();
			char c;

			if (!TryPeek(out c))
			{
				throw new System.IO.InvalidDataException(string.Format("Line {0} character {1}: Unexpected end of file.", _line, _lineCharacter));
			}
			if (c == '-')
			{
				stringRepresentation.Append(c);
				Advance();
			}

			if (!TryPeek(out c))
			{
				throw new System.IO.InvalidDataException(string.Format("Line {0} character {1}: Unexpected end of file.", _line, _lineCharacter));
			}
			if (c == '0')
			{
				stringRepresentation.Append(c);
				Advance();
			}
			else if (IsDecimalDigit(c))
			{
				while (TryPeek(out c) && IsDecimalDigit(c))
				{
					stringRepresentation.Append(c);
					Advance();
				}
			}
			else
			{
				throw new System.IO.InvalidDataException(string.Format("Line {0} character {1}: Unexpected {2}.", _line, _lineCharacter, c));
			}

			if (TryPeek(out c) && c == '.')
			{
				stringRepresentation.Append(c);
				Advance();

				bool hasDigitAfterDecimal = false;
				while (TryPeek(out c) && IsDecimalDigit(c))
				{
					stringRepresentation.Append(c);
					Advance();
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
				stringRepresentation.Append(c);
				Advance();

				if (TryPeek(out c) &&
					(c == '+' || c == '-'))
				{
					stringRepresentation.Append(c);
					Advance();
				}

				bool hasDigitInExponent = false;
				while (TryPeek(out c) && IsDecimalDigit(c))
				{
					stringRepresentation.Append(c);
					Advance();
				}
				if (!hasDigitInExponent)
				{
					throw new System.IO.InvalidDataException(string.Format("Line {0} character {1}: Numbers must have at least one digit in an exponent.", _line, _lineCharacter));
				}
			}

			return new Number(stringRepresentation.ToString());
		}

		private void ReadConstant(string constant)
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

		private bool TryPeek(out char character)
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

		private char Advance()
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

		private enum State
		{
			StartValue,
			EndValue,
			StartKey,
			EndKey,
		}
	}
}
