using CER.Json;
using CER.Json.Stream;
using System;
using System.IO;

namespace Filter
{
	class Program
	{
		internal const string VersionString = "1.1.0.0";

		static void Main(string[] args)
		{
			Version compiled = new Version(AssemblyInfo.Major, AssemblyInfo.Minor, AssemblyInfo.Build, AssemblyInfo.Revision);
			Version runtime = typeof(AssemblyInfo).Assembly.GetName().Version;
			if (compiled.Major != runtime.Major ||
				compiled.Minor != runtime.Minor)
			{
				string application = AppDomain.CurrentDomain.FriendlyName;
				Console.Error.WriteLine(Strings.CompileRuntimeMismatch, application,
					typeof(AssemblyInfo).Assembly.FullName, compiled);
			}

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
					Console.Error.WriteLine(Strings.UnrecognizedOption, arg);
				}
			}

			if (beautify && minify)
			{
				Console.Error.WriteLine(Strings.PrettyAndMini);
				return;
			}

			JsonReader json = new JsonReader(Console.In);

			uint depth = 0;
			bool inValue = false;
			bool hasItem = false;
			Exception e;
			while (ReadException(json, out e))
			{
				switch (json.CurrentToken)
				{
					case TokenType.BeginArray:
						if (beautify && !inValue)
						{
							Indent(depth);
						}
						inValue = false;
						Console.Out.Write("[");
						if (beautify)
						{
							Console.Out.WriteLine();
						}
						hasItem = false;
						depth += 1;
						break;
					case TokenType.BeginObject:
						if (beautify && !inValue)
						{
							Indent(depth);
						}
						inValue = false;
						Console.Out.Write("{");
						if (beautify)
						{
							Console.Out.WriteLine();
						}
						hasItem = false;
						depth += 1;
						break;
					case TokenType.Boolean:
						if (beautify && !inValue)
						{
							Indent(depth);
						}
						Console.Out.Write(json.BooleanValue ? "true" : "false");
						hasItem = true;
						break;
					case TokenType.EndArray:
						depth -= 1;
						if (beautify)
						{
							if (hasItem)
							{
								Console.Out.WriteLine();
							}
							Indent(depth);
						}
						Console.Out.Write("]");
						hasItem = true;
						break;
					case TokenType.EndObject:
						depth -= 1;
						if (beautify)
						{
							if (hasItem)
							{
								Console.Out.WriteLine();
							}
							Indent(depth);
						}
						Console.Out.Write("}");
						hasItem = true;
						break;
					case TokenType.KeyValueSeparator:
						Console.Out.Write(":");
						inValue = true;
						break;
					case TokenType.ListSeparator:
						Console.Out.Write(",");
						inValue = false;
						if (beautify)
						{
							Console.Out.WriteLine();
						}
						break;
					case TokenType.Null:
						if (beautify && !inValue)
						{
							Indent(depth);
						}
						Console.Out.Write("null");
						hasItem = true;
						break;
					case TokenType.Number:
						if (beautify && !inValue)
						{
							Indent(depth);
						}
						Console.Out.Write(json.NumberValue);
						hasItem = true;
						break;
					case TokenType.String:
						if (beautify && !inValue)
						{
							Indent(depth);
						}
						Console.Out.Write("\"");
						Console.Out.Write(json.StringValue);
						Console.Out.Write("\"");
						hasItem = true;
						break;
					case TokenType.Whitespace:
						if (!beautify && !minify)
						{
							Console.Out.Write(json.Whitespace.Value);
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

		static bool ReadException(JsonReader json, out Exception e)
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
			catch (InvalidJsonException ex)
			{
				e = ex;
				return false;
			}
		}

		static void Indent(uint depth)
		{
			for (uint i = 0; i < depth; i++)
			{
				Console.Out.Write("\t");
			}
		}
	}
}
