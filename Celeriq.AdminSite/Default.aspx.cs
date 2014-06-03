using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Celeriq.AdminSite.Objects;

namespace Celeriq.AdminSite
{
    public partial class Default : Celeriq.AdminSite.Objects.BasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (SessionHelper.IsLoggedIn)
            {
                this.Response.Redirect("/AuthUser");
            }
        }
    }
}