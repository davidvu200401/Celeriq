#pragma warning disable 0168
using System;
using System.Collections.Generic;
using System.Linq;
using Celeriq.Server.Interfaces;
using Celeriq.Utilities;
using System.IO;
using Celeriq.Common;
using System.ServiceModel;
using System.Runtime.Serialization;
using System.ServiceModel.Description;
using System.Security.Permissions;
using System.Security;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Celeriq.DataCore.EFDAL;

namespace Celeriq.Server.Core
{
    [Serializable()]
    [KnownType(typeof(RepositorySchema))]
    [KnownType(typeof(FieldDefinition))]
    [KnownType(typeof(DimensionDefinition))]
    [KnownType(typeof(IFieldDefinition))]
    [KnownType(typeof(Celeriq.Common.IRemotingObject))]
    [KnownType(typeof(Celeriq.Common.BaseRemotingObject))]
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single)]
    public class SystemCore : MarshalByRefObject, ISystemServerCore, IDisposable
    {
        #region Win32 Callout

        [DllImport("psapi.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetPerformanceInfo([Out] out PerformanceInformation PerformanceInformation, [In] int Size);

        [StructLayout(LayoutKind.Sequential)]
        public struct PerformanceInformation
        {
            public int Size;
            public IntPtr CommitTotal;
            public IntPtr CommitLimit;
            public IntPtr CommitPeak;
            public IntPtr PhysicalTotal;
            public IntPtr PhysicalAvailable;
            public IntPtr SystemCache;
            public IntPtr KernelTotal;
            public IntPtr KernelPaged;
            public IntPtr KernelNonPaged;
            public IntPtr PageSize;
            public int HandlesCount;
            public int ProcessCount;
            public int ThreadCount;
        }

        #endregion

        #region Class Members

        private System.Timers.Timer _timerDelayLoad = null;
        private System.Timers.Timer _timer = null;
        private System.Timers.Timer _timerStats = null;
        private ServerResourceSettings _resourceSettings = null;
        private System.Diagnostics.PerformanceCounter _cpuCounter;
        private Dictionary<Guid, int> _loadDelta = new Dictionary<Guid, int>();
        private Dictionary<Guid, int> _unloadDelta = new Dictionary<Guid, int>();
        private int _createdDelta = 0;
        private int _deletedDelta = 0;
        private RepositoryManager _manager = null;
        private StorageTypeConstants _storage = StorageTypeConstants.Database;
        private bool _allowCounters = false;
        private DateTime _lastLogSend;

        private const int HouseKeepingInterval = 5;
        private const int CompressInterval = 15;

        #endregion

        #region Constructors

        public SystemCore(StorageTypeConstants storage, ConfigurationSetup setup)
            : base()
        {
            try
            {
                _resourceSettings = new ServerResourceSettings();
                _resourceSettings.Initialize();

                _manager = new RepositoryManager(this, storage);
                _storage = storage;
                StatLogger.Storage = storage;
                ConfigHelper.Storage = storage;
                UserDomain.Storage = storage;
                this.StatLocker = new Celeriq.Utilities.CeleriqLock(System.Threading.LockRecursionPolicy.SupportsRecursion);
                _lastLogSend = DateTime.Now.Date;

                if (_storage == StorageTypeConstants.Database)
                {
                    //Setup system
                    using (var context = new DataCoreEntities())
                    {
                        if (string.IsNullOrEmpty(ConfigHelper.PrivateKey) || string.IsNullOrEmpty(ConfigHelper.PublicKey))
                        {
                            var keys = SecurityHelper.GenerateSymmetricKeys();
                            ConfigHelper.PrivateKey = keys.PrivateKey;
                            ConfigHelper.PublicKey = keys.PublicKey;
                        }
                    }
                }
                else
                {
                    if (!Directory.Exists(setup.DataPath))
                        Directory.CreateDirectory(setup.DataPath);

                    if (string.IsNullOrEmpty(setup.PrivateKey) || string.IsNullOrEmpty(setup.PublicKey))
                    {
                        var keys = SecurityHelper.GenerateSymmetricKeys();
                        setup.PrivateKey = keys.PrivateKey;
                        setup.PublicKey = keys.PublicKey;
                    }

                    ConfigHelper.PrivateKey = setup.PrivateKey;
                    ConfigHelper.PublicKey = setup.PublicKey;
                    ConfigHelper.DataPath = setup.DataPath;
                }

                ConfigHelper.AutoLoad = setup.AutoLoad;
                ConfigHelper.AllowStatistics = setup.AllowStatistics;

                //Do this to init user list
                UserDomain.InitUsers();
                StatLogger.Initialize();

                this.SetupCounters();

                if (setup.IsDebug)
                {
                    DelayStartup(null, null);
                }
                else
                {
                    _timerDelayLoad = new System.Timers.Timer(10);
                    _timerDelayLoad.Elapsed += DelayStartup;
                    _timerDelayLoad.Start();
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                throw;
            }
        }

        private Celeriq.Utilities.CeleriqLock _statLocker = null;
        private Celeriq.Utilities.CeleriqLock StatLocker
        {
            get { return _statLocker; }
            set
            {
                if (_statLocker == null) _statLocker = value;
                else throw new Exception("Object cannot be reset!");
            }
        }

        private void SetupCounters()
        {
            try
            {
                var CounterDatas = new System.Diagnostics.CounterCreationDataCollection();

                //if (System.Diagnostics.PerformanceCounterCategory.Exists(StatLogger.PERFMON_CATEGORY))
                //{
                //    System.Diagnostics.PerformanceCounterCategory.Delete(StatLogger.PERFMON_CATEGORY);
                //}

                if (!System.Diagnostics.PerformanceCounterCategory.Exists(StatLogger.PERFMON_CATEGORY))
                {
                    //Memory Usage Process
                    var cMemoryUsageProcess = new System.Diagnostics.CounterCreationData();
                    cMemoryUsageProcess.CounterName = StatLogger.COUNTER_MEMUSAGE;
                    cMemoryUsageProcess.CounterHelp = "Total memory used by the service";
                    cMemoryUsageProcess.CounterType = PerformanceCounterType.NumberOfItems64;
                    CounterDatas.Add(cMemoryUsageProcess);

                    //Repository In Mem
                    var cRepositoryInMem = new System.Diagnostics.CounterCreationData();
                    cRepositoryInMem.CounterName = StatLogger.COUNTER_INMEM;
                    cRepositoryInMem.CounterHelp = "Total number of repositories in memory";
                    cRepositoryInMem.CounterType = PerformanceCounterType.NumberOfItems32;
                    CounterDatas.Add(cRepositoryInMem);

                    //Repository Load Delta
                    var cRepositoryLoadDelta = new System.Diagnostics.CounterCreationData();
                    cRepositoryLoadDelta.CounterName = StatLogger.COUNTER_LOADDELTA;
                    cRepositoryLoadDelta.CounterHelp = "Number of repository loads/interval";
                    cRepositoryLoadDelta.CounterType = PerformanceCounterType.RateOfCountsPerSecond32;
                    CounterDatas.Add(cRepositoryLoadDelta);

                    //Repository Unload Delta
                    var cRepositoryUnloadDelta = new System.Diagnostics.CounterCreationData();
                    cRepositoryUnloadDelta.CounterName = StatLogger.COUNTER_UNLOADDELTA;
                    cRepositoryUnloadDelta.CounterHelp = "Number of repository unloads/interval";
                    cRepositoryUnloadDelta.CounterType = PerformanceCounterType.RateOfCountsPerSecond32;
                    CounterDatas.Add(cRepositoryUnloadDelta);

                    //Repository Total
                    var cRepositoryTotal = new System.Diagnostics.CounterCreationData();
                    cRepositoryTotal.CounterName = StatLogger.COUNTER_REPOTOTAL;
                    cRepositoryTotal.CounterHelp = "Total number of system repositories";
                    cRepositoryTotal.CounterType = PerformanceCounterType.NumberOfItems32;
                    CounterDatas.Add(cRepositoryTotal);

                    //Repository Create Delta
                    var cRepositoryCreateDelta = new System.Diagnostics.CounterCreationData();
                    cRepositoryCreateDelta.CounterName = StatLogger.COUNTER_REPOCREATE;
                    cRepositoryCreateDelta.CounterHelp = "Number of repository creates/interval";
                    cRepositoryCreateDelta.CounterType = PerformanceCounterType.RateOfCountsPerSecond32;
                    CounterDatas.Add(cRepositoryCreateDelta);

                    //Repository Delete Delta
                    var cRepositoryDeleteDelta = new System.Diagnostics.CounterCreationData();
                    cRepositoryDeleteDelta.CounterName = StatLogger.COUNTER_REPODELETE;
                    cRepositoryDeleteDelta.CounterHelp = "Number of repository deletes/interval";
                    cRepositoryDeleteDelta.CounterType = PerformanceCounterType.RateOfCountsPerSecond32;
                    CounterDatas.Add(cRepositoryDeleteDelta);

                    //Add all counters
                    System.Diagnostics.PerformanceCounterCategory.Create(StatLogger.PERFMON_CATEGORY, "Metrics for the Celeriq faceted navigation engine",
                        PerformanceCounterCategoryType.SingleInstance, CounterDatas);
                }
                _allowCounters = true;
            }
            catch (Exception ex)
            {
                Logger.LogInfo("The service does not have permission to create performance counters!");
                _allowCounters = false;
            }

        }

        public RepositoryManager Manager
        {
            get { return _manager; }
        }

        public StorageTypeConstants Storage
        {
            get { return _storage; }
        }

        private void DelayStartup(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                if (_timerDelayLoad != null)
                {
                    _timerDelayLoad.Stop();
                    _timerDelayLoad = null;
                }

                Logger.LogInfo("Core Initialize: Server=" + Environment.MachineName + ", Mode=x" + (Environment.Is64BitProcess ? "64" : "32"));

                var timer = new Stopwatch();
                timer.Start();
                using (var q = new AcquireWriterLock(_manager.SyncObject, "DelayStartup"))
                {
                    #region HouseKeeping

                    //Initialize user file if need be
                    InitUserFile();

                    #endregion

                    //Verify that all permissions are in place
                    if (!this.IsSetupValid())
                    {
                        throw new Exception("This application does not have the proper permissions!");
                    }
                    _manager.Initialize();

                }
                timer.Stop();
                Logger.LogInfo("Core Initialize Complete: Elapsed=" + timer.ElapsedMilliseconds);

                _cpuCounter = new System.Diagnostics.PerformanceCounter();
                _cpuCounter.CategoryName = "Processor";
                _cpuCounter.CounterName = "% Processor Time";
                _cpuCounter.InstanceName = "_Total";

                _timer = new System.Timers.Timer(30000);
                _timer.Elapsed += _timer_Elapsed;
                _timer.Start();

                if (_allowCounters)
                {
                    _timerStats = new System.Timers.Timer(6000);
                    _timerStats.Elapsed += _timerStats_Elapsed;
                    _timerStats.Start();
                }

                //If auto load all repositories then spawn new thread to do this
                if (ConfigHelper.AutoLoad)
                {
                    var o = new AutoLoader(this);
                    var t = new System.Threading.Thread(o.Run);
                    t.Start();
                }

            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                throw;
            }
        }

        #endregion

        #region Event Handlers

        private long GetProcessMemory()
        {
            try
            {
                var p = Process.GetProcessesByName(System.Reflection.Assembly.GetEntryAssembly().GetName().Name).FirstOrDefault();
                if (p != null) return p.PrivateMemorySize64;
                return 0;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                throw;
            }
        }

        private DateTime _lastStatDbCall = DateTime.MinValue;
        private int _lastLoadDeltaPerfmonSummary = 0;
        private int _lastUnloadDeltaPerfmonSummary = 0;
        private int _lastCreateDeltaPerfmonSummary = 0;
        private int _lastDeleteDeltaPerfmonSummary = 0;
        private void _timerStats_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (!_allowCounters) return;
            _timerStats.Stop();
            try
            {
                #region CPU

                var cpu = (int)_cpuCounter.NextValue();

                #endregion

                #region Perf Counter

                var info = new PerformanceInformation();
                GetPerformanceInfo(out info, Marshal.SizeOf(info));

                #endregion

                #region In Mem

                var inMemCount = _manager.List.Count(x => x.GetIsLoaded());

                #endregion

                #region Write stat
                using (var q = new AcquireWriterLock(this.StatLocker, "_timerStats_Elapsed"))
                {
                    var newItem = new RealtimeStats
                               {
                                   Timestamp = DateTime.Now,
                                   MemoryUsageAvailable = info.PhysicalAvailable.ToInt64() * info.PageSize.ToInt64(),
                                   MemoryUsageProcess = GetProcessMemory(),
                                   MemoryUsageTotal = info.PhysicalTotal.ToInt64() * info.PageSize.ToInt64(),
                                   ProcessorUsage = cpu,
                                   RepositoryInMem = inMemCount,
                                   RepositoryLoadDelta = _loadDelta.Keys.Count,
                                   RepositoryUnloadDelta = _unloadDelta.Keys.Count,
                                   RepositoryTotal = _manager.List.Count,
                                   RepositoryCreateDelta = _createdDelta,
                                   RepositoryDeleteDelta = _deletedDelta,
                               };

                    //Log to database once per minute
                    if (DateTime.Now.Subtract(_lastStatDbCall).TotalSeconds >= 60)
                    {
                        _lastStatDbCall = DateTime.Now;
                        StatLogger.Log(newItem);

                        _loadDelta = new Dictionary<Guid, int>();
                        _unloadDelta = new Dictionary<Guid, int>();
                        _createdDelta = 0;
                        _deletedDelta = 0;
                    }

                    //Log Performance counters
                    try
                    {
                        (new System.Diagnostics.PerformanceCounter(StatLogger.PERFMON_CATEGORY, StatLogger.COUNTER_MEMUSAGE, string.Empty, false)).RawValue = newItem.MemoryUsageProcess;
                        (new System.Diagnostics.PerformanceCounter(StatLogger.PERFMON_CATEGORY, StatLogger.COUNTER_INMEM, string.Empty, false)).RawValue = newItem.RepositoryInMem;
                        (new System.Diagnostics.PerformanceCounter(StatLogger.PERFMON_CATEGORY, StatLogger.COUNTER_LOADDELTA, string.Empty, false)).RawValue = newItem.RepositoryLoadDelta - _lastLoadDeltaPerfmonSummary;
                        (new System.Diagnostics.PerformanceCounter(StatLogger.PERFMON_CATEGORY, StatLogger.COUNTER_UNLOADDELTA, string.Empty, false)).RawValue = newItem.RepositoryUnloadDelta - _lastUnloadDeltaPerfmonSummary;
                        (new System.Diagnostics.PerformanceCounter(StatLogger.PERFMON_CATEGORY, StatLogger.COUNTER_REPOTOTAL, string.Empty, false)).RawValue = newItem.RepositoryTotal;
                        (new System.Diagnostics.PerformanceCounter(StatLogger.PERFMON_CATEGORY, StatLogger.COUNTER_REPOCREATE, string.Empty, false)).RawValue = newItem.RepositoryCreateDelta - _lastCreateDeltaPerfmonSummary;
                        (new System.Diagnostics.PerformanceCounter(StatLogger.PERFMON_CATEGORY, StatLogger.COUNTER_REPODELETE, string.Empty, false)).RawValue = newItem.RepositoryDeleteDelta - _lastDeleteDeltaPerfmonSummary;
                    }
                    catch (Exception ex)
                    {
                        //Do Nothing
                    }

                    //Reset delta lists for perf counters
                    _lastLoadDeltaPerfmonSummary = _loadDelta.Count;
                    _lastUnloadDeltaPerfmonSummary = _unloadDelta.Count;
                    _lastCreateDeltaPerfmonSummary = _createdDelta;
                    _lastDeleteDeltaPerfmonSummary = _deletedDelta;

                }
                #endregion

                //If we have not sent the log today then send it
                if (_lastLogSend != DateTime.Now.Date && !string.IsNullOrEmpty(ConfigHelper.DebugEmail))
                {
                    _lastLogSend = DateTime.Now.Date;
                    SendLogs();
                }

            }
            catch (Exception ex)
            {
                //Logger.LogError(ex);
            }
            finally
            {
                _timerStats.Start();
            }
        }

        private void SendLogs()
        {
            if (string.IsNullOrEmpty(ConfigHelper.DebugEmail)) return;
            try
            {
                var logDate = DateTime.Now.Date.AddDays(-1);
                var logFile = Path.Combine((new FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location)).DirectoryName, "logs", logDate.ToString("yyyy-MM-dd")) + ".txt";
                if (!File.Exists(logFile)) return;
                EmailDomain.SendMail(new EmailSettings
                {
                    Body = string.Empty,
                    From = ConfigHelper.FromEmail,
                    Subject = "Logs for " + logDate.ToString("yyyy-MM-dd") + " [" + Environment.MachineName + "]",
                    To = ConfigHelper.DebugEmail,
                    Attachments = new List<string>() { logFile },
                });
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
        }

        private DateTime _lastCache = DateTime.Now;
        private DateTime _lastGarbageCollection = DateTime.Now;
        private DateTime _lastCompress = DateTime.Now;
        private void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                if (_timer != null) _timer.Stop();
                #region This will unload the data in a repository if there is a set # of minutes to keep inactive repository

                var masterList = _manager.List
                        .Where(x => x.GetIsLoaded())
                        .ToList();

                var wasUnloaded = false;
                foreach (var item in masterList)
                {
                    if (item.ServiceInstance.LastAccess != DateTime.MinValue &&
                        (_resourceSettings.AutoDataUnloadTime > 0) &&
                        (DateTime.Now.Subtract(item.ServiceInstance.LastAccess).TotalMinutes >= _resourceSettings.AutoDataUnloadTime))
                    {
                        item.ServiceInstance.UnloadData();
                        wasUnloaded = true;
                    }
                }

                #endregion

                #region Unload if thre are too many repositories in memory

                if (_resourceSettings.MaxRunningRepositories > 0)
                {
                    var tooManyList = masterList
                        .OrderByDescending(x => x.ServiceInstance.LastAccess)
                        .Skip(_resourceSettings.MaxRunningRepositories)
                        .ToList();

                    foreach (var item in tooManyList)
                    {
                        item.ServiceInstance.UnloadData();
                        wasUnloaded = true;
                    }
                }

                #endregion

                #region Unload if used too much memory

                if (_resourceSettings.MaxMemory > 0)
                {
                    long TOTAL = _resourceSettings.MaxMemory * 1024 * 1024;
                    var list = masterList
                        .OrderByDescending(x => x.ServiceInstance.LastAccess)
                        .ToList();
                    long usedMemory = 0;
                    foreach (var item in list)
                    {
                        usedMemory += item.ServiceInstance.GetDataMemorySize(this.RootUser);
                        if (usedMemory > TOTAL)
                        {
                            item.ServiceInstance.UnloadData();
                            wasUnloaded = true;
                        }
                    }
                }

                #endregion

                //collect garbage to free memory (do not do too often. Like > N minutes)
                if (wasUnloaded && DateTime.Now.Subtract(_lastGarbageCollection).TotalMinutes >= HouseKeepingInterval)
                {
                    var before = GC.GetTotalMemory(false);
                    GC.Collect();
                    var after = GC.GetTotalMemory(true);
                    Logger.LogInfo(string.Format("Garbage Collection: {0:N0} / {1:N0}", before, after));
                    _lastGarbageCollection = DateTime.Now;
                }

                //Every N minutes make sure cache is written
                if (DateTime.Now.Subtract(_lastCache).TotalMinutes >= HouseKeepingInterval)
                {
                    _lastCache = DateTime.Now; //Just in case error
                    _manager.FlushCache();
                    _lastCache = DateTime.Now;
                }

                //For now do not automatically compress. Causes timeouts in production if repository is still being used.
                //if (DateTime.Now.Subtract(_lastCompress).TotalMinutes >= CompressInterval)
                //{
                //    masterList.ForEach(x => x.ServiceInstance.Compress());
                //    _lastCompress = DateTime.Now;
                //}

            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                //throw; //Do not kill app because of this
            }
            finally
            {
                if (_timer != null) _timer.Start();
            }
        }

        #endregion

        #region Properties

        private object _cacheCryptedLocker = new object();
        private SystemCredentials _cacheCrypted = null;

        internal SystemCredentials RootUser
        {
            get
            {
                try
                {
                    lock (_cacheCryptedLocker)
                    {
                        if (_cacheCrypted == null)
                        {
                            var user = UserDomain.UserList.FirstOrDefault(x => x.UserName == "root");
                            if (user == null) return null;
                            user = new SystemCredentials() { UserName = user.UserName, Password = user.Password };
                            user.Password = Celeriq.Utilities.SecurityHelper.Encrypt(this.GetPublicKey(), user.Password);
                            _cacheCrypted = user;
                            return user;
                        }
                        else
                        {
                            return _cacheCrypted;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex);
                    throw;
                }
            }
        }

        public int GetRepositoryCount(UserCredentials credentials, PagingInfo paging)
        {
            if (!_manager.IsInitialized)
                throw new Exception("System core is loading.");

            try
            {
                using (var q = new AcquireReaderLock(_manager.SyncObject))
                {
                    var lambda = _manager.List.AsQueryable();
                    if (!string.IsNullOrEmpty(paging.Keyword))
                    {
                        lambda = lambda.Where(x =>
                            x.Repository.ID.ToString().Contains(paging.Keyword, StringComparison.InvariantCultureIgnoreCase) ||
                            x.Repository.Name.Contains(paging.Keyword, StringComparison.InvariantCultureIgnoreCase) ||
                            x.Repository.VersionHash.ToString().Contains(paging.Keyword, StringComparison.InvariantCultureIgnoreCase)
                            );
                    }
                    return lambda.Count();
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                throw;
            }
        }

        public List<BaseRemotingObject> GetRepositoryPropertyList(UserCredentials credentials, PagingInfo paging)
        {
            #region Validation
            if (!_manager.IsInitialized)
                throw new Exception("System core is loading.");

            if (!IsValidCredentials(credentials))
            {
                Logger.LogInfo("GetRepositoryPropertyList: Invalid credentials");
                throw new Exception("Invalid credentials");
            }

            if (paging.PageOffset < 1)
            {
                throw new Exception("PageOffset must be greater than 0.");
            }

            if (paging.RecordsPerPage < 1)
            {
                throw new Exception("RecordsPerPage must be greater than 0.");
            }

            #endregion

            var timer = new Stopwatch();
            timer.Start();
            try
            {
                using (var q = new AcquireReaderLock(_manager.SyncObject))
                {
                    var startIndex = (paging.PageOffset - 1) * paging.RecordsPerPage;

                    var lambda = _manager.List.AsQueryable();
                    if (!string.IsNullOrEmpty(paging.Keyword))
                    {
                        lambda = lambda.Where(x =>
                            x.Repository.ID.ToString().Contains(paging.Keyword, StringComparison.InvariantCultureIgnoreCase) ||
                            x.Repository.Name.Contains(paging.Keyword, StringComparison.InvariantCultureIgnoreCase) ||
                            x.Repository.VersionHash.ToString().Contains(paging.Keyword, StringComparison.InvariantCultureIgnoreCase)
                            );
                    }

                    switch ((paging.SortField + string.Empty).ToLower())
                    {
                        case "name":
                            if (paging.SortAsc)
                                lambda = lambda.OrderBy(x => x.Repository.Name);
                            else
                                lambda = lambda.OrderByDescending(x => x.Repository.Name);
                            break;
                        case "id":
                            if (paging.SortAsc)
                                lambda = lambda.OrderBy(x => x.Repository.ID);
                            else
                                lambda = lambda.OrderByDescending(x => x.Repository.ID);
                            break;
                        case "disksize":
                            if (paging.SortAsc)
                                lambda = lambda.OrderBy(x => x.DataDiskSize);
                            else
                                lambda = lambda.OrderByDescending(x => x.DataDiskSize);
                            break;
                        case "memorysize":
                            if (paging.SortAsc)
                                lambda = lambda.OrderBy(x => x.DataMemorySize);
                            else
                                lambda = lambda.OrderByDescending(x => x.DataMemorySize);
                            break;
                        case "hash":
                            if (paging.SortAsc)
                                lambda = lambda.OrderBy(x => x.Repository.VersionHash);
                            else
                                lambda = lambda.OrderByDescending(x => x.Repository.VersionHash);
                            break;
                        case "count":
                            if (paging.SortAsc)
                                lambda = lambda.OrderBy(x => x.ItemCount);
                            else
                                lambda = lambda.OrderByDescending(x => x.ItemCount);
                            break;
                        case "created":
                            if (paging.SortAsc)
                                lambda = lambda.OrderBy(x => x.Repository.CreatedDate);
                            else
                                lambda = lambda.OrderByDescending(x => x.Repository.CreatedDate);
                            break;
                        default:
                            lambda = lambda.OrderBy(x => x.Repository.Name);
                            break;
                    }

                    var tempList = lambda
                        .Skip(startIndex)
                        .Take(paging.RecordsPerPage)
                        .ToList();

                    paging.TotalItemCount = lambda.Count();

                    if (_storage == StorageTypeConstants.File)
                    {
                        foreach (var item in tempList)
                        {
                            item.ItemCount = item.ServiceInstance.GetItemCount(credentials);
                            item.DataDiskSize = item.ServiceInstance.GetDataDiskSize(credentials);
                            item.DataMemorySize = item.ServiceInstance.GetDataMemorySize(credentials);
                            item.IsLoaded = item.ServiceInstance.IsLoaded;
                        }
                    }

                    tempList.ForEach(x => x.IsLoaded = x.GetIsLoaded());
                    var retval = new List<BaseRemotingObject>();
                    tempList.ForEach(x => retval.Add((BaseRemotingObject)((ICloneable)x).Clone()));
                    return retval;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return null;
            }
            finally
            {
                timer.Stop();
                Logger.LogInfo("GetRepositoryPropertyList: Elapsed=" + timer.ElapsedMilliseconds + ", Keyword='" + paging.Keyword + "'");
            }
        }

        #endregion

        #region Methods

        public void LogRepositoryPerf(RepositorySummmaryStats stat)
        {
            try
            {
                StatLogger.Log(stat);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
        }

        public string GetPublicKey()
        {
            return ConfigHelper.MasterKeys.PublicKey;
        }

        private static readonly Dictionary<string, string> _keyCache = new Dictionary<string, string>();

        public bool IsValidCredentials(UserCredentials credentials)
        {
            try
            {
                lock (_keyCache)
                {
                    var user = UserDomain.UserList.FirstOrDefault(x => x.UserName == credentials.UserName);
                    if (user == null) return false;

                    var prehash = string.Empty;
                    var cacheKey = ConfigHelper.MasterKeys.PrivateKey + "||" + credentials.Password;
                    if (_keyCache.ContainsKey(cacheKey))
                    {
                        prehash = _keyCache[cacheKey];
                    }
                    else
                    {
                        prehash = Celeriq.Utilities.SecurityHelper.Decrypt(ConfigHelper.MasterKeys.PrivateKey, credentials.Password);

                        //Cache
                        _keyCache.Add(cacheKey, prehash);
                    }
                    return (prehash == user.Password);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return false;
            }
        }

        private void InitUserFile()
        {
            try
            {
                if (UserDomain.UserList.Count == 0)
                {
                    UserDomain.AddUser(new SystemCredentials { UserName = "root", Password = "password" });
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                throw;
            }
        }

        private bool IsSetupValid()
        {
            var eventLogPermission = new System.Diagnostics.EventLogPermission();
            try
            {
                eventLogPermission.Demand();
            }
            catch (Exception ex)
            {
                Logger.LogError("The service does not have permission to the event log!");
                return false;
            }
            return true;
        }

        public BaseRemotingObject SaveRepository(RepositorySchema repository, UserCredentials credentials)
        {
            if (!_manager.IsInitialized)
                throw new Exception("System core is loading.");

            if (!IsValidCredentials(credentials))
                throw new Exception("Invalid credentials");

            if (repository == null)
                throw new Exception("Object cannot be null!");

            try
            {
                //Logger.LogInfo("SaveRepository: " + repository.ID);
                if (repository.CreatedDate == DateTime.MinValue)
                    repository.CreatedDate = DateTime.Now;

                using (var q = new AcquireReaderLock(_manager.SyncObject))
                {
                    if (!_manager.Contains(repository.ID))
                    {
                        using (var q2 = new AcquireWriterLock(this.StatLocker, "SaveRepository"))
                        {
                            _createdDelta++;
                        }
                    }

                    long didx = 1000000;
                    foreach (var d in repository.DimensionList)
                    {
                        d.DIdx = didx;
                        didx++;
                    }
                }

                var retval = _manager.AddRepository(repository.ID, repository);
                var active = _manager.List.FirstOrDefault(x => x.Repository.ID == repository.ID);
                if (active != null)
                    return ((ICloneable)((BaseRemotingObject)active)).Clone() as BaseRemotingObject;

                Logger.LogWarning("SaveRepository: " + repository.ID + ", NULL Returned");
                return null;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return null;
            }
        }

        private void UnloadOldest()
        {
            try
            {
                var item = _manager.List
                    .OrderBy(x => x.ServiceInstance.LastAccess)
                    .FirstOrDefault();

                if (item != null)
                {
                    item.ServiceInstance.UnloadData();
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                throw;
            }
        }

        private int LoadedCount()
        {
            try
            {
                return _manager.List.Count(x => x.GetIsLoaded());
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                throw;
            }
        }

        public bool RepositoryExists2(Guid repositoryId, UserCredentials credentials)
        {
            if (!_manager.IsInitialized)
                throw new Exception("System core is loading.");

            if (!IsValidCredentials(credentials))
                throw new Exception("Invalid credentials");

            try
            {
                using (var q = new AcquireReaderLock(_manager.SyncObject))
                {
                    return _manager.List.Any(x => x.Repository.ID == repositoryId);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                throw;
            }
        }

        public bool RepositoryExists(Guid repositoryId, UserCredentials credentials)
        {
            if (!_manager.IsInitialized)
                throw new Exception("System core is loading.");

            if (!IsValidCredentials(credentials))
                throw new Exception("Invalid credentials");

            try
            {
                using (var q = new AcquireReaderLock(_manager.SyncObject))
                {
                    return _manager.List.Any(x => x.Repository.ID == repositoryId);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                throw;
            }
        }

        public BaseRemotingObject DataLoadRepository(Guid repositoryId, UserCredentials credentials)
        {
            if (!_manager.IsInitialized)
                throw new Exception("System core is loading.");
            
            if (!IsValidCredentials(credentials))
                throw new Exception("Invalid credentials");

            try
            {
                using (var q = new AcquireReaderLock(_manager.SyncObject))
                {
                    var active = _manager.List.FirstOrDefault(x => x.Repository.ID == repositoryId);
                    if (active == null) return null; //cannot find repository, should never happen
                    if (active.ServiceInstance == null) return null; //cannot find repository, should never happen
                    if (active.GetIsLoaded()) return ((ICloneable)((BaseRemotingObject)active)).Clone() as BaseRemotingObject;
                    active.ServiceInstance.LoadData(credentials);
                    return ((ICloneable)((BaseRemotingObject)active)).Clone() as BaseRemotingObject;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                throw;
            }
        }

        public void DeleteRepository(RepositorySchema repository, UserCredentials credentials)
        {
            if (!_manager.IsInitialized)
                throw new Exception("System core is loading.");

            if (!IsValidCredentials(credentials))
                throw new Exception("Invalid credentials");

            try
            {
                var active = _manager.List.FirstOrDefault(x => x.Repository.ID == repository.ID);
                if (active != null)
                {
                    _manager.RemoveRepository(active.Repository.ID);
                    using (var q = new AcquireWriterLock(this.StatLocker, "DeleteRepository"))
                    {
                        _deletedDelta++;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                throw;
            }
        }

        /// <summary>
        /// Shuts down all repositories
        /// </summary>
        public void ShutDown()
        {
            try
            {
                _timer.Stop();
                _timerStats.Stop();

                //Log Performance counters
                try
                {
                    (new System.Diagnostics.PerformanceCounter(StatLogger.PERFMON_CATEGORY, StatLogger.COUNTER_MEMUSAGE, string.Empty, false)).RawValue = 0;
                    (new System.Diagnostics.PerformanceCounter(StatLogger.PERFMON_CATEGORY, StatLogger.COUNTER_INMEM, string.Empty, false)).RawValue = 0;
                    (new System.Diagnostics.PerformanceCounter(StatLogger.PERFMON_CATEGORY, StatLogger.COUNTER_LOADDELTA, string.Empty, false)).RawValue = 0;
                    (new System.Diagnostics.PerformanceCounter(StatLogger.PERFMON_CATEGORY, StatLogger.COUNTER_UNLOADDELTA, string.Empty, false)).RawValue = 0;
                    (new System.Diagnostics.PerformanceCounter(StatLogger.PERFMON_CATEGORY, StatLogger.COUNTER_REPOTOTAL, string.Empty, false)).RawValue = 0;
                    (new System.Diagnostics.PerformanceCounter(StatLogger.PERFMON_CATEGORY, StatLogger.COUNTER_REPOCREATE, string.Empty, false)).RawValue = 0;
                    (new System.Diagnostics.PerformanceCounter(StatLogger.PERFMON_CATEGORY, StatLogger.COUNTER_REPODELETE, string.Empty, false)).RawValue = 0;
                }
                catch (Exception ex)
                {
                    //Do Nothing
                }

                using (var q = new AcquireReaderLock(_manager.SyncObject))
                {
                    var timer = new Stopwatch();
                    timer.Start();
                    StatLogger.Shutdown();
                    Logger.LogInfo("ShutDown Start: Count=" + _manager.List.Count);
                    foreach (var item in _manager.List)
                    {
                        item.ServiceInstance.ShutDown();
                    }
                    timer.Stop();
                    Logger.LogInfo("ShutDown Complete: Elapsed=" + timer.ElapsedMilliseconds);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                throw;
            }
        }

        public bool ExportSchema(Guid repositoryId, UserCredentials credentials, string backupFile)
        {
            if (!_manager.IsInitialized)
                throw new Exception("System core is loading.");

            if (!IsValidCredentials(credentials))
                throw new Exception("Invalid credentials");

            try
            {
                using (var q = new AcquireReaderLock(_manager.SyncObject))
                {
                    var active = _manager.List.FirstOrDefault(x => x.Repository.ID == repositoryId);
                    if (active == null) return false;
                    active.ServiceInstance.ExportSchema(credentials, backupFile);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                throw;
            }
        }

        public ServerResourceSettings ServerResourceSettings { 
            get { return _resourceSettings; }
        }

        public ServerResourceSettings GetServerResourceSetting(UserCredentials credentials)
        {
            if (!IsValidCredentials(credentials))
                throw new Exception("Invalid credentials");

            using (var q = new AcquireReaderLock(_manager.SyncObject))
            {
                return _resourceSettings;
            }
        }

        public bool SaveServerResourceSetting(UserCredentials credentials, ServerResourceSettings settings)
        {
            if (!IsValidCredentials(credentials))
                throw new Exception("Invalid credentials");

            try
            {
                using (var q = new AcquireWriterLock(_manager.SyncObject, "SaveServerResourceSettings"))
                {
                    _resourceSettings = settings;
                    _resourceSettings.Save();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                throw;
            }
        }

        public ProfileItem[] GetProfile(Guid repositoryId, UserCredentials credentials, long lastProfileId)
        {
            if (!_manager.IsInitialized)
                throw new Exception("System core is loading.");

            if (!IsValidCredentials(credentials))
                throw new Exception("Invalid credentials");

            try
            {
                var active = _manager.List.FirstOrDefault(x => x.Repository.ID == repositoryId);
                if (active == null || active.ServiceInstance == null)
                {
                    return new ProfileItem[] { };
                }
                else if (!active.ServiceInstance.IsLoaded)
                {
                    return new ProfileItem[] { };
                }
                return active.ServiceInstance.GetProfile(credentials, lastProfileId);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                throw;
            }
        }

        public bool AddSystemUser(UserCredentials credentials, UserCredentials user)
        {
            try
            {
                return UserDomain.AddUser(new SystemCredentials() { UserName = user.UserName, Password = user.Password });
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                throw;
            }
        }

        public bool DeleteSystemUser(UserCredentials credentials, UserCredentials user)
        {
            try
            {
                return UserDomain.DeleteUser(new SystemCredentials() { UserName = user.UserName, Password = user.Password });
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                throw;
            }
        }

        public UserCredentials[] GetUserList(UserCredentials credentials)
        {
            if (!_manager.IsInitialized)
                throw new Exception("System core is loading.");

            if (!IsValidCredentials(credentials))
                throw new Exception("Invalid credentials");

            try
            {
                var retval = new List<UserCredentials>();
                UserDomain.UserList.ToList().ForEach(x => retval.Add((UserCredentials)((ICloneable)x).Clone()));
                retval.ForEach(x => x.Password = string.Empty);
                return retval.ToArray();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                throw;
            }
        }

        public void NotifyLoad(Guid repositoryId, int elapsed, int itemsAffected)
        {
            try
            {
                using (var q = new AcquireWriterLock(this.StatLocker, "NotifyLoad"))
                {
                    if (_loadDelta.ContainsKey(repositoryId))
                        _loadDelta[repositoryId]++;
                    else
                        _loadDelta.Add(repositoryId, 1);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                throw;
            }
        }

        public void NotifyUnload(Guid repositoryId, int elapsed, int itemsAffected)
        {
            try
            {
                using (var q = new AcquireWriterLock(this.StatLocker, "NotifyUnload"))
                {
                    if (_unloadDelta.ContainsKey(repositoryId))
                        _unloadDelta[repositoryId]++;
                    else
                        _unloadDelta.Add(repositoryId, 1);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                throw;
            }
        }

        public RealtimeStats[] PerformanceCounters(UserCredentials credentials, DateTime minDate, DateTime maxDate)
        {
            if (!IsValidCredentials(credentials))
                throw new Exception("Invalid credentials");
            
            try
            {
                using (var q = new AcquireReaderLock(this.StatLocker))
                {
                    return StatLogger.QueryServerStats(minDate, maxDate).ToArray();
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                throw;
            }
        }

        public RepositorySummmaryStats GetRepositoryStats(UserCredentials credentials, Guid repositoryId, DateTime minDate, DateTime maxDate)
        {
            if (!_manager.IsInitialized)
                throw new Exception("System core is loading.");

            if (!IsValidCredentials(credentials))
                throw new Exception("Invalid credentials");

            try
            {
                using (var q = new AcquireReaderLock(this.StatLocker))
                {
                    return StatLogger.QueryRepositoryStats(repositoryId, minDate, maxDate);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                throw;
            }
        }

        public SystemStats GetSystemStats()
        {
            if (!_manager.IsInitialized)
                throw new Exception("System core is loading.");

            try
            {
                var info = new PerformanceInformation();
                GetPerformanceInfo(out info, Marshal.SizeOf(info));
                var retval = new SystemStats()
                             {
                                 MachineName = Environment.MachineName,
                                 OSVersion = Environment.OSVersion.ToString(),
                                 ProcessorCount = Environment.ProcessorCount,
                                 TickCount = Environment.TickCount,
                                 TotalMemory = info.PhysicalTotal.ToInt64() * info.PageSize.ToInt64(),
                             };

                var repoList = _manager.List.ToList();
                if (repoList.Count > 0)
                {
                    retval.UsedDisk = repoList.Sum(x => x.DataDiskSize);
                    retval.UsedMemory = repoList.Where(x => x.ServiceInstance.IsLoaded).Sum(x => x.DataMemorySize);
                }

                if (_storage == StorageTypeConstants.Database)
                {
                    retval.UsedDisk = -1;
                }

                retval.InMemoryCount = repoList.Count(x => x.ServiceInstance.IsLoaded);
                retval.RepositoryCount = repoList.Count;
                return retval;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                throw;
            }
        }

        public bool IsInitialized()
        {
            return _manager.IsInitialized;
        }

        #endregion

        void IDisposable.Dispose()
        {
            this.ShutDown();
        }

    }

}