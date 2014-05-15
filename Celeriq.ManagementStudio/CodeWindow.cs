#pragma warning disable 0168
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Celeriq.Common;

namespace Celeriq.ManagementStudio
{
	public partial class CodeWindow : Form
	{
		public CodeWindow()
		{
			InitializeComponent();
			this.KeyUp += new KeyEventHandler(CodeWindow_KeyUp);
		}

		public CodeWindow(string text, string config) :
			this()
		{
			txtCode.Text = text;
		}

		private void CodeWindow_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Escape)
			{
				this.Close();
			}
		}

		private void cmdCopy_Click(object sender, EventArgs e)
		{
			try
			{
				if (tabControl1.SelectedIndex == 0)
					Clipboard.SetText(txtCode.Text, TextDataFormat.Text);
			}
			catch (System.Runtime.InteropServices.ExternalException) { }
		}

		private void cmdCreate_Click(object sender, EventArgs e)
		{
			try
			{
				var d = new System.Windows.Forms.FolderBrowserDialog();
				d.RootFolder = Environment.SpecialFolder.MyComputer;
				d.SelectedPath = @"c:\temp";
				if (d.ShowDialog() == System.Windows.Forms.DialogResult.OK)
				{
					var path = d.SelectedPath;
					var di = new DirectoryInfo(path);
					if (di.Exists)
					{
						if (di.GetFiles().Length != 0 || di.GetDirectories().Length != 0)
						{
							if (MessageBox.Show("The specified folder is not empty. Do you wish to proceed and overwrite the contents?", "Proceed?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != System.Windows.Forms.DialogResult.Yes)
								return;
						}
					}

					//Emit all embeeded resources
					var embedRoot = "Celeriq.ManagementStudio.Embedded.SampleProject";
					var a = System.Reflection.Assembly.GetExecutingAssembly();
					var files = a.GetManifestResourceNames().Where(x => x.StartsWith(embedRoot)).ToList();
					foreach (var f in files)
					{
						var outputName = f.Replace(embedRoot + ".", string.Empty);
						var data = new StreamReader(a.GetManifestResourceStream(f)).ReadToEnd();

						//Perform replacements
						data = data.Replace("%REPOSITORY%", txtCode.Text);
						data = data.Replace("[MYNAMESPACE]", "CeleriqTestWebsite");

						var outputFolder = string.Empty;
						if (outputName.StartsWith("Objects."))
						{
							outputFolder = "Objects";
							outputName = outputName.Substring(outputFolder.Length + 1, outputName.Length - outputFolder.Length - 1);
						}
						else if (outputName.StartsWith("Properties."))
						{
							outputFolder = "Properties";
							outputName = outputName.Substring(outputFolder.Length + 1, outputName.Length - outputFolder.Length - 1);
						}
						else if (outputName.StartsWith("UserControls."))
						{
							outputFolder = "UserControls";
							outputName = outputName.Substring(outputFolder.Length + 1, outputName.Length - outputFolder.Length - 1);
						}
						else
						{

						}

						outputName = outputName.Trim(".embed");
						var destFolder = Path.Combine(path, outputFolder);
						if (!Directory.Exists(destFolder)) Directory.CreateDirectory(destFolder);
						File.WriteAllText(Path.Combine(destFolder, outputName), data);

					}

					//Copy Celeriq.Common library
					var mypath = (new FileInfo(a.Location)).DirectoryName;
					var binFolder = Path.Combine(path, "bin");
						if (!Directory.Exists(binFolder)) Directory.CreateDirectory(binFolder);
					var targetBinFile = Path.Combine(binFolder, "Celeriq.Common.dll");
					if (!File.Exists(targetBinFile))
						File.Copy(Path.Combine(mypath, "Celeriq.Common.dll"), targetBinFile);

					if (MessageBox.Show("Do you wish to open the sample project?", "Open", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
					{
						System.Diagnostics.Process.Start(Path.Combine(path, "CeleriqTestWebsite.sln"));
					}

				}
			}
			catch (Exception ex)
			{
				throw;
			}

		}

	}
}
