using System;
using System.Windows.Forms;

namespace UI
{
	static class Program
	{
		internal const string VersionString = "1.1.0.0";

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new EditJson());
		}
	}
}
