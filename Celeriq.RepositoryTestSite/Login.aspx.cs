using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Celeriq.RepositoryTestSite.Objects;

namespace Celeriq.RepositoryTestSite
{
    public partial class Login : System.Web.UI.Page
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            cmdConnect.Click += cmdConnect_Click;
        }

        private void cmdConnect_Click(object sender, EventArgs e)
        {
            if (SessionHelper.Login(txtServer.Text, txtUser.Text, txtPassword.Text))
            {
                this.Response.Redirect("/");
            }
            else
            {
                this.Page.Validators.Add(new CustomValidator() {IsValid = false, ErrorMessage = "The login was not valid."});
            }
        }
    }
}