using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Celeriq.Profiler
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			var F = new SplashForm(true);
			F.ShowDialog();
			System.Windows.Forms.Application.DoEvents();

			Application.Run(new MDIParent());
		}
	}
}
