using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;

namespace CER.JSON.DocumentObjectModel
{
	/// <summary>
	/// A numeric JSON value.
	/// </summary>
	[DebuggerDisplay("{_json}")]
	public class Number : Element
	{
		/// <summary>
		/// Create a number from a decimal value.
		/// </summary>
		/// <param name="value">The value.</param>
		public Number(decimal value) => Value = value;

		/// <summary>
		/// Create a number from a JSON number string.
		/// </summary>
		/// <param name="json">A JSON number string.</param>
		/// <exception cref="System.FormatException">json is not in the correct format.</exception>
		/// <exception cref="System.ArgumentNullException">json is null.</exception>
		public Number(string json) => JSON = json;

		string _json;

		/// <summary>
		/// The value of the number as a decimal.
		/// </summary>
		/// <exception cref="System.OverflowException">The value being retrieved is less than System.Decimal.MinValue or greater than System.Decimal.MaxValue.</exception>
		public decimal Value
		{
			get => Parse(_json);
			set => _json = value.ToString("G", CultureInfo.InvariantCulture);
		}

		/// <summary>
		/// The value of the number as a JSON string.
		/// </summary>
		/// <exception cref="System.FormatException">The value being set is not in the correct format.</exception>
		/// <exception cref="System.ArgumentNullException">value is null.</exception>
		public string JSON
		{
			get => _json;
			set
			{
				if (value is null)
				{
					throw new ArgumentNullException(nameof(value));
				}

				try
				{
					_ = Parse(value);
				}
				catch (OverflowException)
				{
				}
				_json = value;
			}
		}

		/// <summary>
		/// Write the number and whitespace as JSON to the stream.
		/// </summary>
		/// <param name="writer">The writer to write to.</param>
		/// <exception cref="System.ObjectDisposedException">The writer is closed.</exception>
		/// <exception cref="System.IO.IOException">An I/O error occurs.</exception>
		public override void Serialize(TextWriter writer)
		{
			writer.Write(Leading.Value);
			writer.Write(JSON);
			writer.Write(Trailing.Value);
		}

		static decimal Parse(string json)
		{
			if (json.StartsWith(CultureInfo.InvariantCulture.NumberFormat.PositiveSign))
			{
				throw new FormatException("Value cannot have leading positive sign.");
			}
			return decimal.Parse(json, NumberStyles.AllowExponent | NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture);
		}
	}
}
