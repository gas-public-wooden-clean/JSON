using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using UI;

[assembly: ComVisible(false)]
[assembly: Guid("f083c118-06cd-4f57-87e7-f7fec2b52348")]
[assembly: CLSCompliant(true)]
[assembly: AssemblyVersion(Program.VersionString)]
[assembly: AssemblyFileVersion(Program.VersionString)]

namespace UI
{
	static class Program
	{
		internal const string VersionString = "1.3.0.0";

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.SetHighDpiMode(HighDpiMode.SystemAware);
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new EditJson());
		}
	}
}
