using System;

using StreamReader = CER.JSON.Stream.StreamReader;
using Type = CER.JSON.Stream.Type;

namespace CER.JSON.DocumentObjectModel
{
	public abstract class Element
	{
		/// <summary>
		/// Create an element with no leading or trailing whitespace.
		/// </summary>
		public Element()
		{
			Leading = Whitespace.Empty;
			Trailing = Whitespace.Empty;
		}

		/// <summary>
		/// Create an element with the given leading or trailing whitespace.
		/// </summary>
		/// <param name="leading">Leading whitespace.</param>
		/// <param name="trailing">Trailing whitespace.</param>
		public Element(Whitespace leading, Whitespace trailing)
		{
			Leading = leading;
			Trailing = trailing;
		}

		private Whitespace _leading;
		private Whitespace _trailing;

		public Whitespace Leading
		{
			get { return _leading; }
			set { _leading = value ?? throw new ArgumentNullException(nameof(value)); }
		}

		public Whitespace Trailing
		{
			get { return _trailing; }
			set { _trailing = value ?? throw new ArgumentNullException(nameof(value)); }
		}

		/// <summary>
		/// Read the JSON element from a stream.
		/// </summary>
		/// <param name="reader">The stream to read from, which should be positioned at the start.</param>
		/// <returns>The JSON element contained within the document.</returns>
		/// <exception cref="System.InvalidOperationException">reader is in an invalid state from a previous exception.</exception>
		/// <exception cref="CER.JSON.Stream.InvalidTextException">The underlying text stream is not valid JSON.</exception>
		/// <exception cref="System.ObjectDisposedException">The underlying stream has been closed.</exception>
		/// <exception cref="System.IO.IOException">An I/O error occurs.</exception>
		public static Element Deserialize(StreamReader reader)
		{
			_ = reader.Read();
			return ReadElement(reader);
		}

		/// <summary>
		/// Write the JSON element to a stream.
		/// </summary>
		/// <param name="writer">The stream to write to.</param>
		/// <exception cref="System.IO.IOException">An I/O error occurs.</exception>
		public abstract void Serialize(System.IO.TextWriter writer);

		private static Element ReadElement(StreamReader reader)
		{
			Element retval;

			Whitespace leading = Whitespace.Empty;
			if (reader.Type == Type.Whitespace)
			{
				leading = reader.WhitespaceValue;
				_ = reader.Read();
			}

			Whitespace first;

			switch (reader.Type)
			{
				case Type.BeginArray:
					Array array = new Array();
					_ = reader.Read();
					first = Whitespace.Empty;
					if (reader.Type == Type.Whitespace)
					{
						// The first whitespace belongs to the first element, but only if it exists.
						first = reader.WhitespaceValue;
						_ = reader.Read();
					}
					while (reader.Type != Type.EndArray)
					{
						array.Values.Add(ReadElement(reader));
						if (reader.Type == Type.ListSeparator)
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
				case Type.BeginObject:
					Object obj = new Object();
					_ = reader.Read();
					first = Whitespace.Empty;
					if (reader.Type == Type.Whitespace)
					{
						// The first whitespace belongs to the first key, but only if it exists.
						first = reader.WhitespaceValue;
						_ = reader.Read();
					}
					while (reader.Type != Type.EndObject)
					{
						ObjectPair pair = new ObjectPair();
						pair.Key = (String)ReadElement(reader);
						// Read past the key/value separator.
						_ = reader.Read();
						pair.Value = ReadElement(reader);
						obj.Values.Add(pair);
						if (reader.Type == Type.ListSeparator)
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
				case Type.Boolean:
					retval = new Boolean(reader.BooleanValue);
					break;
				case Type.Null:
					retval = new Null();
					break;
				case Type.Number:
					retval = reader.NumberValue;
					break;
				case Type.String:
					retval = reader.StringValue;
					break;
				default:
					// This should never happen.
					retval = null;
					break;
			}

			_ = reader.Read();

			retval.Leading = leading;

			if (reader.Type == Type.Whitespace)
			{
				retval.Trailing = reader.WhitespaceValue;
				_ = reader.Read();
			}

			return retval;
		}
	}
}
