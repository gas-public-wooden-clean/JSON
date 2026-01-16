using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CER.Json.DocumentObjectModel
{
	/// <summary>
	/// Path to a value in a document.
	/// </summary>
	public class JsonPath : IEnumerable<JsonString?>
	{
		public JsonPath()
		{
			Count = 0;
			_tail = null;
		}

		internal JsonPath(JsonPath previous, JsonString? current)
		{
			Count = previous.Count + 1;
			_tail = new JsonPathElement(previous._tail, current);
		}

		readonly JsonPathElement? _tail;

		public int Count { get; }

		public JsonString? this[int index]
		{
			get
			{
				ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(index, Count);
				JsonPathElement element = _tail!;
				for (int i = Count - 1; i > index; i--)
				{
					element = element.Previous!;
				}
				return element.Item;
			}
		}

		public bool StartsWith(IList<JsonString?> path, IEqualityComparer<JsonString> comparer = null)
		{
			ArgumentNullException.ThrowIfNull(path, nameof(path));

			if (path.Count > Count)
			{
				return false;
			}

			JsonPathElement element = _tail!;
			for (int i = Count - 1; i >= 0; element = element.Previous!, i--)
			{
				if (i >= path.Count)
				{
					continue;
				}

				if (!(comparer ?? JsonStringComparer.Ordinal).Equals(element.Item, path[i]))
				{
					return false;
				}
			}

			return true;
		}

		public bool StartsWith(IEnumerable<JsonString?> path, IEqualityComparer<JsonString> comparer = null)
		{
			ArgumentNullException.ThrowIfNull(path, nameof(path));

			int i = 0;
			foreach (var piece in path)
			{
				if (i >= Count)
				{
					return false;
				}

				if (!(comparer ?? JsonStringComparer.Ordinal).Equals(this[i++], piece))
				{
					return false;
				}
			}
			return true;
		}

		public IEnumerator<JsonString?> GetEnumerator()
		{
			for (int i = 0; i < Count; i++)
			{
				yield return this[i];
			}
		}

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		public override string ToString()
		{
			StringBuilder retval = new StringBuilder();
			for (JsonPathElement? element = _tail; element is not null; element = element.Previous)
			{
				if (retval.Length > 0)
				{
					retval.Insert(0, '/');
				}
				retval.Insert(0, element.Item?.Json);
			}
			return retval.ToString();
		}

		class JsonPathElement
		{
			public JsonPathElement(JsonPathElement? previous, JsonString? item)
			{
				Previous = previous;
				Item = item;
			}

			public JsonPathElement? Previous { get; }
			public JsonString? Item { get; }
		}
	}
}
