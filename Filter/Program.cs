using CER.Json;
using CER.Json.DocumentObjectModel;
using CER.Json.Stream;
using Filter;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

[assembly: ComVisible(false)]
[assembly: Guid("184d891c-a747-4561-ad60-00a25ee5906c")]
[assembly: CLSCompliant(true)]
[assembly: AssemblyVersion(Program.VersionString)]
[assembly: AssemblyFileVersion(Program.VersionString)]

namespace Filter
{
	class Program
	{
		internal const string VersionString = "1.3.0.0";

		static async Task Main(string[] args)
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
			Tuple<bool, Exception> readResult;
			AsyncOutput output = new AsyncOutput();
			while ((readResult = await ReadException(json)).Item1)
			{
				switch (json.CurrentToken)
				{
					case TokenType.BeginArray:
						if (beautify && !inValue)
						{
							output.Indent(depth);
						}
						inValue = false;
						output.Append('[');
						if (beautify)
						{
							output.Append(Environment.NewLine);
						}
						await output.Write();
						hasItem = false;
						depth += 1;
						break;
					case TokenType.BeginObject:
						if (beautify && !inValue)
						{
							output.Indent(depth);
						}
						inValue = false;
						if (beautify)
						{
							output.Append(Environment.NewLine);
						}
						output.Append('{');
						await output.Write();
						hasItem = false;
						depth += 1;
						break;
					case TokenType.Boolean:
						if (beautify && !inValue)
						{
							output.Indent(depth);
						}
						output.Append(json.BooleanValue ? "true" : "false");
						await output.Write();
						hasItem = true;
						break;
					case TokenType.EndArray:
						depth -= 1;
						if (beautify)
						{
							if (hasItem)
							{
								output.Append(Environment.NewLine);
							}
							output.Indent(depth);
						}
						output.Append(']');
						await output.Write();
						hasItem = true;
						break;
					case TokenType.EndObject:
						depth -= 1;
						if (beautify)
						{
							if (hasItem)
							{
								output.Append(Environment.NewLine);
							}
							output.Indent(depth);
						}
						output.Append('}');
						await output.Write();
						hasItem = true;
						break;
					case TokenType.KeyValueSeparator:
						output.Append(':');
						await output.Write();
						inValue = true;
						break;
					case TokenType.ListSeparator:
						output.Append(',');
						inValue = false;
						if (beautify)
						{
							output.Append(Environment.NewLine);
						}
						await output.Write();
						break;
					case TokenType.Null:
						if (beautify && !inValue)
						{
							output.Indent(depth);
						}
						output.Append("null");
						await output.Write();
						hasItem = true;
						break;
					case TokenType.Number:
						if (beautify && !inValue)
						{
							output.Indent(depth);
						}
						output.Append(json.NumberValue);
						await output.Write();
						hasItem = true;
						break;
					case TokenType.String:
						if (beautify && !inValue)
						{
							output.Indent(depth);
						}
						output.Append('\"');
						output.Append(json.StringValue);
						output.Append('\"');
						await output.Write();
						hasItem = true;
						break;
					case TokenType.Whitespace:
						if (!beautify && !minify)
						{
							output.Append(json.Whitespace.Value);
							await output.Write();
						}
						break;
					default:
						Debug.Fail($"Invalid JSON token type {json.CurrentToken}.");
						break;
				}
			}
			await output.Finish();
			if (readResult.Item2 is not null)
			{
				Console.Error.WriteLine(readResult.Item2.Message);
			}
		}

		static async Task<Tuple<bool, Exception>> ReadException(JsonReader json)
		{
			try
			{
				return new Tuple<bool, Exception>(await json.ReadAsync(), null);
			}
			catch (InvalidDataException ex)
			{
				return new Tuple<bool, Exception>(false, ex);
			}
			catch (InvalidJsonException ex)
			{
				return new Tuple<bool, Exception>(false, ex);
			}
		}

		class AsyncOutput
		{
			public AsyncOutput()
			{
				_task = Task.CompletedTask;
				_builder = new StringBuilder();
			}

			Task _task;
			readonly StringBuilder _builder;

			public async Task Write()
			{
				await _task;
				_task = Console.Out.WriteAsync(_builder);
				_builder.Clear();
			}

			public void Indent(uint depth)
			{
				for (uint i = 0; i < depth; i++)
				{
					_builder.Append('\t');
				}
			}

			public void Append(char value)
			{
				_builder.Append(value);
			}

			public void Append(string value)
			{
				_builder.Append(value);
			}

			public async Task Finish()
			{
				await _task;
				_task = Task.CompletedTask;
			}
		}
	}
}
