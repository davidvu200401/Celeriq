using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Celeriq.Common
{
	[Serializable]
	[DataContract]
	[KnownType(typeof(GeoCode))]
	[KnownType(typeof(GeoCodeFieldFilter))]
	[KnownType(typeof(DimensionItem))]
	[KnownType(typeof(RefinementItem))]
	[KnownType(typeof(BaseListingQuery))]
	[KnownType(typeof(DataItem))]
	[KnownType(typeof(DataQuery))]
	[KnownType(typeof(FieldSort))]
	[KnownType(typeof(FieldFilter))]
	[KnownType(typeof(GeoCodeFieldFilter))]
	[KnownType(typeof(string[]))]
	[KnownType(typeof(NamedItem))]
	[KnownType(typeof(NamedItemList))]
	public class DataQueryResults
	{
		public DataQueryResults()
		{
			this.QueryTime = DateTime.Now;
		}

		[DataMember]
		public List<DimensionItem> DimensionList;

		[DataMember]
		public DataQuery Query;

		[DataMember]
		public List<DataItem> RecordList;

		[DataMember]
		public int TotalRecordCount;

		[DataMember]
		public long ComputeTime;

		[DataMember]
		public string[] ErrorList;

		[DataMember]
		public long VersionHash { get; set; }

		public DateTime QueryTime { get; set; }

	}
}
