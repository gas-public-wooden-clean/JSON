using CER.Json.Stream;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace CER.Json.DocumentObjectModel
{
	/// <summary>
	/// A JSON object.
	/// </summary>
	public class JsonObject : JsonElement, IList<JsonObjectPair>
	{
		/// <summary>
		/// Create an empty object with no leading, trailing, or contained whitespace.
		/// </summary>
		public JsonObject() : base()
		{
			_values = new List<JsonObjectPair>();
			EmptyWhitespace = Whitespace.Empty;
		}

		Whitespace _emptyWhitespace;

		readonly IList<JsonObjectPair> _values;

		/// <summary>
		/// Whitespace that will be written inside the object if there are no elements in it.
		/// </summary>
		/// <exception cref="ArgumentNullException">The value being set is null.</exception>
		public Whitespace EmptyWhitespace
		{
			get => _emptyWhitespace;
			set => _emptyWhitespace = value ?? throw new ArgumentNullException(nameof(value));
		}

		/// <summary>
		/// Get the number of entries contained in the object.
		/// </summary>
		/// <returns>The number of entries contained in the object.</returns>
		public int Count => _values.Count;

		/// <summary>
		/// Get whether the collection is read-only, which objects are not.
		/// </summary>
		/// <returns>False.</returns>
		public bool IsReadOnly => _values.IsReadOnly;

		/// <summary>
		/// Gets or sets the pair at the specified index.
		/// </summary>
		/// <param name="index">The pair index.</param>
		/// <returns>The pair at the given index.</returns>
		/// <exception cref="ArgumentOutOfRangeException">Index is not a valid index.</exception>
		/// <exception cref="ArgumentNullException">The key or value is null.</exception>
		public JsonObjectPair this[int index]
		{
			get => _values[index];
			set
			{
				if (value.Key is null)
				{
					throw new ArgumentNullException(nameof(value), string.Format(CultureInfo.CurrentCulture, Strings.ValueIsNull, nameof(value.Key)));
				}

				if (value.Value is null)
				{
					throw new ArgumentNullException(nameof(value), string.Format(CultureInfo.CurrentCulture, Strings.ValueIsNull, nameof(value.Value)));
				}

				_values[index] = value;
			}
		}

		/// <summary>
		/// Adds a pair to the object.
		/// </summary>
		/// <param name="item">The pair to add to the object.</param>
		/// <exception cref="ArgumentNullException">The key or value is null.</exception>
		public void Add(JsonObjectPair item) => Insert(Count, item);

		/// <summary>
		/// Adds a pair to the object.
		/// </summary>
		/// <param name="key">The key to add to the object.</param>
		/// <param name="value">The value to add to the object.</param>
		/// <exception cref="ArgumentNullException">The key or value is null.</exception>
		public void Add(JsonString key, JsonElement value) => Add(new JsonObjectPair(key, value));

		/// <summary>
		/// Removes all elements from the object.
		/// </summary>
		public void Clear() => _values.Clear();

		/// <summary>
		/// Determines whether the object contains a specific pair.
		/// </summary>
		/// <param name="item">The pair to locate in the object.</param>
		/// <returns>True if the pair is found in the object; otherwise false.</returns>
		public bool Contains(JsonObjectPair item) => _values.Contains(item);

		/// <summary>
		/// Determines whether the object contains a specific key.
		/// </summary>
		/// <param name="key">The pair to locate in the object.</param>
		/// <param name="comparisonType">One of the enumeration values that specifies how the strings will be compared.</param>
		/// <returns>True if the key is found in the object; otherwise false.</returns>
		public bool Contains(string key, StringComparison comparisonType = StringComparison.Ordinal)
		{
			for (int i = 0; i < Count; i++)
			{
				if (_values[i].Key.Value.Equals(key, comparisonType))
				{
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Copies the pairs of the object to a System.Array, starting at a particular System.Array index.
		/// </summary>
		/// <param name="array">The one-dimensional System.Array that is the destination of the elements copied from the array. The System.Array must have zero-based indexing.</param>
		/// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
		/// <exception cref="ArgumentNullException">The destination array is null.</exception>
		/// <exception cref="ArgumentOutOfRangeException">The array index is less than zero.</exception>
		/// <exception cref="ArgumentException">The number of elements in the source array is greater than the available space from arrayIndex to the end of the destination array.</exception>
		public void CopyTo(JsonObjectPair[] array, int arrayIndex) => _values.CopyTo(array, arrayIndex);

		/// <summary>
		/// Returns an enumerator that iterates through the elements.
		/// </summary>
		/// <returns>An enumerator that can be used to iterate through the elements.</returns>
		public IEnumerator<JsonObjectPair> GetEnumerator() => _values.GetEnumerator();

		/// <summary>
		/// Returns an enumerator that iterates through the elements.
		/// </summary>
		/// <returns>An enumerator that can be used to iterate through the elements.</returns>
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		/// <summary>
		/// Get the only value with an equivalent key.
		/// </summary>
		/// <param name="key">The value of a JSON string (not the JSON representation).</param>
		/// <param name="comparisonType">One of the enumeration values that specifies how the strings will be compared.</param>
		/// <returns>The value for the given key.</returns>
		/// <exception cref="KeyNotFoundException">The key doesn't exist.</exception>
		/// <exception cref="ArgumentException">The key has more than one value.</exception>
		public JsonElement GetValue(string key, StringComparison comparisonType = StringComparison.Ordinal)
		{
			return GetTypedValue<JsonElement>(key, comparisonType);
		}

		/// <summary>
		/// Get an array that is the only value with an equivalent key.
		/// </summary>
		/// <param name="key">The value of a JSON string (not the JSON representation).</param>
		/// <param name="comparisonType">One of the enumeration values that specifies how the strings will be compared.</param>
		/// <returns>The array that is the only value with an equivalent key.</returns>
		/// <exception cref="KeyNotFoundException">The key doesn't exist.</exception>
		/// <exception cref="ArgumentException">The key has more than one value.</exception>
		/// <exception cref="InvalidCastException">The value was not an array.</exception>
		public JsonArray GetArray(string key, StringComparison comparisonType = StringComparison.Ordinal)
		{
			return GetTypedValue<JsonArray>(key, comparisonType);
		}

		/// <summary>
		/// Get a boolean that is the only value with an equivalent key.
		/// </summary>
		/// <param name="key">The value of a JSON string (not the JSON representation).</param>
		/// <param name="comparisonType">One of the enumeration values that specifies how the strings will be compared.</param>
		/// <returns>The boolean that is the only value with an equivalent key.</returns>
		/// <exception cref="KeyNotFoundException">The key doesn't exist.</exception>
		/// <exception cref="ArgumentException">The key has more than one value.</exception>
		/// <exception cref="InvalidCastException">The value was not a boolean.</exception>
		public JsonBoolean GetBoolean(string key, StringComparison comparisonType = StringComparison.Ordinal)
		{
			return GetTypedValue<JsonBoolean>(key, comparisonType);
		}

		/// <summary>
		/// Get a null element that is the only value with an equivalent key.
		/// </summary>
		/// <param name="key">The value of a JSON string (not the JSON representation).</param>
		/// <param name="comparisonType">One of the enumeration values that specifies how the strings will be compared.</param>
		/// <returns>The null element that is the only value with an equivalent key.</returns>
		/// <exception cref="KeyNotFoundException">The key doesn't exist.</exception>
		/// <exception cref="ArgumentException">The key has more than one value.</exception>
		/// <exception cref="InvalidCastException">The value was not a null element.</exception>
		public JsonNull GetNull(string key, StringComparison comparisonType = StringComparison.Ordinal)
		{
			return GetTypedValue<JsonNull>(key, comparisonType);
		}

		/// <summary>
		/// Get a number that is the only value with an equivalent key.
		/// </summary>
		/// <param name="key">The value of a JSON string (not the JSON representation).</param>
		/// <param name="comparisonType">One of the enumeration values that specifies how the strings will be compared.</param>
		/// <returns>The number that is the only value with an equivalent key.</returns>
		/// <exception cref="KeyNotFoundException">The key doesn't exist.</exception>
		/// <exception cref="ArgumentException">The key has more than one value.</exception>
		/// <exception cref="InvalidCastException">The value was not a number.</exception>
		public JsonNumber GetNumber(string key, StringComparison comparisonType = StringComparison.Ordinal)
		{
			return GetTypedValue<JsonNumber>(key, comparisonType);
		}

		/// <summary>
		/// Get an object that is the only value with an equivalent key.
		/// </summary>
		/// <param name="key">The value of a JSON string (not the JSON representation).</param>
		/// <param name="comparisonType">One of the enumeration values that specifies how the strings will be compared.</param>
		/// <returns>The object that is the only value with an equivalent key.</returns>
		/// <exception cref="KeyNotFoundException">The key doesn't exist.</exception>
		/// <exception cref="ArgumentException">The key has more than one value.</exception>
		/// <exception cref="InvalidCastException">The value was not an object.</exception>
		public JsonObject GetObject(string key, StringComparison comparisonType = StringComparison.Ordinal)
		{
			return GetTypedValue<JsonObject>(key, comparisonType);
		}

		/// <summary>
		/// Get a string that is the only value with an equivalent key.
		/// </summary>
		/// <param name="key">The value of a JSON string (not the JSON representation).</param>
		/// <param name="comparisonType">One of the enumeration values that specifies how the strings will be compared.</param>
		/// <returns>The string that is the only value with an equivalent key.</returns>
		/// <exception cref="KeyNotFoundException">The key doesn't exist.</exception>
		/// <exception cref="ArgumentException">The key has more than one value.</exception>
		/// <exception cref="InvalidCastException">The value was not a string.</exception>
		public JsonString GetString(string key, StringComparison comparisonType = StringComparison.Ordinal)
		{
			return GetTypedValue<JsonString>(key, comparisonType);
		}

		/// <summary>
		/// Determines the index of a specific pair in the object.
		/// </summary>
		/// <param name="item">The pair to locate in the object.</param>
		/// <returns>The index of the pair if found in the array; otherwise, -1.</returns>
		public int IndexOf(JsonObjectPair item) => _values.IndexOf(item);

		/// <summary>
		/// Determines the index of a specific key in the object.
		/// </summary>
		/// <param name="key">The key to locate in the object.</param>
		/// <param name="comparisonType">One of the enumeration values that specifies how the strings will be compared.</param>
		/// <returns>The index of the key if found in the array; otherwise, -1.</returns>
		public int IndexOf(string key, StringComparison comparisonType = StringComparison.Ordinal)
		{
			for (int i = 0; i < Count; i++)
			{
				if (_values[i].Key.Value.Equals(key, comparisonType))
				{
					return i;
				}
			}

			return -1;
		}

		/// <summary>
		/// Inserts a pair to the object at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index at which item should be inserted.</param>
		/// <param name="item">The pair to insert into the object.</param>
		/// <exception cref="ArgumentOutOfRangeException">Index is not a valid index in the object.</exception>
		/// <exception cref="ArgumentNullException">The key or value is null.</exception>
		public void Insert(int index, JsonObjectPair item)
		{
			if (item.Key is null)
			{
				throw new ArgumentNullException(nameof(item), string.Format(CultureInfo.CurrentCulture, Strings.ValueIsNull, nameof(item.Key)));
			}

			if (item.Value is null)
			{
				throw new ArgumentNullException(nameof(item), string.Format(CultureInfo.CurrentCulture, Strings.ValueIsNull, nameof(item.Value)));
			}

			_values.Insert(index, item);
		}

		/// <summary>
		/// Inserts a pair to the object at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index at which item should be inserted.</param>
		/// <param name="key">The key to insert into the object.</param>
		/// <param name="value">The value to insert into the object.</param>
		/// <exception cref="ArgumentOutOfRangeException">Index is not a valid index in the array.</exception>
		/// <exception cref="ArgumentNullException">The key or value is null.</exception>
		public void Insert(int index, JsonString key, JsonElement value)
		{
			Insert(index, new JsonObjectPair(key, value));
		}

		/// <summary>
		/// Removes the first occurrence of a specific pair from the object.
		/// </summary>
		/// <param name="item">The pair to remove.</param>
		/// <returns>True if the pair was successfully removed from the object; otherwise, false. This method also returns false if the pair is not found in the original object.</returns>
		public bool Remove(JsonObjectPair item) => _values.Remove(item);

		/// <summary>
		/// Removes the first occurrence of a specific key from the object.
		/// </summary>
		/// <param name="key">The key to remove.</param>
		/// <param name="comparisonType">One of the enumeration values that specifies how the strings will be compared.</param>
		/// <returns>True if the key was successfully removed from the object; otherwise, false. This method also returns false if the key is not found in the original object.</returns>
		public bool Remove(string key, StringComparison comparisonType = StringComparison.Ordinal)
		{
			for (int i = 0; i < Count; i++)
			{
				if (_values[i].Key.Value.Equals(key, comparisonType))
				{
					_values.RemoveAt(i);
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Removes the pair at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index of the pair to remove.</param>
		/// <exception cref="ArgumentOutOfRangeException">Index is not a valid index in the array.</exception>
		public void RemoveAt(int index) => _values.RemoveAt(index);

		/// <summary>
		/// Write the object, whitespace, and elements within it, as JSON to the stream.
		/// </summary>
		/// <param name="writer">The writer to write to.</param>
		/// <exception cref="ObjectDisposedException">The writer is closed.</exception>
		/// <exception cref="IOException">An I/O error occurs.</exception>
		public override void Serialize(TextWriter writer)
		{
			if (writer is null)
			{
				throw new ArgumentNullException(nameof(writer));
			}

			writer.Write(Leading.Value);
			writer.Write("{");
			if (_values.Count == 0)
			{
				writer.Write(EmptyWhitespace.Value);
			}
			else
			{
				bool first = true;
				foreach (JsonObjectPair entry in _values)
				{
					if (!first)
					{
						writer.Write(",");
					}
					first = false;

					entry.Key.Serialize(writer);
					writer.Write(":");
					entry.Value.Serialize(writer);
				}
			}
			writer.Write("}");
			writer.Write(Trailing.Value);
		}

		/// <summary>
		/// Set the value of a specific key.
		/// </summary>
		/// <param name="key">The key, which must exist already.</param>
		/// <param name="value">The value to set.</param>
		/// <param name="comparisonType">One of the enumeration values that specifies how the strings will be compared.</param>
		/// <exception cref="KeyNotFoundException">The key does not exist.</exception>
		/// <exception cref="ArgumentException">More than one instance of the key exists.</exception>
		public void SetValue(string key, JsonElement value, StringComparison comparisonType = StringComparison.Ordinal)
		{
			int index;
			bool unique = TryGetIndex(key, out index, comparisonType);
			if (index < 0)
			{
				throw new KeyNotFoundException();
			}
			if (!unique)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Strings.KeyNotUnique, key), nameof(key));
			}

			_values[index] = new JsonObjectPair(_values[index].Key, value);
		}

		/// <summary>
		/// Get the first value, if any, with an equivalent key.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key">The value of a JSON string (not the JSON representation).</param>
		/// <param name="value">The first value with a matching key.</param>
		/// <param name="comparisonType">One of the enumeration values that specifies how the strings will be compared.</param>
		/// <returns>Whether exactly one pair was found with the given key.</returns>
		/// <exception cref="ArgumentNullException">key is null.</exception>
		/// <exception cref="InvalidCastException">The value is not the right type.</exception>
		public bool TryGetValue<T>(string key, out T value, StringComparison comparisonType = StringComparison.Ordinal)
			where T : JsonElement
		{
			if (key is null)
			{
				throw new ArgumentNullException(nameof(key));
			}

			value = null;

			foreach (JsonObjectPair pair in _values)
			{
				if (pair.Key.Value.Equals(key, comparisonType))
				{
					if (value is null)
					{
						value = (T)pair.Value;
					}
					else
					{
						return false;
					}
				}
			}

			return value != null;
		}

		/// <summary>
		/// Get the index of the first pair, if any, with an equivalent key.
		/// </summary>
		/// <param name="key"></param>
		/// <param name="index"></param>
		/// <returns>Whether exactly one pair was found with the given key.</returns>
		/// <exception cref="ArgumentNullException">Key is null.</exception>
		public bool TryGetIndex(string key, out int index) => TryGetIndex(key, out index, StringComparison.Ordinal);

		/// <summary>
		/// Get the index of the first pair, if any, with an equivalent key.
		/// </summary>
		/// <param name="key"></param>
		/// <param name="index"></param>
		/// <param name="comparisonType">One of the enumeration values that specifies how the strings will be compared.</param>
		/// <returns>Whether exactly one pair was found with the given key.</returns>
		/// <exception cref="ArgumentNullException">Key is null.</exception>
		public bool TryGetIndex(string key, out int index, StringComparison comparisonType = StringComparison.Ordinal)
		{
			if (key is null)
			{
				throw new ArgumentNullException(nameof(key));
			}

			index = -1;

			for (int i = 0; i < Count; i++)
			{
				if (_values[i].Key.Value.Equals(key, comparisonType))
				{
					if (index == -1)
					{
						index = i;
					}
					else
					{
						return false;
					}
				}
			}

			return index >= 0;
		}

		/// <summary>
		/// Get the only value with an equivalent key.
		/// </summary>
		/// <param name="key">The value of a JSON string (not the JSON representation).</param>
		/// <param name="comparisonType">One of the enumeration values that specifies how the strings will be compared.</param>
		/// <returns>The value for the given key.</returns>
		/// <exception cref="KeyNotFoundException">The key doesn't exist.</exception>
		/// <exception cref="ArgumentException">The key has more than one value.</exception>
		/// <exception cref="InvalidCastException">The value was not of the requested type.</exception>
		T GetTypedValue<T>(string key, StringComparison comparisonType = StringComparison.Ordinal) where T : JsonElement
		{
			JsonElement retval;
			bool unique = TryGetValue(key, out retval, comparisonType);
			if (retval is null)
			{
				throw new KeyNotFoundException();
			}
			if (!unique)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Strings.KeyNotUnique, key), nameof(key));
			}
			return (T)retval;
		}
	}
}
