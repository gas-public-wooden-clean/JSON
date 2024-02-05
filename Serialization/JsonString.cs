using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;

namespace CER.Json.DocumentObjectModel
{
	/// <summary>
	/// A string JSON value.
	/// </summary>
	[DebuggerDisplay("{_json}")]
	public class JsonString : JsonElement
	{
		/// <summary>
		/// Create an empty JSON string with no leading or trailing whitespace.
		/// </summary>
		public JsonString() : base()
		{
			_value = string.Empty;
			_json = string.Empty;
		}

		/// <summary>
		/// Create a JSON string with the given value.
		/// </summary>
		/// <param name="representation">A string representation of the JSON string.</param>
		/// <param name="isJson">Whether the representation of the JSON string is a JSON string or a native string.</param>
		/// <exception cref="ArgumentNullException">The given representation is null.</exception>
		/// <exception cref="FormatException">The representation is a JSON string that is not well-formed or is not a sequence of unicode codepoints.</exception>
		public JsonString(string representation, bool isJson = false)
		{
			if (isJson)
			{
				Json = representation;
			}
			else
			{
				Value = representation;
			}
		}

		string _value;
		string _json;

		/// <summary>
		/// The native string value.
		/// </summary>
		/// <exception cref="ArgumentNullException">The given representation is null.</exception>
		public string Value
		{
			get => _value;
			set
			{
				_value = value ?? throw new ArgumentNullException(nameof(value));
				StringBuilder json = new StringBuilder(value);
				_ = json.Replace("\\", "\\\\");
				_ = json.Replace("\"", "\\\"");
				_ = json.Replace("\b", "\\b");
				_ = json.Replace("\f", "\\f");
				_ = json.Replace("\r", "\\r");
				_ = json.Replace("\n", "\\n");
				_ = json.Replace("\t", "\\t");
				for (ushort controlValue = 0; controlValue <= 31; controlValue++)
				{
					_ = json.Replace(((char)controlValue).ToString(), EscapeChar((char)controlValue));
				}
				for (int i = 0; i < json.Length; i++)
				{
					bool nextIsLowSurrogate = i + 1 < json.Length && char.IsLowSurrogate(json[i + 1]);
					bool unpairedHighSurrogate = char.IsHighSurrogate(json[i]) && !nextIsLowSurrogate;
					if (unpairedHighSurrogate || char.IsLowSurrogate(json[i]))
					{
						// The value contains an unpaired surrogate, which can't be written to a
						// stream unless we escape it.
						string toInsert = EscapeChar(json[i]);
						_ = json.Remove(i, 1);
						_ = json.Insert(i, toInsert);
						i += toInsert.Length - 1;
					}
					else if (char.IsHighSurrogate(json[i]))
					{
						// This is a valid surrogate pair. Skip checking the next because we already
						// know it's the low surrogate.
						i += 1;
					}
				}
				_json = json.ToString();
			}
		}

		/// <summary>
		/// The value as a JSON string.
		/// </summary>
		/// <exception cref="ArgumentNullException">The given representation is null.</exception>
		/// <exception cref="FormatException">The representation is a JSON string that is not well-formed or is not a sequence of unicode codepoints.</exception>
		public string Json
		{
			get => _json;
			set
			{
				if (value is null)
				{
					throw new ArgumentNullException(nameof(value));
				}

				_value = JsonToString(value);
				_json = value;
			}
		}

		/// <summary>
		/// Convert a JSON string to a native string.
		/// </summary>
		/// <param name="json">The JSON representation of a string.</param>
		/// <returns>The value of the JSON string.</returns>
		/// <exception cref="ArgumentNullException">The given representation is null.</exception>
		/// <exception cref="FormatException">The representation is a JSON string that is not well-formed or is not a sequence of unicode codepoints.</exception>
		public static string JsonToString(string json)
		{
			if (json is null)
			{
				throw new ArgumentNullException(nameof(json));
			}

			StringBuilder builder = new StringBuilder();
			bool isEscaped = false;
			for (int i = 0; i < json.Length; i++)
			{
				if ((ushort)json[i] <= 31)
				{
					throw new FormatException(Strings.ValueContainsControlCharacter);
				}

				if (IsUnpairedSurrogate(json, i))
				{
					throw new FormatException(Strings.ValueContainsAnUnpairedSurrogate);
				}

				if (isEscaped)
				{
					isEscaped = false;
					switch (json[i])
					{
						case '"':
							_ = builder.Append('"');
							break;
						case '\\':
							_ = builder.Append('\\');
							break;
						case '/':
							_ = builder.Append('/');
							break;
						case 'b':
							_ = builder.Append('\b');
							break;
						case 'f':
							_ = builder.Append('\f');
							break;
						case 'n':
							_ = builder.Append('\n');
							break;
						case 'r':
							_ = builder.Append('\r');
							break;
						case 't':
							_ = builder.Append('\t');
							break;
						case 'u':
							const int length = 4;
							if (json.Length <= i + length)
							{
								throw new FormatException(Strings.ValueEndsWithIncompleteUnicode);
							}
							string hexadecimal = json.Substring(i + 1, length);
							ushort unicode;
							if (!ushort.TryParse(hexadecimal, NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out unicode))
							{
								throw new FormatException(string.Format(CultureInfo.CurrentCulture, Strings.ValueContainsInvalidHex, hexadecimal));
							}
							_ = builder.Append((char)unicode);
							i += length;
							break;
						default:
							throw new FormatException(string.Format(CultureInfo.CurrentCulture, Strings.ValueContainsInvalidEscape, json.Substring(i - 1, 2)));
					}
				}
				else
				{
					switch (json[i])
					{
						case '\\':
							isEscaped = true;
							break;
						case '"':
							throw new FormatException(Strings.ValueContainsUnescapedDoubleQuote);
						default:
							_ = builder.Append(json[i]);
							break;
					}
				}
			}

			if (isEscaped)
			{
				throw new FormatException(Strings.ValueEndsWithIncompleteEscapeSequence);
			}

			return builder.ToString();
		}

		/// <summary>
		/// Write the string value and whitespace as JSON to the stream.
		/// </summary>
		/// <param name="writer">The writer to write to.</param>
		/// <exception cref="ObjectDisposedException">The writer is closed.</exception>
		/// <exception cref="IOException">An I/O error occurs.</exception>
		public override void Serialize(TextWriter writer)
		{
			if (writer is null)
			{
				throw new ArgumentNullException(nameof(writer));
			}

			writer.Write(Leading.Value);
			writer.Write("\"");

			for (int i = 0; i < _json.Length; i++)
			{
				int sequenceLength;

				if (_json[i] == '\\')
				{
					if (_json[i + 1] == 'u')
					{
						// Unicode escape sequence.
						sequenceLength = 6;
					}
					else
					{
						sequenceLength = 2;
					}

					writer.Write(_json.Substring(i, sequenceLength));
				}
				else
				{
					sequenceLength = char.IsHighSurrogate(_json[i]) ? 2 : 1;

					bool needsEscape = false;
					// Check if the sequence is representable natively in the target encoding.
					try
					{
						_ = writer.Encoding.GetByteCount(_json.Substring(i, sequenceLength));
					}
					catch (EncoderFallbackException)
					{
						needsEscape = true;
					}

					if (needsEscape)
					{
						for (int sequenceIndex = 0; sequenceIndex < sequenceLength; sequenceIndex++)
						{
							writer.Write(EscapeChar(_json[i + sequenceIndex]));
						}
					}
					else
					{
						writer.Write(_json.Substring(i, sequenceLength));
					}
				}

				i += sequenceLength - 1;
			}

			writer.Write("\"");
			writer.Write(Trailing.Value);
		}

		static string EscapeChar(char character)
		{
			return string.Format(CultureInfo.InvariantCulture, "\\u{0:X4}", (ushort)character);
		}

		static bool IsUnpairedSurrogate(string value, int index)
		{
			if (char.IsHighSurrogate(value[index]))
			{
				return index == value.Length - 1 || !char.IsLowSurrogate(value[index + 1]);
			}
			else if (char.IsLowSurrogate(value[index]))
			{
				return index == 0 || !char.IsHighSurrogate(value[index - 1]);
			}
			else
			{
				return false;
			}
		}
	}
}
