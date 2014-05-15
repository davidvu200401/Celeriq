using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Celeriq.Common;

namespace CeleriqTestWebsite.UserControls
{
    public partial class DimensionControl : System.Web.UI.UserControl
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            rptItem.ItemDataBound += rptItem_ItemDataBound;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        private void rptItem_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                var pnlItem = e.Item.FindControl("pnlItem") as PlaceHolder;
                var dataItem = e.Item.DataItem as RefinementItem;
                var row = new TableRow();
                row.Cells.Add(new TableCell());
                var query2 = new ListingQuery(this.Request.Url.PathAndQuery);
                query2.DimensionValueList.Add(dataItem.DVIdx);

                //Create the link
                var link = new Literal();
                var linkText = dataItem.FieldValue;
                linkText += " (" + dataItem.Count + ")";
                link.Text = "<a href=\"" + query2.ToString() + "\" title=\"" + linkText + "\">" + linkText + "</a>";

                pnlItem.Controls.Add(link);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void Populate(DimensionItem dimension)
        {
            lblHeader.Text = dimension.Name;
            rptItem.DataSource = dimension.RefinementList;
            rptItem.DataBind();
        }

    }
}