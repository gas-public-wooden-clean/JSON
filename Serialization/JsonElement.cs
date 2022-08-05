using CER.Json.Stream;
using System;
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
				using (TextReader reader = new StreamReader(mem, Encoding.Unicode))
				{
					return reader.ReadToEnd();
				}
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
