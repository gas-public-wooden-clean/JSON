namespace CER.JSON.Stream
{
	public enum Type
	{
		BeginArray,
		BeginObject,
		Boolean,
		EndArray,
		EndObject,
		Invalid,
		KeyValueSeparator,
		ListSeparator,
		Null,
		Number,
		String,
		Whitespace
	}
}
