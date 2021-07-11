using System;
using System.IO;

using InvalidTextException = CER.JSON.Stream.InvalidTextException;
using StreamReader = CER.JSON.Stream.StreamReader;
using Type = CER.JSON.Stream.Type;

namespace Filter
{
	class Program
	{
		static void Main(string[] args)
		{
			bool beautify = false;
			bool minify = false;
			foreach (string arg in args)
			{
				if (arg == "--pretty")
				{
					beautify = true;
				}
				else if (arg == "--mini")
				{
					minify = true;
				}
				else
				{
					Console.Error.WriteLine("Unrecognized option: {0}.", arg);
				}
			}

			if (beautify && minify)
			{
				Console.Error.WriteLine("You cannot specify --pretty and --mini.");
				return;
			}

			using (System.IO.TextReader console = new System.IO.StreamReader(Console.OpenStandardInput()))
			{
				StreamReader json = new StreamReader(console);

				uint depth = 0;
				bool inValue = false;
				bool hasItem = false;
				Exception e;
				while (ReadException(json, out e))
				{
					switch (json.Type)
					{
						case Type.BeginArray:
							if (beautify && !inValue)
							{
								Indent(depth);
							}
							inValue = false;
							Console.Write("[");
							if (beautify)
							{
								Console.WriteLine();
							}
							hasItem = false;
							depth += 1;
							break;
						case Type.BeginObject:
							if (beautify && !inValue)
							{
								Indent(depth);
							}
							inValue = false;
							Console.Write("{");
							if (beautify)
							{
								Console.WriteLine();
							}
							hasItem = false;
							depth += 1;
							break;
						case Type.Boolean:
							if (beautify && !inValue)
							{
								Indent(depth);
							}
							Console.Write(json.BooleanValue ? "true" : "false");
							hasItem = true;
							break;
						case Type.EndArray:
							depth -= 1;
							if (beautify)
							{
								if (hasItem)
								{
									Console.WriteLine();
								}
								Indent(depth);
							}
							Console.Write("]");
							hasItem = true;
							break;
						case Type.EndObject:
							depth -= 1;
							if (beautify)
							{
								if (hasItem)
								{
									Console.WriteLine();
								}
								Indent(depth);
							}
							Console.Write("}");
							hasItem = true;
							break;
						case Type.KeyValueSeparator:
							Console.Write(":");
							inValue = true;
							break;
						case Type.ListSeparator:
							Console.Write(",");
							inValue = false;
							if (beautify)
							{
								Console.WriteLine();
							}
							break;
						case Type.Null:
							if (beautify && !inValue)
							{
								Indent(depth);
							}
							Console.Write("null");
							hasItem = true;
							break;
						case Type.Number:
							if (beautify && !inValue)
							{
								Indent(depth);
							}
							Console.Write(json.NumberValue.JSON);
							hasItem = true;
							break;
						case Type.String:
							if (beautify && !inValue)
							{
								Indent(depth);
							}
							Console.Write("\"");
							Console.Write(json.StringValue.JSON);
							Console.Write("\"");
							hasItem = true;
							break;
						case Type.Whitespace:
							if (!beautify && !minify)
							{
								Console.Write(json.WhitespaceValue.Value);
							}
							break;
						default:
							throw new Exception();
					}
				}
				if (e != null)
				{
					Console.Error.WriteLine(e.Message);
				}
			}
		}

		static bool ReadException(StreamReader json, out Exception e)
		{
			e = null;
			try
			{
				return json.Read();
			}
			catch (InvalidDataException ex)
			{
				e = ex;
				return false;
			}
			catch (InvalidTextException ex)
			{
				e = ex;
				return false;
			}
		}

		static void Indent(uint depth)
		{
			for (uint i = 0; i < depth; i++)
			{
				Console.Write("\t");
			}
		}
	}
}
