using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
			_inBuffer = new char[32];
			_outBuffer = new StringBuilder();
			_stack = new List<bool>();
			_state = State.StartValue;
			_line = 1;
			_lineCharacter = 0;
			CurrentToken = TokenType.Invalid;
		}

		readonly TextReader _text;
		readonly List<bool> _stack;
		State _state;
		char[] _inBuffer;

		/// <summary>
		/// The start index of characters in the buffer.
		/// </summary>
		int _start;
		/// <summary>
		/// The number of characters in the buffer.
		/// </summary>
		int _size;
		/// <summary>
		/// Line number that the cursor is currently positioned at.
		/// </summary>
		ulong _line;
		/// <summary>
		/// Character within the line that the cursor is currently positioned at.
		/// </summary>
		ulong _lineCharacter;
		string _stringValue;
		readonly StringBuilder _outBuffer;

		/// <summary>
		/// The type of data that the reader is currently positioned at.
		/// </summary>
		public TokenType CurrentToken { get; private set; }

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
				return field;
			}
			private set;
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
				return field;
			}
			private set;
		}

		/// <summary>
		/// Advance the stream to the next JSON token.
		/// </summary>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is System.Threading.CancellationToken.None.</param>
		/// <returns>Whether the next token was read. Otherwise, the end of the stream was reached.</returns>
		/// <exception cref="InvalidJsonException">The underlying text stream is not valid JSON.</exception>
		/// <exception cref="ObjectDisposedException">The underlying stream has been closed.</exception>
		/// <exception cref="OperationCanceledException">The cancellation token was canceled.</exception>
		public async Task<bool> ReadAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			StringValue = null;
			NumberValue = null;
			BooleanValue = false;
			Whitespace = null;

			if (!await FillBuffer(0, cancellationToken))
			{
				if (_state != State.EndValue || _stack.Count > 0)
				{
					throw new InvalidJsonException(_line, _lineCharacter, Strings.UnexpectedEndOfFile);
				}
				CurrentToken = TokenType.Invalid;
				return false;
			}

			if (await TryReadWhitespace(cancellationToken))
			{
				return true;
			}

			switch (Peek(0))
			{
				case '{':
					if (_state != State.StartValue &&
						(_state != State.EnterContainer || _stack[^1]))
					{
						throw new InvalidJsonException(_line, _lineCharacter, Strings.UnexpectedObjectStart);
					}
					_stack.Add(true);
					CurrentToken = TokenType.BeginObject;
					_state = State.EnterContainer;
					Advance(1, true);
					return true;
				case '}':
					if (_state != State.EnterContainer &&
						_state != State.EndValue)
					{
						throw new InvalidJsonException(_line, _lineCharacter, Strings.UnexpectedObjectEnd);
					}
					if (_stack.Count < 1 || !_stack[^1])
					{
						throw new InvalidJsonException(_line, _lineCharacter, Strings.NoObjectToEnd);
					}
					_stack.RemoveAt(_stack.Count - 1);
					CurrentToken = TokenType.EndObject;
					_state = State.EndValue;
					Advance(1, true);
					return true;
				case '[':
					if (_state != State.StartValue &&
						(_state != State.EnterContainer || _stack[^1]))
					{
						throw new InvalidJsonException(_line, _lineCharacter, Strings.UnexpectedArrayStart);
					}
					_stack.Add(false);
					CurrentToken = TokenType.BeginArray;
					_state = State.EnterContainer;
					Advance(1, true);
					return true;
				case ']':
					if (_state != State.EnterContainer &&
						_state != State.EndValue)
					{
						throw new InvalidJsonException(_line, _lineCharacter, Strings.UnexpectedArrayEnd);
					}
					if (_stack.Count < 1 || _stack[^1])
					{
						throw new InvalidJsonException(_line, _lineCharacter, Strings.NoArrayToEnd);
					}
					_stack.RemoveAt(_stack.Count - 1);
					CurrentToken = TokenType.EndArray;
					_state = State.EndValue;
					Advance(1, true);
					return true;
				case ',':
					if (_state != State.EndValue || _stack.Count < 1)
					{
						throw new InvalidJsonException(_line, _lineCharacter, Strings.UnexpectedComma);
					}
					CurrentToken = TokenType.ListSeparator;
					if (_stack[^1])
					{
						_state = State.StartKey;
					}
					else
					{
						_state = State.StartValue;
					}
					Advance(1, true);
					return true;
				case ':':
					if (_state != State.EndKey || _stack.Count < 1 || !_stack[^1])
					{
						throw new InvalidJsonException(_line, _lineCharacter, Strings.UnexpectedColon);
					}
					CurrentToken = TokenType.KeyValueSeparator;
					_state = State.StartValue;
					Advance(1, true);
					return true;
				case 'n':
					if (_state != State.StartValue &&
						(_state != State.EnterContainer || _stack[^1]))
					{
						throw new InvalidJsonException(_line, _lineCharacter, string.Format(CultureInfo.CurrentCulture, Strings.UnexpectedCharacter, Peek(0)));
					}
					await ReadConstant("null", cancellationToken);
					CurrentToken = TokenType.Null;
					return true;
				case 't':
					if (_state != State.StartValue &&
						(_state != State.EnterContainer || _stack[^1]))
					{
						throw new InvalidJsonException(_line, _lineCharacter, string.Format(CultureInfo.CurrentCulture, Strings.UnexpectedCharacter, Peek(0)));
					}
					await ReadConstant("true", cancellationToken);
					CurrentToken = TokenType.Boolean;
					BooleanValue = true;
					return true;
				case 'f':
					if (_state != State.StartValue &&
						(_state != State.EnterContainer || _stack[^1]))
					{
						throw new InvalidJsonException(_line, _lineCharacter, string.Format(CultureInfo.CurrentCulture, Strings.UnexpectedCharacter, Peek(0)));
					}
					await ReadConstant("false", cancellationToken);
					CurrentToken = TokenType.Boolean;
					BooleanValue = false;
					return true;
				case '"':
					await ReadString(cancellationToken);
					return true;
				default:
					if (_state != State.StartValue &&
						(_state != State.EnterContainer || _stack[^1]))
					{
						throw new InvalidJsonException(_line, _lineCharacter, string.Format(CultureInfo.CurrentCulture, Strings.UnexpectedCharacter, Peek(0)));
					}
					await ReadNumber(cancellationToken);
					return true;
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

		async Task<bool> TryReadWhitespace(CancellationToken cancellationToken)
		{
			_outBuffer.Clear();
			for (int i = 0; await FillBuffer(i, cancellationToken) && Whitespace.IsLegal(Peek(i)); i++)
			{
				if (Peek(i) == '\n')
				{
					_line += 1;
					_lineCharacter = 0;
				}
				else
				{
					_lineCharacter += 1;
				}
				_ = _outBuffer.Append(Peek(i));
			}
			if (_outBuffer.Length > 0)
			{
				Whitespace = new Whitespace(_outBuffer.ToString());
				CurrentToken = TokenType.Whitespace;
				Advance(_outBuffer.Length, false);
				return true;
			}
			else
			{
				return false;
			}
		}

		async Task ReadString(CancellationToken cancellationToken)
		{
			State nextState;
			switch (_state)
			{
				case State.StartValue:
					nextState = State.EndValue;
					break;
				case State.StartKey:
					nextState = State.EndKey;
					break;
				case State.EnterContainer:
					if (_stack[^1])
					{
						nextState = State.EndKey;
					}
					else
					{
						nextState = State.EndValue;
					}
					break;
				default:
					throw new InvalidJsonException(_line, _lineCharacter, string.Format(CultureInfo.CurrentCulture, Strings.UnexpectedCharacter, '"'));
			}

			_outBuffer.Clear();
			int i;
			bool escaped = false;
			for (i = 1; await FillBuffer(i, cancellationToken); i++)
			{
				if ((ushort)Peek(i) < 32)
				{
					throw new InvalidJsonException(_line, _lineCharacter, Strings.ControlCharacterInString);
				}

				if (escaped)
				{
					_ = _outBuffer.Append(Peek(i));
					escaped = false;
					switch (Peek(i))
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
							for (int unicodeIndex = 1; unicodeIndex <= 4; unicodeIndex++)
							{
								if (!await FillBuffer(i + unicodeIndex, cancellationToken))
								{
									throw new InvalidJsonException(_line, _lineCharacter + (ulong)(i + unicodeIndex), Strings.UnexpectedEndOfFileInString);
								}
								if (!IsHexDigit(Peek(i + unicodeIndex)))
								{
									throw new InvalidJsonException(_line, _lineCharacter + (ulong)(i + unicodeIndex), string.Format(CultureInfo.CurrentCulture, Strings.InvalidUnicodeHexadecimal, Peek(i + unicodeIndex)));
								}
								_ = _outBuffer.Append(Peek(i + unicodeIndex));
							}
							i += 4;
							break;
						default:
							throw new InvalidJsonException(_line, _lineCharacter + (ulong)i, string.Format(CultureInfo.CurrentCulture, Strings.InvalidEscapeSequence, Peek(i)));
					}
				}
				else
				{
					switch (Peek(i))
					{
						case '\\':
							escaped = true;
							_ = _outBuffer.Append(Peek(i));
							break;
						case '"':
							StringValue = _outBuffer.ToString();
							CurrentToken = TokenType.String;
							_state = nextState;
							Advance(i + 1, true);
							return;
						default:
							_ = _outBuffer.Append(Peek(i));
							break;
					}
				}
			}
			throw new InvalidJsonException(_line, _lineCharacter + (ulong)i, Strings.UnexpectedEndOfFileInString);
		}

		async Task ReadNumber(CancellationToken cancellationToken)
		{
			_outBuffer.Clear();
			int i = 0;

			if (Peek(i) == '-')
			{
				_ = _outBuffer.Append(Peek(i++));
			}

			if (!await FillBuffer(i, cancellationToken))
			{
				throw new InvalidJsonException(_line, _lineCharacter + (ulong)i, Strings.UnexpectedEndOfFile);
			}
			if (Peek(i) == '0')
			{
				_ = _outBuffer.Append(Peek(i++));
			}
			else if (IsDecimalDigit(Peek(i)))
			{
				do
				{
					_ = _outBuffer.Append(Peek(i++));
				} while (await FillBuffer(i, cancellationToken) && IsDecimalDigit(Peek(i)));
			}
			else
			{
				throw new InvalidJsonException(_line, _lineCharacter + (ulong)i, string.Format(CultureInfo.CurrentCulture, Strings.UnexpectedNumberCharacter, Peek(i)));
			}

			if (await FillBuffer(i, cancellationToken) && Peek(i) == '.')
			{
				_ = _outBuffer.Append(Peek(i++));

				bool hasDigitAfterDecimal = false;
				while (await FillBuffer(i, cancellationToken) && IsDecimalDigit(Peek(i)))
				{
					_ = _outBuffer.Append(Peek(i++));
					hasDigitAfterDecimal = true;
				}
				if (!hasDigitAfterDecimal)
				{
					throw new InvalidJsonException(_line, _lineCharacter + (ulong)i, Strings.NumbersMustHaveAtLeastOneDigitAfterDecimalPoint);
				}
			}

			if (await FillBuffer(i, cancellationToken) &&
				(Peek(i) == 'e' || Peek(i) == 'E'))
			{
				_ = _outBuffer.Append(Peek(i++));

				if (await FillBuffer(i, cancellationToken) &&
					(Peek(i) == '+' || Peek(i) == '-'))
				{
					_ = _outBuffer.Append(Peek(i++));
				}

				bool hasDigitInExponent = false;
				while (await FillBuffer(i, cancellationToken) && IsDecimalDigit(Peek(i)))
				{
					_ = _outBuffer.Append(Peek(i++));
					hasDigitInExponent = true;
				}
				if (!hasDigitInExponent)
				{
					throw new InvalidJsonException(_line, _lineCharacter + (ulong)i, Strings.NumbersMustHaveAtLeastOneDigitInExponent);
				}
			}

			NumberValue = _outBuffer.ToString();
			CurrentToken = TokenType.Number;
			_state = State.EndValue;
			Advance(_outBuffer.Length, true);
		}

		async Task ReadConstant(string constant, CancellationToken cancellationToken)
		{
			if (!await FillBuffer(constant.Length - 1, cancellationToken))
			{
				throw new InvalidJsonException(_line, _lineCharacter + (ulong)_size, Strings.UnexpectedEndOfFileInConstant);
			}

			for (int i = 0; i < constant.Length; i++)
			{
				if (constant[i] != Peek(i))
				{
					throw new InvalidJsonException(_line, _lineCharacter + (ulong)i, string.Format(CultureInfo.CurrentCulture, Strings.InvalidCharacterInConstant, Peek(i), constant, constant[i]));
				}
			}

			_state = State.EndValue;

			Advance(constant.Length, true);
		}

		async Task<bool> FillBuffer(int index, CancellationToken cancellationToken)
		{
			if (_start + index >= _inBuffer.Length)
			{
				Array.Resize(ref _inBuffer, Math.Max(_start + index + 1, 2 * _inBuffer.Length));
			}

			while (index >= _size)
			{
				int numRead = await _text.ReadAsync(_inBuffer.AsMemory((_start + _size)..^0), cancellationToken);
				if (0 == numRead)
				{
					return false;
				}
				_size += numRead;
			}

			return true;
		}

		char Peek(int index)
		{
			ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(index, _size);

			return _inBuffer[_start + index];
		}

		void Advance(int count, bool updatePosition)
		{
			ArgumentOutOfRangeException.ThrowIfGreaterThan(count, _size, nameof(count));

			_start += count;
			_size -= count;
			if (_start > _size)
			{
				Array.Copy(_inBuffer, _start, _inBuffer, 0, _size);
				_start = 0;
			}
			if (updatePosition)
			{
				_lineCharacter += (ulong)count;
			}
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
