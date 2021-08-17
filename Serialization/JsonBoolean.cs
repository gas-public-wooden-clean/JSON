using CER.Json.Stream;
using System;
using System.Diagnostics;
using System.IO;

namespace CER.Json.DocumentObjectModel
{
	/// <summary>
	/// A boolean JSON value: 'true' or 'false'.
	/// </summary>
	[DebuggerDisplay("{Value}")]
	public class JsonBoolean : JsonElement
	{
		/// <summary>
		/// Create a boolean element with the given value and no whitespace.
		/// </summary>
		/// <param name="value">The value.</param>
		public JsonBoolean(bool value) => Value = value;

		/// <summary>
		/// Create a boolean element the given value and whitespace.
		/// </summary>
		/// <param name="leading">Leading whitespace.</param>
		/// <param name="trailing">Trailing whitespace.</param>
		/// <param name="value">The value.</param>
		/// <exception cref="ArgumentNullException">One of the whitespace values is null.</exception>
		public JsonBoolean(Whitespace leading, Whitespace trailing, bool value)
			: base(leading, trailing) => Value = value;

		/// <summary>
		/// The value.
		/// </summary>
		public bool Value
		{
			get;
			set;
		}

		/// <summary>
		/// Write the boolean and whitespace as JSON to the stream.
		/// </summary>
		/// <param name="writer">The writer to write to.</param>
		/// <exception cref="ObjectDisposedException">The writer is closed.</exception>
		/// <exception cref="IOException">An I/O error occurs.</exception>
		public override void Serialize(TextWriter writer)
		{
			writer.Write(Leading.Value);
			writer.Write(Value ? "true" : "false");
			writer.Write(Trailing.Value);
		}
	}
}
