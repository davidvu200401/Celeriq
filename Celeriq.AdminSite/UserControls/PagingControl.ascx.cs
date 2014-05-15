using System;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Celeriq.AdminSite.Objects;
using Celeriq.Common;

namespace Celeriq.AdminSite.UserControls
{
    public partial class PagingControl : System.Web.UI.UserControl
    {
        #region Class Members

        #endregion

        #region Page Load

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.PopulateClickBar();
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
                var query = new PagingURL(InstanceID, this.Request.Url.PathAndQuery);
                var sb = new StringBuilder();
                sb.AppendLine("<ul class='pagination'>");
                pnlPagingation.Controls.Clear();

                if (this.PageIndex > 1)
                {
                    //Add the 'Previous' page link
                    query.PageOffset = this.PageIndex - 1;
                    sb.Append("<li><a rel='nofollow' href='" + query.ToString() + "'>«</a></li>");
                }

                //Try to position the current PageIndex in the middle of the selection
                var startIndex = this.PageIndex - 5;
                if (startIndex < 1) startIndex = 1;
                var endIndex = startIndex + 9;
                if (endIndex > this.PageCount) endIndex = this.PageCount;

                for (var ii = startIndex; ii <= endIndex; ii++)
                {
                    var classRef = string.Empty;
                    if (ii == this.PageIndex) classRef = " class='active'";
                    query.PageOffset = ii;
                    sb.Append("<li" + classRef + "><a rel='nofollow' href='" + query.ToString() + "'>" + ii.ToString() + "</a></li>");
                }

                if (this.PageIndex < this.PageCount)
                {
                    //Add the 'Next' page link
                    query.PageOffset = this.PageIndex + 1;
                    sb.Append("<li><a rel='nofollow' href='" + query.ToString() + "'>»</a></li>");
                }

                sb.AppendLine("</ul>");

                var l = new Literal();
                l.Text = sb.ToString();
                pnlPagingation.Controls.Add(l);
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

            var button = (LinkButton)sender;
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

        public int PageIndex
        {
            get
            {
                var url = new PagingURL(this.InstanceID, this.Request.Url.PathAndQuery);
                return url.PageOffset;
            }
        }

        public int PageCount
        {
            get
            {
                if (this.ViewState["PageCount"] == null)
                    this.ViewState["PageCount"] = 1;
                return (int)this.ViewState["PageCount"];
            }
            set
            {
                this.ViewState["PageCount"] = value;
                this.PopulateClickBar();
            }
        }

        public int RecordsPerPage
        {
            get { return 10; }
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
                return (int)this.ViewState["ItemCount"];
            }
            set
            {
                this.ViewState["ItemCount"] = value;
                if (this.RecordsPerPage == 0)
                    this.PageCount = 0;
                else
                    this.PageCount = (value / this.RecordsPerPage) + ((value % this.RecordsPerPage == 0 ? 0 : 1));
                this.UpdateDisplay();
                if (value == 0) this.Visible = false;
            }
        }

        public string ObjectSingular
        {
            get
            {
                if (this.ViewState["ObjectSingular"] == null)
                    this.ViewState["ObjectSingular"] = string.Empty;
                return (string)this.ViewState["ObjectSingular"];
            }
            set { this.ViewState["ObjectSingular"] = value; }
        }

        public string ObjectPlural
        {
            get
            {
                if (this.ViewState["ObjectPlural"] == null)
                    this.ViewState["ObjectPlural"] = string.Empty;
                return (string)this.ViewState["ObjectPlural"];
            }
            set { this.ViewState["ObjectPlural"] = value; }
        }

        public string FoundHeaderText
        {
            get
            {
                if (this.ViewState["FoundHeaderText"] == null)
                    this.ViewState["FoundHeaderText"] = string.Empty;
                return (string)this.ViewState["FoundHeaderText"];
            }
            set { this.ViewState["FoundHeaderText"] = value; }
        }

        #endregion

        #region Methods

        private void UpdateDisplay()
        {
            var text = string.Empty;

            //Determine if this is an H1 tag or not
            text += "<span class=\"pagingheader\">";

            var url = new PagingURL(this.Request.Url.PathAndQuery);
            var windowCount = url.StartRecordIndex + url.RecordsPerPage;
            if (windowCount > this.ItemCount) windowCount = this.ItemCount;
            var prefix = (url.StartRecordIndex + 1) + " - " + windowCount + " of ";

            if (this.FoundHeaderText == string.Empty)
            {
                if (this.ItemCount == 0)
                    text = "No items found";
                else if (this.ItemCount == 1)
                    text += prefix + this.ItemCount.FormatNumber() + " " + this.ObjectSingular;
                else
                    text += prefix + this.ItemCount.FormatNumber() + " " + this.ObjectPlural;
            }
            else if (this.ItemCount == 0)
            {
                text += "No items found";
            }
            else
            {
                text += this.FoundHeaderText.Replace("{count}", this.ItemCount.FormatNumber());
            }

            text += "</span>";

            lblFound.Text = text;
        }

        #endregion

    }
}