using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Celeriq.AdminSite.Objects;

namespace Celeriq.AdminSite.UserControls
{
    public partial class LoginControl : System.Web.UI.UserControl
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.Visible = !SessionHelper.IsLoggedIn;
        }
    }
}