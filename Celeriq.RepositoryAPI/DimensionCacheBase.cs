using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Celeriq.Common;
using Celeriq.Utilities;

namespace Celeriq.RepositoryAPI
{
    internal abstract class DimensionCacheBase : IDimensionCache
    {
        protected int _repositoryId;
        protected Guid _repositoryKey;
        protected long _lastDIdx = 0;
        protected const long FIRST_DIDX = 1000000;

        public DimensionCacheBase(int repositoryId, Guid repositoryKey, DimensionDefinition dimensionDefinition, int index)
        {
            _repositoryId = repositoryId;
            _repositoryKey = repositoryKey;
            this.DimensionDefinition = dimensionDefinition;
            this.RefinementList = new List<RefinementItem>();
        }

        public List<RefinementItem> RefinementList { get; protected set; }

        public DimensionDefinition DimensionDefinition { get; protected set; }

        public abstract void WriteItem(RefinementItem refinementItem);

    }
}
