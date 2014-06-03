using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Celeriq.Common;
using Celeriq.Server.Interfaces;

namespace Celeriq.RepositoryAPI
{
    internal abstract class DataProviderBase : IDataProvider
    {
        internal DataProviderBase(ISystemServerCore system, RepositorySchema repositoryDefinition)
        {
            _system = system;
            _repositoryDefinition = repositoryDefinition;
            this.PkIndex = -1;
            this.LastInserted = null;
            this.MemoryDirty = true;
        }

        protected ISystemServerCore _system = null;
        protected RepositorySchema _repositoryDefinition = null;
        protected List<DataItemExtension> _list = null;
        protected Repository _repository = null;
        protected int _pkindex = -1; //the index in the data array of the primary key
        protected HashSet<int> _pkList = null;

        public DateTime LastAccess { get; set; }
        public int PkIndex { get; set; }
        public DataItem LastInserted { get; set; }
        public bool MemoryDirty { get; set; }

        public abstract void Load(
            int repositoryId,
            List<DataItemExtension> _list,
            List<DimensionItem> dimensionList,
            List<DimensionCacheBase> dimensionCache,
            HashSet<int> _pkList,
            Dictionary<long, List<DataItemExtension>> _dimensionMappedItemCache,
            int _pkindex,
            Func<DataItemExtension, bool> processDimensionsFunc);

        public abstract void PersistAll();
        public abstract void WriteItem(DataItemExtension newItem);
        public abstract void WriteItem(List<DataItemExtension> newItem);
        public abstract void DeleteItem(int primaryKey);
        public abstract void SaveData();
        public abstract void Compress();
        public abstract bool NeedsCompress { get; }

        public long MaxRecordIndex { get; set; }
        public long MemorySize { get; set; }
        public abstract void UpdateRepositoryStats(RepositoryStatItem stat);
        public abstract void Clear();
        public abstract bool Export(string backupFile);
        public abstract void RefreshStatsFromCache(string cacheFolder, out long itemCount, out long memorySize);
    }
}
