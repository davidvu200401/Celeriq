#pragma warning disable 0168
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Celeriq.Common;
using Celeriq.Profiler.Objects;
using Celeriq.Common;

namespace Celeriq.Profiler
{
    public partial class ServerConnectionForm : Form
    {
        private ConnectionCache _cache = new ConnectionCache();
        private string _publicKey = string.Empty;

        public ServerConnectionForm()
        {
            InitializeComponent();

            cboRepository.SelectedValueChanged += cboRepository_SelectedValueChanged;
            cboServer.Items.AddRange(_cache.Connections.ToArray());
            UpdateMenus();
        }

        private void cboRepository_SelectedValueChanged(object sender, EventArgs e)
        {
            UpdateMenus();
        }

        public string ServerName
        {
            get { return cboServer.Text.Trim(); }
        }

        public string PublicKey
        {
            get { return _publicKey; }
        }

        public BaseRemotingObject Repository
        {
            get { return cboRepository.SelectedItem as BaseRemotingObject; }
        }

        public UserCredentials Credentials
        {
            get { return SystemCoreInteractDomain.GetCredentials(this.ServerName, txtUser.Text, txtPassword.Text); }
        }

        private void UpdateMenus()
        {
            cboRepository.Enabled = (cboRepository.Items.Count > 0);
            cmdConnect.Enabled = (cboRepository.SelectedItem != null);
        }

        private void cmdConnect_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.ServerName))
            {
                MessageBox.Show("The server name must be set.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            _cache.Connections.Remove(cboServer.Text);
            _cache.Connections.Insert(0, cboServer.Text);
            _cache.Save();
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void cmdRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                using (var factory = SystemCoreInteractDomain.GetFactory(this.ServerName))
                {
                    var server = factory.CreateChannel();
                    _publicKey = server.GetPublicKey();

                    if (!server.IsValidCredentials(this.Credentials))
                    {
                        MessageBox.Show("Login failed for user '" + txtUser.Text + "'.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred connecting to server.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //Load repositories
            var list = SystemCoreInteractDomain.GetRepositoryPropertyList(this.ServerName, this.Credentials);
            cboRepository.Items.Clear();
            foreach (var item in list)
            {
                cboRepository.Items.Add(item);
            }
            cboRepository.Enabled = (cboRepository.Items.Count > 0);

        }
    }
}