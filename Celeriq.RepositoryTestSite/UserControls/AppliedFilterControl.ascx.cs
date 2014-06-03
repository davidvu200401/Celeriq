using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Celeriq.Common;
using Celeriq.RepositoryTestSite.Objects;

namespace Celeriq.RepositoryTestSite.UserControls
{
    public partial class AppliedFilterControl : System.Web.UI.UserControl
    {
        #region Page Events

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            var url = new ListingQuery(this.Request.Url.PathAndQuery);
            url.DimensionValueList.Clear();
            url.FieldFilters.Clear();
            url.FieldSorts.Clear();
            url.Keyword = null;
            if (url.NonParsedFieldList["sg"] != "0")
                url.NonParsedFieldList["sg"] = null;

            lblClear.Text = "<a id=\"facetclearall\" style=\"float:right; font-size: 15px; font-weight: normal;margin-right:10px;\" href=\"" + url + "\" title=\"Clear all filters\" rel=\"nofollow\"> clear all</a>";
        }

        #endregion

        #region Methods

        public void Populate(Celeriq.Common.DataQueryResults results, string key)
        {
            var masterResults = RepositoryHelper.MasterResults(key);

            #region Group items by dimension
            var dimList = new Dictionary<string, List<RefinementItem>>();
            foreach (var dvidx in results.Query.DimensionValueList)
            {
                foreach (var dItem in masterResults.DimensionList)
                {
                    var rItem = dItem.RefinementList.FirstOrDefault(x => x.DVIdx == dvidx);
                    if (rItem != null)
                    {
                        //Normal dimensions
                        if (!dimList.ContainsKey(dItem.Name)) dimList.Add(dItem.Name, new List<RefinementItem>());
                        dimList[dItem.Name].Add(rItem);
                    }
                }
            }
            #endregion

            var realDimensionCount = dimList.Count;

            var count = 0;

            //Loop through dimensions and create a list of filters
            foreach (var dName in dimList.Keys)
            {
                var dText = dName;

                //There are no explicit filters so add "(None)" text
                if (count == realDimensionCount && realDimensionCount == 0)
                {
                    var noneText = new Literal();
                    noneText.Text += "<span class=\"data\">(None)</span>";
                    pnlFilterDisplay.Controls.Add(noneText);
                }

                #region If we are going to add implicit dimensions then add a break
                if (count == realDimensionCount)
                {
                    using (var dimBreak = new Literal())
                    {
                        dimBreak.Text = "<hr /><span style=\"margin-bottom:8px;\"><strong>Implicit Filters: </strong></span>";
                        pnlFilterDisplay.Controls.Add(dimBreak);
                    }
                }
                #endregion

                //Create the label
                var l = new Literal();
                l.Text = "<span class=\"fb\"><span class=\"prompt\">" + dText + "</span>: ";

                var rCount = 0;
                foreach (var rItem in dimList[dName])
                {
                    //Create the Dimension Value
                    var fieldValue = (rCount > 0 ? ", &nbsp;" : "") + rItem.FieldValue;
                    l.Text += "<span class=\"data\">" + fieldValue + "</span>";

                    if (count < realDimensionCount)
                    {
                        l.Text += this.CreateRemoveLink(this.Request.Url.PathAndQuery, rItem);
                    }
                    else if (count < dimList.Count - 1)
                    {
                        l.Text += ", ";
                    }

                    rCount++;
                }
                count++;
                l.Text += "</span>";
                pnlFilterDisplay.Controls.Add(l);
            }

            //If there is text filter then add it too
            if (!string.IsNullOrEmpty(results.Query.Keyword))
            {
                //Create the label
                var l = new Literal();
                l.Text = "<span class=\"fb\"><span class=\"prompt\">Text filter</span>: " + results.Query.Keyword;
                l.Text += this.CreateRemoveLinkNoTextFilter(this.Request.Url.PathAndQuery);
                l.Text += "</span>";
                pnlFilterDisplay.Controls.Add(l);
                count++;
            }

            //Do not show this if there is nothing to show
            if (count == 0)
                this.Visible = false;
        }

        private string CreateRemoveLink(string queryString, RefinementItem refinement)
        {
            var query = new ListingQuery(queryString);
            query.DimensionValueList.Remove(refinement.DVIdx);
            return "<a title=\"Remove this filter\" rel=\"nofollow\" href=\"" + query.ToString() + "\" style=\"margin:0px 8px 0px 8px\"><img src=\"/images/trash.gif\" /></a>";
        }

        private string CreateRemoveLinkNoTextFilter(string queryString)
        {
            var query = new ListingQuery(queryString);
            query.Keyword = string.Empty;
            return "<a title=\"Remove this filter\" rel=\"nofollow\" href=\"" + query.ToString() + "\" style=\"margin:0px 8px 0px 8px\"><img src=\"/images/trash.gif\" /></a>";
        }

        #endregion

    }
}