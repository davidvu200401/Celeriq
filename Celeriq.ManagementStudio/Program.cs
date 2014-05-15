using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Reflection;

namespace Celeriq.ManagementStudio
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var F = new SplashForm(true);
            F.ShowDialog();
            System.Windows.Forms.Application.DoEvents();

            //Application.Run(new MainForm(args));
            Application.Run(new MainForm2(args));
        }

    }

}