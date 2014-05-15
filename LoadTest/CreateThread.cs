using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Celeriq.Common;
using Celeriq.Utilities;

namespace LoadTest
{
    internal class CreateThread : BaseTask
    {
        public CreateThread(string server, UserCredentials credentials, DataSetup data, int threadCount)
            : base(server, credentials, data, threadCount)
        {
        }

        public string RepositoryTemplate { get; set; }

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
                    var options = new ParallelOptions { MaxDegreeOfParallelism = _threadCount };
                    Parallel.For(0, 50, ii =>
                    {
                        CreateRepository(_credentials);
                    });

                    System.Threading.Thread.Sleep(4000);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void CreateRepository(UserCredentials credentials)
        {
            try
            {
                #region Create repository if not exists

                var item = new BaseRemotingObject();
                item.Repository = new Celeriq.Common.RepositorySchema();
                item.Repository.LoadXml(this.RepositoryTemplate);
                item.Repository.ID = Guid.NewGuid();
                item.Repository.Name = "LoadTest-Iterate";
                using (var factory = SystemCoreInteractDomain.GetFactory(this.Server))
                {
                    var server = factory.CreateChannel();
                    Console.WriteLine("[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "] Create Repository: " + item.Repository.Name);
                    var repository = server.SaveRepository(item.Repository, credentials);
                    if (repository != null)
                    {
                        LoadRepository(item.Repository.ID, credentials, server);
                        Logger.LogInfo("Celeriq Success Create");
                    }
                }

                #endregion

                System.Threading.Thread.Sleep(500);

            }
            catch (Exception ex)
            {
                Logger.LogInfo("Celeriq Failure Create [CreateRepository]");
                //throw;
            }
        }

        private void LoadRepository(Guid repositoryId, UserCredentials credentials, ISystemCore core)
        {
            try
            {
                const int LOOPS = 500;
                var list = new List<ListingItem>();
                for (var ii = 0; ii < LOOPS; ii++)
                {
                    var item = new ListingItem();
                    item.ID = ii;
                    item.Dim1 = _data.Dim1Array[_rnd.Next(0, _data.Dim1Array.Length)];
                    item.Dim2 = _data.Dim2Array[_rnd.Next(0, _data.Dim2Array.Length)];
                    item.Dim3 = _data.Dim3Array[_rnd.Next(0, _data.Dim2Array.Length)];
                    item.Dim4 = _data.Dim4Array[_rnd.Next(0, _data.Dim2Array.Length)];
                    item.Field1 = _data.RandomString(50);
                    item.Field2 = _data.RandomString(50);
                    item.Field3 = _data.RandomString(50);
                    item.Field4 = _data.RandomString(50);
                    item.Field5 = _data.RandomString(50);
                    list.Add(item);
                }

                using (var factory = RepositoryConnection.GetFactory(this.Server))
                {
                    var service = factory.CreateChannel();

                    //Validate item format
                    var b = RepositoryConnection.IsValidFormat(repositoryId, list.First(), credentials, service);
                    if (core.RepositoryExists(repositoryId, credentials))
                    {
                        RepositoryConnection.UpdateData(repositoryId, list, credentials, service);
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("The repository has not been initialized"))
                {
                    //Do Nothing
                }
                else
                {
                    Logger.LogInfo("Celeriq Failure Create [LoadRepository]");
                    throw;
                }
            }
        }

    }
}