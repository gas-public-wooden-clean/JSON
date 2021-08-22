using CER.Json.Stream;
using System;
using System.IO;

namespace CER.Json.DocumentObjectModel
{
	/// <summary>
	/// A null JSON value.
	/// </summary>
	public class JsonNull : JsonElement
	{
		/// <summary>
		/// Create an instance of a null JSON value with no whitespace.
		/// </summary>
		public JsonNull() : base()
		{ }

		/// <summary>
		/// Write the null value and whitespace as JSON to the stream.
		/// </summary>
		/// <param name="writer">The writer to write to.</param>
		/// <exception cref="ObjectDisposedException">The writer is closed.</exception>
		/// <exception cref="IOException">An I/O error occurs.</exception>
		public override void Serialize(TextWriter writer)
		{
			if (writer is null)
			{
				throw new ArgumentNullException(nameof(writer));
			}

			writer.Write(Leading.Value);
			writer.Write("null");
			writer.Write(Trailing.Value);
		}
	}
}
