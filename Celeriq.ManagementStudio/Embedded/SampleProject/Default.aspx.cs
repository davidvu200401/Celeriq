using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Celeriq.Common;
using CeleriqTestWebsite.Objects;

namespace CeleriqTestWebsite
{
    public partial class Default : System.Web.UI.Page
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            rptDimension.ItemDataBound += rptDimension_ItemDataBound;
            grdResults.RowDataBound += grdResults_RowDataBound;
            PagingControl1.RecordsPerPageChanged += PagingControl1_RecordsPerPageChanged;
            this.Populate();
        }

        private void PagingControl1_RecordsPerPageChanged(object sender, EventArgs e)
        {
            var url = new PagingURL(this.Request.Url.PathAndQuery);
            url.PageOffset = 1;
            url.RecordsPerPage = PagingControl1.RecordsPerPage;
            this.Response.Redirect(url.ToString());
        }

        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);
        }

        protected override void OnDataBinding(EventArgs e)
        {
            base.OnDataBinding(e);
            
        }

        private void Populate()
        {
            using (var factory = RepositoryConnection.GetFactory())
            {
                var service = factory.CreateChannel();

                var query = new ListingQuery(this.Request.Url.PathAndQuery);
                var results = RepositoryConnection.QueryData(query, service);
                rptDimension.DataSource = results.DimensionList;
                rptDimension.DataBind();
                grdResults.DataSource = results.RecordList;
                grdResults.DataBind();

                PagingControl1.ObjectSingular = "Widget";
                PagingControl1.ObjectPlural = "Widgets";
                PagingControl1.ItemCount = results.TotalRecordCount;
                AppliedFiltersControl1.Populate(results);
            }

        }

        private void grdResults_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
            }
        }

        private void rptDimension_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item)
            {
                var ctrl = LoadControl("~/UserControls/DimensionControl.ascx") as CeleriqTestWebsite.UserControls.DimensionControl;
                e.Item.Controls.Add(ctrl);
                ctrl.Populate(e.Item.DataItem as DimensionItem);
            }
        }

    }
}