
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace CER.Json.DocumentObjectModel
{
	public class NativeStringComparer : IEqualityComparer<JsonString>
	{
		public NativeStringComparer(IEqualityComparer<string>? nativeComparer = null)
		{
			_comparer = nativeComparer ?? StringComparer.Ordinal;
		}

		public static readonly NativeStringComparer Ordinal = new NativeStringComparer(StringComparer.Ordinal);
		public static readonly NativeStringComparer OrdinalIgnoreCase = new NativeStringComparer(StringComparer.OrdinalIgnoreCase);
		public static readonly NativeStringComparer CurrentCulture = new NativeStringComparer(StringComparer.CurrentCulture);
		public static readonly NativeStringComparer CurrentCultureIgnoreCase = new NativeStringComparer(StringComparer.CurrentCultureIgnoreCase);
		public static readonly NativeStringComparer InvariantCulture = new NativeStringComparer(StringComparer.InvariantCulture);
		public static readonly NativeStringComparer InvariantCultureIgnoreCase = new NativeStringComparer(StringComparer.InvariantCultureIgnoreCase);

		readonly IEqualityComparer<string> _comparer;

		public bool Equals(JsonString? x, JsonString? y)
		{
			return _comparer.Equals(x?.Value, y?.Value);
		}

		public int GetHashCode([DisallowNull] JsonString obj)
		{
			ArgumentNullException.ThrowIfNull(obj, nameof(obj));
			return _comparer.GetHashCode(obj.Value);
		}
	}

	public class JsonStringComparer : IEqualityComparer<JsonString>
	{
		public JsonStringComparer(IEqualityComparer<string>? jsonComparer = null)
		{
			_comparer = jsonComparer ?? StringComparer.Ordinal;
		}

		public static readonly JsonStringComparer Ordinal = new JsonStringComparer(StringComparer.Ordinal);
		
		readonly IEqualityComparer<string> _comparer;

		public bool Equals(JsonString? x, JsonString? y)
		{
			return _comparer.Equals(x?.Value, y?.Value);
		}

		public int GetHashCode([DisallowNull] JsonString obj)
		{
			ArgumentNullException.ThrowIfNull(obj, nameof(obj));
			return _comparer.GetHashCode(obj.Value);
		}
	}
}