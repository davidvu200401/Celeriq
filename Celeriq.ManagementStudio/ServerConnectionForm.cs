using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Celeriq.Common;
using Celeriq.ManagementStudio.Objects;
using Celeriq.Common;
using Celeriq.Utilities;

namespace Celeriq.ManagementStudio
{
    public partial class ServerConnectionForm : Form
    {
        private ConnectionCache _cache = new ConnectionCache();
        private string _publicKey = string.Empty;

        public ServerConnectionForm()
        {
            InitializeComponent();
            cboServer.Items.AddRange(_cache.Connections.ToArray());
        }

        public string ServerName
        {
            get { return cboServer.Text.Trim(); }
        }

        public string PublicKey
        {
            get { return _publicKey; }
        }

        public UserCredentials Credentials
        {
            get
            {
                try
                {
                    using (var factory = SystemCoreInteractDomain.GetFactory(this.ServerName))
                    {
                        var server = factory.CreateChannel();
                        var credentials = new UserCredentials();
                        credentials.UserName = txtUser.Text;
                        credentials.Password = txtPassword.Text;
                        credentials.Password = Celeriq.Utilities.SecurityHelper.Encrypt(_publicKey, credentials.Password);
                        return credentials;
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex);
                    return null;
                }
            }
        }

        private void cmdConnect_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.ServerName))
            {
                MessageBox.Show("The server name must be set.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (var factory = SystemCoreInteractDomain.GetFactory(this.ServerName))
                {
                    var server = factory.CreateChannel();
                    _publicKey = server.GetPublicKey();

                    if (!server.IsValidCredentials(this.Credentials))
                    {
                        Logger.LogInfo("Login failed: Server=" + this.ServerName + " / User=" + txtUser.Text);
                        MessageBox.Show("Login failed for user '" + txtUser.Text + "'.", "Error", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                MessageBox.Show("An error occurred connecting to server.", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
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
    }
}