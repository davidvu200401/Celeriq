using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Celeriq.Common
{
    [Serializable]
    [DataContract]
    [KnownType(typeof (NamedItem))]
    [KnownType(typeof (NamedItemList))]
    [KnownType(typeof (UserCredentials))]
    public abstract class BaseListingQuery : Celeriq.Common.IListingQuery
    {
        protected BaseListingQuery()
        {
            this.DimensionValueList = new List<long>();
            this.FieldFilters = new List<IFieldFilter>();
            this.FieldSorts = new List<IFieldSort>();
            this.PageOffset = 1;
            this.RecordsPerPage = 10;
            this.Keyword = null;
            this.NonParsedFieldList = new NamedItemList();
            this.QueryID = Guid.NewGuid().ToString();
        }

        [DataMember]
        public UserCredentials Credentials { get; set; }

        [DataMember]
        public List<long> DimensionValueList { get; set; }

        [DataMember]
        public int PageOffset { get; set; }

        [DataMember]
        public int RecordsPerPage { get; set; }

        [DataMember]
        public string Keyword { get; set; }

        [DataMember]
        public List<Celeriq.Common.IFieldFilter> FieldFilters { get; set; }

        [DataMember]
        public List<Celeriq.Common.IFieldSort> FieldSorts { get; set; }

        [DataMember]
        public NamedItemList NonParsedFieldList { get; set; }

        [DataMember]
        public string PageName { get; set; }

        [DataMember]
        public string QueryID { get; set; }

        [DataMember]
        public bool IncludeRefinementAgg { get; set; }

        /// <summary>
        /// The IP to use as the source for logging. If missing the requesting IP will be logged
        /// </summary>
        [DataMember]
        public string IPMask { get; set; }

        public override int GetHashCode()
        {
            var hash = string.Empty;
            if (this.DimensionValueList == null || this.DimensionValueList.Count == 0)
                hash += "NULL|";
            else
                hash += string.Join("-", this.DimensionValueList.ToArray()) + "|";

            hash += this.PageOffset + "|" + this.RecordsPerPage + "|" + this.Keyword + "|";

            if (this.FieldFilters == null || this.FieldFilters.Count == 0)
                hash += "NULL|";
            else
            {
                foreach (var o in this.FieldFilters)
                    hash += o.GetHashCode() + "~|";
            }

            if (this.FieldSorts == null || this.FieldSorts.Count == 0)
                hash += "NULL|";
            else
            {
                foreach (var o in this.FieldSorts)
                    hash += o.GetHashCode() + "!|";
            }

            if ((this.NonParsedFieldList == null) || (this.NonParsedFieldList.Count == 0))
                hash += "NULL|";
            else
            {
                foreach (var item in this.NonParsedFieldList)
                    hash += item.Key + "|" + item.Value + "|";
            }

            return Utilities.EncryptionDomain.Hash(hash);
        }

        public void Reset()
        {
            this.DimensionValueList.Clear();
            this.FieldFilters.Clear();
            this.FieldSorts.Clear();
            this.Keyword = null;
            this.NonParsedFieldList.Clear();
            this.PageName = null;
            this.PageOffset = 1;
            this.RecordsPerPage = 10;
        }

        public override string ToString()
        {
            var retval = new StringBuilder();

            #region Dimensions

            if (this.DimensionValueList != null && this.DimensionValueList.Count > 0)
            {
                retval.Append("d=" + string.Join("+", this.DimensionValueList.Select(x => x.ToString())));
            }

            #endregion

            #region Field Filters

            if (this.FieldFilters != null && this.FieldFilters.Count > 0)
            {
                var ffURL = string.Empty;
                foreach (var ff in this.FieldFilters)
                {
                    var fieldName = ff.Name;

                    var f1 = (Celeriq.Common.IFieldFilter) ff;
                    if (ff is Celeriq.Common.GeoCodeFieldFilter)
                    {
                        var gff = ff as Celeriq.Common.GeoCodeFieldFilter;
                        if (gff != null)
                            ffURL += fieldName + "," + ff.Comparer.ToString() + "," + gff.Latitude.ToString() + "," + gff.Longitude.ToString() + "," + gff.Radius.ToString() + "|";
                    }
                    else
                    {
                        if (ff.Comparer == Celeriq.Common.ComparisonConstants.Between)
                        {
                            if ((f1.Value != null) && (f1.Value2 != null))
                            {
                                ffURL += fieldName + "," + ff.Comparer.ToString() + "," + f1.Value.ToString() + "," + f1.Value2.ToString() + "|";
                            }
                        }
                        else
                        {
                            if (f1.Value != null)
                            {
                                ffURL += fieldName + "," + ff.Comparer.ToString() + "," + f1.Value.ToString() + "|";
                            }
                        }
                    }
                }

                ffURL = ffURL.Trim('|');
                if (!string.IsNullOrEmpty(ffURL))
                    retval.Append("&ff=" + ffURL);
            }

            #endregion

            #region Field Sorts

            if (this.FieldSorts != null && this.FieldSorts.Count > 0)
            {
                var fsURL = string.Empty;
                foreach (var fs in this.FieldSorts)
                {
                    fsURL += fs.Name;
                    fsURL += (fs.SortDirection == Celeriq.Common.SortDirectionConstants.Asc ? string.Empty : ",0");
                    fsURL += "|";
                }

                fsURL = fsURL.Trim('|');
                if (!string.IsNullOrEmpty(fsURL))
                    retval.Append("&fs=" + fsURL);
            }

            #endregion

            #region Keyword

            if (!string.IsNullOrEmpty(this.Keyword))
            {
                retval.Append("&srch=" + this.Keyword);
            }

            #endregion

            if (this.NonParsedFieldList != null)
            {
                foreach (var item in this.NonParsedFieldList)
                    retval.Append("&" + item.Key + "=" + item.Value);
            }

            if (this.PageOffset != 1)
                retval.Append("&po=" + this.PageOffset);
            if (this.RecordsPerPage != 10)
                retval.Append("&rpp=" + this.RecordsPerPage);

            var r = retval.ToString();
            r = r.Trim('&');
            if (string.IsNullOrEmpty(this.PageName))
                return r;
            else
                return this.PageName + (string.IsNullOrEmpty(r) ? string.Empty : "?" + r);

        }
    }
}