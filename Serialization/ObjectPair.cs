using System.Diagnostics;

namespace CER.JSON.DocumentObjectModel
{
	[DebuggerDisplay("{Key}:{Value}")]
	public class ObjectPair
	{
		public ObjectPair()
		{
			Key = new String();
			Value = new Null();
		}

		public ObjectPair(String key, Element value)
		{
			Key = key;
			Value = value;
		}

		public String Key
		{
			get;
			set;
		}

		public Element Value
		{
			get;
			set;
		}
	}
}
