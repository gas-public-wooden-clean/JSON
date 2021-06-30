using System;
using System.Collections.Generic;
using System.IO;

namespace CER.JSON.DocumentObjectModel
{
	public class Object : Element
	{
		public Object() : base()
		{
			Values = new List<ObjectPair>();
			EmptyWhitespace = Whitespace.Empty;
		}

		private Whitespace _emptyWhitespace;

		public IList<ObjectPair> Values
		{
			get;
			private set;
		}

		public Whitespace EmptyWhitespace
		{
			get { return _emptyWhitespace; }
			set { _emptyWhitespace = value ?? throw new ArgumentNullException(nameof(value)); }
		}

		/// <summary>
		/// Get the first value, if any, with an equivalent key.
		/// </summary>
		/// <param name="key">The value of a JSON string (not the JSON representation).</param>
		/// <param name="value">The first value with a matching key.</param>
		/// <returns>Whether exactly one value was found for the given key.</returns>
		public bool TryGetValue(string key, out Element value)
		{
			value = null;

			if (key == null)
			{
				throw new ArgumentNullException(nameof(key));
			}

			foreach (ObjectPair pair in Values)
			{
				if (pair.Key.Value.Equals(key))
				{
					if (value == null)
					{
						value = pair.Value;
					}
					else
					{
						return false;
					}
				}
			}

			return value != null;
		}

		public override void Serialize(TextWriter writer)
		{
			writer.Write(Leading.Value);
			writer.Write("{");
			if (Values.Count == 0)
			{
				writer.Write(EmptyWhitespace.Value);
			}
			else
			{
				bool first = true;
				foreach (ObjectPair entry in Values)
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
	}
}
