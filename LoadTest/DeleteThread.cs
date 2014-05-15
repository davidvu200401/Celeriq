using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Celeriq.Common;
using Celeriq.Utilities;

namespace LoadTest
{
    internal class DeleteThread : BaseTask
    {
        public DeleteThread(string server, UserCredentials credentials, DataSetup data, int threadCount)
            : base(server, credentials, data, threadCount)
        {
        }

        public override void Run()
        {
            try
            {
                var isInitialized = false;
                do
                {
                    using (var factory = SystemCoreInteractDomain.GetFactory(this.Server))
                    {
                        var server = factory.CreateChannel();
                        isInitialized = server.IsInitialized();
                        if (!isInitialized)
                        {
                            System.Threading.Thread.Sleep(2000);
                        }
                    }
                } while (!isInitialized && !this.Cancel);

                while (!this.Cancel)
                {
                    List<BaseRemotingObject> list = null;
                    while (list == null)
                    {
                        try
                        {
                            using (var factory = SystemCoreInteractDomain.GetFactory(this.Server))
                            {
                                var server = factory.CreateChannel();
                                var paging = new PagingInfo { PageOffset = 1, RecordsPerPage = 10 };
                                if (_maxPageCount > 0) paging = new PagingInfo { PageOffset = _rnd.Next(1, _maxPageCount), RecordsPerPage = 10 };

                                //Try to load list and if the service is not loaded yet and get error then wait and try again.
                                list = server.GetRepositoryPropertyList(_credentials, paging);
                                var count = server.GetRepositoryCount(_credentials, paging);
                                _maxPageCount = (count / paging.RecordsPerPage) + (count / paging.RecordsPerPage == 0 ? 0 : 1);
                            }
                        }
                        catch (Exception ex)
                        {
                            list = null;
                            System.Threading.Thread.Sleep(5000);
                        }
                    }

                    if (list != null)
                    {
                        var options = new ParallelOptions { MaxDegreeOfParallelism = _threadCount };
                        Parallel.For(0, list.Count, ii =>
                        {
                            DeleteRepository(_credentials, list[ii].Repository.ID);
                            //Console.WriteLine("[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "] Delete Repository: " + list[ii].Repository.ID);
                        });
                        System.Threading.Thread.Sleep(600);
                    }

                }
            }
            catch (Exception ex)
            {
                Logger.LogInfo("Celeriq Failure Delete [Run]");
                throw;
            }
        }

        private void DeleteRepository(UserCredentials credentials, Guid repositoryId)
        {
            try
            {
                using (var factory = SystemCoreInteractDomain.GetFactory(this.Server))
                {
                    var server = factory.CreateChannel();
                    var schema = new Celeriq.Common.RepositorySchema();
                    schema.ID = repositoryId;
                    schema.Name = "LoadTest-Iterate";
                    server.DeleteRepository(schema, credentials);
                    Logger.LogInfo("Celeriq Success Delete");
                }
            }
            catch (Exception ex)
            {
                Logger.LogInfo("Celeriq Failure Delete [DeleteRepository]");
                //throw;
            }
        }

    }
}