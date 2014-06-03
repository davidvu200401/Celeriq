#pragma warning disable 0168
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Celeriq.Common;
using Celeriq.ManagementStudio.Objects;
using Celeriq.Client.Interfaces;

namespace Celeriq.ManagementStudio
{
	public partial class BackupForm : Form
	{
		private string _serverName = string.Empty;
		private List<BaseRemotingObject> _repositoryList = null;
		private UserCredentials _credentials = null;

		public BackupForm()
		{
			InitializeComponent();
			cboRepository.SelectedIndexChanged += cboRepository_SelectedIndexChanged;
		}

		public BackupForm(string serverName, UserCredentials credentials, Guid selectedId)
			: this()
		{
			_credentials = credentials;
			_serverName = serverName;
			this.RefreshRepositories(selectedId);
		}

		private void cboRepository_SelectedIndexChanged(object sender, EventArgs e)
		{
			var repository = _repositoryList[cboRepository.SelectedIndex];
			txtBackup.Text = repository.Repository.Name + ".cqbak";
			txtBackup.SelectionStart = 0;
			txtBackup.SelectionLength = repository.Repository.Name.Length;
		}

		private void RefreshRepositories(Guid selectedId)
		{
			cboRepository.Items.Clear();
			_repositoryList = SystemCoreInteractDomain.GetRepositoryPropertyList(_serverName, _credentials);
			cboRepository.Items.AddRange(_repositoryList.Select(x => x.Repository.Name).ToArray());
			cboRepository.SelectedIndex = _repositoryList.IndexOf(_repositoryList.Find(x => x.Repository.ID == selectedId));
		}

		private void cmdOK_Click(object sender, EventArgs e)
		{
			try
			{
				var fileName = txtBackup.Text.Trim();
				
				if (cboRepository.SelectedIndex == -1)
				{
					MessageBox.Show("A repository must be selected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}

				if (string.IsNullOrEmpty(fileName) || !Celeriq.ManagementStudio.Objects.Utilities.IsFileNameValid(fileName))
				{
					MessageBox.Show("The file name is not valid.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}

				var fileName2 = (new FileInfo(fileName)).Name;
				if (fileName != fileName2)
				{
					MessageBox.Show("The file name cannot contain any path information.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}

				using (var factory = SystemCoreInteractDomain.GetFactory(_serverName))
				{
					var server = factory.CreateChannel();
					var repository = _repositoryList[cboRepository.SelectedIndex];
					server.Backup(repository.Repository.ID, _credentials, fileName);
				}

				MessageBox.Show("The repository has been backed up and is in the backup folder on the server.", "Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);

				this.DialogResult = System.Windows.Forms.DialogResult.OK;
				this.Close();

			}
			catch (Exception ex)
			{
				MessageBox.Show("An error occurred!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void cmdCancel_Click(object sender, EventArgs e)
		{
			this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.Close();
		}
	}
}
