namespace CER.Json.Stream
{
	/// <summary>
	/// A token type that can be encountered while parsing JSON.
	/// </summary>
	public enum TokenType
	{
		/// <summary>
		/// The beginning of an array i.e. '['.
		/// </summary>
		BeginArray,
		/// <summary>
		/// The beginning of an object i.e. '{'.
		/// </summary>
		BeginObject,
		/// <summary>
		/// A boolean value i.e. 'true' or 'false'.
		/// </summary>
		Boolean,
		/// <summary>
		/// The end of an array i.e. ']'.
		/// </summary>
		EndArray,
		/// <summary>
		/// The end of an object i.e. '}'.
		/// </summary>
		EndObject,
		/// <summary>
		/// The parser has encountered an error or it is positioned at the start or end of a stream.
		/// </summary>
		Invalid,
		/// <summary>
		/// A key-value separator i.e. ':'.
		/// </summary>
		KeyValueSeparator,
		/// <summary>
		/// An array or object list separator i.e. ','.
		/// </summary>
		ListSeparator,
		/// <summary>
		/// A null value i.e. 'null'.
		/// </summary>
		Null,
		/// <summary>
		/// A numeric JSON value.
		/// </summary>
		Number,
		/// <summary>
		/// A JSON string value.
		/// </summary>
		String,
		/// <summary>
		/// JSON whitespace.
		/// </summary>
		WhiteSpace
	}
}
