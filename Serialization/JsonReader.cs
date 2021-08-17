using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace CER.Json.Stream
{
	/// <summary>
	/// A parser that reads JSON as a stream, maintaining the minimum of information as it goes. Therefore, it can be used to read very large documents while still only using a small amount of memory.
	/// </summary>
	public class JsonReader
	{
		/// <summary>
		/// Create a reader from the given text stream, which should be positioned before the JSON data.
		/// </summary>
		/// <param name="text">Text stream to read JSON from, which should be positioned before the JSON data.</param>
		/// <exception cref="ArgumentNullException">The given text reader is null.</exception>
		public JsonReader(TextReader text)
		{
			_text = text ?? throw new ArgumentNullException(nameof(text));
			_stack = new List<bool>();
			_state = State.StartValue;
			_line = 1;
			_lineCharacter = 0;
			CurrentToken = TokenType.Invalid;
		}

		readonly TextReader _text;
		readonly IList<bool> _stack;
		State _state;
		int? _buffer;
		bool _hasError;
		ulong _line;
		ulong _lineCharacter;
		TokenType _type;
		string _stringValue;
		bool _booleanValue;
		string _whitespace;

		/// <summary>
		/// The type of data that the reader is currently positioned at.
		/// </summary>
		/// <exception cref="InvalidOperationException">The reader previously encountered an error.</exception>
		public TokenType CurrentToken
		{
			get => !_hasError ? _type : throw new InvalidOperationException();
			private set => _type = value;
		}

		/// <summary>
		/// The string in JSON representation that the reader is currently positioned at. Only use if CurrentToken == TokenType.String.
		/// Can be converted to a native string with JsonString.JsonToString(). Note that the conversion is lossy.
		/// </summary>
		/// <exception cref="InvalidOperationException">The reader is not currently positioned at a string.</exception>
		public string StringValue
		{
			get
			{
				if (CurrentToken != TokenType.String)
				{
					throw new InvalidOperationException();
				}
				return _stringValue;
			}
			private set => _stringValue = value;
		}

		/// <summary>
		/// The number in JSON representation that the reader is currently positioned at. Only use if CurrentToken == TokenType.Number.
		/// Can be converted to a native decimal with JsonNumber.JsonToDecimal(). Note that the conversion is lossy.
		/// </summary>
		/// <exception cref="InvalidOperationException">The reader is not currently positioned at a number.</exception>
		public string NumberValue
		{
			get
			{
				if (CurrentToken != TokenType.Number)
				{
					throw new InvalidOperationException();
				}
				return _stringValue;
			}
			private set => _stringValue = value;
		}

		/// <summary>
		/// The boolean value that the reader is currently positioned at. Only use if CurrentToken == TokenType.Boolean.
		/// </summary>
		/// <exception cref="InvalidOperationException">The reader is not currently positioned at a boolean.</exception>
		public bool BooleanValue
		{
			get
			{
				if (CurrentToken != TokenType.Boolean)
				{
					throw new InvalidOperationException();
				}
				return _booleanValue;
			}
		}

		/// <summary>
		/// The whitespace value that the reader is currently positioned at. Only use if CurrentToken == TokenType.Whitespace.
		/// </summary>
		/// <exception cref="InvalidOperationException">The reader is not currently positioned at whitespace.</exception>
		public Whitespace Whitespace
		{
			get
			{
				if (CurrentToken != TokenType.Whitespace)
				{
					throw new InvalidOperationException();
				}
				return new Whitespace(_whitespace);
			}
		}

		/// <summary>
		/// Advance the stream to the next JSON token.
		/// </summary>
		/// <returns>Whether the next token was read. Otherwise, the end of the stream was reached.</returns>
		/// <exception cref="InvalidOperationException">reader is in an invalid state from a previous exception.</exception>
		/// <exception cref="InvalidJsonException">The underlying text stream is not valid JSON.</exception>
		/// <exception cref="ObjectDisposedException">The underlying stream has been closed.</exception>
		/// <exception cref="IOException">An I/O error occurs.</exception>
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
				_whitespace = null;

				char character;
				if (!TryPeek(out character))
				{
					if (_state != State.EndValue || _stack.Count > 0)
					{
						throw new InvalidJsonException(_line, _lineCharacter, Strings.UnexpectedEndOfFile);
					}
					CurrentToken = TokenType.Invalid;
					return false;
				}

				if (TryReadWhitespace(out _whitespace))
				{
					CurrentToken = TokenType.Whitespace;
					return true;
				}

				switch (character)
				{
					case '{':
						_ = Advance();
						if (_state != State.StartValue &&
							(_state != State.EnterContainer || _stack[_stack.Count - 1]))
						{
							throw new InvalidJsonException(_line, _lineCharacter, Strings.UnexpectedObjectStart);
						}
						_stack.Add(true);
						CurrentToken = TokenType.BeginObject;
						_state = State.EnterContainer;
						return true;
					case '}':
						_ = Advance();
						if (_state != State.EnterContainer &&
							_state != State.EndValue)
						{
							throw new InvalidJsonException(_line, _lineCharacter, Strings.UnexpectedObjectEnd);
						}
						if (_stack.Count < 1 || !_stack[_stack.Count - 1])
						{
							throw new InvalidJsonException(_line, _lineCharacter, Strings.NoObjectToEnd);
						}
						_stack.RemoveAt(_stack.Count - 1);
						CurrentToken = TokenType.EndObject;
						_state = State.EndValue;
						return true;
					case '[':
						_ = Advance();
						if (_state != State.StartValue &&
							(_state != State.EnterContainer || _stack[_stack.Count - 1]))
						{
							throw new InvalidJsonException(_line, _lineCharacter, Strings.UnexpectedArrayStart);
						}
						_stack.Add(false);
						CurrentToken = TokenType.BeginArray;
						_state = State.EnterContainer;
						return true;
					case ']':
						_ = Advance();
						if (_state != State.EnterContainer &&
							_state != State.EndValue)
						{
							throw new InvalidJsonException(_line, _lineCharacter, Strings.UnexpectedArrayEnd);
						}
						if (_stack.Count < 1 || _stack[_stack.Count - 1])
						{
							throw new InvalidJsonException(_line, _lineCharacter, Strings.NoArrayToEnd);
						}
						_stack.RemoveAt(_stack.Count - 1);
						CurrentToken = TokenType.EndArray;
						_state = State.EndValue;
						return true;
					case ',':
						_ = Advance();
						if (_state != State.EndValue || _stack.Count < 1)
						{
							throw new InvalidJsonException(_line, _lineCharacter, Strings.UnexpectedComma);
						}
						CurrentToken = TokenType.ListSeparator;
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
							throw new InvalidJsonException(_line, _lineCharacter, Strings.UnexpectedColon);
						}
						CurrentToken = TokenType.KeyValueSeparator;
						_state = State.StartValue;
						return true;
					case 'n':
						if (_state != State.StartValue &&
							(_state != State.EnterContainer || _stack[_stack.Count - 1]))
						{
							throw new InvalidJsonException(_line, _lineCharacter, string.Format(CultureInfo.CurrentCulture, Strings.UnexpectedCharacter, character));
						}
						ReadConstant("null");
						CurrentToken = TokenType.Null;
						_state = State.EndValue;
						return true;
					case 't':
						if (_state != State.StartValue &&
							(_state != State.EnterContainer || _stack[_stack.Count - 1]))
						{
							throw new InvalidJsonException(_line, _lineCharacter, string.Format(CultureInfo.CurrentCulture, Strings.UnexpectedCharacter, character));
						}
						ReadConstant("true");
						CurrentToken = TokenType.Boolean;
						_state = State.EndValue;
						_booleanValue = true;
						return true;
					case 'f':
						if (_state != State.StartValue &&
							(_state != State.EnterContainer || _stack[_stack.Count - 1]))
						{
							throw new InvalidJsonException(_line, _lineCharacter, string.Format(CultureInfo.CurrentCulture, Strings.UnexpectedCharacter, character));
						}
						ReadConstant("false");
						CurrentToken = TokenType.Boolean;
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
							case State.EnterContainer:
								if (_stack[_stack.Count - 1])
								{
									_state = State.EndKey;
								}
								else
								{
									_state = State.EndValue;
								}
								break;
							default:
								throw new InvalidJsonException(_line, _lineCharacter, string.Format(CultureInfo.CurrentCulture, Strings.UnexpectedCharacter, character));
						}
						CurrentToken = TokenType.String;
						StringValue = ReadString();
						return true;
					default:
						if (_state != State.StartValue)
						{
							throw new InvalidJsonException(_line, _lineCharacter, string.Format(CultureInfo.CurrentCulture, Strings.UnexpectedCharacter, character));
						}
						NumberValue = ReadNumber();
						CurrentToken = TokenType.Number;
						_state = State.EndValue;
						return true;
				}
			}
			catch (InvalidJsonException)
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
				if (result is null)
				{
					result = string.Empty;
				}
				result += character;
				_ = Advance();
			}
			return result != null;
		}

		string ReadString()
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
					throw new InvalidJsonException(_line, _lineCharacter, Strings.ControlCharacterInString);
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
									throw new InvalidJsonException(_line, _lineCharacter, Strings.UnexpectedEndOfFileInString);
								}
								if (!IsHexDigit(c))
								{
									throw new InvalidJsonException(_line, _lineCharacter, string.Format(CultureInfo.CurrentCulture, Strings.InvalidUnicodeHexadecimal, c));
								}
								_ = stringRepresentation.Append(c);
								_ = Advance();
							}
							break;
						default:
							throw new InvalidJsonException(_line, _lineCharacter, string.Format(CultureInfo.CurrentCulture, Strings.InvalidEscapeSequence, c));
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
							return stringRepresentation.ToString();
						default:
							_ = stringRepresentation.Append(c);
							break;
					}
				}
			}
			throw new InvalidJsonException(_line, _lineCharacter, Strings.UnexpectedEndOfFileInString);
		}

		string ReadNumber()
		{
			StringBuilder stringRepresentation = new StringBuilder();
			char c;

			if (!TryPeek(out c))
			{
				throw new InvalidJsonException(_line, _lineCharacter, Strings.UnexpectedEndOfFile);
			}
			if (c == '-')
			{
				_ = stringRepresentation.Append(c);
				_ = Advance();
			}

			if (!TryPeek(out c))
			{
				throw new InvalidJsonException(_line, _lineCharacter, Strings.UnexpectedEndOfFile);
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
				throw new InvalidJsonException(_line, _lineCharacter, string.Format(CultureInfo.CurrentCulture, Strings.UnexpectedNumberCharacter, c));
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
					throw new InvalidJsonException(_line, _lineCharacter, Strings.NumbersMustHaveAtLeastOneDigitAfterDecimalPoint);
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
					throw new InvalidJsonException(_line, _lineCharacter, Strings.NumbersMustHaveAtLeastOneDigitInExponent);
				}
			}

			return stringRepresentation.ToString();
		}

		void ReadConstant(string constant)
		{
			foreach (char expected in constant)
			{
				char actual = Advance();
				if (expected != actual)
				{
					throw new InvalidJsonException(_line, _lineCharacter, string.Format(CultureInfo.CurrentCulture, Strings.InvalidCharacterInConstant, actual, constant, expected));
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
					throw new EndOfStreamException();
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
			EnterContainer,
		}
	}
}
