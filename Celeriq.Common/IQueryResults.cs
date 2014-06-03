using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Celeriq.Common
{
	public interface IQueryResults
	{
		[DataMember]
		List<DimensionItem> DimensionList { get; }
		
		[DataMember]
		IListingQuery Query { get; }
		
		[DataMember]
		IEnumerable<Celeriq.Common.RepositoryItem> RecordList { get; }

		[DataMember]
		int TotalRecordCount { get; set; }

		[DataMember]
		long ComputeTime { get; set; }
	}
}
