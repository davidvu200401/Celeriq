using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Celeriq.Common
{
	public interface IListingResults
	{
		List<Celeriq.Common.DimensionItem> DimensionList { get; }
		IListingQuery Query { get; }
		List<IListingItem> RecordList { get; }
		int TotalRecordCount { get; }
		int TotalPageCount { get; }
		//DimensionStore AllDimensions { get; private set; }
		long ComputeTime { get; }
	}
}
