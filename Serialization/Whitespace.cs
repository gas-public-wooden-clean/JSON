using System;

namespace CER.JSON.DocumentObjectModel
{
	public class Whitespace
	{
		public Whitespace(string whitespace)
		{
			Value = whitespace;
		}

		private static Whitespace _empty = new Whitespace(string.Empty);
		private string _validated;

		public static Whitespace Empty
		{
			get { return _empty; }
		}

		public string Value
		{
			get { return _validated; }
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

		public static bool IsLegal(char c)
		{
			return c == ' ' || c == '\t' || c == '\r' || c == '\n';
		}
	}
}
