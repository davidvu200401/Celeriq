using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Celeriq.Common;

namespace Celeriq.ManagementStudio.Objects
{
    internal static class ClientCodeHelper
    {
        public static string GenerateClientCode(Celeriq.Common.RepositorySchema repository)
        {
            var sb = new StringBuilder();
            sb.AppendLine("namespace [MYNAMESPACE]");
            sb.AppendLine("{");
            sb.AppendLine("	using System;");
            sb.AppendLine("	using System.Collections.Generic;");
            sb.AppendLine("	using System.Linq;");
            sb.AppendLine("	using System.ServiceModel;");
            sb.AppendLine("	using Celeriq.Common;");
            sb.AppendLine();
            sb.AppendLine("	[Serializable]");
            sb.AppendLine("	public partial class ListingItem : Celeriq.Common.IListingItem");
            sb.AppendLine("	{");
            sb.AppendLine("		public ListingItem()");
            sb.AppendLine("		{");
            sb.AppendLine("			_dimensionList = new List<DimensionItem>();");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		public DimensionStore AllDimensions { get; private set; }");
            sb.AppendLine();
            sb.AppendLine("		internal ListingItem(Celeriq.Common.DataItem item, ListingResults results)");
            sb.AppendLine("			: this()");
            sb.AppendLine("		{");
            sb.AppendLine();
            sb.AppendLine("		this.AllDimensions = new DimensionStore(null);");
            sb.AppendLine();

            var index = 0;
            foreach (var field in repository.FieldList)
            {
                sb.AppendLine("			this." + GetToken(field.Name) + " = (" + GetCodeType(field.DataType, !field.IsPrimaryKey) + ")item.ItemArray[" + index + "];");
                index++;
            }

            sb.AppendLine();
            sb.AppendLine("			RefinementItem r;");
            foreach (var dItem in repository.DimensionList)
            {
                if (dItem.NumericBreak != null)
                {
                    sb.AppendLine("			if (this." + GetToken(dItem.Name) + " != null)");
                    sb.AppendLine("			{");
                    sb.AppendLine("				r = results.AllDimensions." + GetToken(dItem.Name) + "Dimension.GetRefinementByMinValue(FloorToNearest(this." + GetToken(dItem.Name) + ".Value, " + dItem.NumericBreak.Value + "));");
                    sb.AppendLine("				if (r != null)");
                    sb.AppendLine("				{");
                    sb.AppendLine("					var d = ((System.ICloneable)results.AllDimensions." + GetToken(dItem.Name) + "Dimension).Clone() as DimensionItem;");
                    sb.AppendLine("					d.RefinementList.RemoveAll(x => x.MinValue != FloorToNearest(this." + GetToken(dItem.Name) + ".Value, " + dItem.NumericBreak.Value + "));");
                    sb.AppendLine("					d.RefinementList.First().Count = 1;");
                    sb.AppendLine("					_dimensionList.Add(d);");
                    sb.AppendLine("					this.AllDimensions.MasterList.Add(d);");
                    sb.AppendLine("				}");
                    sb.AppendLine("			}");
                }
                else if (dItem.DataType == RepositorySchema.DataTypeConstants.List)
                {
                    sb.AppendLine("			if (this." + GetToken(dItem.Name) + " != null)");
                    sb.AppendLine("			{");
                    sb.AppendLine("				foreach (var s in this." + GetToken(dItem.Name) + ")");
                    sb.AppendLine("				{");
                    sb.AppendLine("					r = results.AllDimensions." + GetToken(dItem.Name) + "Dimension.GetRefinementByValue(s);");
                    sb.AppendLine("					if (r != null)");
                    sb.AppendLine("					{");
                    sb.AppendLine("						var d = ((System.ICloneable) results.AllDimensions." + GetToken(dItem.Name) + "Dimension).Clone() as DimensionItem;");
                    sb.AppendLine("						d.RefinementList.RemoveAll(x => x.FieldValue != s);");
                    sb.AppendLine("						d.RefinementList.First().Count = 1;");
                    sb.AppendLine("						_dimensionList.Add(d);");
                    sb.AppendLine("						this.AllDimensions.MasterList.Add(d);");
                    sb.AppendLine("					}");
                    sb.AppendLine("				}");
                    sb.AppendLine("			}");
                }
                else if ((dItem.DataType == RepositorySchema.DataTypeConstants.Int) || (dItem.DataType == RepositorySchema.DataTypeConstants.Float))
                {
                    sb.AppendLine("			r = results.AllDimensions." + GetToken(dItem.Name) + "Dimension.GetRefinementByValue(this." + GetToken(dItem.Name) + ".ToString());");
                    sb.AppendLine("			if (r != null && this." + GetToken(dItem.Name) + " != null)");
                    sb.AppendLine("			{");
                    sb.AppendLine("				var d = ((System.ICloneable) results.AllDimensions." + GetToken(dItem.Name) + "Dimension).Clone() as DimensionItem;");
                    sb.AppendLine("				d.RefinementList.RemoveAll(x => x.FieldValue != this." + GetToken(dItem.Name) + ".ToString());");
                    sb.AppendLine("				d.RefinementList.First().Count = 1;");
                    sb.AppendLine("				_dimensionList.Add(d);");
                    sb.AppendLine("				this.AllDimensions.MasterList.Add(d);");
                    sb.AppendLine("			}");
                }
                else if (dItem.DataType == RepositorySchema.DataTypeConstants.DateTime)
                {
                    sb.AppendLine("			r = results.AllDimensions." + GetToken(dItem.Name) + "Dimension.GetRefinementByValue(this." + GetToken(dItem.Name) + ".ToCeleriqDateString());");
                    sb.AppendLine("			if (r != null && this." + GetToken(dItem.Name) + " != null)");
                    sb.AppendLine("			{");
                    sb.AppendLine("				var d = ((System.ICloneable) results.AllDimensions." + GetToken(dItem.Name) + "Dimension).Clone() as DimensionItem;");
                    sb.AppendLine("				d.RefinementList.RemoveAll(x => x.FieldValue != this." + GetToken(dItem.Name) + ".ToCeleriqDateString());");
                    sb.AppendLine("				d.RefinementList.First().Count = 1;");
                    sb.AppendLine("				_dimensionList.Add(d);");
                    sb.AppendLine("				this.AllDimensions.MasterList.Add(d);");
                    sb.AppendLine("			}");
                }
                else if (dItem.DataType == RepositorySchema.DataTypeConstants.Bool)
                {
                    sb.AppendLine("			r = results.AllDimensions." + GetToken(dItem.Name) + "Dimension.GetRefinementByValue(this." + GetToken(dItem.Name) + " == null ? null : (this." + GetToken(dItem.Name) + ".Value ? \"true\" : \"false\"));");
                    sb.AppendLine("			if (r != null && this." + GetToken(dItem.Name) + " != null)");
                    sb.AppendLine("			{");
                    sb.AppendLine("				var d = ((System.ICloneable) results.AllDimensions." + GetToken(dItem.Name) + "Dimension).Clone() as DimensionItem;");
                    sb.AppendLine("				d.RefinementList.RemoveAll(x => x.FieldValue != (this." + GetToken(dItem.Name) + ".Value ? \"true\" : \"false\"));");
                    sb.AppendLine("				d.RefinementList.First().Count = 1;");
                    sb.AppendLine("				_dimensionList.Add(d);");
                    sb.AppendLine("				this.AllDimensions.MasterList.Add(d);");
                    sb.AppendLine("			}");
                }
                else
                {
                    sb.AppendLine("			r = results.AllDimensions." + GetToken(dItem.Name) + "Dimension.GetRefinementByValue(this." + GetToken(dItem.Name) + ");");
                    sb.AppendLine("			if (r != null && this." + GetToken(dItem.Name) + " != null)");
                    sb.AppendLine("			{");
                    sb.AppendLine("				var d = ((System.ICloneable) results.AllDimensions." + GetToken(dItem.Name) + "Dimension).Clone() as DimensionItem;");
                    sb.AppendLine("				d.RefinementList.RemoveAll(x => x.FieldValue != this." + GetToken(dItem.Name) + ");");
                    sb.AppendLine("				d.RefinementList.First().Count = 1;");
                    sb.AppendLine("				_dimensionList.Add(d);");
                    sb.AppendLine("				this.AllDimensions.MasterList.Add(d);");
                    sb.AppendLine("			}");
                }
                sb.AppendLine();
            }

            sb.AppendLine("		}");
            sb.AppendLine();

            foreach (var field in repository.FieldList)
            {
                sb.AppendLine("		public " + GetCodeType(field.DataType, !field.IsPrimaryKey) + " " + GetToken(field.Name) + " { get; set; }");
            }

            sb.AppendLine("		internal List<DimensionItem> _dimensionList = new List<DimensionItem>();");
            sb.AppendLine("		public IEnumerable<DimensionItem> DimensionList");
            sb.AppendLine("		{");
            sb.AppendLine("			get { return _dimensionList; }");
            sb.AppendLine("		}");
            sb.AppendLine();

            sb.AppendLine("		private static long FloorToNearest(long d, long n)");
            sb.AppendLine("		{");
            sb.AppendLine("			if (n <= 0) throw new Exception(\"The rounding operation cannot be performed.\");");
            sb.AppendLine("			return d - (d % n);");
            sb.AppendLine("		}");
            sb.AppendLine();

            sb.AppendLine("		public Celeriq.Common.DataItem ToTransfer()");
            sb.AppendLine("		{");
            sb.AppendLine("			var retval = new Celeriq.Common.DataItem();");
            sb.AppendLine("			retval.ItemArray = new object[] {");

            foreach (var field in repository.FieldList)
            {
                sb.AppendLine("				this." + GetToken(field.Name) + ",");
            }

            sb.AppendLine("			};");
            sb.AppendLine("			return retval;");
            sb.AppendLine("		}");
            sb.AppendLine("	}");
            sb.AppendLine();
            sb.AppendLine("	[Serializable]");
            sb.AppendLine("	public partial class ListingQuery : Celeriq.Common.BaseListingQuery, System.ICloneable");
            sb.AppendLine("	{");
            sb.AppendLine("		partial void LoadFromUrlComplete(string originalUrl);");
            sb.AppendLine("		partial void PostToString(ref string url);");
            sb.AppendLine("		public bool UseDefaults { get; set; }");
            sb.AppendLine();
            sb.AppendLine("		public ListingQuery()");
            sb.AppendLine("			: base()");
            sb.AppendLine("		{");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		internal ListingQuery(Celeriq.Common.DataQuery query)");
            sb.AppendLine("			: this()");
            sb.AppendLine("		{");
            sb.AppendLine("			this.DimensionValueList.AddRange(query.DimensionValueList);");
            sb.AppendLine("			this.PageOffset = query.PageOffset;");
            sb.AppendLine("			this.RecordsPerPage = query.RecordsPerPage;");
            sb.AppendLine("			this.Keyword = query.Keyword;");
            sb.AppendLine("			this.PageName = query.PageName;");
            sb.AppendLine("			this.NonParsedFieldList = new NamedItemList();");
            sb.AppendLine("			if (query.NonParsedFieldList != null)");
            sb.AppendLine("				this.NonParsedFieldList.AddRange(query.NonParsedFieldList);");
            sb.AppendLine();
            sb.AppendLine("			#region Field Filters");
            sb.AppendLine("			if (query.FieldFilters != null)");
            sb.AppendLine("			{");
            sb.AppendLine("				foreach (var ff in query.FieldFilters)");
            sb.AppendLine("				{");
            sb.AppendLine("					switch (ff.Name)");
            sb.AppendLine("					{");

            foreach (var field in repository.FieldList)
            {
                if (field.DataType == Common.RepositorySchema.DataTypeConstants.GeoCode)
                {
                    sb.AppendLine("						case \"" + GetToken(field.Name) + "\":");
                    sb.AppendLine("							this.FieldFilters.Add(new FieldFilter" + GetToken(field.Name) + "()");
                    sb.AppendLine("								{");
                    sb.AppendLine("									Latitude = ((Celeriq.Common.GeoCodeFieldFilter)ff).Latitude,");
                    sb.AppendLine("									Longitude = ((Celeriq.Common.GeoCodeFieldFilter)ff).Longitude,");
                    sb.AppendLine("									Radius = ((Celeriq.Common.GeoCodeFieldFilter)ff).Radius,");
                    sb.AppendLine("									Comparer = ff.Comparer,");
                    sb.AppendLine("								});");
                    sb.AppendLine("							break;");
                }
                else if (field.DataType == Celeriq.Common.RepositorySchema.DataTypeConstants.List)
                {
                    sb.AppendLine("						case \"" + GetToken(field.Name) + "\":");
                    sb.AppendLine("							this.FieldFilters.Add(new FieldFilter" + GetToken(field.Name) + " { " + GetToken(field.Name) + " = (string)ff.Value, Comparer = ff.Comparer });");
                    sb.AppendLine("							break;");
                }
                else
                {
                    sb.AppendLine("						case \"" + GetToken(field.Name) + "\":");
                    sb.AppendLine("							this.FieldFilters.Add(new FieldFilter" + GetToken(field.Name) + " { " + GetToken(field.Name) + " = (" + GetCodeType(field.DataType) + ")ff.Value, Comparer = ff.Comparer });");
                    sb.AppendLine("							break;");
                }
            }

            sb.AppendLine("						default:");
            sb.AppendLine("							throw new Exception(\"Unknown filter!\");");
            sb.AppendLine("					}");
            sb.AppendLine("				}");
            sb.AppendLine("			}");
            sb.AppendLine("			#endregion");
            sb.AppendLine();
            sb.AppendLine("			#region Field Sorts");
            sb.AppendLine("			if (query.FieldSorts != null)");
            sb.AppendLine("			{");
            sb.AppendLine("				foreach (var fs in query.FieldSorts)");
            sb.AppendLine("				{");
            sb.AppendLine("					switch(fs.Name)");
            sb.AppendLine("					{");

            foreach (var field in repository.FieldList.Where(x => x.SortingSupported))
            {
                sb.AppendLine("						case \"" + GetToken(field.Name) + "\":");
                sb.AppendLine("							this.FieldSorts.Add(new FieldSort" + GetToken(field.Name) + " { Name = fs.Name, SortDirection = fs.SortDirection });");
                sb.AppendLine("							break;");
            }

            sb.AppendLine("						default:");
            sb.AppendLine("							throw new Exception(\"Unknown sort!\");");
            sb.AppendLine("					}");
            sb.AppendLine("				}");
            sb.AppendLine("			}");
            sb.AppendLine("			#endregion");
            sb.AppendLine();
            sb.AppendLine("		}");
            sb.AppendLine();

            #region Constructor with URL
            sb.AppendLine("		public ListingQuery(string url)");
            sb.AppendLine("			: this()");
            sb.AppendLine("		{");
            sb.AppendLine("			if (string.IsNullOrEmpty(url)) return;");
            sb.AppendLine("			if (url.Contains(\"%\")) url = System.Web.HttpUtility.UrlDecode(url);");
            sb.AppendLine("			var originalUrl = url;");
            sb.AppendLine();
            sb.AppendLine("			var pageBreak = url.IndexOf('?');");
            sb.AppendLine("			if (pageBreak != -1 && pageBreak < url.Length - 1)");
            sb.AppendLine("			{");
            sb.AppendLine("				this.PageName = url.Substring(0, pageBreak);");
            sb.AppendLine("				url = url.Substring(pageBreak + 1, url.Length - pageBreak - 1);");
            sb.AppendLine("			}");
            sb.AppendLine("			else");
            sb.AppendLine("			{");
            sb.AppendLine("				this.PageName = url;");
            sb.AppendLine("				return;");
            sb.AppendLine("			}");
            sb.AppendLine();
            sb.AppendLine("			#region Parse Query String");
            sb.AppendLine("			var tuplets = url.Split('&');");
            sb.AppendLine("			foreach (var gset in tuplets)");
            sb.AppendLine("			{");
            sb.AppendLine("				var values = gset.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);");
            sb.AppendLine("				if (values.Length == 2)");
            sb.AppendLine("				{");
            sb.AppendLine("					switch (values[0])");
            sb.AppendLine("					{");
            sb.AppendLine("						case \"d\":");
            sb.AppendLine("							{");
            sb.AppendLine("								var dValues = values[1].Split(new char[] { '+', ' ' }, StringSplitOptions.RemoveEmptyEntries);");
            sb.AppendLine("								foreach (var dvidxV in dValues)");
            sb.AppendLine("								{");
            sb.AppendLine("									long dvidx;");
            sb.AppendLine("									if (long.TryParse(dvidxV, out dvidx))");
            sb.AppendLine("										this.DimensionValueList.Add(dvidx);");
            sb.AppendLine("								}");
            sb.AppendLine("							}");
            sb.AppendLine("							break;");
            sb.AppendLine("						case \"po\":");
            sb.AppendLine("							{");
            sb.AppendLine("								int po;");
            sb.AppendLine("								if (int.TryParse(values[1], out po))");
            sb.AppendLine("									this.PageOffset = po;");
            sb.AppendLine("							}");
            sb.AppendLine("							break;");
            sb.AppendLine("						case \"rpp\":");
            sb.AppendLine("							{");
            sb.AppendLine("								int rpp;");
            sb.AppendLine("								if (int.TryParse(values[1], out rpp))");
            sb.AppendLine("									this.RecordsPerPage = rpp;");
            sb.AppendLine("							}");
            sb.AppendLine("							break;");
            sb.AppendLine("						case \"ff\":");
            sb.AppendLine("							{");
            sb.AppendLine("								var filters = values[1].Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);");
            sb.AppendLine("								foreach (var s in filters)");
            sb.AppendLine("								{");
            sb.AppendLine("									var svalues = s.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);");
            sb.AppendLine("									if (svalues.Length >= 3)");
            sb.AppendLine("									{");
            sb.AppendLine("										Celeriq.Common.ComparisonConstants enumValue;");
            sb.AppendLine("										if (Enum.TryParse<Celeriq.Common.ComparisonConstants>(svalues[1], true, out enumValue))");
            sb.AppendLine("										{");
            sb.AppendLine("											switch (svalues[0].ToLower())");
            sb.AppendLine("											{");

            #region FieldList
            foreach (var field in repository.FieldList.OrderBy(x => x.Name))
            {
                if (field.DataType == Celeriq.Common.RepositorySchema.DataTypeConstants.GeoCode) //GeoCode
                {
                    sb.AppendLine("												case \"" + GetToken(field.Name).ToLower() + "\":");
                    sb.AppendLine("													double " + GetToken(field.Name).ToLower() + "Value1, " + GetToken(field.Name).ToLower() + "Value2, radius;");
                    sb.AppendLine("													if (svalues.Length > 4)");
                    sb.AppendLine("													{");
                    sb.AppendLine("														if (double.TryParse(svalues[2], out " + GetToken(field.Name).ToLower() + "Value1) && double.TryParse(svalues[3], out " + GetToken(field.Name).ToLower() + "Value2) && double.TryParse(svalues[4], out radius))");
                    sb.AppendLine("															this.FieldFilters.Add(new FieldFilter" + GetToken(field.Name) + "() { Name = \"" + GetToken(field.Name) + "\", Comparer = enumValue, Latitude = " + GetToken(field.Name).ToLower() + "Value1, Longitude = " + GetToken(field.Name).ToLower() + "Value2, Radius = radius });");
                    sb.AppendLine("														else");
                    sb.AppendLine("															throw new Exception(\"The URL for field filter '" + GetToken(field.Name) + "' is invalid!\");");
                    sb.AppendLine("													}");
                    sb.AppendLine("													else");
                    sb.AppendLine("														throw new Exception(\"The URL for field filter '" + GetToken(field.Name) + "' is invalid!\");");
                    sb.AppendLine("													break;");
                }
                else if (field.DataType == Celeriq.Common.RepositorySchema.DataTypeConstants.String) //String
                {
                    sb.AppendLine("												case \"" + GetToken(field.Name).ToLower() + "\":");
                    sb.AppendLine("													this.FieldFilters.Add(new FieldFilter" + GetToken(field.Name) + "() { Name = \"" + GetToken(field.Name) + "\", Comparer = enumValue, " + GetToken(field.Name) + " = svalues[2] });");
                    sb.AppendLine("													if (enumValue == Celeriq.Common.ComparisonConstants.Between)");
                    sb.AppendLine("													{");
                    sb.AppendLine("														if (svalues.Length > 3)");
                    sb.AppendLine("															this.FieldFilters.Last().Value2 = svalues[3];");
                    sb.AppendLine("														else");
                    sb.AppendLine("															throw new Exception(\"The URL for field filter '" + GetToken(field.Name) + "' is invalid!\");");
                    sb.AppendLine("													}");
                    sb.AppendLine("													break;");
                }
                else if (field.DataType == Celeriq.Common.RepositorySchema.DataTypeConstants.List) //List
                {
                    //Do Nothing
                }
                else //All other types
                {
                    sb.AppendLine("												case \"" + GetToken(field.Name).ToLower() + "\":");
                    sb.AppendLine("													" + GetCodeType(field.DataType, false) + " " + GetToken(field.Name).ToLower() + "Value;");
                    sb.AppendLine("													if (" + GetCodeType(field.DataType, false) + ".TryParse(svalues[2], out " + GetToken(field.Name).ToLower() + "Value))");
                    sb.AppendLine("														this.FieldFilters.Add(new FieldFilter" + GetToken(field.Name) + "() { Name = \"" + GetToken(field.Name) + "\", Comparer = enumValue, " + GetToken(field.Name) + " = " + GetToken(field.Name).ToLower() + "Value });");
                    sb.AppendLine("													if (enumValue == Celeriq.Common.ComparisonConstants.Between)");
                    sb.AppendLine("													{");
                    sb.AppendLine("														if (svalues.Length > 3)");
                    sb.AppendLine("														{");
                    sb.AppendLine("															if (" + GetCodeType(field.DataType, false) + ".TryParse(svalues[3], out " + GetToken(field.Name).ToLower() + "Value))");
                    sb.AppendLine("																this.FieldFilters.Last().Value2 = " + GetToken(field.Name).ToLower() + "Value;");
                    sb.AppendLine("															else");
                    sb.AppendLine("																throw new Exception(\"The URL for field filter '" + GetToken(field.Name) + "' is invalid!\");");
                    sb.AppendLine("														}");
                    sb.AppendLine("														else");
                    sb.AppendLine("															throw new Exception(\"The URL for field filter '" + GetToken(field.Name) + "' is invalid!\");");
                    sb.AppendLine("													}");
                    sb.AppendLine("													break;");
                }
            }
            #endregion

            sb.AppendLine("											}");
            sb.AppendLine("										}");
            sb.AppendLine("									}");
            sb.AppendLine("								}");
            sb.AppendLine("							}");
            sb.AppendLine("							break;");
            sb.AppendLine("						case \"fs\":");
            sb.AppendLine("							{");
            sb.AppendLine("								var sorts = values[1].Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);");
            sb.AppendLine("								foreach (var s in sorts)");
            sb.AppendLine("								{");
            sb.AppendLine("									var svalues = s.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);");
            sb.AppendLine("									if (svalues.Length > 0)");
            sb.AppendLine("									{");
            sb.AppendLine("										switch (svalues[0].ToLower())");
            sb.AppendLine("										{");

            foreach (var field in repository.FieldList.Where(x => x.SortingSupported).OrderBy(x => x.Name))
            {
                sb.AppendLine("											case \"" + GetToken(field.Name).ToLower() + "\":");
                sb.AppendLine("												this.FieldSorts.Add(new FieldSort" + GetToken(field.Name) + "() { Name = \"" + GetToken(field.Name) + "\", SortDirection = (svalues.Length == 1 || svalues[1] != \"0\" ? Celeriq.Common.SortDirectionConstants.Asc : Celeriq.Common.SortDirectionConstants.Desc) });");
                sb.AppendLine("												break;");
            }

            sb.AppendLine("										}");
            sb.AppendLine("									}");
            sb.AppendLine("								}");
            sb.AppendLine("							}");
            sb.AppendLine("							break;");
            sb.AppendLine("						case \"srch\":");
            sb.AppendLine("							this.Keyword = values[1];");
            sb.AppendLine("							break;");
            sb.AppendLine("						default:");
            sb.AppendLine("							if (values.Length >= 2)");
            sb.AppendLine("							{");
            sb.AppendLine("								if (this.NonParsedFieldList.Count(x => x.Key == values[0]) > 0)");
            sb.AppendLine("									this.NonParsedFieldList.First(x => x.Key == values[0]).Value = values[1];");
            sb.AppendLine("								else");
            sb.AppendLine("									this.NonParsedFieldList.Add(new NamedItem() { Key = values[0], Value = values[1] });");
            sb.AppendLine("							}");
            sb.AppendLine("							break;");
            sb.AppendLine("					}");
            sb.AppendLine("				}");
            sb.AppendLine("			}");
            sb.AppendLine("			#endregion");
            sb.AppendLine();
            sb.AppendLine("			LoadFromUrlComplete(originalUrl);");
            sb.AppendLine();
            sb.AppendLine("		}");
            sb.AppendLine();
            #endregion

            #region Static Methods
            sb.AppendLine("		public static Celeriq.Common.FieldSort GetSortByName(string name)");
            sb.AppendLine("		{");
            foreach (var field in repository.FieldList.Where(x => x.SortingSupported))
            {
                sb.AppendLine("			if (name.ToLower() == \"" + GetToken(field.Name) + "\".ToLower()) return new FieldSort" + GetToken(field.Name) + "() { Name = \"" + GetToken(field.Name) + "\", SortDirection = Celeriq.Common.SortDirectionConstants.Asc };");
            }
            sb.AppendLine("			return null;");
            sb.AppendLine("		}");
            sb.AppendLine();
            #endregion

            sb.AppendLine("		public Celeriq.Common.DataQuery ToTransfer()");
            sb.AppendLine("		{");
            sb.AppendLine("			var retval = new Celeriq.Common.DataQuery();");
            sb.AppendLine("			retval.DimensionValueList = this.DimensionValueList;");
            sb.AppendLine("			retval.Credentials = this.Credentials;");
            sb.AppendLine("			retval.IPMask = this.IPMask;");
            sb.AppendLine();
            sb.AppendLine("			retval.FieldFilters = new List<Celeriq.Common.IFieldFilter>();");
            sb.AppendLine("			if (this.FieldFilters == null)");
            sb.AppendLine("				this.FieldFilters = new List<IFieldFilter>();");
            sb.AppendLine("			foreach (var o in this.FieldFilters)");
            sb.AppendLine("			{");
            sb.AppendLine("				if (o is Celeriq.Common.GeoCodeFieldFilter)");
            sb.AppendLine("				{");
            sb.AppendLine("					var newFilter = new Celeriq.Common.GeoCodeFieldFilter()");
            sb.AppendLine("					{");
            sb.AppendLine("						Latitude = ((Celeriq.Common.GeoCodeFieldFilter)o).Latitude,");
            sb.AppendLine("						Longitude = ((Celeriq.Common.GeoCodeFieldFilter)o).Longitude,");
            sb.AppendLine("						Radius = ((Celeriq.Common.GeoCodeFieldFilter)o).Radius,");
            sb.AppendLine("						Comparer = o.Comparer,");
            sb.AppendLine("						Name = o.Name,");
            sb.AppendLine("					};");
            sb.AppendLine("					((Celeriq.Common.IFieldFilter)newFilter).Value = ((Celeriq.Common.IFieldFilter)o).Value;");
            sb.AppendLine("					((Celeriq.Common.IFieldFilter)newFilter).Value2 = ((Celeriq.Common.IFieldFilter)o).Value2;");
            sb.AppendLine("					retval.FieldFilters.Add(newFilter);");
            sb.AppendLine("				}");
            sb.AppendLine("				else");
            sb.AppendLine("				{");
            sb.AppendLine("					var newFilter = new Celeriq.Common.FieldFilter()");
            sb.AppendLine("					{");
            sb.AppendLine("						Comparer = o.Comparer,");
            sb.AppendLine("						Name = o.Name,");
            sb.AppendLine("					};");
            sb.AppendLine("					((Celeriq.Common.IFieldFilter)newFilter).Value = ((Celeriq.Common.IFieldFilter)o).Value;");
            sb.AppendLine("					((Celeriq.Common.IFieldFilter)newFilter).Value2 = ((Celeriq.Common.IFieldFilter)o).Value2;");
            sb.AppendLine("					retval.FieldFilters.Add(newFilter);");
            sb.AppendLine("				}");
            sb.AppendLine("			}");
            sb.AppendLine();
            sb.AppendLine("			retval.FieldSorts = new List<Celeriq.Common.IFieldSort>();");
            sb.AppendLine("			if (this.FieldSorts == null)");
            sb.AppendLine("				this.FieldSorts = new List<IFieldSort>();");
            sb.AppendLine("			foreach (var o in this.FieldSorts)");
            sb.AppendLine("			{");
            sb.AppendLine("				retval.FieldSorts.Add(new Celeriq.Common.FieldSort()");
            sb.AppendLine("					{");
            sb.AppendLine("						Name = o.Name,");
            sb.AppendLine("						SortDirection = o.SortDirection,");
            sb.AppendLine("					});");
            sb.AppendLine("			}");
            sb.AppendLine();
            sb.AppendLine("			if (this.NonParsedFieldList != null)");
            sb.AppendLine("			{");
            sb.AppendLine("				this.NonParsedFieldList.ForEach(x => retval.NonParsedFieldList.Add(x));");
            sb.AppendLine("			}");
            sb.AppendLine();
            sb.AppendLine("			retval.Keyword = this.Keyword;");
            sb.AppendLine("			retval.PageOffset = this.PageOffset;");
            sb.AppendLine("			retval.RecordsPerPage = this.RecordsPerPage;");
            sb.AppendLine("			return retval;");
            sb.AppendLine("		}");
            sb.AppendLine();

            #region ToString
            sb.AppendLine("		public override string ToString()");
            sb.AppendLine("		{");
            sb.AppendLine("			var retval = string.Empty;");
            sb.AppendLine();
            sb.AppendLine("			#region Dimensions");
            sb.AppendLine("			if (this.DimensionValueList != null && this.DimensionValueList.Count > 0)");
            sb.AppendLine("			{");
            sb.AppendLine("				retval += \"d=\" + string.Join(\"+\", this.DimensionValueList.Select(x => x.ToString()));");
            sb.AppendLine("			}");
            sb.AppendLine("			#endregion");
            sb.AppendLine();

            sb.AppendLine("			#region Field Filters");
            sb.AppendLine("			if (this.FieldFilters != null && this.FieldFilters.Count > 0)");
            sb.AppendLine("			{");
            sb.AppendLine("				var ffURL = string.Empty;");
            sb.AppendLine("				foreach (var ff in this.FieldFilters)");
            sb.AppendLine("				{");

            sb.AppendLine("					var fieldName = string.Empty;");
            sb.AppendLine("					if (1 == 0) ;");
            foreach (var field in repository.FieldList.Where(x => x.FilteringSupported).OrderBy(x => x.Name))
            {
                sb.AppendLine("					else if (ff is FieldFilter" + GetToken(field.Name) + ") fieldName = \"" + GetToken(field.Name) + "\";");
            }
            sb.AppendLine("					else throw new Exception(\"The filter type is not valid!\");");
            sb.AppendLine();

            sb.AppendLine("					var f1 = (Celeriq.Common.IFieldFilter)ff;");
            sb.AppendLine("					if (ff is Celeriq.Common.GeoCodeFieldFilter)");
            sb.AppendLine("					{");
            sb.AppendLine("						var gff = ff as Celeriq.Common.GeoCodeFieldFilter;");
            sb.AppendLine("						if (gff != null)");
            sb.AppendLine("							ffURL += fieldName + \",\" + ff.Comparer.ToString() + \",\" + gff.Latitude.ToString() + \",\" + gff.Longitude.ToString() + \",\" + gff.Radius.ToString() + \"|\";");
            sb.AppendLine("					}");
            sb.AppendLine("					else");
            sb.AppendLine("					{");
            sb.AppendLine("						if (ff.Comparer == Celeriq.Common.ComparisonConstants.Between)");
            sb.AppendLine("						{");
            sb.AppendLine("							if ((f1.Value != null) && (f1.Value2 != null))");
            sb.AppendLine("							{");
            sb.AppendLine("								ffURL += fieldName + \",\" + ff.Comparer.ToString() + \",\" + f1.Value.ToString() + \",\" + f1.Value2.ToString() + \"|\";");
            sb.AppendLine("							}");
            sb.AppendLine("						}");
            sb.AppendLine("						else");
            sb.AppendLine("						{");
            sb.AppendLine("							if (f1.Value != null)");
            sb.AppendLine("							{");
            sb.AppendLine("								ffURL += fieldName + \",\" + ff.Comparer.ToString() + \",\" + f1.Value.ToString() + \"|\";");
            sb.AppendLine("							}");
            sb.AppendLine("						}");
            sb.AppendLine("					}");
            sb.AppendLine("				}");
            sb.AppendLine();
            sb.AppendLine("				ffURL = ffURL.Trim('|');");
            sb.AppendLine("				if (!string.IsNullOrEmpty(ffURL))");
            sb.AppendLine("					retval += \"&ff=\" + ffURL;");
            sb.AppendLine("			}");
            sb.AppendLine("			#endregion");
            sb.AppendLine();

            sb.AppendLine("			#region Field Sorts");
            sb.AppendLine("			if (this.FieldSorts != null && this.FieldSorts.Count > 0)");
            sb.AppendLine("			{");
            sb.AppendLine("				var fsURL = string.Empty;");
            sb.AppendLine("				foreach (var fs in this.FieldSorts)");
            sb.AppendLine("				{");
            sb.AppendLine("					if (1 == 0) ;");

            foreach (var field in repository.FieldList.Where(x => x.SortingSupported).OrderBy(x => x.Name))
            {
                sb.AppendLine("					else if (fs is FieldSort" + GetToken(field.Name) + ") fsURL += \"" + GetToken(field.Name) + "\";");
            }
            sb.AppendLine("					else throw new Exception(\"The sort type is not valid!\");");
            sb.AppendLine("					fsURL += (fs.SortDirection == Celeriq.Common.SortDirectionConstants.Asc ? string.Empty : \",0\");");

            sb.AppendLine("				}");
            sb.AppendLine();
            sb.AppendLine("				fsURL = fsURL.Trim('|');");
            sb.AppendLine("				if (!string.IsNullOrEmpty(fsURL))");
            sb.AppendLine("					retval += \"&fs=\" + fsURL;");
            sb.AppendLine("			}");
            sb.AppendLine("			#endregion");
            sb.AppendLine();
            sb.AppendLine("			#region Keyword");
            sb.AppendLine("			if (!string.IsNullOrEmpty(this.Keyword))");
            sb.AppendLine("			{");
            sb.AppendLine("				retval += \"&srch=\" + this.Keyword;");
            sb.AppendLine("			}");
            sb.AppendLine("			#endregion");
            sb.AppendLine();
            sb.AppendLine("			if (this.PageOffset != 1 || this.UseDefaults)");
            sb.AppendLine("				retval += \"&po=\" + this.PageOffset;");
            sb.AppendLine("			if (this.RecordsPerPage != 10 || this.UseDefaults)");
            sb.AppendLine("				retval += \"&rpp=\" + this.RecordsPerPage;");
            sb.AppendLine();
            sb.AppendLine("			#region NonParsed Field");
            sb.AppendLine("			if (this.NonParsedFieldList != null)");
            sb.AppendLine("			{");
            sb.AppendLine("				foreach (var item in this.NonParsedFieldList.Where(x => !string.IsNullOrEmpty(x.Value)))");
            sb.AppendLine("				{");
            sb.AppendLine("					retval += \"&\" + item.Key + \"=\" + item.Value;");
            sb.AppendLine("				}");
            sb.AppendLine("			}");
            sb.AppendLine("			#endregion");
            sb.AppendLine();
            sb.AppendLine("			retval = retval.Trim('&');");
            sb.AppendLine();
            sb.AppendLine("			if (string.IsNullOrEmpty(this.PageName))");
            sb.AppendLine("				return retval;");
            sb.AppendLine("			else if (!string.IsNullOrEmpty(retval))");
            sb.AppendLine("				return this.PageName + \"?\" + retval;");
            sb.AppendLine("			else");
            sb.AppendLine("				return this.PageName;");
            sb.AppendLine("			if (string.IsNullOrEmpty(this.PageName))");
            sb.AppendLine("			{");
            sb.AppendLine("				//Do Nothing");
            sb.AppendLine("			}");
            sb.AppendLine("			else if (!string.IsNullOrEmpty(retval))");
            sb.AppendLine("			{");
            sb.AppendLine("				retval = this.PageName + \"?\" + retval;");
            sb.AppendLine("			}");
            sb.AppendLine("			else");
            sb.AppendLine("			{");
            sb.AppendLine("				retval = this.PageName;");
            sb.AppendLine("			}");
            sb.AppendLine();
            sb.AppendLine("			PostToString(ref retval);");
            sb.AppendLine();
            sb.AppendLine("			return retval;");
            sb.AppendLine("		}");
            sb.AppendLine();
            #endregion

            sb.AppendLine("		partial void Cloning(ListingQuery query);");
            sb.AppendLine();
            sb.AppendLine("		object ICloneable.Clone()");
            sb.AppendLine("		{");
            sb.AppendLine("			return this.Clone();");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		public ListingQuery Clone()");
            sb.AppendLine("		{");
            sb.AppendLine("			var retval = new ListingQuery();");
            sb.AppendLine("			retval.DimensionValueList.AddRange(this.DimensionValueList);");
            sb.AppendLine("			retval.FieldFilters.AddRange(this.FieldFilters);");
            sb.AppendLine("			retval.FieldSorts.AddRange(this.FieldSorts);");
            sb.AppendLine("			retval.Keyword = this.Keyword;");
            sb.AppendLine("			retval.NonParsedFieldList.AddRange(this.NonParsedFieldList);");
            sb.AppendLine("			retval.PageOffset = this.PageOffset;");
            sb.AppendLine("			retval.PageName = this.PageName;");
            sb.AppendLine("			retval.RecordsPerPage = this.RecordsPerPage;");
            sb.AppendLine("			this.Cloning(retval);");
            sb.AppendLine("			return retval;");
            sb.AppendLine("		}");
            sb.AppendLine();

            sb.AppendLine("	}");
            sb.AppendLine();
            sb.AppendLine("	#region Filtering");
            sb.AppendLine();

            foreach (var field in repository.FieldList.Where(x => x.FilteringSupported))
            {
                if (field.DataType == Common.RepositorySchema.DataTypeConstants.GeoCode)
                {
                    sb.AppendLine("	[Serializable]");
                    sb.AppendLine("	public sealed class FieldFilter" + GetToken(field.Name) + " : Celeriq.Common.GeoCodeFieldFilter, Celeriq.Common.IFieldFilter");
                    sb.AppendLine("	{");
                    sb.AppendLine("		public FieldFilter" + GetToken(field.Name) + "() : base(\"" + GetToken(field.Name) + "\") { }");
                    sb.AppendLine("	}");
                }
                else if (field.DataType == Celeriq.Common.RepositorySchema.DataTypeConstants.List)
                {
                    sb.AppendLine("	[Serializable]");
                    sb.AppendLine("	public sealed class FieldFilter" + GetToken(field.Name) + " : Celeriq.Common.FieldFilter, Celeriq.Common.IFieldFilter");
                    sb.AppendLine("	{");
                    sb.AppendLine("		public FieldFilter" + GetToken(field.Name) + "() : base(\"" + GetToken(field.Name) + "\") { }");
                    sb.AppendLine("		public string " + GetToken(field.Name) + " { get; set; }");
                    sb.AppendLine("		object Celeriq.Common.IFieldFilter.Value");
                    sb.AppendLine("		{");
                    sb.AppendLine("			get { return this." + GetToken(field.Name) + "; }");
                    sb.AppendLine("			set { this." + GetToken(field.Name) + " = (string)value; }");
                    sb.AppendLine("		}");
                    sb.AppendLine("	}");
                }
                else
                {
                    sb.AppendLine("	[Serializable]");
                    sb.AppendLine("	public sealed class FieldFilter" + GetToken(field.Name) + " : Celeriq.Common.FieldFilter, Celeriq.Common.IFieldFilter");
                    sb.AppendLine("	{");
                    sb.AppendLine("		public FieldFilter" + GetToken(field.Name) + "() : base(\"" + GetToken(field.Name) + "\") { }");
                    sb.AppendLine("		public " + GetCodeType(field.DataType) + " " + GetToken(field.Name) + " { get; set; }");
                    sb.AppendLine("		object Celeriq.Common.IFieldFilter.Value");
                    sb.AppendLine("		{");
                    sb.AppendLine("			get { return this." + GetToken(field.Name) + "; }");
                    sb.AppendLine("			set { this." + GetToken(field.Name) + " = (" + GetCodeType(field.DataType) + ")value; }");
                    sb.AppendLine("		}");
                    sb.AppendLine("	}");
                }
                sb.AppendLine();
            }

            sb.AppendLine("	#endregion");
            sb.AppendLine();
            sb.AppendLine("	#region Sorting");
            sb.AppendLine();

            foreach (var field in repository.FieldList.Where(x => x.SortingSupported))
            {
                sb.AppendLine("	[Serializable]");
                sb.AppendLine("	public sealed class FieldSort" + GetToken(field.Name) + " : Celeriq.Common.FieldSort, Celeriq.Common.IFieldSort");
                sb.AppendLine("	{");
                sb.AppendLine("		public FieldSort" + GetToken(field.Name) + "() : base(\"" + GetToken(field.Name) + "\") { }");
                sb.AppendLine("	}");
                sb.AppendLine();
            }

            sb.AppendLine("	#endregion");
            sb.AppendLine();

            sb.AppendLine("	[Serializable]");
            sb.AppendLine("	public partial class ListingResults : Celeriq.Common.IListingResults");
            sb.AppendLine("	{");
            sb.AppendLine("		private const long VERSION_HASH = " + repository.VersionHash + ";");
            sb.AppendLine();
            sb.AppendLine("		public ListingResults()");
            sb.AppendLine("		{");
            sb.AppendLine("			this.Query = new ListingQuery();");
            sb.AppendLine("			this.DimensionList = new List<Celeriq.Common.DimensionItem>();");
            sb.AppendLine("			this.AllDimensions = new DimensionStore(this);");
            sb.AppendLine("			this.RecordList = new List<ListingItem>();");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		internal ListingResults(Celeriq.Common.DataQueryResults results)");
            sb.AppendLine("			: this()");
            sb.AppendLine("		{");
            sb.AppendLine("			if (results.VersionHash != VERSION_HASH)");
            sb.AppendLine("			{");
            sb.AppendLine("				throw new Celeriq.Common.Exceptions.APIVersionException(VERSION_HASH, results.VersionHash);");
            sb.AppendLine("			}");
            sb.AppendLine();
            sb.AppendLine("			this.Query = new ListingQuery(results.Query);");
            sb.AppendLine("			this.DimensionList = new List<Celeriq.Common.DimensionItem>(results.DimensionList);");
            sb.AppendLine("			this.AllDimensions = new DimensionStore(this);");
            sb.AppendLine("			this.TotalRecordCount = results.TotalRecordCount;");
            sb.AppendLine("			if (results.Query.RecordsPerPage > 0)");
            sb.AppendLine("				this.TotalPageCount = (results.TotalRecordCount / results.Query.RecordsPerPage) + (results.TotalRecordCount % results.Query.RecordsPerPage == 0 ? 0 : 1);");
            sb.AppendLine("			this.ComputeTime = results.ComputeTime;");
            sb.AppendLine();
            sb.AppendLine("			foreach (var d in this.DimensionList)");
            sb.AppendLine("			{");
            sb.AppendLine("				this.AllDimensions.MasterList.Add((DimensionItem)((ICloneable)d).Clone());");
            sb.AppendLine("			}");
            //sb.AppendLine("			this.AllDimensions.MasterList.ForEach(x => x.RefinementList.Clear());");
            sb.AppendLine();
            sb.AppendLine("			if (results.RecordList != null)");
            sb.AppendLine("			{");
            sb.AppendLine("				foreach (var record in results.RecordList)");
            sb.AppendLine("				{");
            sb.AppendLine("					var item = new ListingItem(record, this);");
            sb.AppendLine("					this.RecordList.Add(item);");
            sb.AppendLine("				}");
            sb.AppendLine();
            sb.AppendLine("				var keepRefinements = new List<long>();");
            sb.AppendLine("				if (!string.IsNullOrEmpty(results.Query.NonParsedFieldList[\"ar\"]))");
            sb.AppendLine("				{");
            sb.AppendLine("					var arr = results.Query.NonParsedFieldList[\"ar\"].Split(new char[] {'+', ' '});");
            sb.AppendLine("					foreach (var s in arr)");
            sb.AppendLine("					{");
            sb.AppendLine("						long didx;");
            sb.AppendLine("						if (long.TryParse(s, out didx))");
            sb.AppendLine("						keepRefinements.Add(didx);");
            sb.AppendLine("					}");
            sb.AppendLine("				}");
            sb.AppendLine("				this.DimensionList.RemoveAll(x => x.RefinementList.Count <= 1 && !keepRefinements.Contains(x.DIdx));");
            sb.AppendLine("			}");
            sb.AppendLine();
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		public List<Celeriq.Common.DimensionItem> DimensionList { get; private set; }");
            sb.AppendLine("		public ListingQuery Query { get; private set; }");
            sb.AppendLine("		public List<ListingItem> RecordList { get; private set; }");
            sb.AppendLine("		public int TotalRecordCount { get; private set; }");
            sb.AppendLine("		public int TotalPageCount { get; private set; }");
            sb.AppendLine("		public DimensionStore AllDimensions { get; private set; }");
            sb.AppendLine("		public long ComputeTime { get; private set; }");
            sb.AppendLine();
            sb.AppendLine("		#region IListingResults Members");
            sb.AppendLine();
            sb.AppendLine("		IListingQuery IListingResults.Query");
            sb.AppendLine("		{");
            sb.AppendLine("			get { return this.Query; }");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		List<IListingItem> IListingResults.RecordList");
            sb.AppendLine("		{");
            sb.AppendLine("			get { return this.RecordList.Cast<IListingItem>().ToList(); }");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		#endregion");
            sb.AppendLine();
            sb.AppendLine("	}");
            sb.AppendLine();
            sb.AppendLine("	[Serializable]");
            sb.AppendLine("	public partial class DimensionStore");
            sb.AppendLine("	{");
            sb.AppendLine();
            sb.AppendLine("		#region Dimension Names");
            sb.AppendLine();
            foreach (var d in repository.DimensionList)
            {
                sb.AppendLine("		public const string Dimension_" + GetToken(d.Name) + " = \"" + d.Name + "\";");
            }
            sb.AppendLine();
            sb.AppendLine("		#endregion");
            sb.AppendLine();
            sb.AppendLine("		private ListingResults _results = null;");
            sb.AppendLine("		public DimensionStore(ListingResults results)");
            sb.AppendLine("		{");
            sb.AppendLine("			_results = results;");
            sb.AppendLine("			this.MasterList = new List<DimensionItem>();");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		public List<DimensionItem> MasterList { get; private set; }");
            sb.AppendLine();

            index = 0;
            foreach (var d in repository.DimensionList)
            {
                sb.AppendLine("		public Celeriq.Common.DimensionItem " + GetToken(d.Name) + "Dimension");
                sb.AppendLine("			{");
                sb.AppendLine("				get { return this.MasterList.FirstOrDefault(x => x.Name == \"" + d.Name + "\"); }");
                sb.AppendLine("			}");
                index++;
            }

            sb.AppendLine("	}");
            sb.AppendLine();
            sb.AppendLine("	[Serializable]");
            sb.AppendLine("	public partial class RepositoryConnection");
            sb.AppendLine("	{");
            sb.AppendLine("		static RepositoryConnection()");
            sb.AppendLine("		{");
            sb.AppendLine("			//RepositoryKey: " + repository.ID + ";");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		public static System.ServiceModel.ChannelFactory<Celeriq.Common.IDataModel> GetFactory(string serverName)");
            sb.AppendLine("		{");
            sb.AppendLine("			return GetFactory(serverName, 1973);");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		public static System.ServiceModel.ChannelFactory<Celeriq.Common.IDataModel> GetFactory(string serverName, int port)");
            sb.AppendLine("		{");
            sb.AppendLine("			var myBinding = new NetTcpBinding();");
            sb.AppendLine("			myBinding.Security.Mode = SecurityMode.None;");
            sb.AppendLine("			myBinding.MaxBufferPoolSize = 2147483647;");
            sb.AppendLine("			myBinding.MaxBufferSize = 2147483647;");
            sb.AppendLine("			myBinding.MaxConnections = 10;");
            sb.AppendLine("			myBinding.MaxReceivedMessageSize = 2147483647;");
            sb.AppendLine("			myBinding.ReaderQuotas.MaxDepth = 2147483647;");
            sb.AppendLine("			myBinding.ReaderQuotas.MaxStringContentLength = 2147483647;");
            sb.AppendLine("			myBinding.ReaderQuotas.MaxArrayLength = 2147483647;");
            sb.AppendLine("			myBinding.ReaderQuotas.MaxBytesPerRead = 2147483647;");
            sb.AppendLine("			myBinding.ReaderQuotas.MaxNameTableCharCount = 2147483647;");
            sb.AppendLine("			var endpoint = new EndpointAddress(\"net.tcp://\" + serverName + \":\" + port + \"/__celeriq_engine\");");
            sb.AppendLine("			return new System.ServiceModel.ChannelFactory<Celeriq.Common.IDataModel>(myBinding, endpoint);");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		public static void Clear(Guid repositoryId, Celeriq.Common.IDataModel service, UserCredentials credentials)");
            sb.AppendLine("		{");
            sb.AppendLine("			var errorList = service.Clear(repositoryId, credentials);");
            sb.AppendLine("			if (errorList != null && errorList.Length > 0)");
            sb.AppendLine("			{");
            sb.AppendLine("				throw new Exception(errorList.First());");
            sb.AppendLine("			}");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		public static void UpdateData(Guid repositoryId, ListingItem item, UserCredentials credentials, Celeriq.Common.IDataModel service)");
            sb.AppendLine("		{");
            sb.AppendLine("			UpdateData(repositoryId, new List<ListingItem>() { item }, credentials, service);");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		public static void UpdateData(Guid repositoryId, IEnumerable<ListingItem> list, UserCredentials credentials, Celeriq.Common.IDataModel service)");
            sb.AppendLine("		{");
            sb.AppendLine("			var l = new List<Celeriq.Common.DataItem>();");
            sb.AppendLine("			list.ToList().ForEach(x => l.Add(x.ToTransfer()));");
            sb.AppendLine("			var errorList = service.UpdateData(repositoryId, l, credentials);");
            sb.AppendLine("			if (errorList != null && errorList.Length > 0)");
            sb.AppendLine("			{");
            sb.AppendLine("				throw new Exception(errorList.First());");
            sb.AppendLine("			}");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		public static void DeleteData(Guid repositoryId, ListingItem item, UserCredentials credentials, Celeriq.Common.IDataModel service)");
            sb.AppendLine("		{");
            sb.AppendLine("			DeleteData(repositoryId, new List<ListingItem>() { item }, credentials, service);");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		public static void DeleteData(Guid repositoryId, IEnumerable<ListingItem> list, UserCredentials credentials, Celeriq.Common.IDataModel service)");
            sb.AppendLine("		{");
            sb.AppendLine("			var l = new List<Celeriq.Common.DataItem>();");
            sb.AppendLine("			list.ToList().ForEach(x => l.Add(x.ToTransfer()));");
            sb.AppendLine("			var errorList = service.DeleteData(repositoryId, l, credentials);");
            sb.AppendLine("			if (errorList != null && errorList.Length > 0)");
            sb.AppendLine("			{");
            sb.AppendLine("				throw new Exception(errorList.First());");
            sb.AppendLine("			}");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		public static ListingResults QueryData(Guid repositoryId, ListingQuery query, Celeriq.Common.IDataModel service)");
            sb.AppendLine("		{");
            sb.AppendLine("			var t = service.Query(repositoryId, query.ToTransfer());");
            sb.AppendLine("			if (t.ErrorList != null && t.ErrorList.Length > 0)");
            sb.AppendLine("			{");
            sb.AppendLine("				throw new Exception(t.ErrorList.First());");
            sb.AppendLine("			}");
            sb.AppendLine("			return new ListingResults(t);");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("	}");
            sb.AppendLine();

            sb.AppendLine("}");
            return sb.ToString();
        }

        public static string GenerateClientConfig(Celeriq.Common.RepositorySchema repository, string serverName)
        {
            var sb = new StringBuilder();
            sb.AppendLine("	<system.serviceModel>");
            sb.AppendLine();
            sb.AppendLine("		<bindings>");
            sb.AppendLine("			<netTcpBinding>");
            sb.AppendLine("				<binding name=\"BigNetTcpBinding\" maxBufferPoolSize=\"2147483647\" maxBufferSize=\"2147483647\" maxConnections=\"10\" maxReceivedMessageSize=\"2147483647\">");
            sb.AppendLine("					<readerQuotas maxDepth=\"2147483647\" maxStringContentLength=\"2147483647\" maxArrayLength=\"2147483647\" maxBytesPerRead=\"2147483647\" maxNameTableCharCount=\"2147483647\"/>");
            sb.AppendLine("					<security mode=\"None\"></security>");
            sb.AppendLine("				</binding>");
            sb.AppendLine("			</netTcpBinding>");
            sb.AppendLine("		</bindings>");
            sb.AppendLine();
            sb.AppendLine("		<behaviors>");
            sb.AppendLine("			<endpointBehaviors>");
            sb.AppendLine("				<behavior name=\"CeleriQDataServiceBehavior\">");
            sb.AppendLine("					<dataContractSerializer maxItemsInObjectGraph=\"2147483646\"/>");
            sb.AppendLine("				</behavior>");
            sb.AppendLine("			</endpointBehaviors>");
            sb.AppendLine("		</behaviors>");
            sb.AppendLine();
            sb.AppendLine("		<client>");
            sb.AppendLine("			<endpoint address=\"net.tcp://" + serverName + ":1973/" + repository.ID + "\"");
            sb.AppendLine("								binding=\"netTcpBinding\"");
            sb.AppendLine("								contract=\"Celeriq.Common.IDataModel\"");
            sb.AppendLine("								bindingConfiguration=\"BigNetTcpBinding\"");
            sb.AppendLine("								behaviorConfiguration=\"CeleriQDataServiceBehavior\"");
            sb.AppendLine("								name=\"" + repository.ID + "\"/>");
            sb.AppendLine("		</client>");
            sb.AppendLine();
            sb.AppendLine("	</system.serviceModel>");
            return sb.ToString();
        }

        private static string GetCodeType(Celeriq.Common.RepositorySchema.DataTypeConstants type)
        {
            return GetCodeType(type, true);
        }

        private static string GetCodeType(Celeriq.Common.RepositorySchema.DataTypeConstants type, bool allowNull)
        {
            switch(type)
            {
                case Common.RepositorySchema.DataTypeConstants.Bool:
                    return "bool" + (allowNull ? "?" : string.Empty);
                case Common.RepositorySchema.DataTypeConstants.DateTime:
                    return "DateTime" + (allowNull ? "?" : string.Empty);
                case Common.RepositorySchema.DataTypeConstants.Float:
                    return "double" + (allowNull ? "?" : string.Empty);
                case Common.RepositorySchema.DataTypeConstants.GeoCode:
                    return "Celeriq.Common.GeoCode";
                case Common.RepositorySchema.DataTypeConstants.Int:
                    return "int" + (allowNull ? "?" : string.Empty);
                case Common.RepositorySchema.DataTypeConstants.String:
                    return "string";
                case Common.RepositorySchema.DataTypeConstants.List:
                    return "string[]";
                default:
                    throw new Exception("Unknown type!");
            }
        }

        private const string _validCodeChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_";
        private static string GetToken(string name)
        {
            var sb = new StringBuilder();
            foreach (var c in name)
            {
                if (_validCodeChars.IndexOf(c) == -1)
                    sb.Append("_");
                else
                    sb.Append(c);
            }
            return sb.ToString();
        }
    }
}
