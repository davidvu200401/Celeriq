using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Celeriq.Utilities
{
    /// <summary>
    /// Controls the caching settings
    /// </summary>
    public abstract class BaseCacheControl
    {
        /// <summary />
        public BaseCacheControl()
        {
            this.MaxPageOffset = 1;
            this.MaxItems = 5000;
            this.CacheAll = true;
            this.CacheSorts = true;
            this.CacheFilters = false;
            this.CacheKeywords = false;
            this.CacheTierList = new List<int>();
            this.CacheTierList.Add(1);
        }

        /// <summary>
        /// The maximum number of cache elements allowed
        /// </summary>
        public virtual int MaxItems { get; set; }

        /// <summary>
        /// The maximum page number that can be cached
        /// </summary>
        public virtual int MaxPageOffset { get; set; }

        /// <summary>
        /// Determines if queries with sorting are cached
        /// </summary>
        public virtual bool CacheSorts { get; set; }

        /// <summary>
        /// Determines if queries with field filtering are cached
        /// </summary>
        public virtual bool CacheFilters { get; set; }

        /// <summary>
        /// Determines if all queries are cached with no exceptions
        /// When true all other cache filtering is ignored
        /// </summary>
        public virtual bool CacheAll { get; set; }

        /// <summary>
        /// The levels that should be cached. 1 specified dimension is tier 1, 2 specified dimensions is tier 2, etc.
        /// </summary>
        public virtual List<int> CacheTierList { get; set; }

        /// <summary>
        /// Determines if queries with keywords are cached
        /// </summary>
        public virtual bool CacheKeywords { get; set; }

    }
}