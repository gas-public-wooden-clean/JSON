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
					_ = json.Replace(((char)controlValue).ToString(), string.Format(CultureInfo.InvariantCulture, "\\u{0:X4}", controlValue));
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

		public override void Serialize(TextWriter writer)
		{
			writer.Write("{0}\"{1}\"{2}", Leading.Value, _json, Trailing.Value);
		}
	}
}
