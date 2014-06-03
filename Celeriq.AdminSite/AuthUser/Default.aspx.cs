using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Celeriq.AdminSite.Objects;
using Celeriq.AdminSite.UserControls;

namespace Celeriq.AdminSite.AuthUser
{
    public partial class Default : Celeriq.AdminSite.Objects.BasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            cmdSearch.Click += cmdSearch_Click;
            cmdClear.Click += cmdClear_Click;
            grdList.ItemDataBound += grdList_ItemDataBound;
            PagingControl1.RecordsPerPageChanged += PagingControl1_RecordsPerPageChanged;

            var url = new PagingURL(this.Request.Url.PathAndQuery);
            txtKeyword.Text = url.Text;
        }

        private void PagingControl1_RecordsPerPageChanged(object sender, EventArgs e)
        {
            var url = new PagingURL(this.Request.Url.PathAndQuery);
            url.RecordsPerPage = PagingControl1.RecordsPerPage;
            url.PageOffset = 1;
            this.Response.Redirect(url.ToString());
        }

        private void cmdClear_Click(object sender, EventArgs e)
        {
            var url = new PagingURL(this.Request.Url.PathAndQuery);
            url.Text = string.Empty;
            url.ResetPaging();
            this.Response.Redirect(url.ToString());
        }

        private void cmdSearch_Click(object sender, EventArgs e)
        {
            var url = new PagingURL(this.Request.Url.PathAndQuery);
            url.Text = txtKeyword.Text;
            url.ResetPaging();
            this.Response.Redirect(url.ToString());
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.Populate();
        }

        private void Populate()
        {
            var url = new PagingURL(this.Request.Url.PathAndQuery);

            int count;
            grdList.DataSource = RepositoryConnection.GetRepositoryPropertyList(new Common.PagingInfo
            {
                PageOffset = PagingControl1.PageIndex,
                RecordsPerPage = PagingControl1.RecordsPerPage,
                Keyword = txtKeyword.Text,
                SortField = url.SortExpression,
                SortAsc = (url.SortDirection == SortConstants.Asc),
            }, out count);
            grdList.DataBind();
            PagingControl1.ItemCount = count;
        }

        private int _itemIndex = 0;
        private void grdList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var dataItem = e.Item.DataItem as Celeriq.Common.BaseRemotingObject;

                var RepositoryGridItemControl1 = e.Item.FindControl("RepositoryGridItemControl1") as RepositoryGridItemControl;
                RepositoryGridItemControl1.Populate(dataItem, _itemIndex);
                _itemIndex++;
            }
        }

    }
}