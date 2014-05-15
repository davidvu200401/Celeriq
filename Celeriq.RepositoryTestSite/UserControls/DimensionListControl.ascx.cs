using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Celeriq.Common;
using Celeriq.RepositoryTestSite.Objects;
using Celeriq.Utilities;

namespace Celeriq.RepositoryTestSite.UserControls
{
    public partial class DimensionListControl : System.Web.UI.UserControl
    {
        #region Class Members

        #endregion

        #region Page Events

        #endregion

        #region Methods

        public void Populate(Celeriq.Common.DataQueryResults results)
        {
            lstItem.ItemDataBound += lstItem_ItemDataBound;
            var list = results.DimensionList.Where(x => x.RefinementList.Count > 0).ToList();
            list.RemoveAll(x => x.RefinementList.Count == 1 && x.RefinementList.First().Count == results.TotalRecordCount);
            lstItem.DataSource = list;
            lstItem.DataBind();
        }

        #endregion

        #region Property Implementations

        #endregion

        #region Event Handlers

        private void lstItem_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            try
            {
                var dimensionItem = e.Item.DataItem as DimensionItem;

                const int HIDE_LIMIT = 8;

                var pnlHeader = e.Item.FindControl("pnlHeader") as PlaceHolder;
                if (pnlHeader == null)
                    return;

                var linkHeader = new Literal();
                linkHeader.Text = "<img title=\"Hide/show this section\" src=\"/images/dim-down.png\" class=\"toggle\" expanded=\"1\" />&nbsp;" + dimensionItem.Name;

                pnlHeader.Controls.Add(linkHeader);
                var pnlInside = (Panel) e.Item.FindControl("pnlInside");
                if (pnlInside == null)
                    return;

                if (linkHeader.Text == string.Empty)
                {
                    this.Visible = false;
                    return;
                }

                var hidePanel = new Panel();
                hidePanel.Style.Add("display", "none");

                var showMore = new Literal();

                //Create a template query for speed
                var templateQuery = new ListingQuery(this.Request.Url.PathAndQuery);

                var index = 0;
                foreach (var refinementItem in dimensionItem.RefinementList)
                {
                    var query = (ListingQuery) ((ICloneable) templateQuery).Clone();
                    query.PageOffset = 1;

                    var link = new HtmlAnchor();
                    var rValue = refinementItem.FieldValue;
                    var text = rValue;
                    const int MAXLENGTH = 26;
                    if (text.Length > MAXLENGTH)
                    {
                        text = text.Substring(0, (MAXLENGTH - 3)) + "...";
                    }
                    text += " <span class=\"cb\">" + refinementItem.Count + "</span>";
                    link.InnerHtml = text;

                    if (!query.DimensionValueList.Contains(refinementItem.DVIdx))
                        query.DimensionValueList.Add(refinementItem.DVIdx);

                    link.HRef = query.ToString();
                    link.Title = rValue + " (" + refinementItem.Count + " items)";

                    #region Checkbox for multi-select

                    Literal checkbox = null;
                    if (dimensionItem.Name == "Severity")
                    {
                        checkbox = new Literal();
                        checkbox.Text = "<input type=\"checkbox\" didx=\"" + dimensionItem.DIdx + "\" dvidx=\"" + refinementItem.DVIdx + "\" />";
                    }

                    #endregion

                    var breaker = new Label();
                    breaker.Text = "<br />";

                    if (index < HIDE_LIMIT)
                    {
                        if (checkbox != null) pnlInside.Controls.Add(checkbox);
                        pnlInside.Controls.Add(link);
                        pnlInside.Controls.Add(breaker);
                    }
                    else
                    {
                        if (checkbox != null) hidePanel.Controls.Add(checkbox);
                        hidePanel.Controls.Add(link);
                        hidePanel.Controls.Add(breaker);
                    }

                    if (index == HIDE_LIMIT)
                    {
                        pnlInside.Controls.Add(hidePanel);
                        pnlInside.Controls.Add(showMore);
                    }

                    index++;

                }

                showMore.Text = "<div align='right'><a href='javascript:;' class=\"noautoscroll\" onclick=\"" +
                                "javascript:showMore('" + hidePanel.ClientID + "', this)" + "\">show more</a></div>";
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                throw;
            }

        }

        #endregion

    }
}