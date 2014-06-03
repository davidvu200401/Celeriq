using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Celeriq.Common;
using Celeriq.Utilities;

namespace LoadTest
{
    internal class SingleChangeThread : BaseTask
    {
        public SingleChangeThread(string server, UserCredentials credentials, DataSetup data, int threadCount)
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
                    if (_data.PredefinedLoad.Count == 0)
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
                                Upsert(list[ii].Repository.ID.ToString());
                            });
                            System.Threading.Thread.Sleep(4000);
                        }
                    }
                    else //Predefined list
                    {
                        var options = new ParallelOptions { MaxDegreeOfParallelism = _threadCount };
                        Parallel.For(0, _data.PredefinedLoad.Count, ii =>
                        {
                            Upsert(_data.PredefinedLoad[ii].ToString());
                        });
                        System.Threading.Thread.Sleep(4000);
                    }

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void Upsert(string repositoryId)
        {
            try
            {
                RepositoryConnection.GetFactory(this.Server).Using(factory =>
                {
                    var service = factory.CreateChannel();
                    var masterResults = service.Query(new Guid(repositoryId), new DataQuery() { Credentials = _credentials });
                    Logger.LogInfo("Celeriq Success Query");

                    if (masterResults == null || masterResults.DimensionList == null)
                    {
                        Console.WriteLine("[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "] Query Error");
                    }
                    else
                    {
                        foreach (var d in masterResults.DimensionList)
                        {
                            foreach (var r in d.RefinementList)
                            {
                                var query = new DataQuery() { Credentials = _credentials };
                                query.DimensionValueList = new List<long>();
                                query.DimensionValueList.Add(r.DVIdx);
                                var results = service.Query(new Guid(repositoryId), query);
                                Logger.LogInfo("Celeriq Success Query");

                                //Now get an arbitrary page
                                var pageCount = (results.TotalRecordCount / results.Query.RecordsPerPage);
                                var q = new DataQuery { DimensionValueList = new List<long>(), Credentials = _credentials, PageOffset = _rnd.Next(1, pageCount + 1), RecordsPerPage = results.Query.RecordsPerPage };
                                var results2 = service.Query(new Guid(repositoryId), q);
                                Logger.LogInfo("Celeriq Success Query");

                                for (var ii = 0; ii < results2.RecordList.Count; ii++)
                                {
                                    var timer = new Stopwatch();
                                    timer.Start();
                                    service.UpdateData(new Guid(repositoryId), new DataItem[] { results2.RecordList[ii] }, _credentials);
                                    timer.Stop();
                                    Logger.LogInfo("Celeriq Success Single Update [" + timer.ElapsedMilliseconds + "]");
                                    System.Threading.Thread.Sleep(50);

                                    //Console.WriteLine("[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "] Upsert: repositoryId=" + repositoryId + ", time=" + timer.ElapsedMilliseconds);

                                    if (this.Cancel) return;
                                }
                            }
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                Logger.LogInfo("Celeriq Failure Single Update");
                //throw;
            }
        }

    }
}