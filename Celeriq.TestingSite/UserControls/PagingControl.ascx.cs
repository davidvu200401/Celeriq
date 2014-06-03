using Celeriq.TestingSite.API;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Celeriq.TestingSite.MainSite.UserControls
{
    public partial class PagingControl : System.Web.UI.UserControl
    {
        #region Class Members

        #endregion

        #region Page Load

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            //Populate the page per combo
            cboPagePer.Items.Add(new ListItem("5" + " " + this.ObjectPlural + " per Page", "5"));
            cboPagePer.Items.Add(new ListItem("10" + " " + this.ObjectPlural + " per Page", "10"));
            cboPagePer.Items.Add(new ListItem("20" + " " + this.ObjectPlural + " per Page", "20"));
            cboPagePer.Items.Add(new ListItem("30" + " " + this.ObjectPlural + " per Page", "30"));
            cboPagePer.Items.Add(new ListItem("40" + " " + this.ObjectPlural + " per Page", "40"));
            cboPagePer.Items.Add(new ListItem("50" + " " + this.ObjectPlural + " per Page", "50"));
            //cboPagePer.Items.Add(new ListItem("All", "-1"));

            var url = new PagingUrl(this.InstanceID, this.Request.Url.PathAndQuery);
            cboPagePer.SelectedValue = url.RecordsPerPage.ToString();
            this.rpp.Value = cboPagePer.SelectedValue;

            this.cboPagePer.SelectedIndexChanged += new EventHandler(cboPagePer_SelectedIndexChanged);

        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            cboPagePer.Attributes.Add("onclick", "javascript:if (document.getElementById('" + this.rpp.ClientID + "').value!=document.getElementById('" + this.cboPagePer.ClientID + "').value) document.getElementById('" + this.rpp.ClientID + "').value=document.getElementById('" + this.cboPagePer.ClientID + "').value;");
            this.PopulateClickBar();
            //this.rpp.ValueChanged += new EventHandler(cboPagePer_SelectedIndexChanged);
            this.UpdateDisplay();
        }

        public string InstanceID { get; set; }

        public bool IsSeo { get; set; }

        #endregion

        #region Events

        public event EventHandler PageIndexChanged;
        public event EventHandler RecordsPerPageChanged;

        protected virtual void OnPageIndexChanged(System.EventArgs e)
        {
            if (this.PageIndexChanged != null)
                this.PageIndexChanged(this, e);
        }

        protected virtual void OnRecordsPerPageChanged(System.EventArgs e)
        {
            if (this.RecordsPerPageChanged != null)
                this.RecordsPerPageChanged(this, e);
        }

        #endregion

        #region Methods

        private void PopulateClickBar()
        {
            try
            {
                var query = new PagingUrl(InstanceID, this.Request.Url.PathAndQuery);
                pnlGotoPage.Controls.Clear();
                HtmlAnchor link = null;

                Literal literal = null;
                if (this.PageIndex > 1)
                {
                    //Add the 'Previous' page link
                    link = new HtmlAnchor();
                    link.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                    link.Attributes.Add("rel", "nofollow");
                    link.ID = "linkPrevious";
                    link.InnerText = "<";
                    pnlGotoPage.Controls.Add(link);
                    query.PageOffset = this.PageIndex - 1;
                    link.HRef = query.ToString();

                    literal = new Literal();
                    literal.Text = "&nbsp;&nbsp;";
                    pnlGotoPage.Controls.Add(literal);
                }

                //Try to position the current PageIndex in the middle of the selection
                var startIndex = this.PageIndex - 5;
                if (startIndex < 1)
                    startIndex = 1;
                var endIndex = startIndex + 9;
                if (endIndex > this.PageCount)
                    endIndex = this.PageCount;

                if (startIndex == 1 && endIndex <= 1)
                {
                    pnlGotoContainer.Visible = false;
                    return;
                }
                pnlGotoContainer.Visible = true;

                for (var ii = startIndex; ii <= endIndex; ii++)
                {
                    link = new HtmlAnchor();
                    link.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                    link.Attributes.Add("rel", "nofollow");
                    //link.ID = "linkPage" + ii.ToString();
                    link.InnerText = ii.ToString();
                    if (ii == this.PageIndex)
                        link.Attributes.Add("class", "paginggotopageselected");
                    query.PageOffset = ii;
                    link.HRef = query.ToString();
                    pnlGotoPage.Controls.Add(link);
                    literal = new Literal();
                    literal.Text = "&nbsp;&nbsp;";
                    pnlGotoPage.Controls.Add(literal);
                }

                if (this.PageIndex < this.PageCount)
                {
                    //Add the 'Next' page link
                    link = new HtmlAnchor();
                    link.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                    link.Attributes.Add("rel", "nofollow");
                    link.InnerText = ">";
                    link.ID = "linkNext";
                    query.PageOffset = this.PageIndex + 1;
                    link.HRef = query.ToString();
                    pnlGotoPage.Controls.Add(link);
                }

                //If there are many more records then 
                //add a paging mechanism to this paging control
                var nextIndex = endIndex + 9;
                nextIndex = nextIndex - (nextIndex%10);
                if (nextIndex + 9 <= this.PageCount)
                {
                    literal = new Literal();
                    literal.Text = "&nbsp;&nbsp;";
                    pnlGotoPage.Controls.Add(literal);

                    link = new HtmlAnchor();
                    link.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                    link.Attributes.Add("rel", "nofollow");
                    //link.ID = "linkPageN1";
                    link.InnerText = nextIndex.ToString() + "+";
                    query.PageOffset = nextIndex;
                    link.HRef = query.ToString();
                    pnlGotoPage.Controls.Add(link);
                }

                //If there are many more records then 
                //add a paging mechanism to this paging control
                if (nextIndex + 19 <= this.PageCount)
                {
                    literal = new Literal();
                    literal.Text = "&nbsp;&nbsp;";
                    pnlGotoPage.Controls.Add(literal);

                    link = new HtmlAnchor();
                    link.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                    link.Attributes.Add("rel", "nofollow");
                    //link.ID = "linkPageN2";
                    link.InnerText = (nextIndex + 10).ToString() + "+";
                    query.PageOffset = (nextIndex + 10);
                    if (this.PageIndex == this.PageCount)
                        pnlGotoPage.Controls.Add(link);
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

        #region Event Handlers

        private void GotoPageClick(object sender, EventArgs e)
        {
            var url = new URL(this.Request.Url.PathAndQuery);
            var pageOffset = int.Parse(url.Parameters.GetValue("po", "1"));

            var button = (LinkButton) sender;
            if (button.Text == "<")
            {
                pageOffset--;
            }
            else if (button.Text == ">")
            {
                pageOffset++;
            }
            else if (button.Text.IndexOf("+") != -1)
            {
                //This is a paging mechanism			
                pageOffset = int.Parse(button.CommandArgument);
            }
            else
            {
                //The actual page number
                pageOffset = int.Parse(button.CommandArgument);
            }

            url.Parameters.SetValue("po", pageOffset.ToString());
            this.Response.Redirect(url.ToString());

        }

        protected void cboPagePer_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.OnRecordsPerPageChanged(new EventArgs());
        }

        #endregion

        #region Property Implementations

        public PlaceHolder SortPlaceHolder
        {
            get { return pnlSort; }
        }

        public int PageIndex
        {
            get
            {
                var url = new PagingUrl(this.InstanceID, this.Request.Url.PathAndQuery);
                return url.PageOffset;
            }
        }

        public int PageCount
        {
            get
            {
                if (this.ViewState["PageCount"] == null)
                    this.ViewState["PageCount"] = 1;
                return (int) this.ViewState["PageCount"];
            }
            set
            {
                this.ViewState["PageCount"] = value;
                this.PopulateClickBar();
            }
        }

        public int RecordsPerPage
        {
            get { return int.Parse(this.rpp.Value); }
        }

        public bool ShowAll
        {
            get { return this.RecordsPerPage == -1; }
        }

        public int ItemCount
        {
            get
            {

                if (this.ViewState["ItemCount"] == null)
                    this.ViewState["ItemCount"] = 0;
                return (int) this.ViewState["ItemCount"];
            }
            set
            {
                this.ViewState["ItemCount"] = value;
                if (this.RecordsPerPage == 0)
                    this.PageCount = 0;
                else
                    this.PageCount = (value/this.RecordsPerPage) + ((value%this.RecordsPerPage == 0 ? 0 : 1));
                this.UpdateDisplay();
            }
        }

        public string ObjectSingular
        {
            get
            {
                if (this.ViewState["ObjectSingular"] == null)
                    this.ViewState["ObjectSingular"] = string.Empty;
                return (string) this.ViewState["ObjectSingular"];
            }
            set { this.ViewState["ObjectSingular"] = value; }
        }

        public string ObjectPlural
        {
            get
            {
                if (this.ViewState["ObjectPlural"] == null)
                    this.ViewState["ObjectPlural"] = string.Empty;
                return (string) this.ViewState["ObjectPlural"];
            }
            set { this.ViewState["ObjectPlural"] = value; }
        }

        public string FoundHeaderText
        {
            get
            {
                if (this.ViewState["FoundHeaderText"] == null)
                    this.ViewState["FoundHeaderText"] = string.Empty;
                return (string) this.ViewState["FoundHeaderText"];
            }
            set { this.ViewState["FoundHeaderText"] = value; }
        }

        public bool AllowHeader
        {
            get
            {
                if (this.ViewState["AllowHeader"] == null)
                    this.ViewState["AllowHeader"] = true;
                return (bool) this.ViewState["AllowHeader"];
            }
            set
            {
                this.ViewState["AllowHeader"] = value;
                pnlHeader.Visible = this.AllowHeader;
            }
        }

        public bool UseH1Tag
        {
            get
            {
                if (this.ViewState["UseH1Tag"] == null)
                    this.ViewState["UseH1Tag"] = true;
                return (bool) this.ViewState["UseH1Tag"];
            }
            set { this.ViewState["UseH1Tag"] = value; }
        }

        #endregion

        #region Methods

        private void UpdateDisplay()
        {
            var text = string.Empty;

            //Determine if this is an H1 tag or not
            if (this.UseH1Tag) text += "<h1>";
            else text += "<span class=\"pagingheader\">";

            var url = new PagingUrl(this.Request.Url.PathAndQuery);
            var windowCount = url.StartRecordIndex + url.RecordsPerPage;
            if (windowCount > this.ItemCount) windowCount = this.ItemCount;
            var prefix = (url.StartRecordIndex + 1) + " - " + windowCount + " of ";

            if (this.FoundHeaderText == string.Empty)
            {
                if (this.ItemCount == 0)
                    text = "No items found";
                else if (this.ItemCount == 1)
                    text += prefix + this.ItemCount.ToString("###,###,##0") + " " + this.ObjectSingular;
                else
                    text += prefix + this.ItemCount.ToString("###,###,##0") + " " + this.ObjectPlural;
            }
            else if (this.ItemCount == 0)
            {
                text += "No items found";
            }
            else
            {
                text += this.FoundHeaderText.Replace("{count}", this.ItemCount.ToString("###,###,##0"));
            }

            //Determine if this is an H1 tag or not
            if (this.UseH1Tag) text += "</h1>";
            else text += "</span>";

            lblFound.Text = text;
            pnlPagePer.Visible = (this.ItemCount > 0);
        }

        #endregion

    }
}