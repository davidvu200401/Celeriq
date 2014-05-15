using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Celeriq.Common;

namespace Celeriq.ManagementStudio
{
    public partial class UserListForm : Form
    {
        private string _serverName;
        private UserCredentials _credentials;
        private string _publicKey;

        public UserListForm()
        {
            InitializeComponent();
        }

        public UserListForm(string serverName, UserCredentials credentials, string publicKey)
            : this()
        {
            _serverName = serverName;
            _credentials = credentials;
            _publicKey = publicKey;

            lstItem.SelectedIndexChanged += lstItem_SelectedIndexChanged;

            this.LoadUsers();
        }

        private void lstItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.EnableButtons();
        }

        private void LoadUsers()
        {
            lstItem.Items.Clear();
            var userList = SystemCoreInteractDomain.GetUserList(_serverName, _credentials).OrderBy(x => x.UserName);
            foreach (var item in userList)
            {
                lstItem.Items.Add(item);
            }

            this.EnableButtons();
        }

        private void EnableButtons()
        {
            cmdEdit.Enabled = (lstItem.SelectedItem != null);
            cmdDelete.Enabled = (lstItem.SelectedItem != null);
        }

        private void cmdAdd_Click(object sender, EventArgs e)
        {
            var F = new UserEditForm(_serverName, null, _publicKey);
            if (F.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {

                using (var factory = SystemCoreInteractDomain.GetFactory(_serverName))
                {
                    var server = factory.CreateChannel();
                    server.AddSystemUser(_credentials, F.User);
                }

                this.LoadUsers();
            }
        }

        private void cmdEdit_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Not implemented", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //var user = lstItem.SelectedItem as UserCredentials;
            //var F = new UserEditForm(_serverName, user, _publicKey);
            //if (F.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //{
            //	this.LoadUsers();
            //}
        }

        private void cmdDelete_Click(object sender, EventArgs e)
        {
            var user = lstItem.SelectedItem as UserCredentials;
            if (user == null) return;

            if (user.UserName == "root")
            {
                MessageBox.Show("You cannot delete the root user.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (user.UserName == _credentials.UserName)
            {
                MessageBox.Show("You cannot delete the current user.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Do you wish to delete the selected user?", "Delete User", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != System.Windows.Forms.DialogResult.Yes)
                return;

            using (var factory = SystemCoreInteractDomain.GetFactory(_serverName))
            {
                var server = factory.CreateChannel();
                server.DeleteSystemUser(_credentials, user);
            }

            this.LoadUsers();
        }

        private void cmdClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}