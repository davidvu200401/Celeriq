using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Celeriq.Common;
using Celeriq.RepositoryTestSite.Objects;

namespace Celeriq.RepositoryTestSite
{
    public partial class Default : System.Web.UI.Page
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            grdItem.RowDataBound += grdItem_RowDataBound;
            PagingControl1.RecordsPerPageChanged += PagingControl1_RecordsPerPageChanged;
            this.Populate();
        }

        private void PagingControl1_RecordsPerPageChanged(object sender, EventArgs e)
        {
            var url = new PagingURL(this.Request.Url.PathAndQuery);
            url.RecordsPerPage = PagingControl1.RecordsPerPage;
            url.PageOffset = 1;
            this.Response.Redirect(url.ToString());
        }

        private void Populate()
        {
            var url = new PagingURL(this.Request.Url.PathAndQuery);

            var paging = new Celeriq.Common.PagingInfo()
            {
                PageOffset = url.PageOffset,
                RecordsPerPage = url.RecordsPerPage
            };

            var repositoryList = SystemCoreInteractDomain
                .GetRepositoryPropertyList(SessionHelper.CeleriqServer, SessionHelper.Credentials, paging)
                .OrderBy(x => x.Repository.Name);

            grdItem.DataSource = repositoryList;
            grdItem.DataBind();
            PagingControl1.ItemCount = SystemCoreInteractDomain.GetRepositoryCount(SessionHelper.CeleriqServer, SessionHelper.Credentials, paging);
        }

        private void grdItem_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var dataItem = e.Row.DataItem as BaseRemotingObject;
                var lblName = e.Row.FindControl("lblName") as Literal;
                var lblDisk = e.Row.FindControl("lblDisk") as Literal;
                var lblMemory = e.Row.FindControl("lblMemory") as Literal;
                var lblAction = e.Row.FindControl("lblAction") as Literal;

                lblName.Text = dataItem.Repository.Name;
                lblDisk.Text = dataItem.DataDiskSize.ToString();
                lblMemory.Text = dataItem.DataMemorySize.ToString();

                lblAction.Text = "<a href='/results.aspx?id=" + dataItem.Repository.ID + "'>View</a>";
            }
        }
    }
}