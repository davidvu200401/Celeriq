using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;
using Celeriq.Common;

namespace Celeriq.RepositoryTestSite.Objects
{
    internal static class RepositoryHelper
    {
        public static Celeriq.Common.DataQueryResults Query(ListingQuery query, string key)
        {
            try
            {
                query.Credentials = SessionHelper.Credentials;
                query.IPMask = "127.0.0.1";
                using (var factory = RepositoryHelper.GetFactory(SessionHelper.CeleriqServer, key))
                {
                    var channel = factory.CreateChannel();

                    var t = channel.Query(new Guid(key), query.ToTransfer());
                    if (t.ErrorList != null && t.ErrorList.Length > 0)
                    {
                        throw new Exception(t.ErrorList.First());
                    }
                    return t;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        // this is a cache set of non-filtered results
        public static Celeriq.Common.DataQueryResults MasterResults(string key)
        {
            using (var factory = RepositoryHelper.GetFactory(SessionHelper.CeleriqServer, key))
            {
                var channel = factory.CreateChannel();

                var query = new ListingQuery();
                query.Credentials = SessionHelper.Credentials;
                var t = channel.Query(new Guid(key), query.ToTransfer());
                if (t.ErrorList != null && t.ErrorList.Length > 0)
                {
                    throw new Exception(t.ErrorList.First());
                }
                return t;
            }
        }

        private static System.ServiceModel.ChannelFactory<Celeriq.Common.IDataModel> GetFactory(string serverName, string key)
        {
            var myBinding = new NetTcpBinding();
            myBinding.Security.Mode = SecurityMode.None;
            myBinding.MaxBufferPoolSize = 2147483647;
            myBinding.MaxBufferSize = 2147483647;
            myBinding.MaxConnections = 10;
            myBinding.MaxReceivedMessageSize = 2147483647;
            myBinding.ReaderQuotas.MaxDepth = 2147483647;
            myBinding.ReaderQuotas.MaxStringContentLength = 2147483647;
            myBinding.ReaderQuotas.MaxArrayLength = 2147483647;
            myBinding.ReaderQuotas.MaxBytesPerRead = 2147483647;
            myBinding.ReaderQuotas.MaxNameTableCharCount = 2147483647;
            var endpoint = new EndpointAddress("net.tcp://" + serverName + ":1973/" + key);
            return new System.ServiceModel.ChannelFactory<Celeriq.Common.IDataModel>(myBinding, endpoint);
        }

    }
}