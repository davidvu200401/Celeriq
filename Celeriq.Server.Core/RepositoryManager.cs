using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Celeriq.Common;
using Celeriq.Server.Interfaces;
using Celeriq.Utilities;
using System.IO;
using Celeriq.DataCore.EFDAL;
using System.Threading.Tasks;
using System.Diagnostics;
using Celeriq.RepositoryAPI;

namespace Celeriq.Server.Core
{
    [Serializable]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class RepositoryManager : Celeriq.Common.IDataModel
    {
        private ISystemServerCore _system = null;
        private Dictionary<Guid, RemotingObjectCache> _repositoryList = new Dictionary<Guid, RemotingObjectCache>();
        private StorageTypeConstants _storage = StorageTypeConstants.Database;

        public RepositoryManager(ISystemServerCore system, StorageTypeConstants storage)
        {
            this.SyncObject = new Celeriq.Utilities.CeleriqLock(System.Threading.LockRecursionPolicy.SupportsRecursion);
            _system = system;
            _storage = storage;
        }

        public bool IsInitialized { get; private set; }

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

        public void Initialize()
        {
            try
            {
                var version = System.Reflection.Assembly.GetEntryAssembly().GetName().Version;
                Logger.LogInfo("Manager Starting: Version=" + version.ToString());

                var timer = new Stopwatch();
                timer.Start();

                if (_storage == StorageTypeConstants.Database)
                {
                    //Load all repositories into memory
                    using (var context = new DataCoreEntities())
                    {
                        var masterKeys = ConfigHelper.MasterKeys;
                        var list = context.RepositoryDefinition.OrderBy(x => x.RepositoryId).ToList();
                        foreach (var item in list)
                        {
                            var newItem = new RemotingObjectCache();
                            newItem.DataDiskSize = 0;
                            newItem.DataMemorySize = item.MemorySize;
                            newItem.IsLoaded = false;
                            newItem.ItemCount = item.ItemCount;
                            newItem.LastUnloadTime = null;
                            newItem.VersionHash = item.VersionHash;
                            newItem.Repository = item.DefinitionData.BinToObject<RepositorySchema>();
                            newItem.Repository.ID = item.UniqueKey;

                            var repository = new Celeriq.RepositoryAPI.Repository(item.RepositoryId, newItem.Repository, masterKeys, _system);
                            repository.RemotingObject = newItem;
                            newItem.ServiceInstance = repository;
                            _repositoryList.Add(item.UniqueKey, newItem);
                        }
                    }
                }
                else if (_storage == StorageTypeConstants.File)
                {
                    var files = Directory.GetFiles(ConfigHelper.DataPath, "*.celeriq");
                    var options = new ParallelOptions ();//{ MaxDegreeOfParallelism = 1 };
                    Parallel.ForEach(files, options, file =>
                    {
                        try
                        {
                            var repositoryDef = new RepositorySchema();
                            repositoryDef.Load(file);
                            repositoryDef.CachePath = Path.Combine(ConfigHelper.DataPath, repositoryDef.ID.ToString());
                            var repository = new Celeriq.RepositoryAPI.Repository(0, repositoryDef, ConfigHelper.MasterKeys, _system);
                            var remoteItem = new RemotingObjectCache() { ServiceInstance = repository, Repository = repositoryDef };
                            remoteItem.VersionHash = repositoryDef.VersionHash;
                            repository.RemotingObject = remoteItem;
                            remoteItem.DataDiskSize = repository.GetDataDiskSize(((SystemCore)_system).RootUser);
                            repository.RefreshStatsFromCache();
                            lock (_repositoryList)
                            {
                                if (_repositoryList.ContainsKey(repositoryDef.ID))
                                    Logger.LogWarning("OnLoad Duplicate: ID=" + repositoryDef.ID);
                                else
                                    _repositoryList.Add(repositoryDef.ID, remoteItem);
                            }
                            if (_repositoryList.Count % 500 == 0)
                            {
                                Logger.LogInfo("Manager load files (" + _repositoryList.Count + " / " + files.Length + ")");
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.LogError(ex, "File: " + file);
                        }
                    });

                }

                timer.Stop();
                Logger.LogInfo("Manager Started: Elapsed=" + timer.ElapsedMilliseconds);
                this.IsInitialized = true;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                throw;
            }
        }

        public void RemoveRepository(Guid repositoryId)
        {
            var timer = new Stopwatch();
            timer.Start();
            try
            {
                //Delete from list in case error
                using (var q = new AcquireWriterLock(this.SyncObject, "RemoveRepository"))
                {
                    _repositoryList.Remove(repositoryId);
                }

                if (_storage == StorageTypeConstants.Database)
                {
                    Celeriq.DataCore.EFDAL.Entity.DimensionData.DeleteData(x => x.DimensionStore.RepositoryDefinition.UniqueKey == repositoryId);
                    Celeriq.DataCore.EFDAL.Entity.DimensionStore.DeleteData(x => x.RepositoryDefinition.UniqueKey == repositoryId);
                    Celeriq.DataCore.EFDAL.Entity.RepositoryData.DeleteData(x => x.RepositoryDefinition.UniqueKey == repositoryId);
                    Celeriq.DataCore.EFDAL.Entity.RepositoryStat.DeleteData(x => x.RepositoryDefinition.UniqueKey == repositoryId);
                    Celeriq.DataCore.EFDAL.Entity.RepositoryLog.DeleteData(x => x.RepositoryDefinition.UniqueKey == repositoryId);
                    Celeriq.DataCore.EFDAL.Entity.RepositoryDefinition.DeleteData(x => x.UniqueKey == repositoryId);
                }
                else if (_storage == StorageTypeConstants.File)
                {
                    //Delete the def file
                    var filePath = Path.Combine(ConfigHelper.DataPath, repositoryId + ".celeriq");
                    if (File.Exists(filePath))
                        File.Delete(filePath);

                    //Delete the folder
                    var folderPath = Path.Combine(ConfigHelper.DataPath, repositoryId.ToString());
                    ServerUtilities.DeleteDirectoryWithRetry(folderPath);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "RepositoryId: " + repositoryId);
                throw;
            }
            finally
            {
                timer.Stop();
                Logger.LogInfo("RemoveRepository: Elapsed=" + timer.ElapsedMilliseconds);
            }
        }

        public bool Contains(Guid repositoryId)
        {
            using (var q = new AcquireReaderLock(this.SyncObject))
            {
                return _repositoryList.ContainsKey(repositoryId);
            }
        }

        public List<RemotingObjectCache> List
        {
            get
            {
                try
                {
                    using (var q = new AcquireReaderLock(this.SyncObject))
                    {
                        return _repositoryList.Values.ToList();
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex);
                    throw;
                }
            }
        }

        public Celeriq.Server.Interfaces.IRepository AddRepository(Guid repositoryId, RepositorySchema schema)
        {
            var timer = new Stopwatch();
            timer.Start();
            try
            {
                using (var q = new AcquireWriterLock(this.SyncObject, "AddRepository"))
                {
                    if (_repositoryList.ContainsKey(repositoryId))
                    {
                        _repositoryList.Remove(repositoryId);
                    }

                    if (_storage == StorageTypeConstants.Database)
                    {
                        using (var context = new Celeriq.DataCore.EFDAL.DataCoreEntities())
                        {
                            var item = context.RepositoryDefinition.FirstOrDefault(x => x.UniqueKey == repositoryId);
                            if (item != null)
                            {
                                //item.DimensionStoreList.SelectMany(x => x.DimensionDataList).ToList().Clear();
                                //item.DimensionStoreList.Clear();
                                //item.RepositoryLogList.Clear();
                                //item.RepositoryStatList.Clear();
                                item.RepositoryDataList.ToList().ForEach(x => context.DeleteItem(x));
                                context.SaveChanges();
                            }
                            else
                            {
                                item = new Celeriq.DataCore.EFDAL.Entity.RepositoryDefinition();
                                item.UniqueKey = repositoryId;
                                context.AddItem(item);
                            }

                            item.Name = schema.Name;
                            item.ItemCount = 0;
                            item.MemorySize = 0;
                            item.ItemCount = 0;
                            item.VersionHash = schema.VersionHash;
                            item.DefinitionData = schema.ObjectToBin();
                            var c = context.SaveChanges();

                            var repository = new Celeriq.RepositoryAPI.Repository(item.RepositoryId, schema, ConfigHelper.MasterKeys, _system);
                            var remoteItem = new RemotingObjectCache() { ServiceInstance = repository, Repository = schema };
                            remoteItem.DataDiskSize = 0;
                            remoteItem.DataMemorySize = 0;
                            remoteItem.IsLoaded = false;
                            remoteItem.ItemCount = 0;
                            remoteItem.LastUnloadTime = null;
                            remoteItem.VersionHash = schema.VersionHash;
                            remoteItem.Repository = schema;
                            remoteItem.VersionHash = schema.VersionHash;
                        
                            repository.RemotingObject = remoteItem;
                            _repositoryList.Add(repositoryId, remoteItem);
                        }
                    }
                    else if (_storage == StorageTypeConstants.File)
                    {
                        RemotingObjectCache remoteItem = null;
                        var repository = new Celeriq.RepositoryAPI.Repository(0, schema, ConfigHelper.MasterKeys, _system);
                        if (_repositoryList.ContainsKey(repositoryId))
                        {
                            var filePath = Path.Combine(ConfigHelper.DataPath, repositoryId.ToString(), "repository.data");
                            if (File.Exists(filePath))
                                File.Delete(filePath);

                            remoteItem = _repositoryList[repositoryId];
                        }
                        else
                        {
                            var item = new Celeriq.DataCore.EFDAL.Entity.RepositoryDefinition();
                            item.UniqueKey = repositoryId;
                            item.Name = schema.Name;
                            item.VersionHash = schema.VersionHash;
                            item.DefinitionData = schema.ObjectToBin();
                            remoteItem = new RemotingObjectCache() { ServiceInstance = repository, Repository = schema };
                        }

                        remoteItem.DataDiskSize = 0;
                        remoteItem.DataMemorySize = 0;
                        remoteItem.IsLoaded = false;
                        remoteItem.ItemCount = 0;
                        remoteItem.LastUnloadTime = null;
                        remoteItem.VersionHash = schema.VersionHash;
                        remoteItem.Repository = schema;
                        remoteItem.VersionHash = schema.VersionHash;

                        var defFile = Path.Combine(ConfigHelper.DataPath, schema.ID + ".celeriq");
                        if (File.Exists(defFile)) File.Delete(defFile);
                        schema.ToDisk(defFile);

                        repository.RemotingObject = remoteItem;
                        _repositoryList.Add(repositoryId, remoteItem);
                    }
                }

                using (var q = new AcquireReaderLock(this.SyncObject))
                {
                    if (ConfigHelper.AutoLoad)
                    {
                        LoadData(repositoryId, ((SystemCore)_system).RootUser);
                    }

                    if (_repositoryList.ContainsKey(repositoryId))
                    {
                        var active = _repositoryList[repositoryId];
                        active.VersionHash = schema.VersionHash;
                        return active.ServiceInstance;
                    }
                    return null;
                }

            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "RepositoryId: " + repositoryId);
                throw;
            }
            finally
            {
                timer.Stop();
                Logger.LogInfo("AddRepository: Elapsed=" + timer.ElapsedMilliseconds);
            }
        }

        public DateTime GetLastAccess(Guid repositoryId)
        {
            try
            {
                //EnsureRepositoryLoaded(repositoryId);
                var repository = GetRepository(repositoryId);
                return repository.ServiceInstance.LastAccess;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "RepositoryId: " + repositoryId);
                throw;
            }
        }

        public void UnloadData(Guid repositoryId)
        {
            try
            {
                //EnsureRepositoryLoaded(repositoryId);
                var repository = GetRepository(repositoryId);
                repository.ServiceInstance.UnloadData();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "RepositoryId: " + repositoryId);
            }
        }

        public void LoadData(Guid repositoryId, UserCredentials credentials)
        {
            try
            {
                //EnsureRepositoryLoaded(repositoryId);
                var repository = GetRepository(repositoryId);
                repository.ServiceInstance.Query(new DataQuery() { Credentials = credentials });
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "RepositoryId: " + repositoryId);
            }
        }

        public string[] UpdateData(Guid repositoryId, IEnumerable<DataItem> list, UserCredentials credentials)
        {
            var errorList = new List<string>();
            try
            {
                //EnsureRepositoryLoaded(repositoryId);
                var repository = GetRepository(repositoryId);
                if (repository == null)
                {
                    Logger.LogInfo("Repository not found: ID=" + repositoryId + ", Method=UpdateData");
                    errorList.Add("The repository has not been initialized! ID: " + repositoryId);
                }
                else
                    repository.ServiceInstance.UpdateIndexList(list, credentials);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "RepositoryId: " + repositoryId);
                errorList.Add(ex.ToString());
            }
            return errorList.ToArray();
        }

        public string[] DeleteData(Guid repositoryId, IEnumerable<DataItem> item, UserCredentials credentials)
        {
            var errorList = new List<string>();
            try
            {
                //EnsureRepositoryLoaded(repositoryId);
                var repository = GetRepository(repositoryId);
                if (repository == null)
                {
                    Logger.LogInfo("Repository not found: ID=" + repositoryId + ", Method=DeleteData");
                    errorList.Add("The repository has not been initialized! ID: " + repositoryId);
                }
                else
                    repository.ServiceInstance.DeleteData(item, credentials);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "RepositoryId: " + repositoryId);
                errorList.Add(ex.ToString());
            }
            return errorList.ToArray();
        }

        public string[] Clear(Guid repositoryId, UserCredentials credentials)
        {
            var errorList = new List<string>();
            try
            {
                //EnsureRepositoryLoaded(repositoryId);
                var repository = GetRepository(repositoryId);
                if (repository == null)
                {
                    Logger.LogInfo("Repository not found: ID=" + repositoryId + ", Method=Clear");
                    errorList.Add("The repository has not been initialized! ID: " + repositoryId);
                }
                else
                {
                    repository.ServiceInstance.Clear(credentials);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "RepositoryId: " + repositoryId);
                errorList.Add(ex.ToString());
            }
            return errorList.ToArray();
        }

        public DataQueryResults Query(Guid repositoryId, DataQuery query)
        {
            var retval = new DataQueryResults();
            try
            {
                //EnsureRepositoryLoaded(repositoryId);
                var repository = GetRepository(repositoryId);
                if (repository == null)
                {
                    Logger.LogInfo("Repository not found: ID=" + repositoryId + ", Method=Query");
                    retval.ErrorList = new string[] { "The repository has not been initialized! ID: " + repositoryId };
                }
                else
                {
                    retval = repository.ServiceInstance.Query(query);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "RepositoryId: " + repositoryId);
                retval.ErrorList = new string[] { ex.ToString() };
            }
            return retval;
        }

        public void ShutDown(Guid repositoryId)
        {
            try
            {
                //EnsureRepositoryLoaded(repositoryId);
                var repository = GetRepository(repositoryId);
                if (repository == null)
                {
                    Logger.LogInfo("Repository not found: ID=" + repositoryId + ", Method=ShutDown");
                    throw new Exception("The repository has not been initialized! ID: " + repositoryId);
                }

                repository.ServiceInstance.ShutDown();
                repository = null;

            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "RepositoryId: " + repositoryId);
                throw;
            }
        }

        /// <summary>
        /// Copies the entire repository to an archive file
        /// </summary>
        public bool ExportSchema(Guid repositoryId, UserCredentials credentials, string backupFile)
        {
            try
            {
                //EnsureRepositoryLoaded(repositoryId);
                var repository = GetRepository(repositoryId);
                return repository.ServiceInstance.ExportSchema(credentials, backupFile);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                throw;
            }
        }

        /// <summary>
        /// Copies the entire repository to an archive file
        /// </summary>
        public bool Backup(Guid repositoryId, UserCredentials credentials, string backupFile)
        {
            try
            {
                //EnsureRepositoryLoaded(repositoryId);
                var repository = GetRepository(repositoryId);
                return repository.ServiceInstance.Backup(credentials, backupFile);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "RepositoryId: " + repositoryId);
                throw;
            }
        }

        public bool Restore(Guid repositoryId, UserCredentials credentials, string backupFile)
        {
            try
            {
                //EnsureRepositoryLoaded(repositoryId);
                var repository = GetRepository(repositoryId);
                return repository.ServiceInstance.Restore(credentials, backupFile);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "RepositoryId: " + repositoryId);
                throw;
            }
        }

        /// <summary>
        /// Returns the size on disk of the repository data
        /// </summary>
        /// <returns></returns>
        public long GetDataDiskSize(Guid repositoryId, UserCredentials credentials)
        {
            try
            {
                //EnsureRepositoryLoaded(repositoryId);
                var repository = GetRepository(repositoryId);
                if (repository == null) return 0;
                return repository.ServiceInstance.GetDataDiskSize(credentials);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "RepositoryId: " + repositoryId);
                throw;
            }
        }

        /// <summary>
        /// Returns the size of memory of the repository data
        /// </summary>
        /// <returns></returns>
        public long GetDataMemorySize(Guid repositoryId, UserCredentials credentials)
        {
            try
            {
                //EnsureRepositoryLoaded(repositoryId);
                var repository = GetRepository(repositoryId);
                if (repository == null) return 0;
                return repository.ServiceInstance.GetDataMemorySize(credentials);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "RepositoryId: " + repositoryId);
                throw;
            }
        }

        /// <summary>
        /// Returns the number of items in the repository
        /// </summary>
        /// <returns></returns>
        public long GetItemCount(Guid repositoryId, UserCredentials credentials)
        {
            try
            {
                var repository = GetRepository(repositoryId);
                if (repository == null) return 0;
                return repository.ServiceInstance.GetItemCount(credentials);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "RepositoryId: " + repositoryId);
                throw;
            }
        }

        //public void Compress(Guid repositoryId, UserCredentials credentials)
        //{
        //    try
        //    {
        //        //EnsureRepositoryLoaded(repositoryId);
        //        var repository = GetRepository(repositoryId);
        //        if (repository == null)
        //            throw new Exception("The repository has not been initialized! ID: " + repositoryId);

        //        //repository.ServiceInstance.Compress(credentials);
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.LogError(ex);
        //        throw;
        //    }
        //}

        public bool IsLoaded(Guid repositoryId)
        {
            try
            {
                //EnsureRepositoryLoaded(repositoryId);
                var repository = GetRepository(repositoryId);
                if (repository == null)
                    return false;
                else
                    return repository.IsLoaded;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                throw;
            }
        }

        //public ProfileItem[] GetProfile(Guid repositoryId, UserCredentials credentials, long lastProfileId)
        //{
        //    try
        //    {
        //        var repository = GetRepository(repositoryId);
        //        return repository.ServiceInstance.GetProfile(credentials, lastProfileId);
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.LogError(ex);
        //        throw;
        //    }
        //}

        private RemotingObjectCache GetRepository(Guid repositoryId)
        {
            using (var q = new AcquireReaderLock(this.SyncObject))
            {
                if (!_repositoryList.ContainsKey(repositoryId)) return null;
                return _repositoryList[repositoryId];
            }
        }

        public bool IsValidFormat(Guid repositoryId, DataItem item, UserCredentials credentials)
        {
            try
            {
                var repository = GetRepository(repositoryId);
                if (repository != null)
                {
                    var r = repository.ServiceInstance as Celeriq.RepositoryAPI.Repository;
                    return r.IsValidFormat(item, credentials);
                }
                return false;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "RepositoryId: " + repositoryId);
                throw;
            }
        }

        public void FlushCache()
        {
            var timer = new Stopwatch();
            timer.Start();
            try
            {
                var page = 0;
                var rpp = 10;
                List<RemotingObjectCache> tempList = null;
                do
                {
                    using (var q = new AcquireWriterLock(this.SyncObject, "FlushCache Manager"))
                    {
                        //var t2 = new Stopwatch();
                        //t2.Start();
                        tempList = _repositoryList.Values.ToList().Skip(page * rpp).Take(rpp).ToList();
                        //t2.Stop();
                        //Console.WriteLine("FlushCache list: page: " + page + ", time:" + t2.ElapsedMilliseconds);

                        var options = new ParallelOptions { MaxDegreeOfParallelism = 2 };
                        Parallel.ForEach(tempList, options, item =>
                        {
                            item.FlushCache(true);
                        });
                    }

                    //Get next page...loop until run out of items
                    page++;
                } while (tempList.Count > 0);
            }
            catch (Exception ex)
            {
                Logger.LogWarning(ex.Message);
            }
            finally
            {
                timer.Stop();
                var q = timer.ElapsedMilliseconds;
            }
        }

    }
}