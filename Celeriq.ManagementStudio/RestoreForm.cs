using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Celeriq.ManagementStudio.Objects;
using Celeriq.Common;
using Celeriq.Client.Interfaces;

namespace Celeriq.ManagementStudio
{
    public partial class RestoreForm : Form
    {
        private string _serverName = string.Empty;
        private List<BaseRemotingObject> _repositoryList = null;
        private UserCredentials _credentials = null;

        #region Constructors

        public RestoreForm()
        {
            InitializeComponent();
        }

        public RestoreForm(string serverName, UserCredentials credentials, Guid selectedId)
            : this()
        {
            _credentials = credentials;
            _serverName = serverName;
            this.RefreshBackups();
            this.RefreshRepositories(selectedId);
        }

        #endregion

        #region Methods

        private void RefreshBackups()
        {
            cboFile.Items.Clear();
            using (var factory = SystemCoreInteractDomain.GetFactory(_serverName))
            {
                var server = factory.CreateChannel();
                cboFile.Items.AddRange(server.GetBackups(_credentials).OrderBy(x => x).ToArray());
            }
        }

        private void RefreshRepositories(Guid selectedId)
        {
            cboRepository.Items.Clear();
            _repositoryList = SystemCoreInteractDomain.GetRepositoryPropertyList(_serverName, _credentials);
            cboRepository.Items.AddRange(_repositoryList.Select(x => x.Repository.Name).ToArray());
            cboRepository.SelectedIndex = _repositoryList.IndexOf(_repositoryList.Find(x => x.Repository.ID == selectedId));
        }

        #endregion

        #region Event Handlers

        private void cmdOK_Click(object sender, EventArgs e)
        {
            if (cboFile.SelectedIndex == -1)
            {
                MessageBox.Show("The source backup file must be set.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (cboRepository.SelectedIndex == -1)
            {
                MessageBox.Show("The destination repository must be set.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (var factory = SystemCoreInteractDomain.GetFactory(_serverName))
            {
                var server = factory.CreateChannel();
                var repository = _repositoryList[cboRepository.SelectedIndex];
                server.Restore(repository.Repository.ID, _credentials, cboFile.Text);
            }

            MessageBox.Show("The repository has been restored.", "Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        #endregion

    }
}