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
		/// <exception cref="ArgumentNullException">The writer is null.</exception>
		public override void Serialize(TextWriter writer)
		{
			if (writer is null)
			{
				throw new ArgumentNullException(nameof(writer));
			}

			writer.Write(Leading.Value);
			writer.Write(Value ? "true" : "false");
			writer.Write(Trailing.Value);
		}
	}
}
