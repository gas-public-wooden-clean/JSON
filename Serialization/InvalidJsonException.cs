using System;
using System.Runtime.Serialization;

namespace CER.Json.Stream
{
	/// <summary>
	/// Errors that occur while parsing JSON due to content that is not well-formed.
	/// </summary>
	[Serializable]
	public class InvalidJsonException : Exception
	{
		/// <summary>
		/// Create an error with no information.
		/// </summary>
		public InvalidJsonException()
			: base()
		{ }

		/// <summary>
		/// Create an error with an arbitrary message.
		/// </summary>
		/// <param name="message">An arbitrary message.</param>
		public InvalidJsonException(string message)
			: base(message)
		{ }

		/// <summary>
		/// Create an error with an arbitrary message and an inner exception.
		/// </summary>
		/// <param name="message">An arbitrary message.</param>
		/// <param name="innerException">The exception that caused this one.</param>
		public InvalidJsonException(string message, Exception innerException)
			: base(message, innerException)
		{ }

		/// <summary>
		/// Create an error with a message that calls out the line and character in the JSON text that caused the error.
		/// </summary>
		/// <param name="line">Line number, starting at 1.</param>
		/// <param name="character">Character number within the line, starting at 1.</param>
		/// <param name="message">Additional information to include in the message.</param>
		internal InvalidJsonException(ulong line, ulong character, string message)
			: base(string.Format("Line {0} character {1}: {2}", line, character, message))
		{ }

		/// <summary>
		/// Create an error from serialized data.
		/// </summary>
		/// <param name="info">Serialization information.</param>
		/// <param name="context">Serialization context.</param>
		protected InvalidJsonException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{ }
	}
}
