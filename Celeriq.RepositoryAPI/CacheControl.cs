using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Celeriq.RepositoryAPI
{
	internal class CacheControl : Celeriq.Utilities.BaseCacheControl
	{
		/// <summary>
		/// Given a query object, this method determine if it should be cached
		/// </summary>
		/// <param name="query"></param>
		/// <returns></returns>
		public bool ShouldCache(Celeriq.Common.DataQuery query)
		{
			if (this.CacheAll) return true;
			if (!this.CacheKeywords && !string.IsNullOrEmpty(query.Keyword)) return false;

			var dimensionValueList = query.DimensionValueList;
			var fieldSorts = query.FieldSorts;
			var fieldFilters = query.FieldFilters;

			if (dimensionValueList != null)
			{
				if (this.CacheTierList.Distinct().Count(x => x == dimensionValueList.Count()) == 1) return true;
			}
			else if (dimensionValueList == null)
			{
				if (query.PageOffset == 1) return true; //Always save no dimensions page 1
			}

			//If do not cache sorts then check for them
			if (!this.CacheSorts && fieldSorts != null)
			{
				if (fieldSorts.Count() > 0) return false;
			}

			//If do not cache sorts then check for them
			if (!this.CacheFilters && fieldFilters != null)
			{
				if (fieldFilters.Count() > 0) return false;
			}

			//Make sure page number is in defined page range
			if (query.PageOffset > this.MaxPageOffset)
				return false;

			return true;
		}
	}
}
