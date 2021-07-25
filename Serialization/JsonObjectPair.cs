using System;
using System.Diagnostics;

namespace CER.Json.DocumentObjectModel
{
	/// <summary>
	/// An object pair, consisting of a string key and a corresponding value.
	/// </summary>
	[DebuggerDisplay("{Key}:{Value}")]
	public class JsonObjectPair
	{
		/// <summary>
		/// Create an object pair with an empty string as the key and a JSON null value.
		/// </summary>
		public JsonObjectPair()
		{
			Key = new JsonString();
			Value = new JsonNull();
		}

		/// <summary>
		/// Create an object pair with the given key and value.
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public JsonObjectPair(JsonString key, JsonElement value)
		{
			Key = key;
			Value = value;
		}

		JsonString _key;
		JsonElement _value;

		/// <summary>
		/// The key.
		/// </summary>
		/// <exception cref="System.ArgumentNullException">The given value is null.</exception>
		public JsonString Key
		{
			get => _key;
			set => _key = value ?? throw new ArgumentNullException(nameof(value));
		}

		/// <summary>
		/// The value.
		/// </summary>
		/// <exception cref="System.ArgumentNullException">The given value is null.</exception>
		public JsonElement Value
		{
			get => _value;
			set => _value = value ?? throw new ArgumentNullException(nameof(value));
		}
	}
}
