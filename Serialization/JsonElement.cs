using CER.Json.Stream;
using System;

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

		/// <summary>
		/// Create an element with the given leading or trailing whitespace.
		/// </summary>
		/// <param name="leading">Leading whitespace.</param>
		/// <param name="trailing">Trailing whitespace.</param>
		/// <exception cref="ArgumentNullException">One of the whitespace values is null.</exception>
		protected JsonElement(Whitespace leading, Whitespace trailing)
		{
			Leading = leading;
			Trailing = trailing;
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
		/// <exception cref="InvalidOperationException">reader is in an invalid state from a previous exception.</exception>
		/// <exception cref="InvalidJsonException">The underlying text stream is not valid JSON.</exception>
		/// <exception cref="ObjectDisposedException">The underlying stream has been closed.</exception>
		/// <exception cref="System.IO.IOException">An I/O error occurs.</exception>
		public static JsonElement Deserialize(JsonReader reader)
		{
			_ = reader.Read();
			return ReadElement(reader);
		}

		/// <summary>
		/// Write the JSON element to a stream.
		/// </summary>
		/// <param name="writer">The stream to write to.</param>
		/// <exception cref="ObjectDisposedException">The writer is closed.</exception>
		/// <exception cref="System.IO.IOException">An I/O error occurs.</exception>
		public abstract void Serialize(System.IO.TextWriter writer);

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
						array.Values.Add(ReadElement(reader));
						if (reader.CurrentToken == TokenType.ListSeparator)
						{
							_ = reader.Read();
						}
					}
					if (array.Values.Count == 0)
					{
						// The first whitespace was inside an empty array.
						array.EmptyWhitespace = first;
					}
					else
					{
						// The first whitespace belonged to the first element.
						array.Values[0].Leading = first;
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
						JsonObjectPair pair = new JsonObjectPair
						{
							Key = (JsonString)ReadElement(reader)
						};
						// Read past the key/value separator.
						_ = reader.Read();
						pair.Value = ReadElement(reader);
						obj.Values.Add(pair);
						if (reader.CurrentToken == TokenType.ListSeparator)
						{
							_ = reader.Read();
						}
					}
					if (obj.Values.Count == 0)
					{
						// The first whitespace was inside an empty object.
						obj.EmptyWhitespace = first;
					}
					else
					{
						// The first whitespace belonged to the first key.
						obj.Values[0].Key.Leading = first;
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
					retval = reader.NumberValue;
					break;
				case TokenType.String:
					retval = reader.StringValue;
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
