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
		/// Create an instance of a null JSON value with the given whitespace.
		/// </summary>
		/// <param name="leading">Leading whitespace.</param>
		/// <param name="trailing">Trailing whitespace.</param>
		public JsonNull(Whitespace leading, Whitespace trailing) : base(leading, trailing)
		{ }

		/// <summary>
		/// Write the null value and whitespace as JSON to the stream.
		/// </summary>
		/// <param name="writer">The writer to write to.</param>
		/// <exception cref="System.ObjectDisposedException">The writer is closed.</exception>
		/// <exception cref="IOException">An I/O error occurs.</exception>
		public override void Serialize(TextWriter writer)
		{
			writer.Write(Leading.Value);
			writer.Write("null");
			writer.Write(Trailing.Value);
		}
	}
}
