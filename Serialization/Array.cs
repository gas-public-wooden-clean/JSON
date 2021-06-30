using System;
using System.Collections.Generic;
using System.IO;

namespace CER.JSON.DocumentObjectModel
{
	public class Array : Element
	{
		public Array() : base()
		{
			Values = new List<Element>();
			EmptyWhitespace = Whitespace.Empty;
		}

		private Whitespace _emptyWhitespace;

		public IList<Element> Values
		{
			get;
			private set;
		}

		public Whitespace EmptyWhitespace
		{
			get { return _emptyWhitespace; }
			set { _emptyWhitespace = value ?? throw new ArgumentNullException(nameof(value)); }
		}

		public override void Serialize(TextWriter writer)
		{
			writer.Write(Leading.Value);
			writer.Write("[");
			if (Values.Count == 0)
			{
				writer.Write(EmptyWhitespace.Value);
			}
			else
			{
				bool first = true;
				foreach (Element element in Values)
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
