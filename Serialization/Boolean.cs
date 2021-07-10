using System.Diagnostics;
using System.IO;

namespace CER.JSON.DocumentObjectModel
{
	[DebuggerDisplay("{Value}")]
	public class Boolean : Element
	{
		public Boolean(bool value) => Value = value;

		public Boolean(Whitespace leading, Whitespace trailing, bool value)
			: base(leading, trailing) => Value = value;

		public bool Value
		{
			get;
			set;
		}

		public override void Serialize(TextWriter writer)
		{
			writer.Write(Leading.Value);
			writer.Write(Value ? "true" : "false");
			writer.Write(Trailing.Value);
		}
	}
}
