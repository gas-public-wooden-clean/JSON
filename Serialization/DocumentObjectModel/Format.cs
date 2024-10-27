using CER.Json.Stream;
using System;

namespace CER.Json.DocumentObjectModel
{
	/// <summary>
	/// Whitespace format rules.
	/// </summary>
	public interface IFormat
	{
		/// <summary>
		/// Format an element's whitespace according to the rules.
		/// </summary>
		/// <param name="element">The element to format.</param>
		/// <param name="depth">The element's depth in the tree. Keys and values both have a depth of one
		/// more than the containing object.</param>
		/// <param name="isKey">Whether the element is a key in an object.</param>
		/// <param name="isValue">Whether the element is a value in an object.</param>
		/// <param name="isLastItem">Whether the element is the last item in an array or object. For
		/// objects, both the last key and last value will have this set.</param>
		/// <exception cref="ArgumentNullException">The element is null.</exception>
		void Format(JsonElement element, int depth, bool isKey, bool isValue, bool isLastItem);
	}

	/// <summary>
	/// Default format.
	/// </summary>
	public class DefaultFormat : IFormat
	{
		/// <summary>
		/// Create a default formatter.
		/// </summary>
		/// <param name="newLine">The newline string. If null, Environment.NewLine is used.</param>
		/// <param name="indent">The indent string</param>
		/// <param name="afterKey">The whitespace to insert after a key in an object (before the
		/// colon). If null, an empty string is used.</param>
		/// <param name="beforeValue">The whitespace to insert before a value in an object (after
		/// the colon). If null, a single space is used.</param>
		public DefaultFormat(string newLine = null, string indent = "  ", Whitespace afterKey = null, Whitespace beforeValue = null)
		{
			_newLine = newLine ?? Environment.NewLine;
			_indent = indent;
			_afterKey = afterKey ?? Whitespace.Empty;
			_beforeValue = beforeValue ?? new Whitespace(" ");
		}

		readonly string _newLine;
		readonly string _indent;
		readonly Whitespace _afterKey;
		readonly Whitespace _beforeValue;

		/// <summary>
		/// Format an element's whitespace according to the rules.
		/// </summary>
		/// <param name="element">The element to format.</param>
		/// <param name="depth">The element's depth in the tree. Keys and values both have a depth of one
		/// more than the containing object.</param>
		/// <param name="isKey">Whether the element is a key in an object.</param>
		/// <param name="isValue">Whether the element is a value in an object.</param>
		/// <param name="isLastItem">Whether the element is the last item in an array or object. For
		/// objects, both the last key and last value will have this set.</param>
		/// <exception cref="ArgumentNullException">The element is null.</exception>
		public void Format(JsonElement element, int depth, bool isKey, bool isValue, bool isLastItem)
		{
			if (element is null)
			{
				throw new ArgumentNullException(nameof(element));
			}

			if (isValue)
			{
				element.Leading = _beforeValue;
			}
			else
			{
				string whitespace = depth > 0 ? _newLine : "";
				for (int i = 0; i < depth; i++)
				{
					whitespace += _indent;
				}
				element.Leading = new Whitespace(whitespace);
			}

			if (isKey)
			{
				element.Trailing = _afterKey;
			}
			else if (isLastItem)
			{
				string whitespace = _newLine;
				for (int i = 0; i < depth - 1; i++)
				{
					whitespace += _indent;
				}
				element.Trailing = new Whitespace(whitespace);
			}
			else
			{
				element.Trailing = Whitespace.Empty;
			}

			if (element is JsonArray array)
			{
				array.EmptyWhitespace = Whitespace.Empty;
			}
			else if (element is JsonObject obj)
			{
				obj.EmptyWhitespace = Whitespace.Empty;
			}
		}
	}
}
