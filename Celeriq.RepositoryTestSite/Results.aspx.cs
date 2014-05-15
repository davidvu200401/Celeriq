using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using Celeriq.RepositoryTestSite.Objects;

namespace Celeriq.RepositoryTestSite
{
    public partial class Results : System.Web.UI.Page
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            //There is no ID so nothing to do
            var url = new PagingURL(this.Request.Url.PathAndQuery);
            if (string.IsNullOrEmpty(url.Parameters.GetValue("id")))
            {
                this.Response.Redirect("/");
                return;
            }

            var id = url.Parameters.GetValue("id");

            var results = RepositoryHelper.Query(new ListingQuery(this.Request.Url.PathAndQuery), id);
            AppliedFilterControl1.Populate(results, id);
            DimensionListControl1.Populate(results);
            PagingControl1.ItemCount = results.TotalRecordCount;

        }
    }
}