using CER.Json.Stream;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace CER.Json.DocumentObjectModel
{
	/// <summary>
	/// A JSON element.
	/// </summary>
	public abstract class JsonElement
	{
		/// <summary>
		/// Create an element with no leading or trailing whitespace.
		/// </summary>
		protected JsonElement()
		{
			Leading = Whitespace.Empty;
			Trailing = Whitespace.Empty;
		}

		/// <summary>
		/// Leading whitespace.
		/// </summary>
		/// <exception cref="ArgumentNullException">The given value is null.</exception>
		public Whitespace Leading
		{
			get;
			set => field = value ?? throw new ArgumentNullException(nameof(value));
		}

		/// <summary>
		/// Trailing whitespace.
		/// </summary>
		/// <exception cref="ArgumentNullException">The given value is null.</exception>
		public Whitespace Trailing
		{
			get;
			set => field = value ?? throw new ArgumentNullException(nameof(value));
		}

		/// <summary>
		/// Read the JSON element from a stream.
		/// </summary>
		/// <param name="reader">The stream to read from, which should be positioned at the start.</param>
		/// <param name="progress">The object to report progress to. Progress is reported for each element that is fully read.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is System.Threading.CancellationToken.None.</param>
		/// <returns>The JSON element contained within the document.</returns>
		/// <exception cref="ArgumentNullException">The reader is null.</exception>
		/// <exception cref="InvalidJsonException">The underlying text stream is not valid JSON.</exception>
		/// <exception cref="ObjectDisposedException">The underlying stream has been closed.</exception>
		/// <exception cref="IOException">An I/O error occurs.</exception>
		public static async Task<JsonElement> DeserializeAsync(JsonReader reader, IProgress<JsonParseProgress>? progress = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			ArgumentNullException.ThrowIfNull(reader, nameof(reader));

			await reader.ReadAsync(cancellationToken);
			JsonPath path = new JsonPath();
			JsonElement retval = await ReadElementAsync(reader, path, progress, cancellationToken);
			progress?.Report(new JsonParseProgress(retval, path));
			return retval;
		}

		/// <summary>
		/// Write the JSON element to a stream.
		/// </summary>
		/// <param name="writer">The stream to write to.</param>
		/// <exception cref="ObjectDisposedException">The writer is closed.</exception>
		/// <exception cref="IOException">An I/O error occurs.</exception>
		public abstract void Serialize(TextWriter writer);

		/// <summary>
		/// Write the JSON element to a stream.
		/// </summary>
		/// <param name="writer">The stream to write to.</param>
		/// <exception cref="ObjectDisposedException">The writer is closed.</exception>
		/// <exception cref="IOException">An I/O error occurs.</exception>
		public abstract Task SerializeAsync(TextWriter writer);

		/// <summary>
		/// Returns a string that represents the current object.
		/// </summary>
		/// <returns>A string that represents the current object.</returns>
		public override string ToString()
		{
			StringWriter writer = new StringWriter();
			Serialize(writer);
			return writer.ToString();
		}

		/// <summary>
		/// Set the whitespace of the element and all its children to the default "pretty" format.
		///
		/// Elements other than keys and values are on their own line and indented based on how
		/// deeply nexted they are. Values are led by a single space. Empty arrays and containers
		/// have no internal whitespace.
		/// </summary>
		/// <param name="newLine">The newline string. If null, Environment.NewLine is used.</param>
		/// <param name="indent">The indent string.</param>
		/// <param name="afterKey">The whitespace to insert after a key in an object (before the
		/// colon). If null, an empty string is used.</param>
		/// <param name="beforeValue">The whitespace to insert before a value in an object (after
		/// the colon). If null, a single space is used.</param>
		public void Prettify(string newLine = null, string indent = "  ", Whitespace afterKey = null, Whitespace beforeValue = null)
		{
			IFormat format = new DefaultFormat(newLine, indent, afterKey, beforeValue ?? new Whitespace(" "));
			Format(format);
		}

		/// <summary>
		/// Remove all whitespace from the element and all its children, conserving space and making
		/// it appropriate for a newline delimited JSON format.
		/// </summary>
		public void Minify()
		{
			IFormat format = new DefaultFormat(string.Empty, string.Empty, Whitespace.Empty, Whitespace.Empty);
			Format(format);
		}

		/// <summary>
		/// Set the whitespace of the element and all its children according to the rules of the
		/// format.
		/// </summary>
		/// <param name="format">The formatting rules to use.</param>
		/// <exception cref="ArgumentNullException">The format is null.</exception>
		public void Format(IFormat format)
		{
			ArgumentNullException.ThrowIfNull(format, nameof(format));

			int depth = 0;
			format.Format(this, depth, JsonRole.Value, false);
			List<JsonElement> level = new List<JsonElement>()
			{
				this
			};
			List<JsonElement> nextLevel = new List<JsonElement>();

			while (level.Count > 0)
			{
				depth += 1;

				foreach (var element in level)
				{
					if (element is JsonArray array)
					{
						for (int i = 0; i < array.Count; i++)
						{
							format.Format(array[i], depth, JsonRole.Value, i + 1 >= array.Count);
							nextLevel.Add(array[i]);
						}
					}
					else if (element is JsonObject obj)
					{
						for (int i = 0; i < obj.Count; i++)
						{
							format.Format(obj[i].Key, depth, JsonRole.ObjectKey, i + 1 >= obj.Count);
							format.Format(obj[i].Value, depth, JsonRole.ObjectValue, i + 1 >= obj.Count);
							nextLevel.Add(obj[i].Value);
						}
					}
				}

				(level, nextLevel) = (nextLevel, level);
				nextLevel.Clear();
			}
		}

		static async Task<JsonElement> ReadElementAsync(JsonReader reader, JsonPath path, IProgress<JsonParseProgress>? progress = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			JsonElement retval;

			Whitespace leading = Whitespace.Empty;
			if (reader.CurrentToken == TokenType.Whitespace)
			{
				leading = reader.Whitespace;
				_ = await reader.ReadAsync(cancellationToken);
			}

			Whitespace first;

			switch (reader.CurrentToken)
			{
				case TokenType.BeginArray:
					JsonArray array = new JsonArray();
					_ = await reader.ReadAsync(cancellationToken);
					first = Whitespace.Empty;
					if (reader.CurrentToken == TokenType.Whitespace)
					{
						// The first whitespace belongs to the first element, but only if it exists.
						first = reader.Whitespace;
						_ = await reader.ReadAsync(cancellationToken);
					}
					JsonPath arrayPath = new JsonPath(path, null);
					while (reader.CurrentToken != TokenType.EndArray)
					{
						JsonElement item = await ReadElementAsync(reader, arrayPath, progress, cancellationToken);
						array.Add(item);
						progress?.Report(new JsonParseProgress(item, arrayPath));
						if (reader.CurrentToken == TokenType.ListSeparator)
						{
							_ = await reader.ReadAsync(cancellationToken);
						}
					}
					if (array.Count == 0)
					{
						// The first whitespace was inside an empty array.
						array.EmptyWhitespace = first;
					}
					else
					{
						// The first whitespace belonged to the first element.
						array[0].Leading = first;
					}
					retval = array;
					break;
				case TokenType.BeginObject:
					JsonObject obj = new JsonObject();
					_ = await reader.ReadAsync(cancellationToken);
					first = Whitespace.Empty;
					if (reader.CurrentToken == TokenType.Whitespace)
					{
						// The first whitespace belongs to the first key, but only if it exists.
						first = reader.Whitespace;
						_ = await reader.ReadAsync(cancellationToken);
					}
					while (reader.CurrentToken != TokenType.EndObject)
					{
						JsonString key = (JsonString)await ReadElementAsync(reader, path, progress, cancellationToken);
						JsonPath objectPath = new JsonPath(path, key);
						// Read past the key/value separator.
						_ = await reader.ReadAsync(cancellationToken);
						JsonObjectPair pair = new JsonObjectPair(key, await ReadElementAsync(reader, objectPath, progress, cancellationToken));
						progress?.Report(new JsonParseProgress(pair.Value, objectPath));
						obj.Add(pair);
						if (reader.CurrentToken == TokenType.ListSeparator)
						{
							_ = await reader.ReadAsync(cancellationToken);
						}
					}
					if (obj.Count == 0)
					{
						// The first whitespace was inside an empty object.
						obj.EmptyWhitespace = first;
					}
					else
					{
						// The first whitespace belonged to the first key.
						obj[0].Key.Leading = first;
					}
					retval = obj;
					break;
				case TokenType.Boolean:
					retval = new JsonBoolean(reader.BooleanValue);
					break;
				case TokenType.Null:
					retval = new JsonNull();
					break;
				case TokenType.Number:
					retval = new JsonNumber(reader.NumberValue);
					break;
				case TokenType.String:
					retval = new JsonString(reader.StringValue, true);
					break;
				default:
					Debug.Fail($"Unknown token type {reader.CurrentToken}.");
					retval = null;
					break;
			}

			_ = await reader.ReadAsync(cancellationToken);

			retval.Leading = leading;

			if (reader.CurrentToken == TokenType.Whitespace)
			{
				retval.Trailing = reader.Whitespace;
				_ = await reader.ReadAsync(cancellationToken);
			}

			return retval;
		}
	}
}
