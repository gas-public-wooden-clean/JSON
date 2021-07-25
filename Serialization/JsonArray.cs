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
			EmptyWhiteSpace = WhiteSpace.Empty;
		}

		WhiteSpace _emptyWhiteSpace;

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
		/// <exception cref="System.ArgumentNullException">The value being set is null.</exception>
		public WhiteSpace EmptyWhiteSpace
		{
			get => _emptyWhiteSpace;
			set => _emptyWhiteSpace = value ?? throw new ArgumentNullException(nameof(value));
		}

		/// <summary>
		/// Write the array, whitespace, and elements within it, as JSON to the stream.
		/// </summary>
		/// <param name="writer">The writer to write to.</param>
		/// <exception cref="System.ObjectDisposedException">The writer is closed.</exception>
		/// <exception cref="System.IO.IOException">An I/O error occurs.</exception>
		/// <exception cref="System.ArgumentNullException">The writer is null.</exception>
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
				writer.Write(EmptyWhiteSpace.Value);
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
