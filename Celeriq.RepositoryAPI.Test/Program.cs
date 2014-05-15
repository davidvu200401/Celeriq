using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Celeriq.Common;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using Celeriq.Utilities;
using System.Configuration;

namespace Celeriq.RepositoryAPI.Test
{
    internal class Program
    {
        private const string MACHINE = "localhost";
        private const string REPOID = "22999192-A01F-4E48-8922-50AD48332736";

        private static void Main(string[] args)
        {
            var credentials = GetCredentials();

            //CreateManyRepository(credentials);
            //IterateRepository(credentials);
            //CreateAndLoadItems(GetCredentials());
            //ClearRepository(credentials);
            //LoadBigRepository(credentials);
            //TestBigRepository(credentials);

            DeleteRepository(credentials);
            //CreateAndLoadItems(credentials);
            //QueryRepository(credentials);
            //ClearRepository(credentials);
            //CreateAndLoadItems(credentials);
            //DeleteRepository(credentials);
            //TestCore();

            //var f = File.ReadAllText(@"c:\temp\war-peace.txt");
            ////var f = File.ReadAllText(@"c:\temp\sampletext.txt");
            ////var f = "I was trying to do something cool here for you to see me work.";
            //var timer = new Stopwatch();
            //timer.Start();
            //var s = Celeriq.Server.Interfaces.ServerUtilities.CreateWordBlob(f);
            //timer.Stop();

            //LoadAllInMem();

            //QueryRepository(credentials);
            //System.Threading.Thread.Sleep(20000);
            GetStats(credentials);


            Console.WriteLine("Press any key...");
            Console.ReadLine();

        }

        private static UserCredentials _credentials = null;
        private static UserCredentials GetCredentials()
        {
            if (_credentials == null)
            {
                _credentials = new UserCredentials();
                using (var factory = SystemCoreInteractDomain.GetFactory(MACHINE))
                {
                    var server = factory.CreateChannel();
                    _credentials.UserName = "root";
                    _credentials.Password = "password";
                    _credentials.Password = Celeriq.Utilities.SecurityHelper.Encrypt(server.GetPublicKey(), _credentials.Password);
                }
            }
            return _credentials;
        }

        private static void TestCore()
        {
            var storage = Celeriq.Server.Interfaces.StorageTypeConstants.File;
            #region Setup
            var setup = new Server.Interfaces.ConfigurationSetup() { IsDebug = true };
            if (storage == Celeriq.Server.Interfaces.StorageTypeConstants.File)
            {
                //setup.AllowCaching = Convert.ToBoolean(ConfigurationManager.AppSettings["AllowCaching"]);
                setup.AutoDataUnloadTime = Convert.ToInt32(ConfigurationManager.AppSettings["AutoDataUnloadTime"]);
                setup.MaxMemory = Convert.ToInt32(ConfigurationManager.AppSettings["MaxMemory"]);
                setup.MaxRunningRepositories = Convert.ToInt32(ConfigurationManager.AppSettings["MaxRunningRepositories"]);
                setup.PrivateKey = ConfigurationManager.AppSettings["PrivateKey"];
                setup.PublicKey = ConfigurationManager.AppSettings["PublicKey"];
                setup.DataPath = ConfigurationManager.AppSettings["DataPath"];
            }
            #endregion
            var core = new Celeriq.Server.Core.SystemCore(storage, setup) as ISystemCore;

            var credentials = new UserCredentials();
            credentials.UserName = "root";
            credentials.Password = "password";
            credentials.Password = Celeriq.Utilities.SecurityHelper.Encrypt(core.GetPublicKey(), credentials.Password);

            var key = Guid.NewGuid();

            //Create repository
            if (!core.RepositoryExists(key, credentials))
            {
                var item = new BaseRemotingObject();
                item.Repository = new Celeriq.Common.RepositorySchema();
                item.Repository.LoadXml(GetRepositoryTemplate());
                item.Repository.ID = key;
                item.Repository.Name = "CoreTest";
                core.SaveRepository(item.Repository, credentials);
            }

            //Delete repository
            if (core.RepositoryExists(key, credentials))
            {
                var item = new BaseRemotingObject();
                item.Repository = new Celeriq.Common.RepositorySchema();
                item.Repository.LoadXml(GetRepositoryTemplate());
                item.Repository.ID = key;
                item.Repository.Name = "LoadTest-Iterate";
                core.DeleteRepository(item.Repository, credentials);
            }

            Console.WriteLine("Repository exists: " + core.RepositoryExists(key, credentials));

        }

        private static void LoadAllInMem()
        {
            using (var factory = SystemCoreInteractDomain.GetFactory(MACHINE))
            {
                var server = factory.CreateChannel();
                var useSingle = false;
                var credentials = GetCredentials();

                //Single
                if (useSingle)
                {
                    var r = server.GetRepositoryPropertyList(credentials, new PagingInfo() { PageOffset = 1, RecordsPerPage = 200 }).First();
                    LoadRepoInMem(r.Repository.ID, credentials);
                }
                else
                {
                    //Multi
                    var page = 1;
                    var wasFound = false;
                    do
                    {
                        var pageInfo = new PagingInfo { PageOffset = page, RecordsPerPage = 500 };
                        var repositoryList = server.GetRepositoryPropertyList(credentials, pageInfo);
                        wasFound = (repositoryList.Count > 0);
                        var options = new ParallelOptions { MaxDegreeOfParallelism = 4 };
                        Parallel.ForEach(repositoryList, options, item =>
                        {
                            LoadRepoInMem(item.Repository.ID, credentials);
                        });
                        Console.WriteLine("Processed Page " + page);
                        page++;
                    } while (wasFound);
                }
            }
        }

        private static void LoadRepoInMem(Guid repositoryId, UserCredentials credentials)
        {
            using (var factory = RepositoryConnection.GetFactory(MACHINE))
            {
                var service = factory.CreateChannel();
                service.Query(repositoryId, new DataQuery() { Credentials = credentials });
                Console.WriteLine("Processed Repository " + repositoryId);
            }
        }

        private static void QueryRepository(UserCredentials credentials)
        {
            var item = new Threader(credentials);
            item.Run();
        }

        private static void GetStats(UserCredentials credentials)
        {
            //System.Threading.Thread.Sleep(10000);
            using (var factory = SystemCoreInteractDomain.GetFactory(MACHINE))
            {
                var server = factory.CreateChannel();
                var q = server.GetRepositoryStats(credentials, new Guid(REPOID), DateTime.MinValue, DateTime.MaxValue);
                Console.WriteLine("State Count: " + q.ItemCount);
            }
        }

        private static void ClearRepository(UserCredentials credentials)
        {
            Guid repositoryId = Guid.Empty;
            using (var factory = SystemCoreInteractDomain.GetFactory(MACHINE))
            {
                var server = factory.CreateChannel();
                var repositoryList = server.GetRepositoryPropertyList(credentials, new PagingInfo { PageOffset = 1, RecordsPerPage = 1 });
                if (repositoryList.Count > 0)
                {
                    repositoryId = repositoryList.First().Repository.ID;
                }
            }

            if (repositoryId != Guid.Empty)
            {
                using (var factory = RepositoryConnection.GetFactory(MACHINE))
                {
                    var service = factory.CreateChannel();
                    RepositoryConnection.Clear(repositoryId, service, credentials);
                }
            }
        }

        private static void DeleteRepository(UserCredentials credentials)
        {
            using (var factory = SystemCoreInteractDomain.GetFactory(MACHINE))
            {
                var server = factory.CreateChannel();

                var schema = new Celeriq.Common.RepositorySchema();
                schema.LoadXml(GetRepositoryTemplate());
                schema.ID = new Guid(REPOID);
                schema.Name = "LoadTest-Iterate";

                server.DeleteRepository(schema, credentials);
            }
        }

        private static void CreateAndLoadItems(UserCredentials credentials)
        {
            //Load Test
            #region Create repository if not exists

            var item = new BaseRemotingObject();
            item.Repository = new Celeriq.Common.RepositorySchema();
            item.Repository.LoadXml(GetRepositoryTemplate());
            item.Repository.ID = new Guid(REPOID);
            item.Repository.Name = "LoadTest-Iterate";
            using (var factory = SystemCoreInteractDomain.GetFactory(MACHINE))
            {
                var server = factory.CreateChannel();
                if (!server.RepositoryExists(item.Repository.ID, credentials))
                {
                    Console.WriteLine("Create Repository: " + item.Repository.Name);
                    server.SaveRepository(item.Repository, credentials);
                }
                else
                {
                    Console.WriteLine("Repository Exists: " + item.Repository.Name);
                    //server.DataLoadRepository(item.Repository.ID, credentials);
                }

                //Now load data multiple times
                const int LoadIterations = 1; //1000
                for (var ii = 0; ii < LoadIterations; ii++)
                {
                    LoadRepository(item.Repository.ID, credentials);
                }
                //server.Compress(item.Repository.ID, credentials);
            }

            #endregion

        }

        private static void IterateRepository(UserCredentials credentials)
        {
            //Load Test
            #region Create repository if not exists

            var item = new BaseRemotingObject();
            item.Repository = new Celeriq.Common.RepositorySchema();
            item.Repository.LoadXml(GetRepositoryTemplate());
            item.Repository.ID = new Guid(REPOID);
            item.Repository.Name = "LoadTest-Iterate";
            using (var factory = SystemCoreInteractDomain.GetFactory(MACHINE))
            {
                var server = factory.CreateChannel();
                if (!server.RepositoryExists(item.Repository.ID, credentials))
                {
                    Console.WriteLine("Create Repository: " + item.Repository.Name);
                    server.SaveRepository(item.Repository, credentials);
                }
                else
                {
                    Console.WriteLine("Repository Exists: " + item.Repository.Name);
                    server.DataLoadRepository(item.Repository.ID, credentials);
                }

                //Now load data multiple times
                const int LoadIterations = 100;
                for (var ii = 0; ii < LoadIterations; ii++)
                {
                    LoadRepository(item.Repository.ID, credentials);
                }

                QueryRepository(credentials);

                //server.Compress(item.Repository.ID, credentials);
            }

            #endregion
        }

        private static void CreateManyRepository(UserCredentials credentials)
        {
            //Load Test
            const int TOTALREPOSITORY = 50;
            var baseName = "LoadTest-";
            for (var ii = 0; ii < TOTALREPOSITORY; ii++)
            {
                var repositoryName = "LoadTest-" + ii.ToString("000");

                #region Create repository if not exists

                var item = new BaseRemotingObject();
                item.Repository = new Celeriq.Common.RepositorySchema();
                item.Repository.LoadXml(GetRepositoryTemplate());
                item.Repository.ID = new Guid("F3999192-A01F-4E48-8922-50AD48" + ii.ToString("000000"));
                item.Repository.Name = repositoryName;
                using (var factory = SystemCoreInteractDomain.GetFactory(MACHINE))
                {
                    var server = factory.CreateChannel();
                    if (!server.RepositoryExists(item.Repository.ID, credentials))
                    {
                        Console.WriteLine("Create Repository: " + item.Repository.Name);
                        server.SaveRepository(item.Repository, credentials);
                        //server.StartRepository(new Guid(RepositoryConnection.WCFKey), credentials);
                        LoadRepository(item.Repository.ID, credentials);
                        Console.WriteLine("Loaded Repository: " + item.Repository.Name);
                    }
                    else
                    {
                        Console.WriteLine("Repository Exists: " + item.Repository.Name);
                        //server.StartRepository(new Guid(RepositoryConnection.WCFKey), credentials);
                    }
                    server.DataLoadRepository(item.Repository.ID, credentials);
                }

                #endregion
            }
        }

        private static string GetRepositoryTemplate()
        {
            var a = Assembly.GetExecutingAssembly();
            var st = a.GetManifestResourceStream("Celeriq.RepositoryAPI.Test.LoadTest.celeriq");
            var sr = new StreamReader(st);
            return sr.ReadToEnd();
        }

        private static string[] _Dim1Array = { "AAA", "BBB", "CCC", "DDD", "EEE", "FFF" };
        private static string[] _Dim2Array = { "ZZZ", "XXX", "YYY", "WWW" };

        private static void LoadRepository(Guid repositoryId, UserCredentials credentials)
        {
            const int LOOPS = 100;
            var rnd = new Random(0);
            var list = new List<ListingItem>();
            for (var ii = 0; ii < LOOPS; ii++)
            {
                var item = new ListingItem();
                item.ID = ii;
                item.Dim1 = _Dim1Array[rnd.Next(0, _Dim1Array.Length)];
                item.Dim2 = _Dim2Array[rnd.Next(0, _Dim2Array.Length)];
                list.Add(item);
            }

            using (var factory = RepositoryConnection.GetFactory(MACHINE))
            {
                var service = factory.CreateChannel();

                //Validate item format
                var b = RepositoryConnection.IsValidFormat(repositoryId, list.First(), credentials, service);
                
                RepositoryConnection.UpdateData(repositoryId, list, credentials, service);
            }

        }

        private static void LoadBigRepository(UserCredentials credentials)
        {
            const int VALUES = 500;
            const int LOOPS = 10000;
            var rnd = new Random(0);

            #region Create
            var item = new BaseRemotingObject();
            item.Repository = new Celeriq.Common.RepositorySchema();
            item.Repository.LoadXml(GetRepositoryTemplate());
            item.Repository.ID = new Guid(REPOID);
            item.Repository.Name = "LoadTest-Iterate";
            using (var factory = SystemCoreInteractDomain.GetFactory(MACHINE))
            {
                var server = factory.CreateChannel();
                if (!server.RepositoryExists(item.Repository.ID, credentials))
                {
                    Console.WriteLine("Create Repository: " + item.Repository.Name);
                    server.SaveRepository(item.Repository, credentials);
                }
            }
            #endregion

            var valueList = new List<string>();
            for (var ii = 0; ii < VALUES; ii++)
            {
                valueList.Add(RandomString(8, rnd));
            }

            using (var factory = RepositoryConnection.GetFactory(MACHINE))
            {
                var service = factory.CreateChannel();
                var list = new List<ListingItem>();
                for (var ii = 0; ii < LOOPS; ii++)
                {
                    var newRepoItem = new ListingItem();
                    newRepoItem.ID = ii;
                    newRepoItem.Dim1 = valueList[rnd.Next(0, valueList.Count)];
                    newRepoItem.Dim2 = valueList[rnd.Next(0, valueList.Count)];
                    newRepoItem.Dim3 = valueList[rnd.Next(0, valueList.Count)];
                    newRepoItem.Dim4 = valueList[rnd.Next(0, valueList.Count)];
                    newRepoItem.Dim5 = valueList[rnd.Next(0, valueList.Count)];
                    list.Add(newRepoItem);

                    if (list.Count % 50 == 0)
                    {
                        RepositoryConnection.UpdateData(item.Repository.ID, list, credentials, service);
                        list = new List<ListingItem>();
                    }
                }

                RepositoryConnection.UpdateData(item.Repository.ID, list, credentials, service);
            }

        }

        private static void TestBigRepository(UserCredentials credentials)
        {
            var repositoryId = REPOID;
            using (var factory = RepositoryConnection.GetFactory(MACHINE))
            {
                var service = factory.CreateChannel();
                var masterResults = service.Query(new Guid(repositoryId), new DataQuery() { Credentials = credentials });
            }

            const int WORKERS = 10;
            var workers = new List<Threader>();
            for (var ii = 0; ii < WORKERS; ii++)
            {
                workers.Add(new Threader(credentials));
            }

            var timer = new Stopwatch();
            timer.Start();

            //workers.First().Run();
            //var options = new ParallelOptions { MaxDegreeOfParallelism = 2 };
            Parallel.ForEach(workers, x =>
            {
                x.Run();
            });

            timer.Stop();
            var total = workers.Sum(x => x.Count);
            System.Diagnostics.Debug.WriteLine((timer.ElapsedMilliseconds * 1.0) / total + "ms");
            var s = timer.ElapsedMilliseconds;

        }

        private static string RandomString(int size, Random rnd)
        {
            var builder = new StringBuilder();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * rnd.NextDouble() + 65)));
                builder.Append(ch);
            }

            return builder.ToString();
        }

        public class Threader
        {
            private UserCredentials _credentials = null;
            public Threader(UserCredentials credentials)
            {
                _credentials = credentials;
            }

            public int Count { get; private set; }

            public void Run()
            {
                var repositoryId = REPOID;
                var rnd = new Random(0);
                using (var factory = RepositoryConnection.GetFactory(MACHINE))
                {
                    var service = factory.CreateChannel();
                    var masterResults = service.Query(new Guid(repositoryId), new DataQuery() { Credentials = _credentials });

                    //var timer = new Stopwatch();
                    //timer.Start();
                    foreach (var d in masterResults.DimensionList)
                    //var d = masterResults.DimensionList.First();
                    {
                        foreach (var r in d.RefinementList)
                        {
                            var query = new DataQuery() { Credentials = _credentials };
                            query.DimensionValueList = new List<long>();
                            query.DimensionValueList.Add(r.DVIdx);
                            var results = service.Query(new Guid(repositoryId), query);
                            this.Count++;
                        }
                    }
                    //timer.Stop();
                    //var s = timer.ElapsedMilliseconds;
                }

            }
        }

    }
}