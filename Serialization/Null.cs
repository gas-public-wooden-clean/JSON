using System.IO;

namespace CER.JSON.DocumentObjectModel
{
	public class Null : Element
	{
		public Null() : base()
		{ }

		public Null(Whitespace leading, Whitespace trailing) : base(leading, trailing)
		{ }

		public override void Serialize(TextWriter writer)
		{
			writer.Write(Leading.Value);
			writer.Write("null");
			writer.Write(Trailing.Value);
		}
	}
}
