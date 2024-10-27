using System;
using System.Globalization;

namespace CER.Json.Stream
{
	/// <summary>
	/// A string of only whitespace characters (spaces, tabs, carriage returns, and linefeeds).
	/// </summary>
	public class Whitespace
	{
		/// <summary>
		/// Create whitespace with the given value.
		/// </summary>
		/// <param name="whitespace">A string of whitespace characters.</param>
		/// <exception cref="ArgumentException">The value contains non-whitespace characters.</exception>
		/// <exception cref="ArgumentNullException">The value is null.</exception>
		public Whitespace(string whitespace) => Value = whitespace;

		string _validated;

		/// <summary>
		/// An instance containing no whitespace characters.
		/// </summary>
		public static Whitespace Empty { get; } = new Whitespace(string.Empty);

		/// <summary>
		/// A value consisting of only whitespace characters.
		/// </summary>
		public string Value
		{
			get => _validated;
			private set
			{
				if (value is null)
				{
					throw new ArgumentNullException(nameof(value));
				}

				foreach (char c in value)
				{
					if (!IsLegal(c))
					{
						string message = string.Format(CultureInfo.CurrentCulture, Strings.ValueNotWhitespace, value);
						throw new ArgumentException(message, nameof(value));
					}
				}
				_validated = value;
			}
		}

		/// <summary>
		/// Check whether the character is whitespace.
		/// </summary>
		/// <param name="c">The character to check.</param>
		/// <returns>Whether the character is whitespace.</returns>
		public static bool IsLegal(char c) => c == ' ' || c == '\t' || c == '\r' || c == '\n';
	}
}
