using CER.Json.Stream;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace CER.Json.DocumentObjectModel
{
	/// <summary>
	/// A JSON array.
	/// </summary>
	public class JsonArray : JsonElement, IList<JsonElement>
	{
		/// <summary>
		/// Create an empty array with no leading, trailing, or contained whitespace.
		/// </summary>
		public JsonArray() : base()
		{
			_values = new List<JsonElement>();
			EmptyWhitespace = Whitespace.Empty;
		}

		Whitespace _emptyWhitespace;

		readonly IList<JsonElement> _values;

		/// <summary>
		/// Gets or sets the element at the specified index.
		/// </summary>
		/// <param name="index">The element index.</param>
		/// <returns>The element at the given index.</returns>
		/// <exception cref="ArgumentOutOfRangeException">Index is not a valid index.</exception>
		/// <exception cref="ArgumentNullException">The given value is null.</exception>
		public JsonElement this[int index]
		{
			get => _values[index];
			set => _values[index] = value;
		}

		/// <summary>
		/// Whitespace that will be written inside the array if there are no elements in it.
		/// </summary>
		/// <exception cref="ArgumentNullException">The value being set is null.</exception>
		public Whitespace EmptyWhitespace
		{
			get => _emptyWhitespace;
			set => _emptyWhitespace = value ?? throw new ArgumentNullException(nameof(value));
		}

		/// <summary>
		/// Get the number of elements contained in the array.
		/// </summary>
		/// <returns>The number of elements contained in the array.</returns>
		public int Count => _values.Count;

		/// <summary>
		/// Get whether the collection is read-only, which arrays are not.
		/// </summary>
		/// <returns>False.</returns>
		public bool IsReadOnly => false;

		/// <summary>
		/// Gets the string at the specified index.
		/// </summary>
		/// <param name="index">The element index.</param>
		/// <returns>The value of the JsonString at the specified index.</returns>
		/// <exception cref="ArgumentOutOfRangeException">Index is not a valid index.</exception>
		/// <exception cref="InvalidCastException">The element is not a JsonString.</exception>
		public string GetString(int index)
		{
			JsonString item = (JsonString)_values[index];
			return item.Value;
		}

		/// <summary>
		/// Sets the string at the specified index.
		/// </summary>
		/// <param name="index">The element index.</param>
		/// <param name="value">The value to set on the element.</param>
		/// <param name="throwInvalidCast">Whether to throw an exception if the element is not a JsonString. Otherwise it is replaced with a JsonString.</param>
		/// <exception cref="InvalidCastException">The element is not a JsonString and throwInvalidCast is true.</exception>
		/// <exception cref="ArgumentNullException">Value is null.</exception>
		public void SetString(int index, string value, bool throwInvalidCast = true)
		{
			if (_values[index] is JsonString item)
			{
				item.Value = value;
			}
			else
			{
				if (throwInvalidCast)
				{
					_values[index] = (JsonString)_values[index];
				}
				else
				{
					_values[index] = new JsonString(value);
				}
			}
		}

		/// <summary>
		/// Gets the boolean at the specified index.
		/// </summary>
		/// <param name="index">The element index.</param>
		/// <returns>The value of the JsonBoolean at the specified index.</returns>
		/// <exception cref="ArgumentOutOfRangeException">Index is not a valid index.</exception>
		/// <exception cref="InvalidCastException">The element is not a JsonBoolean.</exception>
		public bool GetBoolean(int index)
		{
			JsonBoolean item = (JsonBoolean)_values[index];
			return item.Value;
		}

		/// <summary>
		/// Sets the boolean at the specified index.
		/// </summary>
		/// <param name="index">The element index.</param>
		/// <param name="value">The value to set on the element.</param>
		/// <param name="throwInvalidCast">Whether to throw an exception if the element is not a JsonBoolean. Otherwise it is replaced with a JsonBoolean.</param>
		/// <exception cref="InvalidCastException">The element is not a JsonBoolean and throwInvalidCast is true.</exception>
		public void SetBoolean(int index, bool value, bool throwInvalidCast = true)
		{
			if (_values[index] is JsonBoolean item)
			{
				item.Value = value;
			}
			else
			{
				if (throwInvalidCast)
				{
					_values[index] = (JsonBoolean)_values[index];
				}
				else
				{
					_values[index] = new JsonBoolean(value);
				}
			}
		}

		/// <summary>
		/// Gets the number at the specified index.
		/// </summary>
		/// <param name="index">The element index.</param>
		/// <returns>The value of the JsonNumber at the specified index.</returns>
		/// <exception cref="ArgumentOutOfRangeException">Index is not a valid index.</exception>
		/// <exception cref="InvalidCastException">The element is not a JsonNumber.</exception>
		/// <exception cref="OverflowException">The value being retrieved is less than System.Decimal.MinValue or greater than System.Decimal.MaxValue.</exception>
		public decimal GetDecimal(int index)
		{
			JsonNumber item = (JsonNumber)_values[index];
			return item.Value;
		}

		/// <summary>
		/// Sets the number at the specified index.
		/// </summary>
		/// <param name="index">The element index.</param>
		/// <param name="value">The value to set on the element.</param>
		/// <param name="throwInvalidCast">Whether to throw an exception if the element is not a JsonNumber. Otherwise it is replaced with a JsonNumber.</param>
		/// <exception cref="InvalidCastException">The element is not a JsonNumber and throwInvalidCast is true.</exception>
		public void SetDecimal(int index, decimal value, bool throwInvalidCast = true)
		{
			if (_values[index] is JsonNumber item)
			{
				item.Value = value;
			}
			else
			{
				if (throwInvalidCast)
				{
					_values[index] = (JsonNumber)_values[index];
				}
				else
				{
					_values[index] = new JsonNumber(value);
				}
			}
		}

		/// <summary>
		/// Adds an element to the array.
		/// </summary>
		/// <param name="item">The element to add to the array.</param>
		/// <exception cref="ArgumentNullException">Element is null.</exception>
		public void Add(JsonElement item)
		{
			Insert(Count, item);
		}

		/// <summary>
		/// Adds a JsonString with the specified value to the array.
		/// </summary>
		/// <param name="value">The value to assign to a new JsonString before adding it to the array.</param>
		/// <exception cref="ArgumentNullException">Value is null.</exception>
		public void Add(string value)
		{
			Insert(Count, value);
		}

		/// <summary>
		/// Adds a JsonBoolean with the specified value to the array.
		/// </summary>
		/// <param name="value">The value to assign to a new JsonBoolean before adding it to the array.</param>
		public void Add(bool value)
		{
			Insert(Count, value);
		}

		/// <summary>
		/// Adds a JsonNumber with the specified value to the array.
		/// </summary>
		/// <param name="value">The value to assign to a new JsonNumber before adding it to the array.</param>
		public void Add(int value)
		{
			Insert(Count, value);
		}

		/// <summary>
		/// Adds a JsonNull to the array.
		/// </summary>
		public void AddNull()
		{
			InsertNull(Count);
		}

		/// <summary>
		/// Inserts an element to the array at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index at which item should be inserted.</param>
		/// <param name="item">The element to insert into the array.</param>
		/// <exception cref="ArgumentOutOfRangeException">Index is not a valid index in the array.</exception>
		/// <exception cref="ArgumentNullException">Element is null.</exception>
		public void Insert(int index, JsonElement item)
		{
			if (item is null)
			{
				throw new ArgumentNullException(nameof(item));
			}

			_values.Insert(index, item);
		}

		/// <summary>
		/// Inserts a JsonString with the specified value at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index at which item should be inserted.</param>
		/// <param name="value">The value to assign to a new JsonString before inserting it into the array.</param>
		/// <exception cref="ArgumentOutOfRangeException">Index is not a valid index in the array.</exception>
		/// <exception cref="ArgumentNullException">Value is null.</exception>
		public void Insert(int index, string value)
		{
			if (value is null)
			{
				throw new ArgumentNullException(nameof(value));
			}

			_values.Insert(index, new JsonString(value));
		}

		/// <summary>
		/// Inserts a JsonBoolean with the specified value at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index at which item should be inserted.</param>
		/// <param name="value">The value to assign to a new JsonBoolean before inserting it into the array.</param>
		/// <exception cref="ArgumentOutOfRangeException">Index is not a valid index in the array.</exception>
		public void Insert(int index, bool value)
		{
			_values.Insert(index, new JsonBoolean(value));
		}

		/// <summary>
		/// Inserts a JsonNumber with the specified value at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index at which item should be inserted.</param>
		/// <param name="value">The value to assign to a new JsonNumber before inserting it into the array.</param>
		/// <exception cref="ArgumentOutOfRangeException">Index is not a valid index in the array.</exception>
		public void Insert(int index, int value)
		{
			_values.Insert(index, new JsonNumber(value));
		}

		/// <summary>
		/// Inserts a JsonNull at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index at which item should be inserted.</param>
		/// <exception cref="ArgumentOutOfRangeException">Index is not a valid index in the array.</exception>
		public void InsertNull(int index)
		{
			_values.Insert(index, new JsonNull());
		}

		/// <summary>
		/// Removes all elements from the array.
		/// </summary>
		public void Clear()
		{
			_values.Clear();
		}

		/// <summary>
		/// Removes the first occurrence of a specific element from the array.
		/// </summary>
		/// <param name="item">The object to remove from the array.</param>
		/// <returns>true if the element was successfully removed from the array; otherwise, false. This method also returns false if the element is not found in the original array.</returns>
		public bool Remove(JsonElement item)
		{
			return _values.Remove(item);
		}

		/// <summary>
		/// Removes the first occurrence of a JsonString with a specific value from the array.
		/// </summary>
		/// <param name="item">The JsonString value to remove from the array.</param>
		/// <param name="comparisonType">One of the enumeration values that specifies how the strings will be compared.</param>
		/// <returns>true if the element was successfully removed from the array; otherwise, false. This method also returns false if the element is not found in the original array.</returns>
		public bool Remove(string item, StringComparison comparisonType = StringComparison.Ordinal)
		{
			int index = IndexOf(item, comparisonType);
			if (index < 0)
			{
				return false;
			}

			RemoveAt(index);
			return true;
		}

		/// <summary>
		/// Removes the first occurrence of a JsonBoolean with a specific value from the array.
		/// </summary>
		/// <param name="item">The JsonBoolean value to remove from the array.</param>
		/// <returns>true if the element was successfully removed from the array; otherwise, false. This method also returns false if the element is not found in the original array.</returns>
		public bool Remove(bool item)
		{
			int index = IndexOf(item);
			if (index < 0)
			{
				return false;
			}

			RemoveAt(index);
			return true;
		}

		/// <summary>
		/// Removes the first occurrence of a JsonNumber with a specific value from the array.
		/// </summary>
		/// <param name="item">The JsonNumber value to remove from the array.</param>
		/// <returns>true if the element was successfully removed from the array; otherwise, false. This method also returns false if the element is not found in the original array.</returns>
		public bool Remove(int item)
		{
			int index = IndexOf(item);
			if (index < 0)
			{
				return false;
			}

			RemoveAt(index);
			return true;
		}

		/// <summary>
		/// Removes the first occurrence of a JsonNull from the array.
		/// </summary>
		/// <returns>true if the element was successfully removed from the array; otherwise, false. This method also returns false if the element is not found in the original array.</returns>
		public bool RemoveNull()
		{
			int index = IndexOfNull();
			if (index < 0)
			{
				return false;
			}

			RemoveAt(index);
			return true;
		}

		/// <summary>
		/// Returns an enumerator that iterates through the elements.
		/// </summary>
		/// <returns>An enumerator that can be used to iterate through the elements.</returns>
		public IEnumerator<JsonElement> GetEnumerator()
		{
			return _values.GetEnumerator();
		}

		/// <summary>
		/// Returns an enumerator that iterates through the elements.
		/// </summary>
		/// <returns>An enumerator that can be used to iterate through the elements.</returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		/// <summary>
		/// Determines whether the array contains a specific element.
		/// </summary>
		/// <param name="item">The element to locate in the array.</param>
		/// <returns>True if the element is found in the array; otherwise false.</returns>
		public bool Contains(JsonElement item) => _values.Contains(item);

		/// <summary>
		/// Determines whether the array contains a JsonString with a specific value.
		/// </summary>
		/// <param name="item">The JsonString value to locate in the array.</param>
		/// <param name="comparisonType">One of the enumeration values that specifies how the strings will be compared.</param>
		/// <returns>True if the value is found in the array; otherwise false.</returns>
		public bool Contains(string item, StringComparison comparisonType = StringComparison.Ordinal)
		{
			if (item is null)
			{
				return false;
			}

			return IndexOf(item, comparisonType) >= 0;
		}

		/// <summary>
		/// Determines whether the array contains a JsonBoolean with a specific value.
		/// </summary>
		/// <param name="item">The JsonBoolean value to locate in the array.</param>
		/// <returns>True if the value is found in the array; otherwise false.</returns>
		public bool Contains(bool item)
		{
			return IndexOf(item) >= 0;
		}

		/// <summary>
		/// Determines whether the array contains a JsonNumber with a specific value.
		/// </summary>
		/// <param name="item">The JsonNumber value to locate in the array.</param>
		/// <returns>True if the value is found in the array; otherwise false.</returns>
		public bool Contains(int item)
		{
			return IndexOf(item) >= 0;
		}

		/// <summary>
		/// Determines whether the array contains a JsonNull.
		/// </summary>
		/// <returns>True if a JsonNull is found in the array; otherwise false.</returns>
		public bool ContainsNull()
		{
			return IndexOfNull() >= 0;
		}

		/// <summary>
		/// Copies the elements of the array to a System.Array, starting at a particular System.Array index.
		/// </summary>
		/// <param name="array">The one-dimensional System.Array that is the destination of the elements copied from the array. The System.Array must have zero-based indexing.</param>
		/// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
		/// <exception cref="ArgumentNullException">The destination array is null.</exception>
		/// <exception cref="ArgumentOutOfRangeException">The array index is less than zero.</exception>
		/// <exception cref="ArgumentException">The number of elements in the source array is greater than the available space from arrayIndex to the end of the destination array.</exception>
		public void CopyTo(JsonElement[] array, int arrayIndex) => _values.CopyTo(array, arrayIndex);

		/// <summary>
		/// Determines the index of a specific element in the array.
		/// </summary>
		/// <param name="item">The element to locate in the array.</param>
		/// <returns>The index of the element if found in the array; otherwise, -1.</returns>
		/// <exception cref="ArgumentNullException">The element to locate is null.</exception>
		public int IndexOf(JsonElement item)
		{
			if (item is null)
			{
				throw new ArgumentNullException(nameof(item));
			}

			return _values.IndexOf(item);
		}

		/// <summary>
		/// Determines the index of the first JsonString with a specific value in the array.
		/// </summary>
		/// <param name="item">The JsonString value to locate in the array.</param>
		/// <param name="comparisonType">One of the enumeration values that specifies how the strings will be compared.</param>
		/// <returns>The index of the element if found in the array; otherwise, -1.</returns>
		/// <exception cref="ArgumentNullException">The element to locate is null.</exception>
		public int IndexOf(string item, StringComparison comparisonType = StringComparison.Ordinal)
		{
			if (item is null)
			{
				throw new ArgumentNullException(nameof(item));
			}

			for (int i = 0; i < _values.Count; i++)
			{
				if (_values[i] is JsonString value && value.Value.Equals(item, comparisonType))
				{
					return i;
				}
			}

			return -1;
		}

		/// <summary>
		/// Determines the index of the first JsonBoolean with a specific value in the array.
		/// </summary>
		/// <param name="item">The JsonBoolean value to locate in the array.</param>
		/// <returns>The index of the element if found in the array; otherwise, -1.</returns>
		public int IndexOf(bool item)
		{
			for (int i = 0; i < _values.Count; i++)
			{
				if (_values[i] is JsonBoolean value && value.Value == item)
				{
					return i;
				}
			}

			return -1;
		}

		/// <summary>
		/// Determines the index of the first JsonNumber with a specific value in the array.
		/// </summary>
		/// <param name="item">The JsonNumber value to locate in the array.</param>
		/// <returns>The index of the element if found in the array; otherwise, -1.</returns>
		public int IndexOf(int item)
		{
			for (int i = 0; i < _values.Count; i++)
			{
				if (_values[i] is JsonNumber value && value.Value == item)
				{
					return i;
				}
			}

			return -1;
		}

		/// <summary>
		/// Determines the index of the first JsonNull in the array.
		/// </summary>
		/// <returns>The index of the element if found in the array; otherwise, -1.</returns>
		public int IndexOfNull()
		{
			for (int i = 0; i < _values.Count; i++)
			{
				if (_values[i] is JsonNull)
				{
					return i;
				}
			}

			return -1;
		}

		/// <summary>
		/// Removes the element at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index of the item to remove.</param>
		/// <exception cref="ArgumentOutOfRangeException">Index is not a valid index in the array.</exception>
		public void RemoveAt(int index) => _values.RemoveAt(index);

		/// <summary>
		/// Write the array, whitespace, and elements within it, as JSON to the stream.
		/// </summary>
		/// <param name="writer">The writer to write to.</param>
		/// <exception cref="ObjectDisposedException">The writer is closed.</exception>
		/// <exception cref="IOException">An I/O error occurs.</exception>
		/// <exception cref="ArgumentNullException">The writer is null.</exception>
		public override void Serialize(TextWriter writer)
		{
			if (writer is null)
			{
				throw new ArgumentNullException(nameof(writer));
			}

			writer.Write(Leading.Value);
			writer.Write("[");
			if (_values.Count == 0)
			{
				writer.Write(EmptyWhitespace.Value);
			}
			else
			{
				bool first = true;
				foreach (JsonElement element in _values)
				{
					if (!first)
					{
						writer.Write(",");
					}
					first = false;

					element.Serialize(writer);
				}
			}
			writer.Write("]");
			writer.Write(Trailing.Value);
		}

		/// <summary>
		/// Get an enumerable view of the elements as a specific type.
		/// </summary>
		/// <typeparam name="T">The type of element to cast to.</typeparam>
		/// <returns>An enumerable view of the elements as a specific type. It will throw InvalidCastExceptions if it encounters elements of another type.</returns>
		public IEnumerable<T> GetTypedElements<T>()
			where T : JsonElement
		{
			foreach (JsonElement element in _values)
			{
				yield return (T)element;
			}
		}
	}
}
