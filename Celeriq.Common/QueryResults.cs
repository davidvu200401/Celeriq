using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Celeriq.Common
{
	[DataContract]
	[Serializable]
	public abstract class QueryResults : Celeriq.Common.IQueryResults
	{
		public QueryResults()
		{
			this.RecordList = new List<Celeriq.Common.RepositoryItem>();
			this.DimensionList = new List<DimensionItem>();
		}

		[DataMember]
		public virtual List<DimensionItem> DimensionList { get; private set; }
		
		[DataMember]
		public virtual long ComputeTime { get; set; }
		
		[DataMember]
		public virtual Celeriq.Common.BaseListingQuery Query { get; set; }
		
		[DataMember]
		public virtual IEnumerable<Celeriq.Common.RepositoryItem> RecordList { get; set; }
		
		[DataMember]
		public virtual int TotalRecordCount { get; set; }
		
		[DataMember]
		public virtual int TotalPageCount { get; set; }

		#region IQueryResults Implementation

		List<DimensionItem> IQueryResults.DimensionList
		{
			get { return this.DimensionList; }
		}

		Celeriq.Common.IListingQuery IQueryResults.Query
		{
			get { return this.Query; }
		}

		IEnumerable<RepositoryItem> IQueryResults.RecordList
		{
			get { return this.RecordList; }
		}

		int IQueryResults.TotalRecordCount
		{
			get { return this.TotalRecordCount; }
			set { this.TotalRecordCount = value; }
		}

		#endregion

	}

}