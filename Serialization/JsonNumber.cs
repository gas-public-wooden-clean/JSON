using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;

namespace CER.Json.DocumentObjectModel
{
	/// <summary>
	/// A numeric JSON value.
	/// </summary>
	[DebuggerDisplay("{_json}")]
	public class JsonNumber : JsonElement
	{
		/// <summary>
		/// Create a number from a decimal value.
		/// </summary>
		/// <param name="value">The value.</param>
		public JsonNumber(decimal value) => Value = value;

		/// <summary>
		/// Create a number from a JSON number string.
		/// </summary>
		/// <param name="json">A JSON number string.</param>
		/// <exception cref="FormatException">json is not in the correct format.</exception>
		/// <exception cref="ArgumentNullException">json is null.</exception>
		public JsonNumber(string json) => Json = json;

		string _json;

		/// <summary>
		/// The value of the number as a decimal.
		/// </summary>
		/// <exception cref="OverflowException">The value being retrieved is less than System.Decimal.MinValue or greater than System.Decimal.MaxValue.</exception>
		public decimal Value
		{
			get => JsonToDecimal(_json);
			set => _json = value.ToString("G", CultureInfo.InvariantCulture);
		}

		/// <summary>
		/// The value of the number as a JSON string.
		/// </summary>
		/// <exception cref="FormatException">The value being set is not in the correct format.</exception>
		/// <exception cref="ArgumentNullException">value is null.</exception>
		public string Json
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
					_ = JsonToDecimal(value);
				}
				catch (OverflowException)
				{
				}
				_json = value;
			}
		}

		/// <summary>
		/// Convert a JSON number to a native Decimal.
		/// </summary>
		/// <param name="json">The JSON representation of a number.</param>
		/// <returns>The value of the JSON number.</returns>
		/// <exception cref="ArgumentNullException">The given representation is null.</exception>
		/// <exception cref="FormatException">The JSON number is not in the correct format.</exception>
		/// <exception cref="OverflowException">The value being retrieved is less than System.Decimal.MinValue or greater than System.Decimal.MaxValue.</exception>
		public static decimal JsonToDecimal(string json)
		{
			if (json is null)
			{
				throw new ArgumentNullException(nameof(json));
			}

			if (json.StartsWith(CultureInfo.InvariantCulture.NumberFormat.PositiveSign, StringComparison.Ordinal))
			{
				throw new FormatException(Strings.ValueCannotHaveLeadingPositiveSign);
			}

			return decimal.Parse(json, NumberStyles.AllowExponent | NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture);
		}

		/// <summary>
		/// Write the number and whitespace as JSON to the stream.
		/// </summary>
		/// <param name="writer">The writer to write to.</param>
		/// <exception cref="ObjectDisposedException">The writer is closed.</exception>
		/// <exception cref="IOException">An I/O error occurs.</exception>
		public override void Serialize(TextWriter writer)
		{
			writer.Write(Leading.Value);
			writer.Write(Json);
			writer.Write(Trailing.Value);
		}
	}
}
