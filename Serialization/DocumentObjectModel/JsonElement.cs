using CER.Json.Stream;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CER.Json.DocumentObjectModel
{
	/// <summary>
	/// A JSON element.
	/// </summary>
	public abstract class JsonElement
	{
		/// <summary>
		/// Create an element with no leading or trailing whitespace.
		/// </summary>
		protected JsonElement()
		{
			Leading = Whitespace.Empty;
			Trailing = Whitespace.Empty;
		}

		Whitespace _leading;
		Whitespace _trailing;

		/// <summary>
		/// Leading whitespace.
		/// </summary>
		/// <exception cref="ArgumentNullException">The given value is null.</exception>
		public Whitespace Leading
		{
			get => _leading;
			set => _leading = value ?? throw new ArgumentNullException(nameof(value));
		}

		/// <summary>
		/// Trailing whitespace.
		/// </summary>
		/// <exception cref="ArgumentNullException">The given value is null.</exception>
		public Whitespace Trailing
		{
			get => _trailing;
			set => _trailing = value ?? throw new ArgumentNullException(nameof(value));
		}

		/// <summary>
		/// Read the JSON element from a stream.
		/// </summary>
		/// <param name="reader">The stream to read from, which should be positioned at the start.</param>
		/// <returns>The JSON element contained within the document.</returns>
		/// <exception cref="ArgumentNullException">The reader is null.</exception>
		/// <exception cref="InvalidOperationException">The reader is in an invalid state from a previous exception.</exception>
		/// <exception cref="InvalidJsonException">The underlying text stream is not valid JSON.</exception>
		/// <exception cref="ObjectDisposedException">The underlying stream has been closed.</exception>
		/// <exception cref="IOException">An I/O error occurs.</exception>
		public static JsonElement Deserialize(JsonReader reader)
		{
			if (reader is null)
			{
				throw new ArgumentNullException(nameof(reader));
			}

			_ = reader.Read();
			return ReadElement(reader);
		}

		/// <summary>
		/// Write the JSON element to a stream.
		/// </summary>
		/// <param name="writer">The stream to write to.</param>
		/// <exception cref="ObjectDisposedException">The writer is closed.</exception>
		/// <exception cref="IOException">An I/O error occurs.</exception>
		public abstract void Serialize(TextWriter writer);

		/// <summary>
		/// Returns a string that represents the current object.
		/// </summary>
		/// <returns>A string that represents the current object.</returns>
		public override string ToString()
		{
			using (MemoryStream mem = new MemoryStream())
			{
				using (TextWriter writer = new StreamWriter(mem, Encoding.Unicode, 1024, true))
				{
					Serialize(writer);
				}
				mem.Position = 0;
				using (StreamReader reader = new StreamReader(mem, Encoding.Unicode))
				{
					return reader.ReadToEnd();
				}
			}
		}

		/// <summary>
		/// Set the whitespace of the element and all its children to the default "pretty" format.
		///
		/// Elements other than keys and values are on their own line and indented based on how
		/// deeply nexted they are. Values are led by a single space. Empty arrays and containers
		/// have no internal whitespace.
		/// </summary>
		/// <param name="newLine">The newline string. If null, Environment.NewLine is used.</param>
		/// <param name="indent">The indent string.</param>
		/// <param name="afterKey">The whitespace to insert after a key in an object (before the
		/// colon). If null, an empty string is used.</param>
		/// <param name="beforeValue">The whitespace to insert before a value in an object (after
		/// the colon). If null, a single space is used.</param>
		public void Prettify(string newLine = null, string indent = "  ", Whitespace afterKey = null, Whitespace beforeValue = null)
		{
			IFormat format = new DefaultFormat(newLine, indent, afterKey, beforeValue ?? new Whitespace(" "));
			Format(format);
		}

		/// <summary>
		/// Remove all whitespace from the element and all its children, conserving space and making
		/// it appropriate for a newline delimited JSON format.
		/// </summary>
		public void Minify()
		{
			IFormat format = new DefaultFormat(string.Empty, string.Empty, Whitespace.Empty, Whitespace.Empty);
			Format(format);
		}

		/// <summary>
		/// Set the whitespace of the element and all its children according to the rules of the
		/// format.
		/// </summary>
		/// <param name="format">The formatting rules to use.</param>
		/// <exception cref="ArgumentNullException">The format is null.</exception>
		public void Format(IFormat format)
		{
			if (format is null)
			{
				throw new ArgumentNullException(nameof(format));
			}

			int depth = 0;
			format.Format(this, depth, false, false, false);
			List<JsonElement> level = new List<JsonElement>()
			{
				this
			};
			List<JsonElement> nextLevel = new List<JsonElement>();

			while (level.Count > 0)
			{
				depth += 1;

				foreach (var element in level)
				{
					if (element is JsonArray array)
					{
						for (int i = 0; i < array.Count; i++)
						{
							format.Format(array[i], depth, false, false, i + 1 >= array.Count);
							nextLevel.Add(array[i]);
						}
					}
					else if (element is JsonObject obj)
					{
						for (int i = 0; i < obj.Count; i++)
						{
							format.Format(obj[i].Key, depth, true, false, i + 1 >= obj.Count);
							format.Format(obj[i].Value, depth, false, true, i + 1 >= obj.Count);
							nextLevel.Add(obj[i].Value);
						}
					}
				}

				(level, nextLevel) = (nextLevel, level);
				nextLevel.Clear();
			}
		}

		static JsonElement ReadElement(JsonReader reader)
		{
			JsonElement retval;

			Whitespace leading = Whitespace.Empty;
			if (reader.CurrentToken == TokenType.Whitespace)
			{
				leading = reader.Whitespace;
				_ = reader.Read();
			}

			Whitespace first;

			switch (reader.CurrentToken)
			{
				case TokenType.BeginArray:
					JsonArray array = new JsonArray();
					_ = reader.Read();
					first = Whitespace.Empty;
					if (reader.CurrentToken == TokenType.Whitespace)
					{
						// The first whitespace belongs to the first element, but only if it exists.
						first = reader.Whitespace;
						_ = reader.Read();
					}
					while (reader.CurrentToken != TokenType.EndArray)
					{
						array.Add(ReadElement(reader));
						if (reader.CurrentToken == TokenType.ListSeparator)
						{
							_ = reader.Read();
						}
					}
					if (array.Count == 0)
					{
						// The first whitespace was inside an empty array.
						array.EmptyWhitespace = first;
					}
					else
					{
						// The first whitespace belonged to the first element.
						array[0].Leading = first;
					}
					retval = array;
					break;
				case TokenType.BeginObject:
					JsonObject obj = new JsonObject();
					_ = reader.Read();
					first = Whitespace.Empty;
					if (reader.CurrentToken == TokenType.Whitespace)
					{
						// The first whitespace belongs to the first key, but only if it exists.
						first = reader.Whitespace;
						_ = reader.Read();
					}
					while (reader.CurrentToken != TokenType.EndObject)
					{
						JsonString key = (JsonString)ReadElement(reader);
						// Read past the key/value separator.
						_ = reader.Read();
						JsonObjectPair pair = new JsonObjectPair(key, ReadElement(reader));
						obj.Add(pair);
						if (reader.CurrentToken == TokenType.ListSeparator)
						{
							_ = reader.Read();
						}
					}
					if (obj.Count == 0)
					{
						// The first whitespace was inside an empty object.
						obj.EmptyWhitespace = first;
					}
					else
					{
						// The first whitespace belonged to the first key.
						obj[0].Key.Leading = first;
					}
					retval = obj;
					break;
				case TokenType.Boolean:
					retval = new JsonBoolean(reader.BooleanValue);
					break;
				case TokenType.Null:
					retval = new JsonNull();
					break;
				case TokenType.Number:
					retval = new JsonNumber(reader.NumberValue);
					break;
				case TokenType.String:
					retval = new JsonString(reader.StringValue, true);
					break;
				default:
					// This should never happen.
					retval = null;
					break;
			}

			_ = reader.Read();

			retval.Leading = leading;

			if (reader.CurrentToken == TokenType.Whitespace)
			{
				retval.Trailing = reader.Whitespace;
				_ = reader.Read();
			}

			return retval;
		}
	}
}
