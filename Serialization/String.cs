using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;

namespace CER.JSON.DocumentObjectModel
{
	/// <summary>
	/// A string JSON value.
	/// </summary>
	[DebuggerDisplay("{_json}")]
	public class String : Element
	{
		/// <summary>
		/// Create an empty JSON string with no leading or trailing whitespace.
		/// </summary>
		public String() : base()
		{
			_value = string.Empty;
			_json = string.Empty;
		}

		/// <summary>
		/// Create a JSON string with the given value.
		/// </summary>
		/// <param name="representation">A string representation of the JSON string.</param>
		/// <param name="isJSON">Whether the representation of the JSON string is a JSON string or a native string.</param>
		/// <exception cref="System.ArgumentNullException">The given representation is null.</exception>
		/// <exception cref="System.FormatException">The representation is a JSON string that is not well-formed or is not a sequence of unicode codepoints.</exception>
		public String(string representation, bool isJSON)
		{
			if (isJSON)
			{
				JSON = representation;
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
		/// <exception cref="System.ArgumentNullException">The given representation is null.</exception>
		public string Value
		{
			get => _value;
			set
			{
				_value = value ?? throw new ArgumentNullException(nameof(value));
				StringBuilder json = new StringBuilder(value);
				_ = json.Replace("\\", "\\\\");
				_ = json.Replace("\"", "\\\"");
				_ = json.Replace("/", "\\/");
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
		/// <exception cref="System.ArgumentNullException">The given representation is null.</exception>
		/// <exception cref="System.FormatException">The representation is a JSON string that is not well-formed or is not a sequence of unicode codepoints.</exception>
		public string JSON
		{
			get => _json;
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException(nameof(value));
				}
				StringBuilder builder = new StringBuilder();
				bool isEscaped = false;
				for (int i = 0; i < value.Length; i++)
				{
					if ((ushort)value[i] <= 31)
					{
						throw new FormatException("Value contains a control character.");
					}

					if (IsUnpairedSurrogate(value, i))
					{
						throw new FormatException("Value contains an unpaired surrogate.");
					}

					if (isEscaped)
					{
						isEscaped = false;
						switch (value[i])
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
								if (value.Length <= i + length)
								{
									throw new FormatException("Value ends with incomplete unicode sequence.");
								}
								string hexadecimal = value.Substring(i + 1, length);
								ushort unicode;
								if (!ushort.TryParse(hexadecimal, NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out unicode))
								{
									throw new FormatException(string.Format("Value contains invalid hexadecimal number {0}.", hexadecimal));
								}
								_ = builder.Append((char)unicode);
								i += length;
								break;
							default:
								throw new FormatException(string.Format("Value contains invalid escape sequence {0}.", value.Substring(i - 1, 2)));
						}
					}
					else
					{
						switch (value[i])
						{
							case '\\':
								isEscaped = true;
								break;
							case '"':
								throw new FormatException("Value contains unescaped double quote.");
							default:
								_ = builder.Append(value[i]);
								break;
						}
					}
				}

				if (isEscaped)
				{
					throw new FormatException("Value ends with incomplete escape sequence.");
				}

				_value = builder.ToString();
				_json = value;
			}
		}

		/// <summary>
		/// Write the string value and whitespace as JSON to the stream.
		/// </summary>
		/// <param name="writer">The writer to write to.</param>
		/// <exception cref="System.ObjectDisposedException">The writer is closed.</exception>
		/// <exception cref="System.IO.IOException">An I/O error occurs.</exception>
		public override void Serialize(TextWriter writer)
		{
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
