using System;

namespace CER.JSON.Stream
{
	public class InvalidTextException : Exception
	{
		public InvalidTextException()
			: base()
		{ }

		public InvalidTextException(string message)
			: base(message)
		{ }

		public InvalidTextException(string message, Exception innerException)
			: base(message, innerException)
		{ }

		internal InvalidTextException(ulong line, ulong character, string message)
			: base(string.Format("Line {0} character {1}: {2}", line, character, message))
		{
		}
	}
}
