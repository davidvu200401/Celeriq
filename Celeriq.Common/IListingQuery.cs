using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Celeriq.Common
{
	public interface IListingQuery
	{
		List<long> DimensionValueList { get; set; }
		List<IFieldSort> FieldSorts { get; set; }
		List<IFieldFilter> FieldFilters { get; set; }
		string Keyword { get; set; }
		int PageOffset { get; set; }
		int RecordsPerPage { get; set; }
		NamedItemList NonParsedFieldList { get; set; }
		string PageName { get; set; }
	}
}
