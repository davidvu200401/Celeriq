using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Celeriq.Common;
using CeleriqTestWebsite.Objects;

namespace CeleriqTestWebsite.UserControls
{
    public partial class AppliedFiltersControl : System.Web.UI.UserControl
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            var url = new PagingURL(this.Request.Url.PathAndQuery);
            url.Parameters.Remove("d");
            linkClear.NavigateUrl = url.ToString();
        }

        public void Populate(ListingResults results)
        {
            ListingResults masterResults;
            using (var factory = RepositoryConnection.GetFactory())
            {
                var service = factory.CreateChannel();
                var query = new ListingQuery();
                masterResults = RepositoryConnection.QueryData(query, service);
            }

            //Group items by dimension
            var count = 0;
            foreach (var dvidx in results.Query.DimensionValueList)
            {
                var dItem = masterResults.DimensionList.FirstOrDefault(x => x.RefinementList.Any(z => z.DVIdx == dvidx));
                if (dItem != null)
                {
                    var rItem = dItem.RefinementList.FirstOrDefault(x => x.DVIdx == dvidx);
                    if (rItem != null)
                    {
                        //Create the label
                        var l = new Label();
                        l.Text = "<span class=\"prompt\">" + dItem.Name + "</span>: ";
                        pnlFilterDisplay.Controls.Add(l);

                        //Create the Dimension Value
                        var li = new Literal();
                        li.Text = rItem.FieldValue;
                        pnlFilterDisplay.Controls.Add(li);

                        //Create the Value Remove Link
                        this.CreateRemoveLink(pnlFilterDisplay, this.Request.Url.PathAndQuery, rItem.DVIdx);

                        //Create dimension trailing spacer
                        var spacer = new Literal();
                        spacer.Text = "<br />";
                        pnlFilterDisplay.Controls.Add(spacer);

                        count++;
                    }
                }
            }

            //Do not show this if there is nothing to show
            if (count == 0)
                this.Visible = false;
        }

        private void CreateRemoveLink(PlaceHolder panel, string queryString, long dvidx)
        {
            var query = new ListingQuery(queryString);
            query.DimensionValueList.Remove(dvidx);

            var link = new HtmlAnchor();
            link.HRef = query.ToString();
            link.InnerHtml = "(undo)";
            link.Attributes.Add("rel", "nofollow");
            link.Title = "Remove this filter";
            panel.Controls.Add(link);
        }

    }
}