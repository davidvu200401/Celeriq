using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Celeriq.Common;
using Celeriq.Utilities;

namespace Celeriq.ManagementStudio
{
	public partial class UserEditForm : Form
	{
		private string _serverName;
		private UserCredentials _user;
		private string _publicKey;

		public UserEditForm()
		{
			InitializeComponent();
		}

		public UserEditForm(string serverName, UserCredentials user, string publicKey)
			: this()
		{
			if (user == null)
			{
				//Add
				user = new UserCredentials();
			}
			else
			{
				//Edit
				txtUser.Enabled = false;
			}

			_serverName = serverName;
			_user = user;
			_publicKey = publicKey;

			txtUser.Text = user.UserName;
		}

		public UserCredentials User
		{
			get { return _user; }
		}

		private void cmdOK_Click(object sender, EventArgs e)
		{
			txtUser.Text = txtUser.Text.Trim();
			txtPassword.Text = txtPassword.Text.Trim();

			if (string.IsNullOrEmpty(txtUser.Text))
			{
				MessageBox.Show("The user name must be set.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			if (string.IsNullOrEmpty(txtPassword.Text) || (txtPassword.Text != txtConfirm.Text))
			{
				MessageBox.Show("The password must be set and must confirmed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			if (!SecurityHelper.IsValidPassword(txtPassword.Text))
			{
				MessageBox.Show("The password does not meet length or complexity requirements.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			_user.UserName = txtUser.Text;
			_user.Password = txtPassword.Text;

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
