using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Celeriq.Common;
using Celeriq.DataCore.EFDAL;
using Celeriq.Server.Interfaces;
using Celeriq.Utilities;

namespace Celeriq.RepositoryAPI
{
    internal class SqlDataProvider : DataProviderBase, IDataProvider
    {
        private Dictionary<long, List<DataItemExtension>> _dimensionMappedItemCache = null;

        //Determines if the cache file should be rebuilt because the item list changed
        private bool _rebuildCache = false;
        private DateTime _lastChange = DateTime.MinValue;
        private int _repositoryId = 0;

        public SqlDataProvider(ISystemServerCore system, RepositorySchema repositoryDefinition, Repository repository)
            : base(system, repositoryDefinition)
        {
            _repository = repository;
        }

        public override void WriteItem(List<DataItemExtension> newItem)
        {
        }

        public override void WriteItem(DataItemExtension newItem)
        {
        }

        public override void UpdateRepositoryStats(RepositoryStatItem stat)
        {
            try
            {
                Celeriq.DataCore.EFDAL.Entity.RepositoryDefinition.UpdateData(x => x.ItemCount, x => x.UniqueKey == _repositoryDefinition.ID, (int)stat.ItemCount);
                Celeriq.DataCore.EFDAL.Entity.RepositoryDefinition.UpdateData(x => x.MemorySize, x => x.UniqueKey == _repositoryDefinition.ID, stat.MemorySize);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
        }

        public override void Clear()
        {
            try
            {
                var repoId = 0;
                using (var context = new Celeriq.DataCore.EFDAL.DataCoreEntities())
                {
                    var item = context.RepositoryDefinition.FirstOrDefault(x => x.UniqueKey == this._repositoryDefinition.ID);
                    if (item == null) return;
                    repoId = item.RepositoryId;
                }
                Celeriq.DataCore.EFDAL.Entity.RepositoryData.DeleteData(x => x.RepositoryId == repoId);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                throw;
            }
        }

        public override bool Export(string backupFile)
        {
            try
            {
                using (var context = new Celeriq.DataCore.EFDAL.DataCoreEntities())
                {
                    var item = context.RepositoryDefinition.FirstOrDefault(x => x.UniqueKey == this._repositoryDefinition.ID);
                    if (item != null)
                    {
                        if (File.Exists(backupFile))
                        {
                            File.Delete(backupFile);
                            System.Threading.Thread.Sleep(400);
                        }

                        File.WriteAllText(backupFile, item.DefinitionData.BinToObject<RepositorySchema>().ToXml());

                        Logger.LogInfo("Repository ExportSchema: ID=" + _repositoryDefinition.ID);
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                throw;
            }
        }

        public override void DeleteItem(int primaryKey)
        {
            try
            {
                //Check if this item is in the list
                if (!_pkList.Contains(primaryKey))
                    return;

                //If this item is in the list then actually find it and remove it
                var existingItem = _list.AsParallel().FirstOrDefault(x => (int)x.ItemArray[_pkindex] == primaryKey);
                if (existingItem != null)
                {
                    _list.Remove(existingItem);

                    //Delete from database
                    Celeriq.DataCore.EFDAL.Entity.RepositoryData.DeleteData(x => x.RepositoryDataId == existingItem.__RecordIndex);

                    // remove the item from the _dimensions
                    foreach (var val in existingItem.DimensionValueArray)
                    {
                        _dimensionMappedItemCache[val].Remove(existingItem);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                throw;
            }
        }

        public override void SaveData()
        {
            //Do Nothing
        }

        public override void Load(int repositoryId,
            List<DataItemExtension> itemList,
            List<DimensionItem> dimensionList,
            List<DimensionCacheBase> dimensionCache,
            HashSet<int> pkList,
            Dictionary<long, List<DataItemExtension>> dimensionMappedItemCache,
            int pkindex,
            Func<DataItemExtension, bool> processDimensionsFunc)
        {
            _repositoryId = repositoryId;
            _list = itemList;
            _pkList = pkList;
            _pkindex = pkindex;
            _dimensionMappedItemCache = dimensionMappedItemCache;

            try
            {
                //Create the master list
                using (var context = new DataCoreEntities())
                {
                    #region Dimensions

                    //Load the dimension files
                    var index = 0;
                    foreach (var d in _repositoryDefinition.DimensionList)
                    {
                        var f = new DimensionCacheDatabase(repositoryId, _repositoryDefinition.ID, d, index);
                        dimensionCache.Add(f);
                        index++;
                    }

                    //Create the dimension lists
                    index = 0;
                    foreach (var d in _repositoryDefinition.DimensionList)
                    {
                        dimensionList.Add(new DimensionItem() { Name = d.Name, DIdx = DimensionDefinition.DGROUP + index, NumericBreak = d.NumericBreak });
                        index++;
                    }

                    var dbDimensions = context.DimensionStore
                        .Include(x => x.DimensionDataList)
                        .Where(x => x.RepositoryDefinition.UniqueKey == _repositoryDefinition.ID)
                        .ToList();

                    //Initialize the dimensions from file
                    var timer2 = new Stopwatch();
                    timer2.Start();
                    index = 0;
                    foreach (var d in _repositoryDefinition.DimensionList)
                    {
                        var didx = dimensionCache[index].DimensionDefinition.DIdx;
                        var store = dbDimensions.FirstOrDefault(x => x.DIdx == didx);
                        if (store != null)
                            store.DimensionDataList.ToList().ForEach(x => dimensionCache[index].RefinementList.Add(x.Data.BinToObject<RefinementItem>()));
                        dimensionList.First(x => x.DIdx == d.DIdx).RefinementList.AddRange(dimensionCache[index].RefinementList);
                        index++;
                    }
                    timer2.Stop();
                    Logger.LogInfo("InitializeDimension All: Elapsed=" + timer2.ElapsedMilliseconds + ", Count=" + _list.Count);

                    //Load dimension/item mapping cache
                    foreach (var d in dimensionList)
                    {
                        foreach (var r in d.RefinementList)
                        {
                            dimensionMappedItemCache.Add(r.DVIdx, new List<DataItemExtension>());
                        }
                    }

                    #endregion

                    #region Load Data

                    var timer = new Stopwatch();
                    timer.Start();

                    try
                    {
                        //TODO: Need to paginate for large repositories
                        //long maxId = 0;
                        var tlist = new List<DataItemExtension>();
                        var list = context.RepositoryData
                            //.Where(x => x.RepositoryId == repositoryId && x.RepositoryDataId > maxId)
                            .Where(x => x.RepositoryId == repositoryId)
                            .OrderBy(x => x.RepositoryDataId)
                            //.Take(500)
                            .ToList();

                        foreach (var dbIitem in list)
                        {
                            var raw = dbIitem.Data.BinToObject<DataItem>();
                            var item = new DataItemExtension(raw, _repositoryDefinition);
                            item.__RecordIndex = dbIitem.RepositoryDataId;
                            item.Keyword = dbIitem.Keyword;
                            tlist.Add(item);
                            item.DimensionValueArray.ForEach(x => dimensionMappedItemCache[x].Add(item));
                            dimensionList.ForEach(x => item.DimensionSingularValueArray.Add(null));
                            processDimensionsFunc(item);
                            _list.Add(item);
                            pkList.Add((int)item.ItemArray[pkindex]);
                            //maxId = dbIitem.RepositoryDataId;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }

                    timer.Stop();
                    _system.LogRepositoryPerf(new RepositorySummmaryStats
                    {
                        ActionType = RepositoryActionConstants.LoadData,
                        Elapsed = (int)timer.ElapsedMilliseconds,
                        RepositoryId = _repositoryDefinition.ID,
                        ItemCount = _list.Count,
                    });

                    #endregion
                }
            }
            catch (Exception ex)
            {
                Logger.LogError("ReloadMeFromDatabase: " + ex.ToString());
                throw;
            }
        }

        public override bool NeedsCompress
        {
            get { return false; }
        }

        public override void Compress()
        {
        }

        public override void PersistAll()
        {
        }

        public override void RefreshStatsFromCache(string cacheFolder, out long itemCount, out long memorySize)
        {
            itemCount = 0;
            memorySize = 0;
            try
            {
                using (var context = new DataCoreEntities())
                {
                    var r = context.RepositoryDefinition.FirstOrDefault(x => x.RepositoryId == _repositoryId);
                    if (r == null) return;
                    memorySize = r.MemorySize;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError("GetItemCountFromCache Error\n" + ex.ToString());
            }
        }

    }
}