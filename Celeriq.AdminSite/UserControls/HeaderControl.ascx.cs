using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Celeriq.AdminSite.Objects;

namespace Celeriq.AdminSite.UserControls
{
    public partial class HeaderControl : System.Web.UI.UserControl
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            pnlLoggedIn.Visible = SessionHelper.IsLoggedIn;
        }
    }
}