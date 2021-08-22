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
		/// Adds an element to the array.
		/// </summary>
		/// <param name="item">The element to add to the array.</param>
		/// <exception cref="ArgumentNullException">Element is null.</exception>
		public void Add(JsonElement item)
		{
			Insert(Count, item);
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
		/// Removes all elements from the array.
		/// </summary>
		public void Clear()
		{
			_values.Clear();
		}

		/// <summary>
		/// Removes the first occurrence of a specific element from the array.
		/// </summary>
		/// <param name="item"></param>
		/// <returns>true if the element was successfully removed from the array; otherwise, false. This method also returns false if the element is not found in the original array.</returns>
		public bool Remove(JsonElement item)
		{
			return _values.Remove(item);
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
	}
}
