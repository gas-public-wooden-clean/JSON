using System;
using System.IO;
using System.Threading.Tasks;

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
			ArgumentNullException.ThrowIfNull(writer, nameof(writer));

			writer.Write(Leading.Value);
			writer.Write("null");
			writer.Write(Trailing.Value);
		}

		/// <summary>
		/// Write the null value and whitespace as JSON to the stream.
		/// </summary>
		/// <param name="writer">The writer to write to.</param>
		/// <exception cref="ObjectDisposedException">The writer is closed.</exception>
		/// <exception cref="IOException">An I/O error occurs.</exception>
		public override async Task SerializeAsync(TextWriter writer)
		{
			ArgumentNullException.ThrowIfNull(writer, nameof(writer));

			await writer.WriteAsync(Leading.Value);
			await writer.WriteAsync("null");
			await writer.WriteAsync(Trailing.Value);
		}
	}
}
