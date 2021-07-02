using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;

namespace CER.JSON.DocumentObjectModel
{
	[DebuggerDisplay("{_json}")]
	public class String : Element
	{
		public String()
		{
			_value = string.Empty;
			_json = string.Empty;
		}

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

		private string _value;
		private string _json;

		public string Value
		{
			get { return _value; }
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
						json.Remove(i, 1);
						json.Insert(i, toInsert);
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

		public string JSON
		{
			get { return _json; }
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

					if (isEscaped)
					{
						isEscaped = false;
						switch (value[i])
						{
							case '"':
								builder.Append('"');
								break;
							case '\\':
								builder.Append('\\');
								break;
							case 'b':
								builder.Append('\b');
								break;
							case 'f':
								builder.Append('\f');
								break;
							case 'n':
								builder.Append('\n');
								break;
							case 'r':
								builder.Append('\r');
								break;
							case 't':
								builder.Append('\t');
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
								builder.Append((char)unicode);
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
								builder.Append(value[i]);
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

		public override void Serialize(TextWriter writer)
		{
			writer.Write(Leading.Value);
			writer.Write("\"");

			for (int i = 0; i < _json.Length; i++)
			{
				if (_json[i] == '\\')
				{
					int sequenceLength;
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
					i += sequenceLength - 1;
				}
				else
				{
					int sequenceLength = char.IsHighSurrogate(_json[i]) ? 2 : 1;

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
			}

			writer.Write("\"");
			writer.Write(Trailing.Value);
		}

		private static string EscapeChar(char character)
		{
			return string.Format(CultureInfo.InvariantCulture, "\\u{0:X4}", (ushort)character);
		}
	}
}
