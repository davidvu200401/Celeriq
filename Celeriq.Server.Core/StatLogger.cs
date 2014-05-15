using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Celeriq.Common;
using Celeriq.Utilities;
using Celeriq.DataCore.EFDAL;
using Celeriq.DataCore.EFDAL.Entity;
using Celeriq.DataCore.EFDAL.Interfaces;
using System.Data.SQLite;

namespace Celeriq.Server.Core
{
    internal static class StatLogger
    {
        private const int LOG_TIMER_INTERVAL = 30 * 1000;
        private static bool _ready = false;
        private static DateTime _pivot = new DateTime(2000, 1, 1);
        private static Dictionary<Guid, List<RepositorySummmaryStats>> _statCache = new Dictionary<Guid, List<RepositorySummmaryStats>>();
        private static System.Timers.Timer _timer = null;

        public const string PERFMON_CATEGORY = "Celeriq";
        public const string COUNTER_MEMUSAGE = "Memory usage";
        public const string COUNTER_INMEM = "Repositories In Mem";
        public const string COUNTER_LOADDELTA = "Repositories loads/interval";
        public const string COUNTER_UNLOADDELTA = "Repositories unloads/interval";
        public const string COUNTER_REPOTOTAL = "Repositories total";
        public const string COUNTER_REPOCREATE = "Repositories creates/interval";
        public const string COUNTER_REPODELETE = "Repositories deletes/interval";

        private static string ConnectionString
        {
            get
            {
                var fi = new FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);
                var filename = Path.Combine(fi.DirectoryName, "stats.db5");
                return @"Data Source=" + filename;
            }
        }

        public static Celeriq.Server.Interfaces.StorageTypeConstants Storage { get; set; }

        private static Celeriq.Utilities.CeleriqLock _syncObject = null;
        public static Celeriq.Utilities.CeleriqLock SyncObject
        {
            get { return _syncObject; }
            set
            {
                if (_syncObject == null) _syncObject = value;
                else throw new Exception("Object cannot be reset!");
            }
        }

        #region Init

        public static void Initialize()
        {
            try
            {
                SyncObject = new Celeriq.Utilities.CeleriqLock(System.Threading.LockRecursionPolicy.SupportsRecursion);

                if (Storage == Interfaces.StorageTypeConstants.Database)
                {
                    //Do Nothing
                }
                else if (Storage == Interfaces.StorageTypeConstants.File)
                {
                    Logger.LogInfo("StatLogger Init: '" + ConnectionString + "'");
                    #region Server table
                    {
                        var sql = @"CREATE TABLE IF NOT EXISTS [Stats] (
                          [Key] TEXT(32) NOT NULL PRIMARY KEY,
                          [Timestamp] INTEGER(4) NOT NULL,
                          [MemoryUsageTotal] INTEGER(8) NOT NULL,
                          [MemoryUsageAvailable] INTEGER(8) NOT NULL,
                          [MemoryUsageProcess] INTEGER(8) NOT NULL,
                          [RepositoryInMem] INTEGER(2) NOT NULL,
                          [RepositoryLoadDelta] INTEGER(2) NOT NULL,
                          [RepositoryUnloadDelta] INTEGER(2) NOT NULL,
                          [RepositoryTotal] INTEGER(2) NOT NULL,
                          [RepositoryCreateDelta] INTEGER(2) NOT NULL,
                          [RepositoryDeleteDelta] INTEGER(2) NOT NULL,
                          [ProcessorUsage] INTEGER(1) NOT NULL
                          )";
                        RunScript(sql);

                        sql = "CREATE INDEX IF NOT EXISTS IDX_STATS_TIMESTAMP ON [Stats] (Timestamp)";
                        RunScript(sql);
                    }
                    #endregion

                    #region Repository table
                    {
                        var sql = @"CREATE TABLE IF NOT EXISTS [RepositoryStats] (
                          [Key] INTEGER NOT NULL PRIMARY KEY,
                          [RepositoryId] TEXT(32) NOT NULL,
                          [Timestamp] INTEGER(4) NOT NULL,
                          [ActionType] INTEGER NOT NULL,
                          [Elapsed] INTEGER NOT NULL,
                          [Count] INTEGER NOT NULL
                          )";
                        RunScript(sql);

                        sql = "CREATE INDEX IF NOT EXISTS IDX_REPOSITORYSTATS_TIMESTAMP ON [Stats] (Timestamp)";
                        RunScript(sql);
                    }
                    #endregion
                }

                //Log repository stats every N seconds
                _timer = new System.Timers.Timer(LOG_TIMER_INTERVAL);
                _timer.AutoReset = false;
                _timer.Elapsed += TimerTick;
                _timer.Start();

                _ready = true;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
        }

        public static void Shutdown()
        {
            try
            {
                _timer.Stop();
                TimerTick(null, null);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

        #region Server Stats

        public static void Log(RealtimeStats item)
        {
            if (!_ready) return;
            try
            {
                if (Storage == Interfaces.StorageTypeConstants.Database)
                {
                    #region Database
                    using (var context = new DataCoreEntities())
                    {
                        var newItem = new ServerStat()
                                      {
                                          MemoryUsageTotal = item.MemoryUsageTotal,
                                          MemoryUsageAvailable = item.MemoryUsageAvailable,
                                          MemoryUsageProcess = item.MemoryUsageProcess,
                                          RepositoryInMem = item.RepositoryInMem,
                                          RepositoryLoadDelta = item.RepositoryLoadDelta,
                                          RepositoryUnloadDelta = item.RepositoryUnloadDelta,
                                          RepositoryTotal = item.RepositoryTotal,
                                          RepositoryCreateDelta = item.RepositoryCreateDelta,
                                          RepositoryDeleteDelta = item.RepositoryDeleteDelta,
                                          ProcessorUsage = item.ProcessorUsage,
                                      };
                        context.AddItem(newItem);
                        context.SaveChanges();
                    }
                    #endregion
                }
                else if (Storage == Interfaces.StorageTypeConstants.File)
                {
                    #region File
                    var timestamp = (int)DateTime.Now.Subtract(_pivot).TotalSeconds;
                    var sql = "INSERT INTO [Stats] ([Key],[Timestamp],[MemoryUsageTotal],[MemoryUsageAvailable],[MemoryUsageProcess],[RepositoryInMem],[RepositoryLoadDelta],[RepositoryUnloadDelta],[RepositoryTotal],[RepositoryCreateDelta],[RepositoryDeleteDelta],[ProcessorUsage]) Values (" +
                              "'" + Guid.NewGuid() + "'," +
                              timestamp + "," +
                              item.MemoryUsageTotal + "," +
                              item.MemoryUsageAvailable + "," +
                              item.MemoryUsageProcess + "," +
                              item.RepositoryInMem + "," +
                              item.RepositoryLoadDelta + "," +
                              item.RepositoryUnloadDelta + "," +
                              item.RepositoryTotal + "," +
                              item.RepositoryCreateDelta + "," +
                              item.RepositoryDeleteDelta + "," +
                              item.ProcessorUsage +
                              ")";
                    RunScript(sql);
                    Logger.LogInfo("StatLogger Log: MemoryUsageProcess=" + item.MemoryUsageProcess.ToString("###,###,###,##0") + ", RepositoryInMem=" + item.RepositoryInMem.ToString("###,###,###,##0") + ", RepositoryTotal=" + item.RepositoryTotal.ToString("###,###,###,##0") + ", ProcessorUsage=" + item.ProcessorUsage);
                    #endregion
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
        }

        public static List<RealtimeStats> QueryServerStats(DateTime start, DateTime end)
        {
            if (!_ready) return null;

            try
            {
                var retval = new List<RealtimeStats>();

                if (Storage == Interfaces.StorageTypeConstants.Database)
                {
                    #region Database
                    using (var context = new DataCoreEntities())
                    {
                        var list = context.ServerStat
                            .Where(x => start <= x.AddedDate && x.AddedDate < end).OrderBy(x => x.AddedDate)
                            .ToList();
                        foreach (var item in list)
                        {
                            retval.Add(new RealtimeStats
                                       {
                                           Timestamp = item.AddedDate,
                                           MemoryUsageAvailable = item.MemoryUsageAvailable,
                                           MemoryUsageProcess = item.MemoryUsageProcess,
                                           MemoryUsageTotal = item.MemoryUsageTotal,
                                           ProcessorUsage = item.ProcessorUsage,
                                           RepositoryCreateDelta = (int)item.RepositoryCreateDelta,
                                           RepositoryDeleteDelta = (int)item.RepositoryDeleteDelta,
                                           RepositoryInMem = item.RepositoryInMem,
                                           RepositoryLoadDelta = item.RepositoryLoadDelta,
                                           RepositoryTotal = item.RepositoryTotal,
                                           RepositoryUnloadDelta = item.RepositoryUnloadDelta,
                                       });
                        }
                    }
                    #endregion
                }
                else if (Storage == Interfaces.StorageTypeConstants.File)
                {
                    QueryServerStatsSqlLite(start, end, retval);
                }

                return retval;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return null;
            }
        }

        private static void QueryServerStatsSqlLite(DateTime start, DateTime end, List<RealtimeStats> retval)
        {
            using (var connection = new System.Data.SQLite.SQLiteConnection(ConnectionString))
            {
                using (var command = new System.Data.SQLite.SQLiteCommand(connection))
                {
                    var sv = (int)start.Subtract(_pivot).TotalSeconds;
                    var ev = (int)end.Subtract(_pivot).TotalSeconds;

                    connection.Open();
                    command.CommandText = "SELECT * FROM [Stats] WHERE '" + sv + "' <= [Timestamp] AND [Timestamp] <= '" + ev + "' ORDER BY [Timestamp]";
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            retval.Add(new RealtimeStats
                            {
                                Timestamp = _pivot.AddSeconds((long)reader["Timestamp"]),
                                MemoryUsageAvailable = (long)reader["MemoryUsageAvailable"],
                                MemoryUsageProcess = (long)reader["MemoryUsageProcess"],
                                MemoryUsageTotal = (long)reader["MemoryUsageTotal"],
                                ProcessorUsage = (int)(long)reader["ProcessorUsage"],
                                RepositoryCreateDelta = (int)(long)reader["RepositoryCreateDelta"],
                                RepositoryDeleteDelta = (int)(long)reader["RepositoryDeleteDelta"],
                                RepositoryInMem = (int)(long)reader["RepositoryInMem"],
                                RepositoryLoadDelta = (int)(long)reader["RepositoryLoadDelta"],
                                RepositoryTotal = (int)(long)reader["RepositoryTotal"],
                                RepositoryUnloadDelta = (int)(long)reader["RepositoryUnloadDelta"],
                            });
                        }
                    }

                }
            }
        }

        #endregion

        #region Repository Stats

        private static void TimerTick(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (!_ready) return;

            _timer.Stop();
            try
            {
                //Lock the stats list and build queries
                Dictionary<Guid, List<RepositorySummmaryStats>> copyCache = null;
                using (var q = new AcquireWriterLock(SyncObject, "TimerTick"))
                {
                    copyCache = _statCache.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
                    _statCache.Clear();
                }

                if (Storage == Interfaces.StorageTypeConstants.Database)
                {
                    #region Database
                    using (var context = new DataCoreEntities())
                    {
                        var typeValues = Enum.GetValues(typeof(RepositoryActionTypeConstants)).Cast<int>().ToList();
                        foreach (var t in typeValues)
                        {
                            foreach (var key in copyCache.Keys)
                            {
                                var q = (RepositoryActionConstants)t;
                                var queryList = copyCache[key].Where(x => x.ActionType == q).ToList();
                                var elapsed = queryList.Sum(x => x.Elapsed); //Total elapsed time
                                var count = queryList.Count; //Number of queries
                                var itemCount = 0;
                                if (queryList.Count > 0)
                                    itemCount = queryList.Sum(x => x.ItemCount);

                                //Ensure repository still exists (may have been removed in interim)
                                var repository = context.RepositoryDefinition.FirstOrDefault(x => x.UniqueKey == key);
                                if (repository != null && (count > 0 || elapsed > 0 || itemCount > 0))
                                {
                                    var newItem = new RepositoryStat()
                                                  {
                                                      Count = count,
                                                      Elapsed = elapsed,
                                                      ItemCount = itemCount,
                                                      RepositoryActionTypeId = int.Parse(t.ToString("d")),
                                                      RepositoryId = repository.RepositoryId,
                                                  };
                                    context.AddItem(newItem);
                                }
                            }
                        }
                        context.SaveChanges();
                    }
                    #endregion
                }
                else if (Storage == Interfaces.StorageTypeConstants.File)
                {
                    TimerTickSqlLite(copyCache);
                }

            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
            finally
            {
                _timer.Start();
            }
        }

        private static void TimerTickSqlLite(Dictionary<Guid, List<RepositorySummmaryStats>> copyCache)
        {
            try
            {
                using (var connection = new System.Data.SQLite.SQLiteConnection(ConnectionString))
                {
                    connection.Open();
                    using (var mytransaction = connection.BeginTransaction())
                    {
                        using (var mycommand = new SQLiteCommand(connection))
                        {
                            mycommand.CommandText = "INSERT INTO [RepositoryStats] ([Timestamp],[ActionType],[Elapsed],[RepositoryId],[Count]) Values (?,?,?,?,?)";
                            mycommand.Parameters.Add(new SQLiteParameter { DbType = System.Data.DbType.Int64, ParameterName = "Timestamp" });
                            mycommand.Parameters.Add(new SQLiteParameter { DbType = System.Data.DbType.Int64, ParameterName = "ActionType" });
                            mycommand.Parameters.Add(new SQLiteParameter { DbType = System.Data.DbType.Int64, ParameterName = "Elapsed" });
                            mycommand.Parameters.Add(new SQLiteParameter { DbType = System.Data.DbType.String, ParameterName = "RepositoryId" });
                            mycommand.Parameters.Add(new SQLiteParameter { DbType = System.Data.DbType.Int64, ParameterName = "Count" });

                            var sqlList = new List<string>();
                            var typeValues = Enum.GetValues(typeof(RepositoryActionConstants)).Cast<RepositoryActionConstants>().ToList();
                            var timestamp = (int)DateTime.Now.Subtract(_pivot).TotalSeconds;
                            foreach (var t in typeValues)
                            {
                                foreach (var key in copyCache.Keys)
                                {
                                    var queryList = copyCache[key].Where(x => x.ActionType == t).ToList();
                                    var elapsed = queryList.Sum(x => x.Elapsed); //Total elapsed time
                                    var count = queryList.Count; //Number of queries

                                    //Do not insert zeros
                                    if (count > 0 && elapsed > 0)
                                    {
                                        mycommand.Parameters[0].Value = timestamp;
                                        mycommand.Parameters[1].Value = int.Parse(t.ToString("d"));
                                        mycommand.Parameters[2].Value = elapsed;
                                        mycommand.Parameters[3].Value = key.ToString().ToLower();
                                        mycommand.Parameters[4].Value = count;
                                        mycommand.ExecuteNonQuery();
                                    }
                                }
                            }

                        }
                        mytransaction.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
        }

        public static void Log(RepositorySummmaryStats item)
        {
            try
            {
                using (var q = new AcquireWriterLock(SyncObject, "Log"))
                {
                    if (!_statCache.ContainsKey(item.RepositoryId))
                        _statCache.Add(item.RepositoryId, new List<RepositorySummmaryStats>());
                    _statCache[item.RepositoryId].Add(item);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
        }

        public static RepositorySummmaryStats QueryRepositoryStats(Guid repositoryId, DateTime start, DateTime end)
        {
            if (!_ready) return null;

            try
            {
                var retval = new RepositorySummmaryStats() { ActionType = RepositoryActionConstants.Query };

                if (Storage == Interfaces.StorageTypeConstants.Database)
                {
                    #region Database
                    using (var context = new DataCoreEntities())
                    {
                        var repository = context.RepositoryDefinition.FirstOrDefault(x => x.UniqueKey == repositoryId);
                        if (repository == null)
                            throw new Exception("Unknown Repository");

                        var actionId = (int)RepositoryActionConstants.Query;
                        var lambda = context.RepositoryStat.Where(x => start <= x.CreatedDate &&
                            x.CreatedDate < end &&
                            (repositoryId == Guid.Empty || repository.RepositoryId == x.RepositoryId) &&
                            x.RepositoryActionTypeId == actionId)
                            .OrderBy(x => x.CreatedDate);

                        long count = 0;
                        long totalElapsed = 0;
                        if (lambda.Any())
                        {
                            count = lambda.Sum(x => x.Count);
                            totalElapsed = lambda.Sum(x => x.Elapsed);
                        }

                        var elapsedPer = 0.0;
                        if (count > 0) elapsedPer = ((totalElapsed * 1.0) / count);

                        retval.ItemCount = (int)count;
                        retval.Elapsed = (int)elapsedPer;
                        retval.RepositoryId = repositoryId;
                    }
                    #endregion
                }
                else if (Storage == Interfaces.StorageTypeConstants.File)
                {
                    QueryRepositoryStatsSqlLite(repositoryId, start, end, retval);
                }

                return retval;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return null;
            }
        }

        private static void QueryRepositoryStatsSqlLite(Guid repositoryId, DateTime start, DateTime end, RepositorySummmaryStats retval)
        {
            try
            {
                using (var connection = new System.Data.SQLite.SQLiteConnection(ConnectionString))
                {
                    using (var command = new System.Data.SQLite.SQLiteCommand(connection))
                    {
                        var sv = (int)start.Subtract(_pivot).TotalSeconds;
                        var ev = (int)end.Subtract(_pivot).TotalSeconds;

                        var whereTime = string.Empty;
                        if (start == DateTime.MinValue) whereTime += "(1=1) AND ";
                        else whereTime += "'" + sv + "' <= [Timestamp] AND ";
                        if (end == DateTime.MaxValue) whereTime += "(1=1)";
                        else whereTime += "[Timestamp] <= '" + ev + "'";

                        connection.Open();
                        if (repositoryId == Guid.Empty)
                            command.CommandText = "SELECT * FROM [RepositoryStats] WHERE " + whereTime + " AND ActionType = " + (int)RepositoryActionConstants.Query + " ORDER BY [Timestamp]";
                        else
                            command.CommandText = "SELECT * FROM [RepositoryStats] WHERE [RepositoryId] = '" + repositoryId.ToString().ToLower() + "' AND " + whereTime + " AND ActionType = " + (int)RepositoryActionConstants.Query + " ORDER BY [Timestamp]";

                        long count = 0;
                        long totalElapsed = 0;
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                count += (long)reader["Count"];
                                totalElapsed += (long)reader["Elapsed"];
                            }
                        }

                        var elapsedPer = 0.0;
                        if (count > 0) elapsedPer = ((totalElapsed * 1.0) / count);

                        //_pivot.AddSeconds((long)reader["Timestamp"]),
                        //(RepositoryActionConstants)(long)reader["ActionType"]
                        retval.ItemCount = (int)count;
                        retval.Elapsed = (int)elapsedPer;
                        retval.RepositoryId = repositoryId;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                throw;
            }
        }

        private static void RunScript(string sql)
        {
            try
            {
                using (var connection = new System.Data.SQLite.SQLiteConnection(ConnectionString))
                {
                    using (var command = new System.Data.SQLite.SQLiteCommand(connection))
                    {
                        connection.Open();
                        command.CommandText = sql;
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ec)
            {
                throw;
            }
        }

        #endregion
    }
}
