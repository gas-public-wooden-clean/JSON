using System;

namespace CER.JSON.Stream
{
	/// <summary>
	/// Errors that occur while parsing JSON due to content that is not well-formed.
	/// </summary>
	public class InvalidTextException : Exception
	{
		/// <summary>
		/// Create an error with no information.
		/// </summary>
		public InvalidTextException()
			: base()
		{ }

		/// <summary>
		/// Create an error with an arbitrary message.
		/// </summary>
		/// <param name="message">An arbitrary message.</param>
		public InvalidTextException(string message)
			: base(message)
		{ }

		/// <summary>
		/// Create an error with an arbitrary message and an inner exception.
		/// </summary>
		/// <param name="message">An arbitrary message.</param>
		/// <param name="innerException">The exception that caused this one.</param>
		public InvalidTextException(string message, Exception innerException)
			: base(message, innerException)
		{ }

		/// <summary>
		/// Create an error with a message that calls out the line and character in the JSON text that caused the error.
		/// </summary>
		/// <param name="line">Line number, starting at 1.</param>
		/// <param name="character">Character number within the line, starting at 1.</param>
		/// <param name="message">Additional information to include in the message.</param>
		internal InvalidTextException(ulong line, ulong character, string message)
			: base(string.Format("Line {0} character {1}: {2}", line, character, message))
		{
		}
	}
}
