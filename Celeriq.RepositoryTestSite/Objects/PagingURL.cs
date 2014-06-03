namespace Celeriq.RepositoryTestSite.Objects
{
    public enum SortConstants
    {
        Asc = 0,
        Desc = 1,
    }

    /// <summary>
    /// Used to get paging information from a URL (1 based)
    /// </summary>
    public class PagingURL : URL
    {
        private const int DEFAULT_RPP = 10;
        private const int DEFAULT_PO = 1;

        #region Constructors

        public PagingURL(string instanceID, string url)
            : base(url)
        {
            InstanceID = instanceID;
            this.AddDefault(PO_KEY, DEFAULT_PO.ToString());
            this.AddDefault(RPP_KEY, DEFAULT_RPP.ToString());
            this.AddDefault(SD_KEY, "0");
            this.AddDefault(SE_KEY, string.Empty);
            //SetParameter("pg", InstanceID);
        }

        public PagingURL()
            : base()
        {
            this.AddDefault(PO_KEY, DEFAULT_PO.ToString());
            this.AddDefault(RPP_KEY, DEFAULT_RPP.ToString());
            this.AddDefault(SD_KEY, "0");
            this.AddDefault(SE_KEY, string.Empty);
        }

        public PagingURL(string url)
            : base(url)
        {
            this.AddDefault(PO_KEY, DEFAULT_PO.ToString());
            this.AddDefault(RPP_KEY, DEFAULT_RPP.ToString());
            this.AddDefault(SD_KEY, "0");
            this.AddDefault(SE_KEY, string.Empty);
        }

        #endregion

        #region Properties

        public string InstanceID { get; set; }

        private string PO_KEY
        {
            get
            {
                if (!string.IsNullOrEmpty(InstanceID))
                    return "po" + InstanceID;
                return "po";
            }
        }

        private string RPP_KEY
        {
            get
            {
                if (!string.IsNullOrEmpty(InstanceID))
                    return "rpp" + InstanceID;
                return "rpp";
            }
        }

        private string SD_KEY
        {
            get
            {
                if (!string.IsNullOrEmpty(InstanceID))
                    return "sortdir" + InstanceID;
                return "sortdir";
            }
        }

        private string SE_KEY
        {
            get
            {
                if (!string.IsNullOrEmpty(InstanceID))
                    return "sort" + InstanceID;
                return "sort";
            }
        }

        private string TEXT_KEY
        {
            get
            {
                if (!string.IsNullOrEmpty(InstanceID))
                    return "srch" + InstanceID;
                return "srch";
            }
        }

        /// <summary>
        /// The current page
        /// </summary>
        public int PageOffset
        {
            get
            {
                int v;
                int.TryParse(this.Parameters.GetValue(PO_KEY, DEFAULT_PO.ToString()), out v);
                if (v < 1) v = DEFAULT_PO;
                return v;
            }
            set { this.Parameters.SetValue(PO_KEY, value.ToString()); }
        }

        /// <summary>
        /// The number of records per page
        /// </summary>
        public int RecordsPerPage
        {
            get
            {
                int v;
                int.TryParse(this.Parameters.GetValue(RPP_KEY, DEFAULT_RPP.ToString()), out v);
                if ((v != -1) && (v < 1)) v = DEFAULT_RPP;
                return v;
            }
            set { this.Parameters.SetValue(RPP_KEY, value.ToString()); }
        }

        /// <summary>
        /// Returns the starting record index based on PageOffset and RecordsPerPage
        /// </summary>
        public int StartRecordIndex
        {
            get
            {
                if (this.RecordsPerPage == -1)
                    return 1;
                else
                    return ((this.PageOffset - 1)*this.RecordsPerPage);
            }
        }

        public string SortExpression
        {
            get { return this.Parameters.GetValue(SE_KEY); }
            set { this.Parameters.SetValue(SE_KEY, value); }
        }

        /// <summary>
        /// The direction of sorting Asc/Desc
        /// </summary>
        public SortConstants SortDirection
        {
            get
            {
                SortConstants v;
                if (System.Enum.TryParse<SortConstants>(this.Parameters.GetValue(SD_KEY, SortConstants.Asc.ToString("d")), out v))
                    return v;
                return SortConstants.Asc;
            }
            set { this.Parameters.SetValue(SD_KEY, value.ToString("d")); }
        }

        public string Text
        {
            get { return this.Parameters.GetValue(TEXT_KEY); }
            set { this.Parameters.SetValue(TEXT_KEY, value); }
        }

        /// <summary>
        /// The output field to hold total number of records when paging
        /// Methods can use this variable to pass back total record count to calling method
        /// </summary>
        public int TotalItemCount { get; set; }

        /// <summary>
        /// Reverse the sort direction
        /// </summary>
        public void ReverseSort()
        {
            if (this.SortDirection == SortConstants.Desc)
                this.SortDirection = SortConstants.Asc;
            else
                this.SortDirection = SortConstants.Desc;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Reset the paging values to their default value
        /// </summary>
        public void ResetPaging()
        {
            this.PageOffset = DEFAULT_PO;
            this.RecordsPerPage = DEFAULT_RPP;
        }

        #endregion

        #region ICloneable Members

        /// <summary>
        /// Creates a clone of this object
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            var retval = new PagingURL();
            retval.Page = this.Page;

            //Need to create new objects not just add existing one
            foreach (var parameter in this.Parameters)
            {
                retval.Parameters.Add(new URLParameter(parameter.Name, parameter.Value));
            }

            return retval;
        }

        #endregion

    }
}