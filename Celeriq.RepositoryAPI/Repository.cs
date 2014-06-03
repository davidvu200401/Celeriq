#pragma warning disable 0168
using System;
using System.Collections.Generic;
using System.Linq;
using Celeriq.Utilities;
using System.IO;
using Celeriq.Common;
using System.Diagnostics;
using System.Xml;
using Celeriq.Server.Interfaces;
using Celeriq.DataCore.EFDAL;
using System.Threading.Tasks;
using System.Text;

namespace Celeriq.RepositoryAPI
{
    public class Repository : IRepository
    {
        #region Class Members

        private const int THREAD_THRESHOLD = 100000; //Number of records needed to use threading
        private const int THREAD_CORE_COUNT = 3; //Number of processors needed to use threading
        private const int MAX_PROFILE_COUNT = 100;

        private RepositorySchema _repositoryDefinition = null;
        private int _repositoryId;
        private QueryLogger _queryLogger;

        private List<DataItemExtension> _list = null;
        private List<DimensionItem> _dimensionList = null;
        private List<DimensionCacheBase> _dDimensionCache = null;
        private int _pkindex = -1; //the index in the data array of the primary key

        private Dictionary<long, List<DataItemExtension>> _dimensionMappedItemCache = null;

        private int _coreCount = 0;

        private HashSet<int> _pkList = new HashSet<int>();
        private SequencedHashTable<int, DataQueryResults> _resultsCache = null;
        private CacheControl _cacheControl = null;
        private int _queryCacheTimeout = 300; //seconds
        private long _versionHash = 0;
        private bool _isLoaded = false;
        private object _isLoadedLocker = new object();
        private KeyPair _masterKeys = null;

        //Determines if the cache file should be rebuilt because the item list changed
        private ISystemServerCore _system = null;
        private List<ProfileItem> _profileList = new List<ProfileItem>();
        private IDataProvider _dataProvider = null;

        #endregion

        #region Constructors

        public Repository(int repositoryId, RepositorySchema schema, KeyPair masterKeys, ISystemServerCore system)
        {
            try
            {
                this.SyncObject = new Celeriq.Utilities.CeleriqLock(System.Threading.LockRecursionPolicy.SupportsRecursion, schema.ID);

                _system = system;
                _masterKeys = masterKeys;
                _repositoryDefinition = schema;
                _repositoryId = repositoryId;

                if (_system.Storage == StorageTypeConstants.Database)
                    _dataProvider = new SqlDataProvider(system, _repositoryDefinition, this);
                else if (_system.Storage == StorageTypeConstants.File)
                    _dataProvider = new FileDataProvider(system, _repositoryDefinition, this);
                else throw new Exception("Unknown storage!");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                throw;
            }
        }

        private bool _isReloading = false;

        private Celeriq.Utilities.CeleriqLock _syncObject = null;
        public Celeriq.Utilities.CeleriqLock SyncObject
        {
            get { return _syncObject; }
            set
            {
                if (_syncObject == null) _syncObject = value;
                else throw new Exception("Object cannot be reset!");
            }
        }

        private void ReloadMe()
        {
            try
            {
                if (_isReloading) return;
                _isReloading = true;

                var timer = new CQTimer();

                _queryLogger = new QueryLogger(_repositoryDefinition.ID, _system.Storage);
                _versionHash = _repositoryDefinition.VersionHash;
                _pkindex = _repositoryDefinition.FieldList.IndexOf(_repositoryDefinition.PrimaryKey);
                _list = new List<DataItemExtension>();
                _coreCount = Environment.ProcessorCount;
                _cacheControl = new CacheControl();

                _dDimensionCache = new List<DimensionCacheBase>();
                _dimensionList = new List<DimensionItem>();
                _dimensionMappedItemCache = new Dictionary<long, List<DataItemExtension>>();
                _dataProvider.Load(_repositoryId, _list, _dimensionList, _dDimensionCache, _pkList, _dimensionMappedItemCache, _pkindex, ProcessDimensions);
                _resultsCache = new SequencedHashTable<int, DataQueryResults>();
                this.IsLoaded = true;
                _dataProvider.MemoryDirty = false;

                //Recalc the memory if it is 0
                if (this.RemotingObject.DataMemorySize == 0)
                    _needStatUpdate = true;

                RefreshStatsFromCache();
                timer.Stop();
                _system.NotifyLoad(_repositoryDefinition.ID, timer.Elapsed, _list.Count);

                double avgTime = 0;
                if (timer.Elapsed > 0) avgTime = (_list.Count / (timer.Elapsed / 1000.0));
                Logger.LogInfo("Repository Data Loaded: ID=" + _repositoryDefinition.ID.ToString() + ", Elapsed=" + timer.Elapsed + ", Count=" + _list.Count + ", Avg=" + avgTime.ToString("#####0.0"));
                AddProfileItem(RepositoryActionConstants.LoadData, timer, _list.Count);
            }
            catch (Exception ex)
            {
                Logger.LogError("Repository Loading Error\n" + ex.ToString());
                throw;
            }
            finally
            {
                _isReloading = false;
            }
        }

        #endregion

        private delegate void LogActionDelegate(DataQuery query, int elapsed, int count, bool fromcache);

        #region Properties

        public RemotingObjectCache RemotingObject { get; set; }

        public DateTime LastAccess
        {
            get { return _dataProvider.LastAccess; }
        }

        public bool IsLoaded
        {
            get
            {
                lock (_isLoadedLocker) { return _isLoaded; }
            }
            private set
            {
                lock (_isLoadedLocker) { _isLoaded = value; }
            }
        }

        #endregion

        #region Unload

        public void UnloadData()
        {
            using (var q = new AcquireWriterLock(this.SyncObject, "UnloadData", _repositoryDefinition.ID))
            {
                try
                {
                    if (!this.IsLoaded) return;
                    var timer = new CQTimer();

                    //Ensure all is saved
                    if (_dataProvider.MemoryDirty)
                        _dataProvider.SaveData();

                    this.FlushCache(true);
                    _dataProvider.MemoryDirty = false;

                    var count = _list.Count;

                    //Clear all data from memory
                    if (_list != null) _list.Clear();
                    _list = null;
                    if (_dimensionList != null) _dimensionList.Clear();
                    _dimensionList = null;
                    _dDimensionCache = null;
                    _pkindex = -1;
                    if (_dimensionMappedItemCache != null) _dimensionMappedItemCache.Clear();
                    _dimensionMappedItemCache = null;
                    _coreCount = 0;
                    if (_pkList != null) _pkList.Clear();
                    _pkList = new HashSet<int>();
                    if (_resultsCache != null) _resultsCache.Clear();
                    _resultsCache = null;
                    _profileList.Clear();
                    //if (_fileCache != null)
                    //    (_fileCache as IDisposable).Dispose();
                    //_fileCache = null;
                    _cacheControl = null;
                    _versionHash = 0;

                    //Mark this repository as unloaded
                    _dataProvider.LastAccess = DateTime.MinValue;
                    this.IsLoaded = false;

                    AddProfileItem(RepositoryActionConstants.UnloadData, timer, count);
                    Logger.LogInfo("Repository UnloadData: ID=" + _repositoryDefinition.ID);

                    _system.LogRepositoryPerf(new RepositorySummmaryStats
                    {
                        ActionType = RepositoryActionConstants.UnloadData,
                        RepositoryId = _repositoryDefinition.ID,
                        Elapsed = timer.Elapsed,
                        ItemCount = count,
                    });

                    _system.NotifyUnload(_repositoryDefinition.ID, timer.Elapsed, count);
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex);
                    throw;
                }
            }
        }

        #endregion

        #region LoadData

        public void LoadData(UserCredentials credentials)
        {
            if (!IsValidCredentials(credentials))
                throw new Exception("Invalid credentials");

            using (var q = new AcquireWriterLock(this.SyncObject, "LoadData", _repositoryDefinition.ID))
            {
                try
                {
                    if (!this.IsLoaded)
                        ReloadMe();
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex);
                    throw;
                }
            }
        }

        #endregion

        #region UpdateIndex

        public void UpdateIndexList(IEnumerable<DataItem> list, UserCredentials credentials)
        {
            if (!IsValidCredentials(credentials))
                throw new Exception("Invalid credentials");

            using (var q = new AcquireWriterLock(this.SyncObject, "UpdateIndexList", _repositoryDefinition.ID))
            {
                try
                {
                    if (!this.IsLoaded)
                        ReloadMe();

                    if (list == null) return;
                    var count = list.Count();
                    if (count == 0) return;

                    var timer = new CQTimer();
                    if (_system.Storage == Server.Interfaces.StorageTypeConstants.Database)
                    {
                        #region Database
                        using (var context = new Celeriq.DataCore.EFDAL.DataCoreEntities())
                        {
                            //Save the DB items and save all at one time
                            var cacheList = new Dictionary<DataItem, Celeriq.DataCore.EFDAL.Entity.RepositoryData>();

                            //Add the actual items to the master list
                            var timerDb = new CQTimer();
                            foreach (var item in list)
                            {
                                //Check to see if this item exists and if so remove it
                                this.DeleteItem((int)item.ItemArray[_pkindex]);

                                var dbItem = new Celeriq.DataCore.EFDAL.Entity.RepositoryData()
                                {
                                    RepositoryId = _repositoryId,
                                    Data = item.ObjectToBin(),
                                    Keyword = ServerUtilities.CreateWordBlob(SumKeywordBlob(item)),
                                };
                                context.AddItem(dbItem);
                                cacheList.Add(item, dbItem);
                            }
                            context.SaveChanges();
                            timerDb.Stop();
                            var dbTimePer = (int)((timerDb.Elapsed * 1.0) / count);

                            //Database items are saved so now 
                            foreach (var item in cacheList.Keys)
                            {
                                var timerInner = new CQTimer();

                                var dbItem = cacheList[item];

                                _dataProvider.LastAccess = DateTime.Now;
                                var newItem = new DataItemExtension(item, _repositoryDefinition);
                                newItem.Keyword = dbItem.Keyword;
                                if (!IsItemValid(newItem))
                                    throw new Celeriq.Common.Exceptions.SchemaVersionException();

                                newItem.__RecordIndex = dbItem.RepositoryDataId;
                                _list.Add(newItem);
                                _dataProvider.MemoryDirty = true;
                                newItem.__RecordIndex = newItem.__RecordIndex;

                                _dimensionList.ForEach(x => newItem.DimensionSingularValueArray.Add(null));
                                ProcessDimensions(newItem);
                                _dataProvider.WriteItem(newItem);
                                _dataProvider.LastInserted = newItem;
                                _pkList.Add((int)newItem.ItemArray[_pkindex]);

                                //Clear cache
                                _resultsCache.Clear();

                                AddProfileItem(RepositoryActionConstants.SaveData, timerInner, count);
                            }
                            context.SaveChanges();
                        }
                    #endregion
                    }
                    else if (_system.Storage == Server.Interfaces.StorageTypeConstants.File)
                    {
                        #region File
                        //Add the actual items to the master list
                        var writeList = new List<DataItemExtension>();
                        foreach (var item in list)
                        {
                            var timerInner = new CQTimer();

                            _dataProvider.LastAccess = DateTime.Now;
                            var newItem = new DataItemExtension(item, _repositoryDefinition);
                            if (!IsItemValid(newItem))
                                throw new Celeriq.Common.Exceptions.SchemaVersionException();

                            newItem.Keyword = ServerUtilities.CreateWordBlob(SumKeywordBlob(item));
                            //Check to see if this item exists and if so remove it
                            this.DeleteItem((int)newItem.ItemArray[_pkindex]);

                            newItem.__RecordIndex = _dataProvider.MaxRecordIndex;
                            _dataProvider.MaxRecordIndex++;
                            _list.Add(newItem);
                            _dataProvider.MemoryDirty = true;
                            newItem.__RecordIndex = newItem.__RecordIndex;

                            _dimensionList.ForEach(x => newItem.DimensionSingularValueArray.Add(null));
                            ProcessDimensions(newItem);
                            writeList.Add(newItem);
                            //_dataProvider.WriteItem(newItem); //Moved to bottom of loop
                            _dataProvider.LastInserted = newItem;
                            _pkList.Add((int)newItem.ItemArray[_pkindex]);

                            //Clear cache
                            _resultsCache.Clear();

                            AddProfileItem(RepositoryActionConstants.SaveData, timerInner, count);
                        }
                        _dataProvider.WriteItem(writeList);
                        #endregion

                    }

                    _needStatUpdate = true;
                    this.FlushCache();

                    timer.Stop();
                    Logger.LogInfo("UpdateIndexList: ID=" + _repositoryDefinition.ID + ", TotalCount=" + _list.Count + ", Count=" + count + ", Elapsed=" + timer.Elapsed + ", Avg=" + ((count == 0) ? "0.0" : ((timer.Elapsed * 1.0) / count).ToString("###,##0.0")));
                    _system.LogRepositoryPerf(new RepositorySummmaryStats
                                              {
                                                  ActionType = RepositoryActionConstants.SaveData,
                                                  Elapsed = timer.Elapsed,
                                                  RepositoryId = _repositoryDefinition.ID,
                                                  ItemCount = count,
                                              });
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex);
                    throw;
                }
            }
        }

        //private long GetItemCountFromCache()
        //{
        //    using (var q = new AcquireWriterLock(this.SyncObject))
        //    {
        //        try
        //        {
        //            _dataProvider.UpdateRepositoryStats
        //            return 0; //TODO
        //        }
        //        catch (Exception ex)
        //        {
        //            Logger.LogError("GetItemCountFromCache Error\n" + ex.ToString());
        //            return 0;
        //        }
        //    }
        //}

        #endregion

        #region DeleteData

        public void DeleteData(IEnumerable<DataItem> list, UserCredentials credentials)
        {
            if (!IsValidCredentials(credentials))
                throw new Exception("Invalid credentials");

            using (var q = new AcquireWriterLock(this.SyncObject, "DeleteData", _repositoryDefinition.ID))
            {
                try
                {
                    if (!this.IsLoaded)
                        ReloadMe();

                    var timer = new Stopwatch();
                    timer.Start();

                    list.ToList().ForEach(x => DeleteItem((int)x.ItemArray[_pkindex]));
                    _needStatUpdate = true;

                    this.FlushCache();
                    timer.Stop();
                    _system.LogRepositoryPerf(new RepositorySummmaryStats
                    {
                        ActionType = RepositoryActionConstants.DeleteData,
                        Elapsed = (int)timer.ElapsedMilliseconds,
                        RepositoryId = _repositoryDefinition.ID,
                        ItemCount = list.Count(),
                    });

                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        private void DeleteItem(int primaryKey)
        {
            try
            {
                var timer = new CQTimer();

                _dataProvider.LastAccess = DateTime.Now;
                _dataProvider.DeleteItem(primaryKey);

                //Clear cache
                _resultsCache.Clear();

                AddProfileItem(RepositoryActionConstants.DeleteData, timer);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                throw;
            }
        }

        #endregion

        #region Query

        public DataQueryResults Query(DataQuery query)
        {
            if (!IsValidCredentials(query.Credentials))
                throw new Exception("Invalid credentials");

            if (!this.IsLoaded)
            {
                using (var q = new AcquireWriterLock(this.SyncObject, "Query", _repositoryDefinition.ID))
                {
                    try
                    {
                        ReloadMe();
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError(ex);
                        throw;
                    }
                }
            }

            using (var q = new AcquireReaderLock(this.SyncObject))
            {
                try
                {
                    _dataProvider.LastAccess = DateTime.Now;
                    var newResults = new DataQueryResults();
                    newResults.DimensionList = new List<DimensionItem>();
                    newResults.RecordList = new List<DataItem>();
                    newResults.Query = query;
                    newResults.VersionHash = _versionHash;

                    if (_list.Count == 0)
                    {
                        return newResults;
                    }

                    //Clear the cache if need be
                    if (!string.IsNullOrEmpty(query.NonParsedFieldList["clearcache"]))
                        ClearCache();

                    var qHash = query.GetHashCode();
                    if (ConfigHelper.AllowCaching && _resultsCache.ContainsKey(qHash))
                    {
                        var timerCache = new CQTimer();
                        var cacheItem = _resultsCache[qHash];
                        try
                        {
                            if (DateTime.Now.Subtract(cacheItem.QueryTime).TotalSeconds < _queryCacheTimeout)
                            {
                                _queryLogger.Log(query, 0, cacheItem.TotalRecordCount, true);
                                return cacheItem;
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.LogError(ex);
                            throw;
                        }
                        finally
                        {
                            AddProfileItem(RepositoryActionConstants.Query, timerCache, query.ToString(), cacheItem.TotalRecordCount);
                            _system.LogRepositoryPerf(new RepositorySummmaryStats
                            {
                                ActionType = RepositoryActionConstants.Query,
                                Elapsed = timerCache.Elapsed,
                                RepositoryId = _repositoryDefinition.ID,
                                ItemCount = cacheItem.RecordList.Count,
                            });
                        }
                    }

                    var timer = new CQTimer();
                    var _timeList = new List<string>();
                    var myWatch2 = new Stopwatch();

                    IEnumerable<DataItemExtension> queriedList = null;

                    //Apply dimension indexes first
                    if (query.DimensionValueList != null && query.DimensionValueList.Any())
                    {
                        var prepList = new Dictionary<DimensionItem, List<long>>();
                        foreach (var dvidx in query.DimensionValueList)
                        {
                            var dItem = _dimensionList.GetDimensionByDVIdx(dvidx);
                            if (dItem != null)
                            {
                                if (!prepList.ContainsKey(dItem)) prepList.Add(dItem, new List<long>());
                                prepList[dItem].Add(dvidx);
                            }
                        }

                        foreach (var dItem in prepList.Keys)
                        {
                            var dDef = _repositoryDefinition.DimensionList.First(x => x.Name == dItem.Name);

                            //Union all values within a dimension
                            IEnumerable<DataItemExtension> tempDList = null;
                            foreach (var dvidx in prepList[dItem])
                            {
                                if (tempDList == null)
                                {
                                    tempDList = _dimensionMappedItemCache[dvidx];
                                }
                                else
                                {
                                    //List dimension refinements always INTERSECT values, all other Union
                                    if (dDef.DataType == RepositorySchema.DataTypeConstants.List && dDef.MultivalueComparison == RepositorySchema.MultivalueComparisonContants.Intersect)
                                        tempDList = tempDList.Intersect(_dimensionMappedItemCache[dvidx]);
                                    else
                                        tempDList = tempDList.Union(_dimensionMappedItemCache[dvidx]);
                                }
                            }

                            //Intersect group between dimensions
                            if (queriedList == null) queriedList = tempDList;
                            else
                            {
                                queriedList = queriedList.Intersect(tempDList);
                            }
                        }


                        //var filterDList = new List<DimensionItem>();
                        //foreach (var dvidx in query.DimensionValueList)
                        //{
                        //	var dItem = _dimensionList.GetDimensionByDVIdx(dvidx);
                        //	if (dItem != null)
                        //	{
                        //		var dDef = _repositoryDefinition.DimensionList.First(x => x.Name == dItem.Name);
                        //		if (_dimensionMappedItemCache.ContainsKey(dvidx))
                        //		{
                        //			if (queriedList == null) queriedList = _dimensionMappedItemCache[dvidx];
                        //			else
                        //			{
                        //				if (dDef.DataType == RepositoryDefinition.DataTypeConstants.List)
                        //				{
                        //					//List dimension refinements always INTERSECT values
                        //					queriedList = queriedList.Intersect(_dimensionMappedItemCache[dvidx]);
                        //				}
                        //				else //All other dimension types
                        //				{
                        //					//Multiple refinements in a dimension should UNION values
                        //					//Refinements across dimensions should INTERSECT
                        //					if (filterDList.Contains(dItem))
                        //						queriedList = queriedList.Union(_dimensionMappedItemCache[dvidx]);
                        //					else
                        //						queriedList = queriedList.Intersect(_dimensionMappedItemCache[dvidx]);
                        //				}
                        //			}
                        //		}

                        //		if (!filterDList.Contains(dItem))
                        //			filterDList.Add(dItem);
                        //	}
                        //}
                    }

                    if (queriedList == null) queriedList = _list;

                    #region Filters

                    if (query.FieldFilters != null)
                    {
                        //Verify there is only one Geo filter
                        if (query.FieldFilters.Count(x => x is GeoCodeFieldFilter) > 1)
                        {
                            throw new Exception("Multiple geo location filters cannot be specified!");
                        }

                        foreach (var filter in query.FieldFilters)
                        {
                            var ff = ((ICloneable)filter).Clone() as IFieldFilter;
                            if (ff == null) throw new Exception("Object cannot be null!");
                            var field = _repositoryDefinition.FieldList.First(x => x.Name == ff.Name);
                            if (field == null) throw new Exception("Object cannot be null!");
                            var fieldIndex = _repositoryDefinition.FieldList.IndexOf(field);

                            #region GeoCode

                            if (field.DataType == RepositorySchema.DataTypeConstants.GeoCode)
                            {
                                var geo = (GeoCodeFieldFilter)ff;
                                switch (ff.Comparer)
                                {
                                    case ComparisonConstants.LessThan:
                                    case ComparisonConstants.LessThanOrEq:
                                        queriedList = queriedList.Where(x => x.ItemArray[fieldIndex] != null && GeoHelper.InRadius(((GeoCode)x.ItemArray[fieldIndex]).Latitude, ((GeoCode)x.ItemArray[fieldIndex]).Longitude, geo.Latitude, geo.Longitude, geo.Radius));
                                        break;
                                    case ComparisonConstants.GreaterThan:
                                    case ComparisonConstants.GreaterThanOrEq:
                                        queriedList = queriedList.Where(x => x.ItemArray[fieldIndex] != null && GeoHelper.OutRadius(((GeoCode)x.ItemArray[fieldIndex]).Latitude, ((GeoCode)x.ItemArray[fieldIndex]).Longitude, geo.Latitude, geo.Longitude, geo.Radius));
                                        break;
                                    case ComparisonConstants.Equals:
                                        queriedList = queriedList.Where(x => x.ItemArray[fieldIndex] != null && GeoHelper.Calc(((GeoCode)x.ItemArray[fieldIndex]).Latitude, ((GeoCode)x.ItemArray[fieldIndex]).Longitude, geo.Latitude, geo.Longitude) == geo.Radius);
                                        break;
                                    case ComparisonConstants.NotEqual:
                                        queriedList = queriedList.Where(x => x.ItemArray[fieldIndex] != null && GeoHelper.Calc(((GeoCode)x.ItemArray[fieldIndex]).Latitude, ((GeoCode)x.ItemArray[fieldIndex]).Longitude, geo.Latitude, geo.Longitude) != geo.Radius);
                                        break;
                                    default:
                                        throw new Exception("This operation is not supported!");
                                }
                            }
                            #endregion
                            #region Bool

                            else if (field.DataType == RepositorySchema.DataTypeConstants.Bool)
                            {
                                switch (ff.Comparer)
                                {
                                    case ComparisonConstants.Equals:
                                        queriedList = queriedList.Where(x => x.ItemArray[fieldIndex] != null && ff.Value != null && (bool)x.ItemArray[fieldIndex] == (bool)ff.Value);
                                        break;
                                    case ComparisonConstants.NotEqual:
                                        queriedList = queriedList.Where(x => x.ItemArray[fieldIndex] != null && ff.Value != null && (bool)x.ItemArray[fieldIndex] != (bool)ff.Value);
                                        break;
                                    default:
                                        throw new Exception("This operation is not supported!");
                                }
                            }
                            #endregion
                            #region DateTime

                            else if (field.DataType == RepositorySchema.DataTypeConstants.DateTime)
                            {
                                switch (ff.Comparer)
                                {
                                    case ComparisonConstants.LessThan:
                                        queriedList = queriedList.Where(x => x.ItemArray[fieldIndex] != null && ff.Value != null && (DateTime)x.ItemArray[fieldIndex] < (DateTime)ff.Value);
                                        break;
                                    case ComparisonConstants.LessThanOrEq:
                                        queriedList = queriedList.Where(x => x.ItemArray[fieldIndex] != null && ff.Value != null && (DateTime)x.ItemArray[fieldIndex] <= (DateTime)ff.Value);
                                        break;
                                    case ComparisonConstants.GreaterThan:
                                        queriedList = queriedList.Where(x => x.ItemArray[fieldIndex] != null && ff.Value != null && (DateTime)x.ItemArray[fieldIndex] > (DateTime)ff.Value);
                                        break;
                                    case ComparisonConstants.GreaterThanOrEq:
                                        queriedList = queriedList.Where(x => x.ItemArray[fieldIndex] != null && ff.Value != null && (DateTime)x.ItemArray[fieldIndex] >= (DateTime)ff.Value);
                                        //queriedList = queriedList.Intersect(_list.Where(x => x.ItemArray[fieldIndex] != null && ff.Value != null && (DateTime)x.ItemArray[fieldIndex] >= (DateTime)ff.Value));
                                        //queriedList = queriedList.Where(x => x.ItemArray[fieldIndex] != null && ff.Value != null && (DateTime)x.ItemArray[fieldIndex] >= (DateTime)ff.Value).ToList();
                                        break;
                                    case ComparisonConstants.Equals:
                                        queriedList = queriedList.Where(x => x.ItemArray[fieldIndex] != null && ff.Value != null && (DateTime)x.ItemArray[fieldIndex] == (DateTime)ff.Value);
                                        break;
                                    case ComparisonConstants.NotEqual:
                                        queriedList = queriedList.Where(x => x.ItemArray[fieldIndex] != null && ff.Value != null && (DateTime)x.ItemArray[fieldIndex] != (DateTime)ff.Value);
                                        break;
                                    default:
                                        throw new Exception("This operation is not supported!");
                                }
                            }
                            #endregion
                            #region Float

                            else if (field.DataType == RepositorySchema.DataTypeConstants.Float)
                            {
                                switch (ff.Comparer)
                                {
                                    case ComparisonConstants.LessThan:
                                        queriedList = queriedList.Where(x => x.ItemArray[fieldIndex] != null && ff.Value != null && (double)x.ItemArray[fieldIndex] < (double)ff.Value);
                                        break;
                                    case ComparisonConstants.LessThanOrEq:
                                        queriedList = queriedList.Where(x => x.ItemArray[fieldIndex] != null && ff.Value != null && (double)x.ItemArray[fieldIndex] <= (double)ff.Value);
                                        break;
                                    case ComparisonConstants.GreaterThan:
                                        queriedList = queriedList.Where(x => x.ItemArray[fieldIndex] != null && ff.Value != null && (double)x.ItemArray[fieldIndex] > (double)ff.Value);
                                        break;
                                    case ComparisonConstants.GreaterThanOrEq:
                                        queriedList = queriedList.Where(x => x.ItemArray[fieldIndex] != null && ff.Value != null && (double)x.ItemArray[fieldIndex] >= (double)ff.Value);
                                        break;
                                    case ComparisonConstants.Equals:
                                        queriedList = queriedList.Where(x => x.ItemArray[fieldIndex] != null && ff.Value != null && (double)x.ItemArray[fieldIndex] == (double)ff.Value);
                                        break;
                                    case ComparisonConstants.NotEqual:
                                        queriedList = queriedList.Where(x => x.ItemArray[fieldIndex] != null && ff.Value != null && (double)x.ItemArray[fieldIndex] != (double)ff.Value);
                                        break;
                                    case ComparisonConstants.Between:
                                        queriedList = queriedList.Where(x => x.ItemArray[fieldIndex] != null && ff.Value != null && ff.Value2 != null && (double)x.ItemArray[fieldIndex] >= (double)ff.Value && (double)x.ItemArray[fieldIndex] <= (double)ff.Value2);
                                        break;
                                    default:
                                        throw new Exception("This operation is not supported!");
                                }
                            }
                            #endregion
                            #region Int

                            else if (field.DataType == RepositorySchema.DataTypeConstants.Int)
                            {
                                switch (ff.Comparer)
                                {
                                    case ComparisonConstants.LessThan:
                                        queriedList = queriedList.Where(x => x.ItemArray[fieldIndex] != null && ff.Value != null && (int)x.ItemArray[fieldIndex] < (int)ff.Value);
                                        break;
                                    case ComparisonConstants.LessThanOrEq:
                                        queriedList = queriedList.Where(x => x.ItemArray[fieldIndex] != null && ff.Value != null && (int)x.ItemArray[fieldIndex] <= (int)ff.Value);
                                        break;
                                    case ComparisonConstants.GreaterThan:
                                        queriedList = queriedList.Where(x => x.ItemArray[fieldIndex] != null && ff.Value != null && (int)x.ItemArray[fieldIndex] > (int)ff.Value);
                                        break;
                                    case ComparisonConstants.GreaterThanOrEq:
                                        queriedList = queriedList.Where(x => x.ItemArray[fieldIndex] != null && ff.Value != null && (int)x.ItemArray[fieldIndex] >= (int)ff.Value);
                                        break;
                                    case ComparisonConstants.Equals:
                                        queriedList = queriedList.Where(x => x.ItemArray[fieldIndex] != null && ff.Value != null && (int)x.ItemArray[fieldIndex] == (int)ff.Value);
                                        break;
                                    case ComparisonConstants.NotEqual:
                                        queriedList = queriedList.Where(x => x.ItemArray[fieldIndex] != null && ff.Value != null && (int)x.ItemArray[fieldIndex] != (int)ff.Value);
                                        break;
                                    case ComparisonConstants.Between:
                                        queriedList = queriedList.Where(x => x.ItemArray[fieldIndex] != null && ff.Value != null && ff.Value2 != null && (int)x.ItemArray[fieldIndex] >= (int)ff.Value && (int)x.ItemArray[fieldIndex] <= (int)ff.Value2);
                                        break;
                                    default:
                                        throw new Exception("This operation is not supported!");
                                }
                            }
                            #endregion
                            #region String

                            else if (field.DataType == RepositorySchema.DataTypeConstants.String)
                            {
                                switch (ff.Comparer)
                                {
                                    case ComparisonConstants.LessThan:
                                        queriedList = queriedList.Where(x => string.Compare((string)x.ItemArray[fieldIndex], (string)ff.Value) < 0);
                                        break;
                                    case ComparisonConstants.LessThanOrEq:
                                        queriedList = queriedList.Where(x => string.Compare((string)x.ItemArray[fieldIndex], (string)ff.Value) <= 0);
                                        break;
                                    case ComparisonConstants.GreaterThan:
                                        queriedList = queriedList.Where(x => string.Compare((string)x.ItemArray[fieldIndex], (string)ff.Value) > 0);
                                        break;
                                    case ComparisonConstants.GreaterThanOrEq:
                                        queriedList = queriedList.Where(x => string.Compare((string)x.ItemArray[fieldIndex], (string)ff.Value) >= 0);
                                        break;
                                    case ComparisonConstants.Equals:
                                        queriedList = queriedList.Where(x => string.Compare((string)x.ItemArray[fieldIndex], (string)ff.Value) == 0);
                                        break;
                                    case ComparisonConstants.NotEqual:
                                        queriedList = queriedList.Where(x => string.Compare((string)x.ItemArray[fieldIndex], (string)ff.Value) != 0);
                                        //queriedList = queriedList.Intersect(_list.Where(x => string.Compare((string)x.ItemArray[fieldIndex], (string)ff.Value) != 0));
                                        //queriedList = queriedList.Where(x => string.Compare((string)x.ItemArray[fieldIndex], (string)ff.Value) != 0).ToList();
                                        break;
                                    case ComparisonConstants.Like:
                                        queriedList = queriedList.Where(x => ((string)x.ItemArray[fieldIndex]).Contains((string)ff.Value, StringComparison.OrdinalIgnoreCase));
                                        break;
                                    //case ComparisonConstants.Between:
                                    //	queriedList = queriedList.Where(x => x.ItemArray[fieldIndex] != null && ff.Value != null && ff.Value2 != null && (string)x.ItemArray[fieldIndex] >= (string)ff.Value && (string)x.ItemArray[fieldIndex] <= (string)ff.Value2);
                                    //	break;
                                    default:
                                        throw new Exception("This operation is not supported!");
                                }
                            }

                            #endregion
                            #region List

                            else if (field.DataType == RepositorySchema.DataTypeConstants.List)
                            {
                                switch (ff.Comparer)
                                {
                                    //case ComparisonConstants.LessThan:
                                    //    queriedList = queriedList.Where(x => string.Compare((string[])x.ItemArray[fieldIndex], (string)ff.Value) < 0);
                                    //    break;
                                    //case ComparisonConstants.LessThanOrEq:
                                    //    queriedList = queriedList.Where(x => string.Compare((string[])x.ItemArray[fieldIndex], (string)ff.Value) <= 0);
                                    //    break;
                                    //case ComparisonConstants.GreaterThan:
                                    //    queriedList = queriedList.Where(x => string.Compare((string[])x.ItemArray[fieldIndex], (string)ff.Value) > 0);
                                    //    break;
                                    //case ComparisonConstants.GreaterThanOrEq:
                                    //    queriedList = queriedList.Where(x => string.Compare((string[])x.ItemArray[fieldIndex], (string)ff.Value) >= 0);
                                    //    break;
                                    //case ComparisonConstants.Equals:
                                    //    queriedList = queriedList.Where(x => string.Compare((string[])x.ItemArray[fieldIndex], (string)ff.Value) == 0);
                                    //    break;
                                    //case ComparisonConstants.NotEqual:
                                    //    queriedList = queriedList.Where(x => string.Compare((string[])x.ItemArray[fieldIndex], (string)ff.Value) != 0);
                                    //    //queriedList = queriedList.Intersect(_list.Where(x => string.Compare((string[])x.ItemArray[fieldIndex], (string)ff.Value) != 0));
                                    //    //queriedList = queriedList.Where(x => string.Compare((string[])x.ItemArray[fieldIndex], (string)ff.Value) != 0).ToList();
                                    //    break;
                                    case ComparisonConstants.Like:
                                        queriedList = queriedList.Where(x => ((string[])x.ItemArray[fieldIndex]).Any(z=>z.Contains((string)ff.Value, StringComparison.OrdinalIgnoreCase)));
                                        break;
                                    //case ComparisonConstants.Between:
                                    //	queriedList = queriedList.Where(x => x.ItemArray[fieldIndex] != null && ff.Value != null && ff.Value2 != null && (string[])x.ItemArray[fieldIndex] >= (string)ff.Value && (string[])x.ItemArray[fieldIndex] <= (string)ff.Value2);
                                    //	break;
                                    default:
                                        throw new Exception("This operation is not supported!");
                                }
                            }

                            #endregion
                        }
                    }

                    #endregion

                    #region Then do text search

                    //var keywordSearchablelist = _repositoryDefinition.FieldList.Where(x => x.AllowTextSearch && x.DataType == RepositorySchema.DataTypeConstants.String).ToList();
                    //if (!string.IsNullOrEmpty(query.Keyword) && keywordSearchablelist.Count > 0)
                    //{
                    //    var keywordList = query.Keyword.Split(new char[] { ' ', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    //    System.Linq.Expressions.Expression<Func<DataItemExtension, bool>> w = x => false;
                    //    foreach (var field in keywordSearchablelist)
                    //    {
                    //        var index = _repositoryDefinition.FieldList.IndexOf(field);
                    //        foreach (var kw in keywordList)
                    //        {
                    //            w = w.Or(x =>
                    //                     x.ItemArray[index] != null &&
                    //                     ((string)x.ItemArray[index]).Contains(kw, StringComparison.OrdinalIgnoreCase)
                    //                );
                    //        }
                    //    }
                    //    queriedList = queriedList.Where(w.Compile());
                    //}

                    if (!string.IsNullOrEmpty(query.Keyword) && _repositoryDefinition.FieldList.Any(x => x.AllowTextSearch))
                    {
                        var keywordList = query.Keyword.Split(new char[] { ' ', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                        System.Linq.Expressions.Expression<Func<DataItemExtension, bool>> w = x => false;
                        foreach (var kw in keywordList)
                        {
                            w = w.Or(x => x.Keyword.Contains(kw, StringComparison.OrdinalIgnoreCase));
                        }
                        queriedList = queriedList.Where(w.Compile());
                    }

                    #endregion

                    //Determine SortBy lambda
                    if (query.FieldSorts != null && query.FieldSorts.Any())
                    {
                        OrderedParallelQuery<DataItemExtension> tempOrder = null;
                        foreach (var sf in query.FieldSorts)
                        {
                            var field = _repositoryDefinition.FieldList.First(x => x.Name == sf.Name);
                            var index = _repositoryDefinition.FieldList.IndexOf(field);
                            if (sf.SortDirection == SortDirectionConstants.Asc)
                            {
                                if (tempOrder == null) tempOrder = queriedList.AsParallel().OrderBy(x => x.ItemArray[index]);
                                else tempOrder = tempOrder.ThenBy(x => x.ItemArray[index]);
                            }
                            else if (sf.SortDirection == SortDirectionConstants.Desc)
                            {
                                if (tempOrder == null) tempOrder = queriedList.AsParallel().OrderByDescending(x => x.ItemArray[index]);
                                else tempOrder = tempOrder.ThenByDescending(x => x.ItemArray[index]);
                            }
                        }
                        queriedList = tempOrder.ThenBy(x => x.__RecordIndex);
                    }
                    else
                    {
                        //Tack on recordindex sort for consistency
                        queriedList = queriedList.AsParallel().OrderBy(x => x.__RecordIndex);
                    }

                    myWatch2.Stop();
                    myWatch2.Reset();
                    myWatch2.Start();

                    //var queriedList2 = queriedList.ToList();

                    myWatch2.Stop();
                    _timeList.Add(string.Empty + myWatch2.ElapsedMilliseconds);
                    myWatch2.Reset();
                    myWatch2.Start();

                    //Convert to list
                    var queriedListRealized = queriedList.AsParallel().ToList();
                    queriedList = null;

                    myWatch2.Stop();
                    _timeList.Add(string.Empty + myWatch2.ElapsedMilliseconds);
                    myWatch2.Reset();
                    myWatch2.Start();

                    var isZeroResult = false;
                    var skipCount = (query.PageOffset - 1) * query.RecordsPerPage;
                    var takeCount = query.RecordsPerPage;
                    if (skipCount >= queriedListRealized.Count)
                    {
                        isZeroResult = true;
                    }
                    else
                    {
                        if (skipCount + takeCount > queriedListRealized.Count)
                            takeCount = queriedListRealized.Count - skipCount;
                    }

                    if (query.RecordsPerPage < 1) isZeroResult = true;

                    //var newResults = new DataQueryResults();
                    //newResults.DimensionList = new List<DimensionItem>();
                    //newResults.RecordList = new List<DataItem>();
                    if (!isZeroResult)
                    {
                        queriedListRealized.Skip(skipCount).Take(takeCount).ToList().ForEach(x => newResults.RecordList.Add(x.ToSerialized()));
                    }
                    newResults.TotalRecordCount = queriedListRealized.Count;

                    //Process the distance for geo-located filters
                    var geoFilter = query.FieldFilters.FirstOrDefault(x => x is GeoCodeFieldFilter) as GeoCodeFieldFilter;
                    if (geoFilter != null)
                    {
                        var fieldDef = _repositoryDefinition.FieldList.FirstOrDefault(x => x.Name == geoFilter.Name);
                        if (fieldDef != null)
                        {
                            var fieldIndex = _repositoryDefinition.FieldList.IndexOf(fieldDef);
                            if (fieldIndex != -1)
                            {
                                foreach (var item in newResults.RecordList)
                                {
                                    var geoField = item.ItemArray[fieldIndex] as GeoCode;
                                    if (geoField == null) throw new Exception("Object cannot be null!");
                                    geoField.Distance = GeoHelper.Calc(geoFilter.Latitude, geoFilter.Longitude, geoField.Latitude, geoField.Longitude);
                                }
                            }
                        }
                    }

                    myWatch2.Stop();
                    _timeList.Add(string.Empty + myWatch2.ElapsedMilliseconds);
                    myWatch2.Reset();
                    myWatch2.Start();

                    //Now build the dimension list
                    if (_list.Count >= THREAD_THRESHOLD && _coreCount >= THREAD_CORE_COUNT)
                    {
                        //Threaded
                        var threadList = new List<System.Threading.Thread>();
                        var dimensionDefList = _repositoryDefinition.DimensionList.ToList();
                        for (var ii = 0; ii < dimensionDefList.Count; ii++)
                        {
                            var dimension = _dimensionList[ii];
                            if (dimension.RefinementList.Count > 0)
                            {
                                var threader = new QueryDimensionTheader(newResults, queriedListRealized, _dimensionList, _repositoryDefinition, ii);
                                var checkDimension = _dimensionList.First(y => y.Name == dimensionDefList[ii].Name);
                                var checkRefinementList = checkDimension.RefinementList.Select(z => z.DVIdx).ToList();
                                if (!newResults.Query.DimensionValueList.Any(x => checkRefinementList.Contains(x)))
                                {
                                    var t = new System.Threading.Thread(threader.ProcessDimension);
                                    threadList.Add(t);
                                    t.Start();
                                }
                            }
                            else
                            {
                                newResults.DimensionList.Add(new DimensionItem() { Name = dimension.Name, DIdx = dimension.DIdx, NumericBreak = dimension.NumericBreak });
                            }

                        }
                        foreach (var t in threadList) t.Join();
                    }
                    else
                    {
                        //Non-Threaded
                        for (var ii = 0; ii < _repositoryDefinition.DimensionList.Count(); ii++)
                        {
                            var dimension = _dimensionList[ii];
                            if (dimension.RefinementList.Count > 0)
                            {
                                myWatch2.Stop();
                                myWatch2.Reset();
                                myWatch2.Start();

                                var threader = new QueryDimensionTheader(newResults, queriedListRealized, _dimensionList, _repositoryDefinition, ii);
                                threader.ProcessDimension();

                                myWatch2.Stop();
                                _timeList.Add(dimension.Name + " " + myWatch2.ElapsedMilliseconds);
                            }
                            else
                            {
                                newResults.DimensionList.Add(new DimensionItem() { Name = dimension.Name, DIdx = dimension.DIdx, NumericBreak = dimension.NumericBreak });
                            }
                        }
                    }

                    #region Aggregate dimension counts

                    if (query.IncludeRefinementAgg)
                    {
                        newResults.DimensionList.ToList().ForEach(x => x.RefinementList.ForEach(z => z.AggCount = 0));
                        foreach (var item in queriedListRealized)
                        {
                            foreach (var dvidx in item.DimensionValueArray)
                            {
                                var d1 = _dimensionList.GetDimensionByDVIdx(dvidx);
                                var d2 = newResults.DimensionList.FirstOrDefault(x => x.DIdx == d1.DIdx);
                                var r = d2.RefinementList.FirstOrDefault(x => x.DVIdx == dvidx);
                                if (r != null)
                                    r.AggCount++;
                                else
                                    System.Diagnostics.Debug.WriteLine("");
                            }
                        }
                    }

                    #endregion

                    //Get dimensions with parent
                    var isMasterResults = (query.NonParsedFieldList["masterresults"] == "true" || query.NonParsedFieldList["masterresults"] == "1");
                    var defItemsWithParent = _repositoryDefinition.DimensionList.Where(y => !string.IsNullOrEmpty(y.Parent)).ToList();
                    if (!isMasterResults)
                    {
                        var isSet = true;
                        while (isSet)
                        {
                            isSet = false;
                            var childDimensions = newResults.DimensionList.Where(x => defItemsWithParent.Any(z => z.Name == x.Name)).ToList();
                            foreach (var dItem in childDimensions)
                            {
                                var childDef = defItemsWithParent.First(x => x.Name == dItem.Name);
                                var parent = newResults.DimensionList.FirstOrDefault(x => x.Name == childDef.Parent);
                                if (parent != null && parent.RefinementList.Count > 1)
                                {
                                    newResults.DimensionList.Remove(dItem);
                                    isSet = true;
                                }
                            }
                        }
                    }

                    //Now associate the parent with each dimension
                    foreach (var dimension in defItemsWithParent)
                    {
                        var d = newResults.DimensionList.FirstOrDefault(x => x.Name == dimension.Name);
                        if (d != null)
                        {
                            //This is the returned dimension object
                            //If its parent dimension is in the returned set then associate them
                            d.Parent = newResults.DimensionList.FirstOrDefault(x => x.Name == dimension.Parent);
                        }
                    }

                    myWatch2.Stop();
                    _timeList.Add(string.Empty + myWatch2.ElapsedMilliseconds);

                    var elapsed = timer.Stop();
                    newResults.ComputeTime = elapsed;
                    AddProfileItem(RepositoryActionConstants.Query, timer, query.ToString(), newResults.TotalRecordCount);
                    Logger.LogInfo("Query: ID=" + _repositoryDefinition.ID + ", Count=" + newResults.TotalRecordCount + ", Elapsed=" + timer.Elapsed);
                    _system.LogRepositoryPerf(new RepositorySummmaryStats
                    {
                        ActionType = RepositoryActionConstants.Query,
                        Elapsed = timer.Elapsed,
                        RepositoryId = _repositoryDefinition.ID,
                        ItemCount = newResults.RecordList.Count,
                    });

                    //Cache this object if necessary
                    if (_cacheControl.ShouldCache(query))
                        this.PerformCaching(qHash, newResults);

                    //Log this query
                    var worker = new LogActionDelegate(_queryLogger.Log);
                    worker.BeginInvoke(query, (int)newResults.ComputeTime, newResults.TotalRecordCount, false, null, null);

                    newResults.VersionHash = _versionHash;
                    return newResults;
                }
                catch (Exception ex)
                {
                    var additionInfo = string.Empty;
                    if (_repositoryDefinition != null) additionInfo += "ID=" + _repositoryDefinition.ID;
                    if (query != null) additionInfo += ", Query: " + query.ToString();
                    Logger.LogError(ex, additionInfo);
                    throw;
                }
            }
        }

        #endregion

        #region Reset

        public void Clear(UserCredentials credentials)
        {
            if (!IsValidCredentials(credentials))
                throw new Exception("Invalid credentials");

            using (var q = new AcquireWriterLock(this.SyncObject, "Clear", _repositoryDefinition.ID))
            {
                try
                {
                    //If loaded then clear the data array
                    if (this.IsLoaded)
                        _list.Clear();

                    var timer = new CQTimer();
                    Logger.LogInfo("Clear: ID=" + _repositoryDefinition.ID);

                    _dataProvider.Clear();
                    AddProfileItem(RepositoryActionConstants.Reset, timer);

                    ReloadMe();
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex);
                    throw;
                }
            }
        }

        public bool ExportSchema(UserCredentials credentials, string backupFile)
        {
            if (!IsValidCredentials(credentials))
                throw new Exception("Invalid credentials");

            using (var q = new AcquireWriterLock(this.SyncObject, "ExportSchema", _repositoryDefinition.ID))
            {
                try
                {
                    return _dataProvider.Export(backupFile);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Copies the entire repository to an archive file
        /// </summary>
        public bool Backup(UserCredentials credentials, string backupFile)
        {
            if (!IsValidCredentials(credentials))
                throw new Exception("Invalid credentials");

            using (var q = new AcquireWriterLock(this.SyncObject, "Backup", _repositoryDefinition.ID))
            {
                try
                {
                    //TODO
                    return false;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Given an repository archive file, this method will create a new repository from it
        /// </summary>
        public bool Restore(UserCredentials credentials, string backupFile)
        {
            if (!IsValidCredentials(credentials))
                throw new Exception("Invalid credentials");

            using (var q = new AcquireWriterLock(this.SyncObject, "Restore", _repositoryDefinition.ID))
            {
                try
                {
                    //TODO
                    return false;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        #endregion

        #region IsValidFormat

        /// <summary>
        /// Verify that the item format is valid without loading the repository off disk
        /// </summary>
        public bool IsValidFormat(DataItem item, UserCredentials credentials)
        {
            if (!IsValidCredentials(credentials))
                throw new Exception("Invalid credentials");

            try
            {
                var newItem = new DataItemExtension(item, _repositoryDefinition);
                return IsItemValid(newItem);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                throw;
            }
        }

        #endregion

        #region ShutDown

        public void ShutDown()
        {
            using (var q = new AcquireWriterLock(this.SyncObject, "Shutdown", _repositoryDefinition.ID))
            {
                try
                {
                    this.FlushCache(true);
                    //Logger.LogInfo("Repository ShutDown: ID=" + _repositoryDefinition.ID.ToString());
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        #endregion

        #region GetDataDiskSize

        /// <summary>
        /// Returns the size on disk of the repository data
        /// </summary>
        public long GetDataDiskSize(UserCredentials credentials)
        {
            if (!IsValidCredentials(credentials))
                throw new Exception("Invalid credentials");

            if (_system.Storage == StorageTypeConstants.Database)
            {
                return 0;
            }
            else if (_system.Storage == StorageTypeConstants.File)
            {
                var repositoryCacheFolder = Path.Combine(ConfigHelper.DataPath, _repositoryDefinition.ID.ToString());
                long retval = 0;
                if (Directory.Exists(repositoryCacheFolder))
                {
                    var files = Directory.GetFiles(repositoryCacheFolder);
                    foreach (var f in files)
                    {
                        retval += (new FileInfo(f)).Length;
                    }
                }
                return retval;
            }
            return 0;
        }

        #endregion

        #region GetDataMemorySize

        /// <summary>
        /// Returns the size of memory of the repository data
        /// </summary>
        /// <returns></returns>
        public long GetDataMemorySize(UserCredentials credentials)
        {
            if (!IsValidCredentials(credentials))
                throw new Exception("Invalid credentials");

            return _dataProvider.MemorySize;
        }

        #endregion

        #region RefreshStatsFromCache

        public void RefreshStatsFromCache()
        {
            try
            {
                //Call this to reload cache file
                var repositoryCacheFolder = Path.Combine(ConfigHelper.DataPath, _repositoryDefinition.ID.ToString());
                long ic;
                long ms;
                _dataProvider.RefreshStatsFromCache(repositoryCacheFolder, out ic, out ms);
                this.RemotingObject.DataMemorySize = ms;
                this.RemotingObject.ItemCount = ic;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
        }

        #endregion

        #region ItemCount

        /// <summary>
        /// Returns the number of items in the repository
        /// </summary>
        /// <returns></returns>
        public long GetItemCount(UserCredentials credentials)
        {
            if (!IsValidCredentials(credentials))
                throw new Exception("Invalid credentials");

            return GetItemCount();
        }

        private long GetItemCount()
        {
            try
            {
                if (this.IsLoaded)
                {
                    return _list.Count;
                }
                else
                {
                    if (this.RemotingObject != null)
                        return this.RemotingObject.ItemCount;
                    else
                        return 0;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

        #region GetProfile

        public ProfileItem[] GetProfile(UserCredentials credentials, long lastProfileId)
        {
            lock (_profileList)
            {
                try
                {
                    var retval = new List<ProfileItem>();
                    var index = _profileList.Select(x => x.ProfileId).ToList().IndexOf(lastProfileId);
                    if (index != -1)
                    {
                        retval.AddRange(_profileList.Skip(index + 1).ToList());
                    }
                    else
                    {
                        retval.AddRange(_profileList);
                    }
                    return retval.ToArray();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        #endregion

        #region Compress

        public void Compress()
        {
            try
            {
                using (var q = new AcquireWriterLock(this.SyncObject, "Compress", _repositoryDefinition.ID))
                {
                    if (_dataProvider.NeedsCompress)
                    {
                        var timer = new Stopwatch();
                        timer.Start();
                        _dataProvider.Compress();
                        timer.Stop();
                        Logger.LogInfo("Repository Compress: ID=" + _repositoryDefinition.ID.ToString() + ", Count=" + _list.Count + ", Elapsed=" + timer.ElapsedMilliseconds);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                throw;
            }
        }

        #endregion

        #region FlushCache

        private bool _needStatUpdate = false;
        public void FlushCache(bool useMemory = false)
        {
            try
            {
                if (_needStatUpdate)
                {
                    var timer = new Stopwatch();
                    timer.Start();
                    using (var q = new AcquireWriterLock(this.SyncObject, "FlushCache", _repositoryDefinition.ID))
                    {
                        this.RemotingObject.ItemCount = _list.Count;
                        if (useMemory)
                        {
                            //if (_list.Count > 10000) //For big repositories we do not want max out memory so buffer to disk
                            //    this.RemotingObject.DataMemorySize = ObjectHelper.SizeOf(_list, true);
                            //else
                            //    this.RemotingObject.DataMemorySize = ObjectHelper.SizeOf(_list);
                        }

                        _dataProvider.UpdateRepositoryStats(new RepositoryStatItem
                        {
                            ItemCount = this.RemotingObject.ItemCount,
                            MemorySize = this.RemotingObject.DataMemorySize,
                        });
                        _needStatUpdate = !useMemory;
                    }
                    timer.Stop();
                    Logger.LogInfo("FlushCache: Mem=" + useMemory + ", ID=" + _repositoryDefinition.ID.ToString() + ", Count=" + this.RemotingObject.ItemCount + ", Elapsed=" + timer.ElapsedMilliseconds);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
        }

        #endregion

        #region Private Helpers

        private Dictionary<string, string> _credCache = new Dictionary<string, string>();

        public bool IsValidCredentials(UserCredentials credentials)
        {
            try
            {
                lock (_credCache)
                {
                    try
                    {
                        if (credentials == null)
                        {
                            Logger.LogError("Credentials was null");
                            return false;
                        }

                        var user = UserDomain.UserList.FirstOrDefault(x => x.UserName == credentials.UserName);
                        if (user == null)
                        {
                            Logger.LogError("Credentials user was not found");
                            return false;
                        }

                        if (_credCache.ContainsKey(credentials.Password))
                            return (_credCache[credentials.Password] == user.Password);

                        var decrypt = Celeriq.Utilities.SecurityHelper.Decrypt(_masterKeys.PrivateKey, credentials.Password);
                        _credCache.Add(credentials.Password, decrypt);

                        if (_credCache.Keys.Count > 200)
                            _credCache.Remove(_credCache.Keys.First());

                        return (decrypt == user.Password);
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError(ex);
                        return false;
                    }
                }

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private void ClearCache()
        {
            try
            {
                _resultsCache = new SequencedHashTable<int, DataQueryResults>();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Determines if this result set should be cached and if so do it
        /// </summary>
        private void PerformCaching(int qHash, DataQueryResults results)
        {
            if (!ConfigHelper.AllowCaching) return;
            if (!_resultsCache.ContainsKey(qHash))
            {
                //Remove the first item cached if we have hit cache limit
                if (_resultsCache.Count >= _cacheControl.MaxItems)
                {
                    var k = _resultsCache.OrderedKeys.FirstOrDefault();
                    if (k != null) _resultsCache.Remove(k);
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("Key not found. 0x2998");
                        Logger.LogDebug("PerformCaching has no OrderedKeys but " + _resultsCache.Count + " cached items");
                    }
                }
                _resultsCache.Add(qHash, results);
            }
            else
            {
                _resultsCache[qHash] = results;
            }
        }

        private bool ProcessDimensions(DataItemExtension newItem)
        {
            try
            {
                var dimensionDefList = _repositoryDefinition.DimensionList.ToList();
                for (var ii = 0; ii < dimensionDefList.Count; ii++)
                {
                    var dimensionDef = dimensionDefList[ii];
                    var dimension = _dimensionList.First(x => x.Name == dimensionDef.Name);
                    var field = _repositoryDefinition.FieldList.First(x => x.Name == dimensionDef.Name);
                    var fieldIndex = _repositoryDefinition.FieldList.IndexOf(field);

                    #region String

                    if (field.DataType == RepositorySchema.DataTypeConstants.String)
                    {
                        if (!string.IsNullOrEmpty((string)newItem.ItemArray[fieldIndex]))
                        {
                            lock (dimension)
                            {
                                var refinement = dimension.RefinementList.FirstOrDefault(x => x.FieldValue == (string)newItem.ItemArray[fieldIndex]);
                                if (refinement == null)
                                {
                                    refinement = new RefinementItem() { Count = 1, DVIdx = dimension.GetNextDVIdx(), FieldValue = (string)newItem.ItemArray[fieldIndex] };
                                    dimension.RefinementList.Add(refinement);
                                    _dDimensionCache[ii].WriteItem(refinement);
                                    _dimensionMappedItemCache.Add(refinement.DVIdx, new List<DataItemExtension>());
                                }
                                else
                                {
                                    refinement.Count++;
                                }

                                //This is for the non-list dimensions for fast dimensions aggregate computation
                                newItem.DimensionSingularValueArray[ii] = refinement.DVIdx;

                                //Add the list of all dimension values for this item
                                if (!newItem.DimensionValueArray.Contains(refinement.DVIdx))
                                    newItem.DimensionValueArray.Add(refinement.DVIdx);

                            }
                        }
                    }

                    #endregion

                    #region List

                    else if (field.DataType == RepositorySchema.DataTypeConstants.List)
                    {
                        if (newItem.ItemArray[fieldIndex] != null)
                        {
                            lock (dimension)
                            {
                                var fieldValueList = (string[])newItem.ItemArray[fieldIndex];
                                foreach (var fieldValue in fieldValueList)
                                {
                                    var refinement = dimension.RefinementList.FirstOrDefault(x => x.FieldValue == fieldValue);
                                    if (refinement == null)
                                    {
                                        refinement = new RefinementItem() { Count = 1, DVIdx = dimension.GetNextDVIdx(), FieldValue = fieldValue };
                                        dimension.RefinementList.Add(refinement);
                                        _dDimensionCache[ii].WriteItem(refinement);
                                        _dimensionMappedItemCache.Add(refinement.DVIdx, new List<DataItemExtension>());
                                    }
                                    else
                                    {
                                        refinement.Count++;
                                    }

                                    //This is for the non-list dimensions for fast dimensions aggregate computation
                                    newItem.DimensionSingularValueArray[ii] = refinement.DVIdx;

                                    //Add the list of all dimension values for this item
                                    if (!newItem.DimensionValueArray.Contains(refinement.DVIdx))
                                        newItem.DimensionValueArray.Add(refinement.DVIdx);

                                }
                            }
                        }
                    }

                    #endregion

                    #region Integer

                    else if ((field.DataType == RepositorySchema.DataTypeConstants.Int) && dimension.NumericBreak == null)
                    {
                        if (newItem.ItemArray[fieldIndex] != null)
                        {
                            lock (dimension)
                            {
                                var fieldValue = (int)newItem.ItemArray[fieldIndex];
                                var refinement = dimension.RefinementList.FirstOrDefault(x => x.FieldValue == fieldValue.ToString());
                                if (refinement == null)
                                {
                                    refinement = new RefinementItem()
                                                     {
                                                         Count = 1,
                                                         DVIdx =
                                                             dimension.GetNextDVIdx(),
                                                         FieldValue = fieldValue.ToString(),
                                                         MinValue = fieldValue,
                                                         MaxValue = fieldValue
                                                     };
                                    dimension.RefinementList.Add(refinement);
                                    _dDimensionCache[ii].WriteItem(refinement);
                                    _dimensionMappedItemCache.Add(refinement.DVIdx, new List<DataItemExtension>());
                                }
                                else
                                {
                                    refinement.Count++;
                                }

                                //This is for the non-list dimensions for fast dimensions aggregate computation
                                newItem.DimensionSingularValueArray[ii] = refinement.DVIdx;

                                //Add the list of all dimension values for this item
                                if (!newItem.DimensionValueArray.Contains(refinement.DVIdx))
                                    newItem.DimensionValueArray.Add(refinement.DVIdx);

                            }
                        }
                    }
                    else if ((field.DataType == RepositorySchema.DataTypeConstants.Int) && dimension.NumericBreak != null)
                    {
                        if (newItem.ItemArray[fieldIndex] != null)
                        {
                            lock (dimension)
                            {
                                var fieldValue = (int)newItem.ItemArray[fieldIndex];
                                var minLevel = ((long)fieldValue / dimension.NumericBreak.Value) * dimension.NumericBreak.Value;
                                var refinement = dimension.RefinementList.FirstOrDefault(x => x.MinValue == minLevel);
                                if (refinement == null)
                                {
                                    refinement = new RefinementItem()
                                                     {
                                                         Count = 1,
                                                         DVIdx = dimension.GetNextDVIdx(),
                                                         FieldValue = minLevel.ToString("###,###,###,##0") + " - " + (minLevel + dimension.NumericBreak.Value).ToString("###,###,###,##0"),
                                                         MinValue = minLevel,
                                                         MaxValue = minLevel + dimension.NumericBreak.Value
                                                     };
                                    dimension.RefinementList.Add(refinement);
                                    _dDimensionCache[ii].WriteItem(refinement);
                                    _dimensionMappedItemCache.Add(refinement.DVIdx, new List<DataItemExtension>());
                                }
                                else
                                {
                                    refinement.Count++;
                                }

                                //This is for the non-list dimensions for fast dimensions aggregate computation
                                newItem.DimensionSingularValueArray[ii] = refinement.DVIdx;

                                //Add the list of all dimension values for this item
                                if (!newItem.DimensionValueArray.Contains(refinement.DVIdx))
                                    newItem.DimensionValueArray.Add(refinement.DVIdx);

                            }
                        }
                    }

                    #endregion

                    #region Bool

                    else if (field.DataType == RepositorySchema.DataTypeConstants.Bool)
                    {
                        if (newItem.ItemArray[fieldIndex] != null)
                        {
                            lock (dimension)
                            {
                                var fieldValue = (bool)newItem.ItemArray[fieldIndex];
                                var refinement = dimension.RefinementList.FirstOrDefault(x => x.FieldValue == fieldValue.ToString());
                                if (refinement == null)
                                {
                                    refinement = new RefinementItem()
                                                     {
                                                         Count = 1,
                                                         DVIdx = dimension.GetNextDVIdx(),
                                                         FieldValue = fieldValue.ToString(),
                                                     };
                                    dimension.RefinementList.Add(refinement);
                                    _dDimensionCache[ii].WriteItem(refinement);
                                    _dimensionMappedItemCache.Add(refinement.DVIdx, new List<DataItemExtension>());
                                }
                                else
                                {
                                    refinement.Count++;
                                }

                                //This is for the non-list dimensions for fast dimensions aggregate computation
                                newItem.DimensionSingularValueArray[ii] = refinement.DVIdx;

                                //Add the list of all dimension values for this item
                                if (!newItem.DimensionValueArray.Contains(refinement.DVIdx))
                                    newItem.DimensionValueArray.Add(refinement.DVIdx);

                            }
                        }
                    }
                    else if ((field.DataType == RepositorySchema.DataTypeConstants.Int) && dimension.NumericBreak != null)
                    {
                        if (newItem.ItemArray[fieldIndex] != null)
                        {
                            lock (dimension)
                            {
                                var fieldValue = (int)newItem.ItemArray[fieldIndex];
                                var minLevel = ((long)fieldValue / dimension.NumericBreak.Value) * dimension.NumericBreak.Value;
                                var refinement = dimension.RefinementList.FirstOrDefault(x => x.MinValue == minLevel);
                                if (refinement == null)
                                {
                                    refinement = new RefinementItem()
                                                     {
                                                         Count = 1,
                                                         DVIdx = dimension.GetNextDVIdx(),
                                                         FieldValue = minLevel.ToString("###,###,###,##0") + " - " + (minLevel + dimension.NumericBreak.Value).ToString("###,###,###,##0"),
                                                         MinValue = minLevel,
                                                         MaxValue = minLevel + dimension.NumericBreak.Value
                                                     };
                                    dimension.RefinementList.Add(refinement);
                                    _dDimensionCache[ii].WriteItem(refinement);
                                    _dimensionMappedItemCache.Add(refinement.DVIdx, new List<DataItemExtension>());
                                }
                                else
                                {
                                    refinement.Count++;
                                }

                                //This is for the non-list dimensions for fast dimensions aggregate computation
                                newItem.DimensionSingularValueArray[ii] = refinement.DVIdx;

                                //Add the list of all dimension values for this item
                                if (!newItem.DimensionValueArray.Contains(refinement.DVIdx))
                                    newItem.DimensionValueArray.Add(refinement.DVIdx);

                            }
                        }
                    }

                    #endregion

                    #region DateTime

                    else if (field.DataType == RepositorySchema.DataTypeConstants.DateTime)
                    {
                        if (newItem.ItemArray[fieldIndex] != null)
                        {
                            lock (dimension)
                            {
                                var fieldValue = (DateTime)newItem.ItemArray[fieldIndex];
                                var refinement = dimension.RefinementList.FirstOrDefault(x => x.FieldValue == fieldValue.ToString(DimensionItem.DateTimeFormat));
                                if (refinement == null)
                                {
                                    refinement = new RefinementItem()
                                                     {
                                                         Count = 1,
                                                         DVIdx = dimension.GetNextDVIdx(),
                                                         FieldValue = fieldValue.ToString(DimensionItem.DateTimeFormat),
                                                     };
                                    dimension.RefinementList.Add(refinement);
                                    _dDimensionCache[ii].WriteItem(refinement);
                                    _dimensionMappedItemCache.Add(refinement.DVIdx, new List<DataItemExtension>());
                                }
                                else
                                {
                                    refinement.Count++;
                                }

                                //This is for the non-list dimensions for fast dimensions aggregate computation
                                newItem.DimensionSingularValueArray[ii] = refinement.DVIdx;

                                //Add the list of all dimension values for this item
                                if (!newItem.DimensionValueArray.Contains(refinement.DVIdx))
                                    newItem.DimensionValueArray.Add(refinement.DVIdx);

                            }
                        }
                    }

                    #endregion

                    else if (field.DataType == RepositorySchema.DataTypeConstants.Float)
                    {
                        throw new Exception("Unsupported dimension data type! Type '" + field.DataType.ToString() + "'");
                    }

                    else if (field.DataType == RepositorySchema.DataTypeConstants.GeoCode)
                    {
                        throw new Exception("Unsupported dimension data type! Type '" + field.DataType.ToString() + "'");
                    }

                    else
                    {
                        throw new Exception("Unsupported dimension data type! Type '" + field.DataType.ToString() + "'");
                    }

                }

                //Update the dimension cache
                foreach (var dvidx in newItem.DimensionValueArray)
                {
                    _dimensionMappedItemCache[dvidx].Add(newItem);
                }

                return true;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                throw;
            }
        }

        private bool IsItemValid(DataItemExtension item)
        {
            if (item.ItemArray == null)
            {
                //Logger.LogWarning("IsItemValid: ItemArray Null");
                return false;
            }
            if (item.ItemArray.Length != _repositoryDefinition.FieldList.Count)
            {
                //Logger.LogWarning("IsItemValid: ItemArray Count mismatch: " + item.ItemArray.Length + "/" + _repositoryDefinition.FieldList.Count);
                return false;
            }

            var index = 0;
            foreach (var field in _repositoryDefinition.FieldList)
            {
                if (item.ItemArray[index] != null)
                {
                    switch (field.DataType)
                    {
                        case RepositorySchema.DataTypeConstants.Bool:
                            if (!(item.ItemArray[index] is bool)) return false;
                            break;
                        case RepositorySchema.DataTypeConstants.DateTime:
                            if (!(item.ItemArray[index] is DateTime)) return false;
                            break;
                        case RepositorySchema.DataTypeConstants.Float:
                            if (!(item.ItemArray[index] is double)) return false;
                            break;
                        case RepositorySchema.DataTypeConstants.GeoCode:
                            if (!(item.ItemArray[index] is GeoCode)) return false;
                            break;
                        case RepositorySchema.DataTypeConstants.Int:
                            if (!(item.ItemArray[index] is int)) return false;
                            break;
                        case RepositorySchema.DataTypeConstants.String:
                            if (!(item.ItemArray[index] is string)) return false;
                            break;
                        case RepositorySchema.DataTypeConstants.List:
                            if (!(item.ItemArray[index] is string[])) return false;
                            break;
                        default:
                            Logger.LogWarning("IsItemValid: Unknown data type: " + field.DataType.ToString());
                            throw new Exception("Unknown data type!");
                    }
                }
                index++;
            }
            return true;
        }

        private System.Linq.Expressions.Expression GetEqualsExpr(System.Linq.Expressions.ParameterExpression param, string property, string value)
        {
            var prop = System.Linq.Expressions.Expression.Property(param, property);
            var val = System.Linq.Expressions.Expression.Constant(value);
            return System.Linq.Expressions.Expression.Equal(prop, val);
        }

        private void AddProfileItem(RepositoryActionConstants action, DateTime startTime, int elapsed)
        {
            AddProfileItem(action, startTime, elapsed, null, 0);
        }

        private void AddProfileItem(RepositoryActionConstants action, CQTimer timer)
        {
            timer.Stop();
            AddProfileItem(action, timer.StartTime, timer.Elapsed, null, 0);
        }

        private void AddProfileItem(RepositoryActionConstants action, CQTimer timer, int itemsAffected)
        {
            timer.Stop();
            AddProfileItem(action, timer.StartTime, timer.Elapsed, null, itemsAffected);
        }

        private void AddProfileItem(RepositoryActionConstants action, CQTimer timer, string query, int itemsAffected)
        {
            timer.Stop();
            AddProfileItem(action, timer.StartTime, timer.Elapsed, query, itemsAffected);
        }

        private long _maxProfileId = 0;
        private void AddProfileItem(RepositoryActionConstants action, DateTime startTime, int elapsed, string query, int itemsAffected)
        {
            if (!ConfigHelper.AllowStatistics) return;
            lock (_profileList)
            {
                try
                {
                    if (_maxProfileId == 0)
                    {
                        if (_profileList.Count > 0)
                            _maxProfileId = _profileList.Select(x => x.ProfileId).Max() + 1;
                        else
                            _maxProfileId = 1;
                    }

                    var newItem = new ProfileItem()
                                      {
                                          Action = action,
                                          Duration = elapsed,
                                          Name = _repositoryDefinition.Name,
                                          ProfileId = _maxProfileId,
                                          RepositoryId = _repositoryDefinition.ID,
                                          StartTime = startTime,
                                          Query = query,
                                          ItemsAffected = itemsAffected,
                                      };
                    _profileList.Add(newItem);
                    _maxProfileId++;

                    //Remove first item until we are back at defined max
                    while (_profileList.Count > MAX_PROFILE_COUNT)
                    {
                        _profileList.RemoveAt(0);
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        private string SumKeywordBlob(DataItem item)
        {
            if (!_repositoryDefinition.FieldList.Any(x => x.AllowTextSearch)) return null;
            try
            {
                var sb = new StringBuilder();
                var index = 0;
                foreach (var c in _repositoryDefinition.FieldList)
                {
                    if (c.AllowTextSearch && item.ItemArray[index] != null)
                    {
                        sb.AppendLine(item.ItemArray[index].ToString());
                    }
                    index++;
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                throw;
            }
        }

        #endregion

    }

}