using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Drawing.Drawing2D;

namespace Celeriq.ManagementStudio
{
	public partial class SplashForm : Form
	{
		public SplashForm()
		{
			InitializeComponent();

			this.KeyUp += SplashForm_KeyUp;

			var assembly = Assembly.GetExecutingAssembly();
			var copyright = (AssemblyCopyrightAttribute)AssemblyCopyrightAttribute.GetCustomAttribute(assembly, typeof(AssemblyCopyrightAttribute));
			var description = (AssemblyDescriptionAttribute)AssemblyDescriptionAttribute.GetCustomAttribute(assembly, typeof(AssemblyDescriptionAttribute));
			var company = (AssemblyCompanyAttribute)AssemblyCompanyAttribute.GetCustomAttribute(assembly, typeof(AssemblyCompanyAttribute));
			var trademark = (AssemblyTrademarkAttribute)AssemblyTrademarkAttribute.GetCustomAttribute(assembly, typeof(AssemblyTrademarkAttribute));
			var product = (AssemblyProductAttribute)AssemblyProductAttribute.GetCustomAttribute(assembly, typeof(AssemblyProductAttribute));
			var title = (AssemblyTitleAttribute)AssemblyTitleAttribute.GetCustomAttribute(assembly, typeof(AssemblyTitleAttribute));

			//Get the version information
			var version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
			var versionString = version.Major + "." + version.Minor + "." + version.Build + "." + version.Revision;

			lblVersion.Text = versionString;
			lblTrademark.Text = trademark.Trademark;
			lblDescription.Text = description.Description;

			System.Windows.Forms.Application.DoEvents();

		}

		public SplashForm(bool isSplash)
			: this()
		{
			timer1.Enabled = true;
			cmdClose.Visible = false;
			this.Text = string.Empty;
		}

		private void SplashForm_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Escape)
			{
				this.Close();
			}
		}

		private void timer1_Tick(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void cmdClose_Click(object sender, EventArgs e)
		{
			this.Close();
		}

	}
}