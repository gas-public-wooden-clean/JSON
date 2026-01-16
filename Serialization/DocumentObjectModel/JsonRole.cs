namespace CER.Json.DocumentObjectModel
{
	/// <summary>
	/// An element's role within the structure of a document.
	/// </summary>
	public enum JsonRole
	{
		/// <summary>
		/// The element is a root value or an array item.
		/// </summary>
		Value,
		/// <summary>
		/// The element is a key in an object.
		/// </summary>
		ObjectKey,
		/// <summary>
		/// The element is a value in an object.
		/// </summary>
		ObjectValue,
	}
}