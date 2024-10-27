using System;
using System.Diagnostics;

namespace CER.Json.DocumentObjectModel
{
	/// <summary>
	/// An object pair, consisting of a string key and a corresponding value.
	/// </summary>
	[DebuggerDisplay("{Key}:{Value}")]
	public readonly struct JsonObjectPair
	{
		/// <summary>
		/// Create an object pair with the given key and value.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value.</param>
		public JsonObjectPair(JsonString key, JsonElement value)
		{
			Key = key ?? throw new ArgumentNullException(nameof(key));
			Value = value ?? throw new ArgumentNullException(nameof(value));
		}

		/// <summary>
		/// The key.
		/// </summary>
		public JsonString Key
		{
			get;
		}

		/// <summary>
		/// The value.
		/// </summary>
		public JsonElement Value
		{
			get;
		}

		/// <summary>
		/// Determines whether the specified object is equal to the current object. Two JsonObjectPairs are equal if their key and value references are equal.
		/// </summary>
		/// <param name="obj">The object to compare with the current object.</param>
		/// <returns>True if the specified object is equal to the current object; otherwise, false.</returns>
		public override bool Equals(object obj)
		{
			if (!(obj is JsonObjectPair pair))
			{
				return false;
			}

			return object.ReferenceEquals(pair.Key, Key) &&
				object.ReferenceEquals(pair.Value, Value);
		}

		/// <summary>
		/// Returns the hash code for this instance.
		/// </summary>
		/// <returns>A 32-bit signed integer hash code.</returns>
		public override int GetHashCode()
		{
			int hashCode = 0;

			if (!(Key is null))
			{
				hashCode ^= Key.GetHashCode();
			}

			if (!(Value is null))
			{
				hashCode ^= Value.GetHashCode();
			}

			return hashCode;
		}

		/// <summary>
		/// Determines whether the values are equal.
		/// </summary>
		/// <param name="left">Left operand.</param>
		/// <param name="right">Right operand.</param>
		/// <returns>Whether the values are equal.</returns>
		public static bool operator ==(JsonObjectPair left, JsonObjectPair right) => left.Equals(right);

		/// <summary>
		/// Determines whether the values are not equal.
		/// </summary>
		/// <param name="left">Left operand.</param>
		/// <param name="right">Right operand.</param>
		/// <returns>Whether the values are not equal.</returns>
		public static bool operator !=(JsonObjectPair left, JsonObjectPair right) => !left.Equals(right);
	}
}
