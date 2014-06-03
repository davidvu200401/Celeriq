namespace LoadTest
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.ServiceModel;
    using Celeriq.Common;

    [Serializable]
    public partial class ListingItem : Celeriq.Common.IListingItem
    {
        public ListingItem()
        {
            _dimensionList = new List<DimensionItem>();
        }

        internal ListingItem(Celeriq.Common.DataItem item, ListingResults results)
            : this()
        {
            this.ID = (int)item.ItemArray[0];
            this.Field1 = (string)item.ItemArray[1];
            this.Field2 = (string)item.ItemArray[2];
            this.Field3 = (string)item.ItemArray[3];
            this.Field4 = (string)item.ItemArray[4];
            this.Field5 = (string)item.ItemArray[5];
            this.Dim1 = (string)item.ItemArray[6];
            this.Dim2 = (string)item.ItemArray[7];
            this.Dim3 = (string)item.ItemArray[8];
            this.Dim4 = (string)item.ItemArray[9];
            this.Dim5 = (string)item.ItemArray[10];

            RefinementItem r;
            r = results.AllDimensions.Dim1Dimension.GetRefinementByValue(this.Dim1);
            if (r != null && this.Dim1 != null)
            {
                var d = ((System.ICloneable)results.AllDimensions.Dim1Dimension).Clone() as DimensionItem;
                d.RefinementList.RemoveAll(x => x.FieldValue != this.Dim1);
                d.RefinementList.First().Count = 1;
                _dimensionList.Add(d);
            }

            r = results.AllDimensions.Dim2Dimension.GetRefinementByValue(this.Dim2);
            if (r != null && this.Dim2 != null)
            {
                var d = ((System.ICloneable)results.AllDimensions.Dim2Dimension).Clone() as DimensionItem;
                d.RefinementList.RemoveAll(x => x.FieldValue != this.Dim2);
                d.RefinementList.First().Count = 1;
                _dimensionList.Add(d);
            }

            r = results.AllDimensions.Dim3Dimension.GetRefinementByValue(this.Dim3);
            if (r != null && this.Dim3 != null)
            {
                var d = ((System.ICloneable)results.AllDimensions.Dim3Dimension).Clone() as DimensionItem;
                d.RefinementList.RemoveAll(x => x.FieldValue != this.Dim3);
                d.RefinementList.First().Count = 1;
                _dimensionList.Add(d);
            }

            r = results.AllDimensions.Dim4Dimension.GetRefinementByValue(this.Dim4);
            if (r != null && this.Dim4 != null)
            {
                var d = ((System.ICloneable)results.AllDimensions.Dim4Dimension).Clone() as DimensionItem;
                d.RefinementList.RemoveAll(x => x.FieldValue != this.Dim4);
                d.RefinementList.First().Count = 1;
                _dimensionList.Add(d);
            }

            r = results.AllDimensions.Dim5Dimension.GetRefinementByValue(this.Dim5);
            if (r != null && this.Dim5 != null)
            {
                var d = ((System.ICloneable)results.AllDimensions.Dim5Dimension).Clone() as DimensionItem;
                d.RefinementList.RemoveAll(x => x.FieldValue != this.Dim5);
                d.RefinementList.First().Count = 1;
                _dimensionList.Add(d);
            }

        }

        public int ID { get; set; }
        public string Field1 { get; set; }
        public string Field2 { get; set; }
        public string Field3 { get; set; }
        public string Field4 { get; set; }
        public string Field5 { get; set; }
        public string Dim1 { get; set; }
        public string Dim2 { get; set; }
        public string Dim3 { get; set; }
        public string Dim4 { get; set; }
        public string Dim5 { get; set; }
        internal List<DimensionItem> _dimensionList = new List<DimensionItem>();
        public IEnumerable<DimensionItem> DimensionList
        {
            get { return _dimensionList; }
        }

        private static long FloorToNearest(long d, long n)
        {
            if (n <= 0) throw new Exception("The rounding operation cannot be performed.");
            return d - (d % n);
        }

        public Celeriq.Common.DataItem ToTransfer()
        {
            var retval = new Celeriq.Common.DataItem();
            retval.ItemArray = new object[] {
                this.ID,
                this.Field1,
                this.Field2,
                this.Field3,
                this.Field4,
                this.Field5,
                this.Dim1,
                this.Dim2,
                this.Dim3,
                this.Dim4,
                this.Dim5,
            };
            return retval;
        }
    }

    [Serializable]
    public partial class ListingQuery : Celeriq.Common.BaseListingQuery, System.ICloneable
    {
        partial void LoadFromUrlComplete(string originalUrl);
        partial void PostToString(ref string url);
        public bool UseDefaults { get; set; }

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

            #region Field Filters
            if (query.FieldFilters != null)
            {
                foreach (var ff in query.FieldFilters)
                {
                    switch (ff.Name)
                    {
                        case "ID":
                            this.FieldFilters.Add(new FieldFilterID { ID = (int?)ff.Value, Comparer = ff.Comparer });
                            break;
                        case "Field1":
                            this.FieldFilters.Add(new FieldFilterField1 { Field1 = (string)ff.Value, Comparer = ff.Comparer });
                            break;
                        case "Field2":
                            this.FieldFilters.Add(new FieldFilterField2 { Field2 = (string)ff.Value, Comparer = ff.Comparer });
                            break;
                        case "Field3":
                            this.FieldFilters.Add(new FieldFilterField3 { Field3 = (string)ff.Value, Comparer = ff.Comparer });
                            break;
                        case "Field4":
                            this.FieldFilters.Add(new FieldFilterField4 { Field4 = (string)ff.Value, Comparer = ff.Comparer });
                            break;
                        case "Field5":
                            this.FieldFilters.Add(new FieldFilterField5 { Field5 = (string)ff.Value, Comparer = ff.Comparer });
                            break;
                        case "Dim1":
                            this.FieldFilters.Add(new FieldFilterDim1 { Dim1 = (string)ff.Value, Comparer = ff.Comparer });
                            break;
                        case "Dim2":
                            this.FieldFilters.Add(new FieldFilterDim2 { Dim2 = (string)ff.Value, Comparer = ff.Comparer });
                            break;
                        case "Dim3":
                            this.FieldFilters.Add(new FieldFilterDim3 { Dim3 = (string)ff.Value, Comparer = ff.Comparer });
                            break;
                        case "Dim4":
                            this.FieldFilters.Add(new FieldFilterDim4 { Dim4 = (string)ff.Value, Comparer = ff.Comparer });
                            break;
                        case "Dim5":
                            this.FieldFilters.Add(new FieldFilterDim5 { Dim5 = (string)ff.Value, Comparer = ff.Comparer });
                            break;
                        default:
                            throw new Exception("Unknown filter!");
                    }
                }
            }
            #endregion

            #region Field Sorts
            if (query.FieldSorts != null)
            {
                foreach (var fs in query.FieldSorts)
                {
                    switch (fs.Name)
                    {
                        case "ID":
                            this.FieldSorts.Add(new FieldSortID { Name = fs.Name, SortDirection = fs.SortDirection });
                            break;
                        case "Field1":
                            this.FieldSorts.Add(new FieldSortField1 { Name = fs.Name, SortDirection = fs.SortDirection });
                            break;
                        case "Field2":
                            this.FieldSorts.Add(new FieldSortField2 { Name = fs.Name, SortDirection = fs.SortDirection });
                            break;
                        case "Field3":
                            this.FieldSorts.Add(new FieldSortField3 { Name = fs.Name, SortDirection = fs.SortDirection });
                            break;
                        case "Field4":
                            this.FieldSorts.Add(new FieldSortField4 { Name = fs.Name, SortDirection = fs.SortDirection });
                            break;
                        case "Field5":
                            this.FieldSorts.Add(new FieldSortField5 { Name = fs.Name, SortDirection = fs.SortDirection });
                            break;
                        case "Dim1":
                            this.FieldSorts.Add(new FieldSortDim1 { Name = fs.Name, SortDirection = fs.SortDirection });
                            break;
                        case "Dim2":
                            this.FieldSorts.Add(new FieldSortDim2 { Name = fs.Name, SortDirection = fs.SortDirection });
                            break;
                        case "Dim3":
                            this.FieldSorts.Add(new FieldSortDim3 { Name = fs.Name, SortDirection = fs.SortDirection });
                            break;
                        case "Dim4":
                            this.FieldSorts.Add(new FieldSortDim4 { Name = fs.Name, SortDirection = fs.SortDirection });
                            break;
                        case "Dim5":
                            this.FieldSorts.Add(new FieldSortDim5 { Name = fs.Name, SortDirection = fs.SortDirection });
                            break;
                        default:
                            throw new Exception("Unknown sort!");
                    }
                }
            }
            #endregion

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
                        case "ff":
                            {
                                var filters = values[1].Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                                foreach (var s in filters)
                                {
                                    var svalues = s.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                                    if (svalues.Length >= 3)
                                    {
                                        Celeriq.Common.ComparisonConstants enumValue;
                                        if (Enum.TryParse<Celeriq.Common.ComparisonConstants>(svalues[1], true, out enumValue))
                                        {
                                            switch (svalues[0].ToLower())
                                            {
                                                case "dim1":
                                                    this.FieldFilters.Add(new FieldFilterDim1() { Name = "Dim1", Comparer = enumValue, Dim1 = svalues[2] });
                                                    if (enumValue == Celeriq.Common.ComparisonConstants.Between)
                                                    {
                                                        if (svalues.Length > 3)
                                                            this.FieldFilters.Last().Value2 = svalues[3];
                                                        else
                                                            throw new Exception("The URL for field filter 'Dim1' is invalid!");
                                                    }
                                                    break;
                                                case "dim2":
                                                    this.FieldFilters.Add(new FieldFilterDim2() { Name = "Dim2", Comparer = enumValue, Dim2 = svalues[2] });
                                                    if (enumValue == Celeriq.Common.ComparisonConstants.Between)
                                                    {
                                                        if (svalues.Length > 3)
                                                            this.FieldFilters.Last().Value2 = svalues[3];
                                                        else
                                                            throw new Exception("The URL for field filter 'Dim2' is invalid!");
                                                    }
                                                    break;
                                                case "dim3":
                                                    this.FieldFilters.Add(new FieldFilterDim3() { Name = "Dim3", Comparer = enumValue, Dim3 = svalues[2] });
                                                    if (enumValue == Celeriq.Common.ComparisonConstants.Between)
                                                    {
                                                        if (svalues.Length > 3)
                                                            this.FieldFilters.Last().Value2 = svalues[3];
                                                        else
                                                            throw new Exception("The URL for field filter 'Dim3' is invalid!");
                                                    }
                                                    break;
                                                case "dim4":
                                                    this.FieldFilters.Add(new FieldFilterDim4() { Name = "Dim4", Comparer = enumValue, Dim4 = svalues[2] });
                                                    if (enumValue == Celeriq.Common.ComparisonConstants.Between)
                                                    {
                                                        if (svalues.Length > 3)
                                                            this.FieldFilters.Last().Value2 = svalues[3];
                                                        else
                                                            throw new Exception("The URL for field filter 'Dim4' is invalid!");
                                                    }
                                                    break;
                                                case "dim5":
                                                    this.FieldFilters.Add(new FieldFilterDim5() { Name = "Dim5", Comparer = enumValue, Dim5 = svalues[2] });
                                                    if (enumValue == Celeriq.Common.ComparisonConstants.Between)
                                                    {
                                                        if (svalues.Length > 3)
                                                            this.FieldFilters.Last().Value2 = svalues[3];
                                                        else
                                                            throw new Exception("The URL for field filter 'Dim5' is invalid!");
                                                    }
                                                    break;
                                                case "field1":
                                                    this.FieldFilters.Add(new FieldFilterField1() { Name = "Field1", Comparer = enumValue, Field1 = svalues[2] });
                                                    if (enumValue == Celeriq.Common.ComparisonConstants.Between)
                                                    {
                                                        if (svalues.Length > 3)
                                                            this.FieldFilters.Last().Value2 = svalues[3];
                                                        else
                                                            throw new Exception("The URL for field filter 'Field1' is invalid!");
                                                    }
                                                    break;
                                                case "field2":
                                                    this.FieldFilters.Add(new FieldFilterField2() { Name = "Field2", Comparer = enumValue, Field2 = svalues[2] });
                                                    if (enumValue == Celeriq.Common.ComparisonConstants.Between)
                                                    {
                                                        if (svalues.Length > 3)
                                                            this.FieldFilters.Last().Value2 = svalues[3];
                                                        else
                                                            throw new Exception("The URL for field filter 'Field2' is invalid!");
                                                    }
                                                    break;
                                                case "field3":
                                                    this.FieldFilters.Add(new FieldFilterField3() { Name = "Field3", Comparer = enumValue, Field3 = svalues[2] });
                                                    if (enumValue == Celeriq.Common.ComparisonConstants.Between)
                                                    {
                                                        if (svalues.Length > 3)
                                                            this.FieldFilters.Last().Value2 = svalues[3];
                                                        else
                                                            throw new Exception("The URL for field filter 'Field3' is invalid!");
                                                    }
                                                    break;
                                                case "field4":
                                                    this.FieldFilters.Add(new FieldFilterField4() { Name = "Field4", Comparer = enumValue, Field4 = svalues[2] });
                                                    if (enumValue == Celeriq.Common.ComparisonConstants.Between)
                                                    {
                                                        if (svalues.Length > 3)
                                                            this.FieldFilters.Last().Value2 = svalues[3];
                                                        else
                                                            throw new Exception("The URL for field filter 'Field4' is invalid!");
                                                    }
                                                    break;
                                                case "field5":
                                                    this.FieldFilters.Add(new FieldFilterField5() { Name = "Field5", Comparer = enumValue, Field5 = svalues[2] });
                                                    if (enumValue == Celeriq.Common.ComparisonConstants.Between)
                                                    {
                                                        if (svalues.Length > 3)
                                                            this.FieldFilters.Last().Value2 = svalues[3];
                                                        else
                                                            throw new Exception("The URL for field filter 'Field5' is invalid!");
                                                    }
                                                    break;
                                                case "id":
                                                    int idValue;
                                                    if (int.TryParse(svalues[2], out idValue))
                                                        this.FieldFilters.Add(new FieldFilterID() { Name = "ID", Comparer = enumValue, ID = idValue });
                                                    if (enumValue == Celeriq.Common.ComparisonConstants.Between)
                                                    {
                                                        if (svalues.Length > 3)
                                                        {
                                                            if (int.TryParse(svalues[3], out idValue))
                                                                this.FieldFilters.Last().Value2 = idValue;
                                                            else
                                                                throw new Exception("The URL for field filter 'ID' is invalid!");
                                                        }
                                                        else
                                                            throw new Exception("The URL for field filter 'ID' is invalid!");
                                                    }
                                                    break;
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                        case "fs":
                            {
                                var sorts = values[1].Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                                foreach (var s in sorts)
                                {
                                    var svalues = s.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                                    if (svalues.Length > 0)
                                    {
                                        switch (svalues[0].ToLower())
                                        {
                                            case "dim1":
                                                this.FieldSorts.Add(new FieldSortDim1() { Name = "Dim1", SortDirection = (svalues.Length == 1 || svalues[1] != "0" ? Celeriq.Common.SortDirectionConstants.Asc : Celeriq.Common.SortDirectionConstants.Desc) });
                                                break;
                                            case "dim2":
                                                this.FieldSorts.Add(new FieldSortDim2() { Name = "Dim2", SortDirection = (svalues.Length == 1 || svalues[1] != "0" ? Celeriq.Common.SortDirectionConstants.Asc : Celeriq.Common.SortDirectionConstants.Desc) });
                                                break;
                                            case "dim3":
                                                this.FieldSorts.Add(new FieldSortDim3() { Name = "Dim3", SortDirection = (svalues.Length == 1 || svalues[1] != "0" ? Celeriq.Common.SortDirectionConstants.Asc : Celeriq.Common.SortDirectionConstants.Desc) });
                                                break;
                                            case "dim4":
                                                this.FieldSorts.Add(new FieldSortDim4() { Name = "Dim4", SortDirection = (svalues.Length == 1 || svalues[1] != "0" ? Celeriq.Common.SortDirectionConstants.Asc : Celeriq.Common.SortDirectionConstants.Desc) });
                                                break;
                                            case "dim5":
                                                this.FieldSorts.Add(new FieldSortDim5() { Name = "Dim5", SortDirection = (svalues.Length == 1 || svalues[1] != "0" ? Celeriq.Common.SortDirectionConstants.Asc : Celeriq.Common.SortDirectionConstants.Desc) });
                                                break;
                                            case "field1":
                                                this.FieldSorts.Add(new FieldSortField1() { Name = "Field1", SortDirection = (svalues.Length == 1 || svalues[1] != "0" ? Celeriq.Common.SortDirectionConstants.Asc : Celeriq.Common.SortDirectionConstants.Desc) });
                                                break;
                                            case "field2":
                                                this.FieldSorts.Add(new FieldSortField2() { Name = "Field2", SortDirection = (svalues.Length == 1 || svalues[1] != "0" ? Celeriq.Common.SortDirectionConstants.Asc : Celeriq.Common.SortDirectionConstants.Desc) });
                                                break;
                                            case "field3":
                                                this.FieldSorts.Add(new FieldSortField3() { Name = "Field3", SortDirection = (svalues.Length == 1 || svalues[1] != "0" ? Celeriq.Common.SortDirectionConstants.Asc : Celeriq.Common.SortDirectionConstants.Desc) });
                                                break;
                                            case "field4":
                                                this.FieldSorts.Add(new FieldSortField4() { Name = "Field4", SortDirection = (svalues.Length == 1 || svalues[1] != "0" ? Celeriq.Common.SortDirectionConstants.Asc : Celeriq.Common.SortDirectionConstants.Desc) });
                                                break;
                                            case "field5":
                                                this.FieldSorts.Add(new FieldSortField5() { Name = "Field5", SortDirection = (svalues.Length == 1 || svalues[1] != "0" ? Celeriq.Common.SortDirectionConstants.Asc : Celeriq.Common.SortDirectionConstants.Desc) });
                                                break;
                                            case "id":
                                                this.FieldSorts.Add(new FieldSortID() { Name = "ID", SortDirection = (svalues.Length == 1 || svalues[1] != "0" ? Celeriq.Common.SortDirectionConstants.Asc : Celeriq.Common.SortDirectionConstants.Desc) });
                                                break;
                                        }
                                    }
                                }
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

        public static Celeriq.Common.FieldSort GetSortByName(string name)
        {
            if (name.ToLower() == "ID".ToLower()) return new FieldSortID() { Name = "ID", SortDirection = Celeriq.Common.SortDirectionConstants.Asc };
            if (name.ToLower() == "Field1".ToLower()) return new FieldSortField1() { Name = "Field1", SortDirection = Celeriq.Common.SortDirectionConstants.Asc };
            if (name.ToLower() == "Field2".ToLower()) return new FieldSortField2() { Name = "Field2", SortDirection = Celeriq.Common.SortDirectionConstants.Asc };
            if (name.ToLower() == "Field3".ToLower()) return new FieldSortField3() { Name = "Field3", SortDirection = Celeriq.Common.SortDirectionConstants.Asc };
            if (name.ToLower() == "Field4".ToLower()) return new FieldSortField4() { Name = "Field4", SortDirection = Celeriq.Common.SortDirectionConstants.Asc };
            if (name.ToLower() == "Field5".ToLower()) return new FieldSortField5() { Name = "Field5", SortDirection = Celeriq.Common.SortDirectionConstants.Asc };
            if (name.ToLower() == "Dim1".ToLower()) return new FieldSortDim1() { Name = "Dim1", SortDirection = Celeriq.Common.SortDirectionConstants.Asc };
            if (name.ToLower() == "Dim2".ToLower()) return new FieldSortDim2() { Name = "Dim2", SortDirection = Celeriq.Common.SortDirectionConstants.Asc };
            if (name.ToLower() == "Dim3".ToLower()) return new FieldSortDim3() { Name = "Dim3", SortDirection = Celeriq.Common.SortDirectionConstants.Asc };
            if (name.ToLower() == "Dim4".ToLower()) return new FieldSortDim4() { Name = "Dim4", SortDirection = Celeriq.Common.SortDirectionConstants.Asc };
            if (name.ToLower() == "Dim5".ToLower()) return new FieldSortDim5() { Name = "Dim5", SortDirection = Celeriq.Common.SortDirectionConstants.Asc };
            return null;
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

            #region Field Filters
            if (this.FieldFilters != null && this.FieldFilters.Count > 0)
            {
                var ffURL = string.Empty;
                foreach (var ff in this.FieldFilters)
                {
                    var fieldName = string.Empty;
                    if (1 == 0) ;
                    else if (ff is FieldFilterDim1) fieldName = "Dim1";
                    else if (ff is FieldFilterDim2) fieldName = "Dim2";
                    else if (ff is FieldFilterDim3) fieldName = "Dim3";
                    else if (ff is FieldFilterDim4) fieldName = "Dim4";
                    else if (ff is FieldFilterDim5) fieldName = "Dim5";
                    else if (ff is FieldFilterField1) fieldName = "Field1";
                    else if (ff is FieldFilterField2) fieldName = "Field2";
                    else if (ff is FieldFilterField3) fieldName = "Field3";
                    else if (ff is FieldFilterField4) fieldName = "Field4";
                    else if (ff is FieldFilterField5) fieldName = "Field5";
                    else if (ff is FieldFilterID) fieldName = "ID";
                    else throw new Exception("The filter type is not valid!");

                    var f1 = (Celeriq.Common.IFieldFilter)ff;
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
                    retval += "&ff=" + ffURL;
            }
            #endregion

            #region Field Sorts
            if (this.FieldSorts != null && this.FieldSorts.Count > 0)
            {
                var fsURL = string.Empty;
                foreach (var fs in this.FieldSorts)
                {
                    if (1 == 0) ;
                    else if (fs is FieldSortDim1) fsURL += "Dim1";
                    else if (fs is FieldSortDim2) fsURL += "Dim2";
                    else if (fs is FieldSortDim3) fsURL += "Dim3";
                    else if (fs is FieldSortDim4) fsURL += "Dim4";
                    else if (fs is FieldSortDim5) fsURL += "Dim5";
                    else if (fs is FieldSortField1) fsURL += "Field1";
                    else if (fs is FieldSortField2) fsURL += "Field2";
                    else if (fs is FieldSortField3) fsURL += "Field3";
                    else if (fs is FieldSortField4) fsURL += "Field4";
                    else if (fs is FieldSortField5) fsURL += "Field5";
                    else if (fs is FieldSortID) fsURL += "ID";
                    else throw new Exception("The sort type is not valid!");
                    fsURL += (fs.SortDirection == Celeriq.Common.SortDirectionConstants.Asc ? string.Empty : ",0");
                }

                fsURL = fsURL.Trim('|');
                if (!string.IsNullOrEmpty(fsURL))
                    retval += "&fs=" + fsURL;
            }
            #endregion

            #region Keyword
            if (!string.IsNullOrEmpty(this.Keyword))
            {
                retval += "&srch=" + this.Keyword;
            }
            #endregion

            if (this.PageOffset != 1 || this.UseDefaults)
                retval += "&po=" + this.PageOffset;
            if (this.RecordsPerPage != 10 || this.UseDefaults)
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
            if (string.IsNullOrEmpty(this.PageName))
            {
                //Do Nothing
            }
            else if (!string.IsNullOrEmpty(retval))
            {
                retval = this.PageName + "?" + retval;
            }
            else
            {
                retval = this.PageName;
            }

            PostToString(ref retval);

            return retval;
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

    #region Filtering

    [Serializable]
    public sealed class FieldFilterID : Celeriq.Common.FieldFilter, Celeriq.Common.IFieldFilter
    {
        public FieldFilterID() : base("ID") { }
        public int? ID { get; set; }
        object Celeriq.Common.IFieldFilter.Value
        {
            get { return this.ID; }
            set { this.ID = (int?)value; }
        }
    }

    [Serializable]
    public sealed class FieldFilterField1 : Celeriq.Common.FieldFilter, Celeriq.Common.IFieldFilter
    {
        public FieldFilterField1() : base("Field1") { }
        public string Field1 { get; set; }
        object Celeriq.Common.IFieldFilter.Value
        {
            get { return this.Field1; }
            set { this.Field1 = (string)value; }
        }
    }

    [Serializable]
    public sealed class FieldFilterField2 : Celeriq.Common.FieldFilter, Celeriq.Common.IFieldFilter
    {
        public FieldFilterField2() : base("Field2") { }
        public string Field2 { get; set; }
        object Celeriq.Common.IFieldFilter.Value
        {
            get { return this.Field2; }
            set { this.Field2 = (string)value; }
        }
    }

    [Serializable]
    public sealed class FieldFilterField3 : Celeriq.Common.FieldFilter, Celeriq.Common.IFieldFilter
    {
        public FieldFilterField3() : base("Field3") { }
        public string Field3 { get; set; }
        object Celeriq.Common.IFieldFilter.Value
        {
            get { return this.Field3; }
            set { this.Field3 = (string)value; }
        }
    }

    [Serializable]
    public sealed class FieldFilterField4 : Celeriq.Common.FieldFilter, Celeriq.Common.IFieldFilter
    {
        public FieldFilterField4() : base("Field4") { }
        public string Field4 { get; set; }
        object Celeriq.Common.IFieldFilter.Value
        {
            get { return this.Field4; }
            set { this.Field4 = (string)value; }
        }
    }

    [Serializable]
    public sealed class FieldFilterField5 : Celeriq.Common.FieldFilter, Celeriq.Common.IFieldFilter
    {
        public FieldFilterField5() : base("Field5") { }
        public string Field5 { get; set; }
        object Celeriq.Common.IFieldFilter.Value
        {
            get { return this.Field5; }
            set { this.Field5 = (string)value; }
        }
    }

    [Serializable]
    public sealed class FieldFilterDim1 : Celeriq.Common.FieldFilter, Celeriq.Common.IFieldFilter
    {
        public FieldFilterDim1() : base("Dim1") { }
        public string Dim1 { get; set; }
        object Celeriq.Common.IFieldFilter.Value
        {
            get { return this.Dim1; }
            set { this.Dim1 = (string)value; }
        }
    }

    [Serializable]
    public sealed class FieldFilterDim2 : Celeriq.Common.FieldFilter, Celeriq.Common.IFieldFilter
    {
        public FieldFilterDim2() : base("Dim2") { }
        public string Dim2 { get; set; }
        object Celeriq.Common.IFieldFilter.Value
        {
            get { return this.Dim2; }
            set { this.Dim2 = (string)value; }
        }
    }

    [Serializable]
    public sealed class FieldFilterDim3 : Celeriq.Common.FieldFilter, Celeriq.Common.IFieldFilter
    {
        public FieldFilterDim3() : base("Dim3") { }
        public string Dim3 { get; set; }
        object Celeriq.Common.IFieldFilter.Value
        {
            get { return this.Dim3; }
            set { this.Dim3 = (string)value; }
        }
    }

    [Serializable]
    public sealed class FieldFilterDim4 : Celeriq.Common.FieldFilter, Celeriq.Common.IFieldFilter
    {
        public FieldFilterDim4() : base("Dim4") { }
        public string Dim4 { get; set; }
        object Celeriq.Common.IFieldFilter.Value
        {
            get { return this.Dim4; }
            set { this.Dim4 = (string)value; }
        }
    }

    [Serializable]
    public sealed class FieldFilterDim5 : Celeriq.Common.FieldFilter, Celeriq.Common.IFieldFilter
    {
        public FieldFilterDim5() : base("Dim5") { }
        public string Dim5 { get; set; }
        object Celeriq.Common.IFieldFilter.Value
        {
            get { return this.Dim5; }
            set { this.Dim5 = (string)value; }
        }
    }

    #endregion

    #region Sorting

    [Serializable]
    public sealed class FieldSortID : Celeriq.Common.FieldSort, Celeriq.Common.IFieldSort
    {
        public FieldSortID() : base("ID") { }
    }

    [Serializable]
    public sealed class FieldSortField1 : Celeriq.Common.FieldSort, Celeriq.Common.IFieldSort
    {
        public FieldSortField1() : base("Field1") { }
    }

    [Serializable]
    public sealed class FieldSortField2 : Celeriq.Common.FieldSort, Celeriq.Common.IFieldSort
    {
        public FieldSortField2() : base("Field2") { }
    }

    [Serializable]
    public sealed class FieldSortField3 : Celeriq.Common.FieldSort, Celeriq.Common.IFieldSort
    {
        public FieldSortField3() : base("Field3") { }
    }

    [Serializable]
    public sealed class FieldSortField4 : Celeriq.Common.FieldSort, Celeriq.Common.IFieldSort
    {
        public FieldSortField4() : base("Field4") { }
    }

    [Serializable]
    public sealed class FieldSortField5 : Celeriq.Common.FieldSort, Celeriq.Common.IFieldSort
    {
        public FieldSortField5() : base("Field5") { }
    }

    [Serializable]
    public sealed class FieldSortDim1 : Celeriq.Common.FieldSort, Celeriq.Common.IFieldSort
    {
        public FieldSortDim1() : base("Dim1") { }
    }

    [Serializable]
    public sealed class FieldSortDim2 : Celeriq.Common.FieldSort, Celeriq.Common.IFieldSort
    {
        public FieldSortDim2() : base("Dim2") { }
    }

    [Serializable]
    public sealed class FieldSortDim3 : Celeriq.Common.FieldSort, Celeriq.Common.IFieldSort
    {
        public FieldSortDim3() : base("Dim3") { }
    }

    [Serializable]
    public sealed class FieldSortDim4 : Celeriq.Common.FieldSort, Celeriq.Common.IFieldSort
    {
        public FieldSortDim4() : base("Dim4") { }
    }

    [Serializable]
    public sealed class FieldSortDim5 : Celeriq.Common.FieldSort, Celeriq.Common.IFieldSort
    {
        public FieldSortDim5() : base("Dim5") { }
    }

    #endregion

    //public class WrongVersionException : System.Exception
    //{
    //    public WrongVersionException() : base() { }

    //    public WrongVersionException(long hash, long queryHash)
    //        : base()
    //    {
    //        this.Hash = hash;
    //        this.QueryHash = queryHash;
    //    }

    //    public long Hash { get; private set; }
    //    public long QueryHash { get; private set; }

    //    public override string Message
    //    {
    //        get { return "The generated API is out of date. The repository model has been changed. Please re-generate this code file. Computed: " + this.Hash + ", Returned: " + this.QueryHash; }
    //    }
    //}

    [Serializable]
    public partial class ListingResults : Celeriq.Common.IListingResults
    {
        private const long VERSION_HASH = 360043455;

        public ListingResults()
        {
            this.Query = new ListingQuery();
            this.DimensionList = new List<Celeriq.Common.DimensionItem>();
            this.AllDimensions = new DimensionStore(this);
            this.RecordList = new List<ListingItem>();
        }

        internal ListingResults(Celeriq.Common.DataQueryResults results)
            : this()
        {
            if (results.VersionHash != VERSION_HASH)
            {
                throw new Celeriq.Common.Exceptions.APIVersionException(VERSION_HASH, results.VersionHash);
            }

            this.Query = new ListingQuery(results.Query);
            this.DimensionList = new List<Celeriq.Common.DimensionItem>(results.DimensionList);
            this.AllDimensions = new DimensionStore(this);
            this.TotalRecordCount = results.TotalRecordCount;
            if (results.Query.RecordsPerPage > 0)
                this.TotalPageCount = (results.TotalRecordCount / results.Query.RecordsPerPage) + (results.TotalRecordCount % results.Query.RecordsPerPage == 0 ? 0 : 1);
            this.ComputeTime = results.ComputeTime;

            foreach (var d in this.DimensionList)
            {
                this.AllDimensions.MasterList.Add((DimensionItem)((ICloneable)d).Clone());
            }

            if (results.RecordList != null)
            {
                foreach (var record in results.RecordList)
                {
                    var item = new ListingItem(record, this);
                    this.RecordList.Add(item);
                }

                var keepRefinements = new List<long>();
                if (!string.IsNullOrEmpty(results.Query.NonParsedFieldList["ar"]))
                {
                    var arr = results.Query.NonParsedFieldList["ar"].Split(new char[] { '+', ' ' });
                    foreach (var s in arr)
                    {
                        long didx;
                        if (long.TryParse(s, out didx))
                            keepRefinements.Add(didx);
                    }
                }
                this.DimensionList.RemoveAll(x => x.RefinementList.Count <= 1 && !keepRefinements.Contains(x.DIdx));
            }

        }

        public List<Celeriq.Common.DimensionItem> DimensionList { get; private set; }
        public ListingQuery Query { get; private set; }
        public List<ListingItem> RecordList { get; private set; }
        public int TotalRecordCount { get; private set; }
        public int TotalPageCount { get; private set; }
        public DimensionStore AllDimensions { get; private set; }
        public long ComputeTime { get; private set; }

        #region IListingResults Members

        IListingQuery IListingResults.Query
        {
            get { return this.Query; }
        }

        List<IListingItem> IListingResults.RecordList
        {
            get { return this.RecordList.Cast<IListingItem>().ToList(); }
        }

        #endregion

    }

    [Serializable]
    public partial class DimensionStore
    {

        #region Dimension Names

        public const string Dimension_Dim1 = "Dim1";
        public const string Dimension_Dim2 = "Dim2";
        public const string Dimension_Dim3 = "Dim3";
        public const string Dimension_Dim4 = "Dim4";
        public const string Dimension_Dim5 = "Dim5";

        #endregion

        private ListingResults _results = null;
        public DimensionStore(ListingResults results)
        {
            _results = results;
            this.MasterList = new List<DimensionItem>();
        }

        public List<DimensionItem> MasterList { get; private set; }

        public Celeriq.Common.DimensionItem Dim1Dimension
        {
            get { return this.MasterList.FirstOrDefault(x => x.Name == "Dim1"); }
        }
        public Celeriq.Common.DimensionItem Dim2Dimension
        {
            get { return this.MasterList.FirstOrDefault(x => x.Name == "Dim2"); }
        }
        public Celeriq.Common.DimensionItem Dim3Dimension
        {
            get { return this.MasterList.FirstOrDefault(x => x.Name == "Dim3"); }
        }
        public Celeriq.Common.DimensionItem Dim4Dimension
        {
            get { return this.MasterList.FirstOrDefault(x => x.Name == "Dim4"); }
        }
        public Celeriq.Common.DimensionItem Dim5Dimension
        {
            get { return this.MasterList.FirstOrDefault(x => x.Name == "Dim5"); }
        }
    }

    [Serializable]
    public partial class RepositoryConnection
    {
        static RepositoryConnection()
        {
            //RepositoryKey: 22999192-a01f-4e48-8922-50ad48332736;
        }

        public static System.ServiceModel.ChannelFactory<Celeriq.Common.IDataModel> GetFactory(string serverName)
        {
            return GetFactory(serverName, 1973);
        }

        public static System.ServiceModel.ChannelFactory<Celeriq.Common.IDataModel> GetFactory(string serverName, int port)
        {
            var myBinding = new NetTcpBinding();
            myBinding.Security.Mode = SecurityMode.None;
            myBinding.MaxBufferPoolSize = 2147483647;
            myBinding.MaxBufferSize = 2147483647;
            myBinding.MaxConnections = 10;
            myBinding.MaxReceivedMessageSize = 2147483647;
            myBinding.ReaderQuotas.MaxDepth = 2147483647;
            myBinding.ReaderQuotas.MaxStringContentLength = 2147483647;
            myBinding.ReaderQuotas.MaxArrayLength = 2147483647;
            myBinding.ReaderQuotas.MaxBytesPerRead = 2147483647;
            myBinding.ReaderQuotas.MaxNameTableCharCount = 2147483647;
            var endpoint = new EndpointAddress("net.tcp://" + serverName + ":" + port + "/__celeriq_engine");
            return new System.ServiceModel.ChannelFactory<Celeriq.Common.IDataModel>(myBinding, endpoint);
        }

        public static void Clear(Guid repositoryId, Celeriq.Common.IDataModel service, UserCredentials credentials)
        {
            var errorList = service.Clear(repositoryId, credentials);
            if (errorList != null && errorList.Length > 0)
            {
                throw new Exception(errorList.First());
            }
        }

        public static void UpdateData(Guid repositoryId, ListingItem item, UserCredentials credentials, Celeriq.Common.IDataModel service)
        {
            UpdateData(repositoryId, new List<ListingItem>() { item }, credentials, service);
        }

        public static bool IsValidFormat(Guid repositoryId, ListingItem item, UserCredentials credentials, Celeriq.Common.IDataModel service)
        {
            return service.IsValidFormat(repositoryId, item.ToTransfer(), credentials);
        }

        public static void UpdateData(Guid repositoryId, IEnumerable<ListingItem> list, UserCredentials credentials, Celeriq.Common.IDataModel service)
        {
            var l = new List<Celeriq.Common.DataItem>();
            list.ToList().ForEach(x => l.Add(x.ToTransfer()));
            var errorList = service.UpdateData(repositoryId, l, credentials);
            if (errorList != null && errorList.Length > 0)
            {
                throw new Exception(errorList.First());
            }
        }

        public static void DeleteData(Guid repositoryId, ListingItem item, UserCredentials credentials, Celeriq.Common.IDataModel service)
        {
            DeleteData(repositoryId, new List<ListingItem>() { item }, credentials, service);
        }

        public static void DeleteData(Guid repositoryId, IEnumerable<ListingItem> list, UserCredentials credentials, Celeriq.Common.IDataModel service)
        {
            var l = new List<Celeriq.Common.DataItem>();
            list.ToList().ForEach(x => l.Add(x.ToTransfer()));
            var errorList = service.DeleteData(repositoryId, l, credentials);
            if (errorList != null && errorList.Length > 0)
            {
                throw new Exception(errorList.First());
            }
        }

        public static ListingResults QueryData(Guid repositoryId, ListingQuery query, Celeriq.Common.IDataModel service)
        {
            var t = service.Query(repositoryId, query.ToTransfer());
            if (t.ErrorList != null && t.ErrorList.Length > 0)
            {
                throw new Exception(t.ErrorList.First());
            }
            return new ListingResults(t);
        }

    }

}
