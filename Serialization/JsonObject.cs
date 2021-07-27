using System;
using System.Collections.Generic;
using System.IO;

namespace CER.Json.DocumentObjectModel
{
	/// <summary>
	/// A JSON object.
	/// </summary>
	public class JsonObject : JsonElement
	{
		/// <summary>
		/// Create an empty object with no leading, trailing, or contained whitespace.
		/// </summary>
		public JsonObject() : base()
		{
			Values = new List<JsonObjectPair>();
			EmptyWhitespace = Whitespace.Empty;
		}

		Whitespace _emptyWhitespace;

		/// <summary>
		/// The elements in the object. Note that multiple values with the same key are allowed.
		/// </summary>
		public IList<JsonObjectPair> Values
		{
			get;
			private set;
		}

		/// <summary>
		/// Whitespace that will be written inside the object if there are no elements in it.
		/// </summary>
		/// <exception cref="ArgumentNullException">The value being set is null.</exception>
		public Whitespace EmptyWhitespace
		{
			get => _emptyWhitespace;
			set => _emptyWhitespace = value ?? throw new ArgumentNullException(nameof(value));
		}

		/// <summary>
		/// Get the first value, if any, with an equivalent key.
		/// </summary>
		/// <param name="key">The value of a JSON string (not the JSON representation).</param>
		/// <param name="value">The first value with a matching key.</param>
		/// <returns>Whether exactly one value was found for the given key.</returns>
		public bool TryGetValue(string key, out JsonElement value)
		{
			value = null;

			if (key is null)
			{
				throw new ArgumentNullException(nameof(key));
			}

			foreach (JsonObjectPair pair in Values)
			{
				if (pair.Key.Value.Equals(key))
				{
					if (value is null)
					{
						value = pair.Value;
					}
					else
					{
						return false;
					}
				}
			}

			return value != null;
		}

		/// <summary>
		/// Write the object, whitespace, and elements within it, as JSON to the stream.
		/// </summary>
		/// <param name="writer">The writer to write to.</param>
		/// <exception cref="ObjectDisposedException">The writer is closed.</exception>
		/// <exception cref="IOException">An I/O error occurs.</exception>
		public override void Serialize(TextWriter writer)
		{
			writer.Write(Leading.Value);
			writer.Write("{");
			if (Values.Count == 0)
			{
				writer.Write(EmptyWhitespace.Value);
			}
			else
			{
				bool first = true;
				foreach (JsonObjectPair entry in Values)
				{
					if (!first)
					{
						writer.Write(",");
					}
					first = false;

					entry.Key.Serialize(writer);
					writer.Write(":");
					entry.Value.Serialize(writer);
				}
			}
			writer.Write("}");
			writer.Write(Trailing.Value);
		}
	}
}
