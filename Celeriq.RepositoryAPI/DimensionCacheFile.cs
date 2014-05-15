using Celeriq.Common;
using Celeriq.DataCore.EFDAL;
using Celeriq.DataCore.EFDAL.Entity;
using Celeriq.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Celeriq.Server.Interfaces;
using System.IO;

namespace Celeriq.RepositoryAPI
{
    internal class DimensionCacheFile : DimensionCacheBase
    {
        private FileCacheHelper<RefinementItem> _cache = null;

        public string FileName { get; private set; }

        public DimensionCacheFile(int repositoryId, Guid repositoryKey, DimensionDefinition dimensionDefinition, int index)
            : base(repositoryId, repositoryKey, dimensionDefinition, index)
        {
            this.FileName = Path.Combine(ConfigHelper.DataPath, repositoryKey.ToString(), "d1" + index.ToString("000000") + ".data");
            _cache = new FileCacheHelper<RefinementItem>(this.FileName);
        }

        public override void WriteItem(RefinementItem refinementItem)
        {
            try
            {
                _cache.WriteItem(refinementItem);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                throw;
            }
        }

    }
}
