using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using Celeriq.Common;
using Celeriq.Server.Interfaces;
using Celeriq.Utilities;

namespace Celeriq.RepositoryAPI
{
    internal class FileDataProvider : DataProviderBase, IDataProvider
    {
        private FileCacheHelper<long> _deletedCache = null;
        private HashSet<long> _deletedList = new HashSet<long>();
        private string _repositoryFile = string.Empty;
        private FileCacheHelper<DataItemExtension> _fileCache = null;
        private Dictionary<long, List<DataItemExtension>> _dimensionMappedItemCache = null;

        //Determines if the cache file should be rebuilt because the item list changed
        private bool _rebuildCache = false;
        private DateTime _lastChange = DateTime.MinValue;

        public FileDataProvider(ISystemServerCore system, RepositorySchema repositoryDefinition, Repository repository)
            : base(system, repositoryDefinition)
        {
            _repository = repository;
        }

        public long MaxRecordIndex { get; set; }

        public override void WriteItem(List<DataItemExtension> newItem)
        {
            _fileCache.WriteItem(newItem.ToArray());
        }

        public override void WriteItem(DataItemExtension newItem)
        {
            _fileCache.WriteItem(newItem);
        }

        public override void UpdateRepositoryStats(RepositoryStatItem stat)
        {
            File.WriteAllText(this.CacheFile, "<repository itemcount=\"" + stat.ItemCount + "\" memorysize=\"" + stat.MemorySize + "\"></repository>");
        }

        public override void Clear()
        {
            try
            {
                //Delete the file off disk
                var repositoryCacheFolder = Path.Combine(ConfigHelper.DataPath, _repositoryDefinition.ID.ToString());
                var f = Path.Combine(repositoryCacheFolder, "repository.data");
                if (File.Exists(f))
                    File.Delete(f);
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
                _repositoryDefinition.ToDisk(backupFile);
                return true;
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
                    _deletedList.Add(existingItem.__RecordIndex);
                    _deletedCache.WriteItem(existingItem.__RecordIndex);

                    // remove the item from the _dimensions
                    foreach (var val in existingItem.DimensionValueArray)
                    {
                        if (_dimensionMappedItemCache.ContainsKey(val))
                            _dimensionMappedItemCache[val].Remove(existingItem);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                //throw;
            }

        }

        public override void SaveData()
        {
            try
            {
                var timer = new Stopwatch();
                timer.Start();

                var newCache = new FileCacheHelper<DataItem>();
                var index = 0;
                while (index < _list.Count)
                {
                    var takeCount = 1000;
                    if (index + takeCount > _list.Count)
                        takeCount = _list.Count - index;
                    newCache.WriteItem(_list.AsParallel().Skip(index).Take(takeCount).ToArray());
                    index += takeCount;
                }

                if (File.Exists(_fileCache.CacheFileName))
                    File.Delete(_fileCache.CacheFileName);

                if (File.Exists(newCache.CacheFileName))
                    File.Move(newCache.CacheFileName, _fileCache.CacheFileName);

                _deletedList.Clear();
                if (File.Exists(_deletedCache.CacheFileName))
                    File.Delete(_deletedCache.CacheFileName);

                timer.Stop();
                Logger.LogInfo("Repository SaveData: ID=" + _repositoryDefinition.ID.ToString() + ", Elapsed=" + timer.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                throw;
            }
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
            _list = itemList;
            _pkList = pkList;
            _pkindex = pkindex;
            _dimensionMappedItemCache = dimensionMappedItemCache;

            try
            {
                var repositoryCacheFolder = Path.Combine(ConfigHelper.DataPath, _repositoryDefinition.ID.ToString());
                if (!Directory.Exists(repositoryCacheFolder))
                    Directory.CreateDirectory(repositoryCacheFolder);

                //Load the dimension files
                var index = 0;
                foreach (var d in _repositoryDefinition.DimensionList)
                {
                    var f = new DimensionCacheFile(repositoryId, _repositoryDefinition.ID, d, index);
                    dimensionCache.Add(f);
                    index++;
                }

                _deletedCache = new FileCacheHelper<long>(Path.Combine(repositoryCacheFolder, "deleted.data"));

                LoadDeletedList();

                //Create the dimension lists
                index = 0;
                foreach (var d in _repositoryDefinition.DimensionList)
                {
                    dimensionList.Add(new DimensionItem() { Name = d.Name, DIdx = DimensionDefinition.DGROUP + index, NumericBreak = d.NumericBreak });
                    index++;
                }

                //Initialize the dimensions from file
                var timer2 = new Stopwatch();
                timer2.Start();
                index = 0;
                foreach (var d in _repositoryDefinition.DimensionList)
                {
                    InitializeDimension(dimensionList[index], dimensionCache[index]);
                    index++;
                }
                timer2.Stop();
                Logger.LogInfo("InitializeDimension All: Elapsed=" + timer2.ElapsedMilliseconds + ", Count=" + _list.Count);

                //The data files
                _repositoryFile = Path.Combine(repositoryCacheFolder, "repository.data");

                _fileCache = new FileCacheHelper<DataItemExtension>(_repositoryFile);

                //Load dimension/item mapping cache
                foreach (var d in dimensionList)
                {
                    foreach (var r in d.RefinementList)
                    {
                        _dimensionMappedItemCache.Add(r.DVIdx, new List<DataItemExtension>());
                    }
                }

                #region Load Data

                var timer = new Stopwatch();
                timer.Start();

                try
                {
                    var tlist = new List<DataItemExtension>();
                    var fh = new FileCacheHelper<DataItemExtension>();
                    tlist.AddRange(fh.LoadAll(_fileCache.CacheFileName));
                    foreach (var item in tlist)
                    {
                        if (!_deletedList.Contains(item.__RecordIndex))
                        {
                            //pre-cache the dimension values to items
                            item.DimensionValueArray.ForEach(
                                x =>
                                {
                                    if (_dimensionMappedItemCache.ContainsKey(x))
                                        _dimensionMappedItemCache[x].Add(item);
                                    else
                                        Console.WriteLine("Cache miss: 0x2876");
                                });
                            _list.Add(item);
                            pkList.Add((int)item.ItemArray[pkindex]);
                        }
                    }

                    timer.Stop();
                    this.Compress();
                    timer.Start();

                    if (_list.Count != 0)
                        this.MaxRecordIndex = _list.Max(x => x.__RecordIndex);

                    this.RefreshStatsFromCache();
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex);
                    throw;
                }

                timer.Stop();
                _system.LogRepositoryPerf(new RepositorySummmaryStats
                {
                    ActionType = RepositoryActionConstants.LoadData,
                    Elapsed = (int)timer.ElapsedMilliseconds,
                    RepositoryId = _repositoryDefinition.ID,
                    ItemCount = 1,
                    //ItemCount = _list.Count,
                });

                #endregion

            }
            catch (Exception ex)
            {
                Logger.LogError("Load: " + ex.ToString());
                throw;
            }
        }

        private void InitializeDimension(DimensionItem dimension, DimensionCacheBase cacheFile)
        {
            try
            {
                var fh = new FileCacheHelper<RefinementItem>();
                dimension.RefinementList.AddRange(fh.LoadAll(((DimensionCacheFile)cacheFile).FileName));
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                throw;
            }
        }

        private void LoadDeletedList()
        {
            try
            {
                var fh = new FileCacheHelper<long>();
                var tlist = fh.LoadAll(_deletedCache.CacheFileName);
                tlist.ForEach(x => _deletedList.Add(x));
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                throw;
            }
        }

        public override bool NeedsCompress
        {
            get { return (_deletedList.Count > 0); }
        }

        public override void Compress()
        {
            try
            {
                //var timer = new Stopwatch();
                //timer.Start();

                //If there were any deleted items then compact the file and reset the deleted items
                if (this.NeedsCompress)
                {
                    //var timerInner = new CQTimer();
                    this.SaveData();
                    this.PersistAll();
                    //AddProfileItem(ActionConstants.Compress, timerInner);
                }
                //timer.Stop();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public override void PersistAll()
        {
        }

        private string CacheFile
        {
            get
            {
                var repositoryCacheFolder = Path.Combine(ConfigHelper.DataPath, _repositoryDefinition.ID.ToString());
                return Path.Combine(repositoryCacheFolder, "repository.cache");
            }
        }

        public override void RefreshStatsFromCache(string cacheFolder, out long itemCount, out long memorySize)
        {
            itemCount = 0;
            memorySize = 0;
            try
            {
                if (Directory.Exists(cacheFolder))
                {
                    var fileName = Path.Combine(cacheFolder, "repository.cache");
                    if (File.Exists(fileName))
                    {
                        var document = new XmlDocument();
                        document.Load(fileName);
                        memorySize = XmlHelper.GetAttribute(document.DocumentElement, "memorysize", 0);
                        itemCount = XmlHelper.GetAttribute(document.DocumentElement, "itemcount", 0);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError("GetItemCountFromCache Error\n" + ex.ToString());
            }
        }

        private void RefreshStatsFromCache()
        {
            long ic;
            long ms;
            RefreshStatsFromCache(this.CacheFile, out ic, out ms);
            _repository.RemotingObject.DataMemorySize = ms;
            _repository.RemotingObject.ItemCount = ic;
            this.MemorySize = _repository.RemotingObject.DataMemorySize;
        }

    }
}