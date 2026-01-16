namespace CER.Json.DocumentObjectModel
{
	/// <summary>
	/// Progress made while deserializing an element.
	/// </summary>
	public class JsonParseProgress
	{
		internal JsonParseProgress(JsonElement element, JsonPath path)
		{
			Element = element;
			Path = path;
		}

		/// <summary>
		/// The last element that was fully parsed.
		/// </summary>
		public JsonElement Element { get; }

		/// <summary>
		/// Path to the element from the root.
		/// </summary>
		public JsonPath Path { get; }
	}
}
