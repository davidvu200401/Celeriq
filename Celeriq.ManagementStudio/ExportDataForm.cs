using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Celeriq.ManagementStudio.Objects;

namespace Celeriq.ManagementStudio
{
	public partial class ExportDataForm : Form
	{
		private ConnectionCache _cache = new ConnectionCache();
		private string _serverName = string.Empty;

		public ExportDataForm()
		{
			InitializeComponent();

			this.Text = "Export Repository Data";

			cboSourceServer.Items.AddRange(_cache.Connections.ToArray());
			cboDestServer.Items.AddRange(_cache.Connections.ToArray());

			cboSourceServer.SelectedIndexChanged += new EventHandler(cboSourceServer_SelectedIndexChanged);
			cboDestServer.SelectedIndexChanged += new EventHandler(cboDestServer_SelectedIndexChanged);
		}

		public ExportDataForm(string server)
			: this()
		{
			_serverName = server;
		}

		public string SourceServerName
		{
			get { return cboSourceServer.Text.Trim(); }
		}

		public string DestServerName
		{
			get { return cboDestServer.Text.Trim(); }
		}

		public string SourceRepositoryName
		{
			get { return cboSourceRepository.Text.Trim(); }
		}

		public string DestRepositoryName
		{
			get { return txtDestRepository.Text.Trim(); }
		}

		private void cmdExport_Click(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(this.SourceServerName))
			{
				MessageBox.Show("The source server must be set.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			else if (string.IsNullOrEmpty(this.DestServerName))
			{
				MessageBox.Show("The destination server must be set.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			else if (string.IsNullOrEmpty(this.SourceRepositoryName))
			{
				MessageBox.Show("The source repository must be set.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			else if (string.IsNullOrEmpty(this.DestRepositoryName))
			{
				MessageBox.Show("The destination repository must be set.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			//Verify that the source does exist
			var sourceList = Celeriq.ManagementStudio.Objects.ConnectionCache.GetRepositoryPropertyList(this.SourceServerName);
			var sourceRepository = sourceList.FirstOrDefault(x => x.Repository.Name == this.SourceRepositoryName);
			if (sourceRepository == null)
			{
				MessageBox.Show("The source repository does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			//Verify that the destination does NOT exist
			var destList = Celeriq.ManagementStudio.Objects.ConnectionCache.GetRepositoryPropertyList(this.DestServerName);
			if (destList.Count(x => x.Repository.Name == this.SourceRepositoryName) != 0)
			{
				MessageBox.Show("The destination repository already exists.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			var xml = sourceRepository.Repository.ToXml();
			Celeriq.ManagementStudio.Objects.ConnectionCache.CreateRepository(this.DestServerName, xml, this.DestRepositoryName);

			this.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.Close();
		}

		private void cmdCancel_Click(object sender, EventArgs e)
		{
			this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.Close();
		}

		private void cboSourceServer_SelectedIndexChanged(object sender, EventArgs e)
		{
			cboSourceRepository.Items.Clear();
			foreach (var item in Celeriq.ManagementStudio.Objects.ConnectionCache.GetRepositoryPropertyList(_serverName))
			{
				cboSourceRepository.Items.Add(item.Repository.Name);
			}
		}

		private void cboDestServer_SelectedIndexChanged(object sender, EventArgs e)
		{
		}

	}
}
