using System;
using System.Diagnostics;

namespace CER.JSON.DocumentObjectModel
{
	/// <summary>
	/// An object pair, consisting of a string key and a corresponding value.
	/// </summary>
	[DebuggerDisplay("{Key}:{Value}")]
	public class ObjectPair
	{
		/// <summary>
		/// Create an object pair with an empty string as the key and a JSON null value.
		/// </summary>
		public ObjectPair()
		{
			Key = new String();
			Value = new Null();
		}

		/// <summary>
		/// Create an object pair with the given key and value.
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public ObjectPair(String key, Element value)
		{
			Key = key;
			Value = value;
		}

		String _key;
		Element _value;

		/// <summary>
		/// The key.
		/// </summary>
		/// <exception cref="System.ArgumentNullException">The given value is null.</exception>
		public String Key
		{
			get => _key;
			set => _key = value ?? throw new ArgumentNullException(nameof(value));
		}

		/// <summary>
		/// The value.
		/// </summary>
		/// <exception cref="System.ArgumentNullException">The given value is null.</exception>
		public Element Value
		{
			get => _value;
			set => _value = value ?? throw new ArgumentNullException(nameof(value));
		}
	}
}
