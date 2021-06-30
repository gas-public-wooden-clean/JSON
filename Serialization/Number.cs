using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;

namespace CER.JSON.DocumentObjectModel
{
	[DebuggerDisplay("{_json}")]
	public class Number : Element
	{
		public Number(decimal value)
		{
			Value = value;
		}

		/// <summary>
		/// Create a number from a JSON number string.
		/// </summary>
		/// <param name="json">A JSON number string.</param>
		/// <exception cref="System.FormatException">json is not in the correct format.</exception>
		/// <exception cref="System.ArgumentNullException">json is null.</exception>
		public Number(string json)
		{
			JSON = json;
		}

		private string _json;

		/// <summary>
		/// The value of the number as a decimal.
		/// </summary>
		/// <exception cref="System.OverflowException">The value being retrieved is less than System.Decimal.MinValue or greater than System.Decimal.MaxValue.</exception>
		public decimal Value
		{
			get { return Parse(_json); }
			set { _json = value.ToString("G", CultureInfo.InvariantCulture); }
		}

		/// <summary>
		/// The value of the number as a JSON string.
		/// </summary>
		/// <exception cref="System.FormatException">The value being set is not in the correct format.</exception>
		/// <exception cref="System.ArgumentNullException">value is null.</exception>
		public string JSON
		{
			get { return _json; }
			set
			{
				try
				{
					Parse(value);
				}
				catch (OverflowException)
				{
				}
				_json = value;
			}
		}

		public override void Serialize(TextWriter writer)
		{
			writer.Write(Leading.Value);
			writer.Write(JSON);
			writer.Write(Trailing.Value);
		}

		private static decimal Parse(string json)
		{
			if (json.StartsWith(CultureInfo.InvariantCulture.NumberFormat.PositiveSign))
			{
				throw new FormatException("Value cannot have leading positive sign.");
			}
			return decimal.Parse(json, NumberStyles.AllowExponent | NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture);
		}
	}
}
