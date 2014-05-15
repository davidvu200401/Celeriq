using System;
using System.Linq;
using System.Text;
using System.IO;
using Celeriq.Common;
using Celeriq.DataCore.EFDAL.Entity;
using Celeriq.DataCore.EFDAL;
using Celeriq.Utilities;

namespace Celeriq.RepositoryAPI
{
    public class QueryLogger
    {
        private bool _isReady = false;
        private Guid _repositoryKey;
        private int _repositoryId;
        private Celeriq.Server.Interfaces.StorageTypeConstants _storage = Server.Interfaces.StorageTypeConstants.Database;

        public QueryLogger(Guid repositoryKey, Celeriq.Server.Interfaces.StorageTypeConstants storage)
        {
            _storage = storage;

            if (_storage == Server.Interfaces.StorageTypeConstants.Database)
            {
                using (var context = new DataCoreEntities())
                {
                    var repository = context.RepositoryDefinition.FirstOrDefault(x => x.UniqueKey == repositoryKey);
                    if (repository == null) return;
                    _repositoryId = repository.RepositoryId;
                }
            }
            else if (_storage == Server.Interfaces.StorageTypeConstants.File)
            {
                //TODO
            }
            _repositoryKey = repositoryKey;
            _isReady = true;
        }

        public void Log(DataQuery query, int elapsed, int count, bool fromcache)
        {
            if (!_isReady) return;
            try
            {
                if (_storage == Server.Interfaces.StorageTypeConstants.Database)
                {
                    using (var context = new DataCoreEntities())
                    {
                        var ipAddress = this.CallerAddress;
                        if (!string.IsNullOrEmpty(query.IPMask))
                            ipAddress = query.IPMask;

                        var newItem = new RepositoryLog
                        {
                            IPAddress = ipAddress,
                            RepositoryId = _repositoryId,
                            Count = count,
                            ElapsedTime = elapsed,
                            UsedCache = fromcache,
                            QueryId = new Guid(query.QueryID),
                            Query = query.ToString(),
                        };
                        context.AddItem(newItem);
                        context.SaveChanges();
                    }
                }
                else if (_storage == Server.Interfaces.StorageTypeConstants.File)
                {
                    //TODO
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
        }

        private string CallerAddress
        {
            get
            {
                if (System.ServiceModel.OperationContext.Current != null)
                {
                    if (System.ServiceModel.OperationContext.Current.IncomingMessageProperties != null)
                    {
                        var o = System.ServiceModel.OperationContext.Current.IncomingMessageProperties["System.ServiceModel.Channels.RemoteEndpointMessageProperty"] as System.ServiceModel.Channels.RemoteEndpointMessageProperty;
                        if (o != null)
                        {
                            return o.Address;
                        }
                    }
                }
                return string.Empty;
            }
        }

    }
}