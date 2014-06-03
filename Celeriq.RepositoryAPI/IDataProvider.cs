using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Celeriq.Common;
using Celeriq.Server.Interfaces;

namespace Celeriq.RepositoryAPI
{
    internal interface IDataProvider
    {
        DateTime LastAccess { get; set; }
        int PkIndex { get; set; } //the index in the data array of the primary key

        //Diagnostic data
        DataItem LastInserted { get; set; }
        bool MemoryDirty { get; set; }

        void Load(int repositoryId,
            List<DataItemExtension> _list,
            List<DimensionItem> dimensionList,
            List<DimensionCacheBase> dimensionCache,
            HashSet<int> _pkList,
            Dictionary<long, List<DataItemExtension>> _dimensionMappedItemCache,
            int _pkindex,
            Func<DataItemExtension, bool> processDimensionsFunc);

        void Compress();
        bool NeedsCompress { get; }
        void PersistAll();
        void WriteItem(DataItemExtension newItem);
        void WriteItem(List<DataItemExtension> newItem);
        void DeleteItem(int primaryKey);
        void SaveData();
        long MaxRecordIndex { get; set; }
        long MemorySize { get; set; }
        void UpdateRepositoryStats(RepositoryStatItem stat);
        void Clear();
        bool Export(string backupFile);
        void RefreshStatsFromCache(string cacheFolder, out long itemCount, out long memorySize);
    }
}
