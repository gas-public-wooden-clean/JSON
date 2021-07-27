using System;
using System.Collections.Generic;
using System.IO;

namespace CER.Json.DocumentObjectModel
{
	/// <summary>
	/// A JSON array.
	/// </summary>
	public class JsonArray : JsonElement
	{
		/// <summary>
		/// Create an empty array with no leading, trailing, or contained whitespace.
		/// </summary>
		public JsonArray() : base()
		{
			Values = new List<JsonElement>();
			EmptyWhitespace = Whitespace.Empty;
		}

		Whitespace _emptyWhitespace;

		/// <summary>
		/// The elements in the array.
		/// </summary>
		public IList<JsonElement> Values
		{
			get;
			private set;
		}

		/// <summary>
		/// Whitespace that will be written inside the array if there are no elements in it.
		/// </summary>
		/// <exception cref="ArgumentNullException">The value being set is null.</exception>
		public Whitespace EmptyWhitespace
		{
			get => _emptyWhitespace;
			set => _emptyWhitespace = value ?? throw new ArgumentNullException(nameof(value));
		}

		/// <summary>
		/// Write the array, whitespace, and elements within it, as JSON to the stream.
		/// </summary>
		/// <param name="writer">The writer to write to.</param>
		/// <exception cref="ObjectDisposedException">The writer is closed.</exception>
		/// <exception cref="IOException">An I/O error occurs.</exception>
		/// <exception cref="ArgumentNullException">The writer is null.</exception>
		public override void Serialize(TextWriter writer)
		{
			if (writer is null)
			{
				throw new ArgumentNullException(nameof(writer));
			}

			writer.Write(Leading.Value);
			writer.Write("[");
			if (Values.Count == 0)
			{
				writer.Write(EmptyWhitespace.Value);
			}
			else
			{
				bool first = true;
				foreach (JsonElement element in Values)
				{
					if (!first)
					{
						writer.Write(",");
					}
					first = false;

					element.Serialize(writer);
				}
			}
			writer.Write("]");
			writer.Write(Trailing.Value);
		}
	}
}
