using System;
using System.Collections.Generic;
using System.IO;

namespace CER.JSON.DocumentObjectModel
{
	/// <summary>
	/// A JSON array.
	/// </summary>
	public class Array : Element
	{
		/// <summary>
		/// Create an empty array with no leading, trailing, or contained whitespace.
		/// </summary>
		public Array() : base()
		{
			Values = new List<Element>();
			EmptyWhitespace = Whitespace.Empty;
		}

		Whitespace _emptyWhitespace;

		/// <summary>
		/// The elements in the array.
		/// </summary>
		public IList<Element> Values
		{
			get;
			private set;
		}

		/// <summary>
		/// Whitespace that will be written inside the array if there are no elements in it.
		/// </summary>
		/// <exception cref="System.ArgumentNullException">The value being set is null.</exception>
		public Whitespace EmptyWhitespace
		{
			get => _emptyWhitespace;
			set => _emptyWhitespace = value ?? throw new ArgumentNullException(nameof(value));
		}

		/// <summary>
		/// Write the array, whitespace, and elements within it, as JSON to the stream.
		/// </summary>
		/// <param name="writer">The writer to write to.</param>
		/// <exception cref="System.ObjectDisposedException">The writer is closed.</exception>
		/// <exception cref="System.IO.IOException">An I/O error occurs.</exception>
		public override void Serialize(TextWriter writer)
		{
			writer.Write(Leading.Value);
			writer.Write("[");
			if (Values.Count == 0)
			{
				writer.Write(EmptyWhitespace.Value);
			}
			else
			{
				bool first = true;
				foreach (Element element in Values)
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
