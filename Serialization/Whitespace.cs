using System;

namespace CER.JSON.DocumentObjectModel
{
	public class Whitespace
	{
		public Whitespace(string whitespace) => Value = whitespace;

		string _validated;

		public static Whitespace Empty { get; } = new Whitespace(string.Empty);

		public string Value
		{
			get => _validated;
			private set
			{
				foreach (char c in value)
				{
					if (!IsLegal(c))
					{
						string message = string.Format("{0} is not whitespace. It can only contain spaces, linefeeds, carriage returns, and/or horizontal tabs.", value);
						throw new ArgumentException(message, nameof(value));
					}
				}
				_validated = value;
			}
		}

		public static bool IsLegal(char c) => c == ' ' || c == '\t' || c == '\r' || c == '\n';
	}
}
