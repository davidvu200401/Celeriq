using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Celeriq.Common;

namespace Celeriq.RepositoryTestSite.Objects
{
    [Serializable]
    public partial class ListingQuery : Celeriq.Common.BaseListingQuery, System.ICloneable
    {
        partial void LoadFromUrlComplete(string originalUrl);

        public ListingQuery()
            : base()
        {
        }

        internal ListingQuery(Celeriq.Common.DataQuery query)
            : this()
        {
            this.DimensionValueList.AddRange(query.DimensionValueList);
            this.PageOffset = query.PageOffset;
            this.RecordsPerPage = query.RecordsPerPage;
            this.Keyword = query.Keyword;
            this.PageName = query.PageName;
            this.NonParsedFieldList = new NamedItemList();
            if (query.NonParsedFieldList != null)
                this.NonParsedFieldList.AddRange(query.NonParsedFieldList);
        }

        public ListingQuery(string url)
            : this()
        {
            if (string.IsNullOrEmpty(url)) return;
            if (url.Contains("%")) url = System.Web.HttpUtility.UrlDecode(url);
            var originalUrl = url;

            var pageBreak = url.IndexOf('?');
            if (pageBreak != -1 && pageBreak < url.Length - 1)
            {
                this.PageName = url.Substring(0, pageBreak);
                url = url.Substring(pageBreak + 1, url.Length - pageBreak - 1);
            }
            else
            {
                this.PageName = url;
                return;
            }

            #region Parse Query String
            var tuplets = url.Split('&');
            foreach (var gset in tuplets)
            {
                var values = gset.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                if (values.Length == 2)
                {
                    switch (values[0])
                    {
                        case "d":
                            {
                                var dValues = values[1].Split(new char[] { '+', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                foreach (var dvidxV in dValues)
                                {
                                    long dvidx;
                                    if (long.TryParse(dvidxV, out dvidx))
                                        this.DimensionValueList.Add(dvidx);
                                }
                            }
                            break;
                        case "po":
                            {
                                int po;
                                if (int.TryParse(values[1], out po))
                                    this.PageOffset = po;
                            }
                            break;
                        case "rpp":
                            {
                                int rpp;
                                if (int.TryParse(values[1], out rpp))
                                    this.RecordsPerPage = rpp;
                            }
                            break;
                        case "srch":
                            this.Keyword = values[1];
                            break;
                        default:
                            if (values.Length >= 2)
                            {
                                if (this.NonParsedFieldList.Count(x => x.Key == values[0]) > 0)
                                    this.NonParsedFieldList.First(x => x.Key == values[0]).Value = values[1];
                                else
                                    this.NonParsedFieldList.Add(new NamedItem() { Key = values[0], Value = values[1] });
                            }
                            break;
                    }
                }
            }
            #endregion

            LoadFromUrlComplete(originalUrl);

        }

        public Celeriq.Common.DataQuery ToTransfer()
        {
            var retval = new Celeriq.Common.DataQuery();
            retval.DimensionValueList = this.DimensionValueList;
            retval.Credentials = this.Credentials;
            retval.IPMask = this.IPMask;

            retval.FieldFilters = new List<Celeriq.Common.IFieldFilter>();
            if (this.FieldFilters == null)
                this.FieldFilters = new List<IFieldFilter>();
            foreach (var o in this.FieldFilters)
            {
                if (o is Celeriq.Common.GeoCodeFieldFilter)
                {
                    var newFilter = new Celeriq.Common.GeoCodeFieldFilter()
                    {
                        Latitude = ((Celeriq.Common.GeoCodeFieldFilter)o).Latitude,
                        Longitude = ((Celeriq.Common.GeoCodeFieldFilter)o).Longitude,
                        Radius = ((Celeriq.Common.GeoCodeFieldFilter)o).Radius,
                        Comparer = o.Comparer,
                        Name = o.Name,
                    };
                    ((Celeriq.Common.IFieldFilter)newFilter).Value = ((Celeriq.Common.IFieldFilter)o).Value;
                    ((Celeriq.Common.IFieldFilter)newFilter).Value2 = ((Celeriq.Common.IFieldFilter)o).Value2;
                    retval.FieldFilters.Add(newFilter);
                }
                else
                {
                    var newFilter = new Celeriq.Common.FieldFilter()
                    {
                        Comparer = o.Comparer,
                        Name = o.Name,
                    };
                    ((Celeriq.Common.IFieldFilter)newFilter).Value = ((Celeriq.Common.IFieldFilter)o).Value;
                    ((Celeriq.Common.IFieldFilter)newFilter).Value2 = ((Celeriq.Common.IFieldFilter)o).Value2;
                    retval.FieldFilters.Add(newFilter);
                }
            }

            retval.FieldSorts = new List<Celeriq.Common.IFieldSort>();
            if (this.FieldSorts == null)
                this.FieldSorts = new List<IFieldSort>();
            foreach (var o in this.FieldSorts)
            {
                retval.FieldSorts.Add(new Celeriq.Common.FieldSort()
                {
                    Name = o.Name,
                    SortDirection = o.SortDirection,
                });
            }

            if (this.NonParsedFieldList != null)
            {
                this.NonParsedFieldList.ForEach(x => retval.NonParsedFieldList.Add(x));
            }

            retval.Keyword = this.Keyword;
            retval.PageOffset = this.PageOffset;
            retval.RecordsPerPage = this.RecordsPerPage;
            return retval;
        }

        public override string ToString()
        {
            var retval = string.Empty;

            #region Dimensions
            if (this.DimensionValueList != null && this.DimensionValueList.Count > 0)
            {
                retval += "d=" + string.Join("+", this.DimensionValueList.Select(x => x.ToString()));
            }
            #endregion

            #region Keyword
            if (!string.IsNullOrEmpty(this.Keyword))
            {
                retval += "&srch=" + this.Keyword;
            }
            #endregion

            if (this.PageOffset != 1)
                retval += "&po=" + this.PageOffset;
            if (this.RecordsPerPage != 10)
                retval += "&rpp=" + this.RecordsPerPage;

            #region NonParsed Field
            if (this.NonParsedFieldList != null)
            {
                foreach (var item in this.NonParsedFieldList.Where(x => !string.IsNullOrEmpty(x.Value)))
                {
                    retval += "&" + item.Key + "=" + item.Value;
                }
            }
            #endregion

            retval = retval.Trim('&');

            if (string.IsNullOrEmpty(this.PageName))
                return retval;
            else if (!string.IsNullOrEmpty(retval))
                return this.PageName + "?" + retval;
            else
                return this.PageName;
        }

        partial void Cloning(ListingQuery query);

        object ICloneable.Clone()
        {
            return this.Clone();
        }

        public ListingQuery Clone()
        {
            var retval = new ListingQuery();
            retval.DimensionValueList.AddRange(this.DimensionValueList);
            retval.FieldFilters.AddRange(this.FieldFilters);
            retval.FieldSorts.AddRange(this.FieldSorts);
            retval.Keyword = this.Keyword;
            retval.NonParsedFieldList.AddRange(this.NonParsedFieldList);
            retval.PageOffset = this.PageOffset;
            retval.PageName = this.PageName;
            retval.RecordsPerPage = this.RecordsPerPage;
            this.Cloning(retval);
            return retval;
        }

    }
}