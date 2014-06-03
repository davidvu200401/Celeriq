using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using Celeriq.Utilities;

namespace Celeriq.Common
{
    public static class SystemCoreInteractDomain
    {
        public static ChannelFactory<ISystemCore> GetFactory(string serverName)
        {
            return GetFactory(serverName, 1973);
        }

        public static ChannelFactory<ISystemCore> GetFactory(string serverName, int port)
        {
            var myBinding = new NetTcpBinding() { MaxBufferSize = 10 * 1024 * 1024, MaxReceivedMessageSize = 10 * 1024 * 1024, MaxBufferPoolSize = 10 * 1024 * 1024 };
            myBinding.ReaderQuotas.MaxStringContentLength = 10 * 1024 * 1024;
            myBinding.ReaderQuotas.MaxBytesPerRead = 10 * 1024 * 1024;
            myBinding.ReaderQuotas.MaxArrayLength = 10 * 1024 * 1024;
            myBinding.ReaderQuotas.MaxDepth = 10 * 1024 * 1024;
            myBinding.ReaderQuotas.MaxNameTableCharCount = 10 * 1024 * 1024;
            myBinding.Security.Mode = SecurityMode.None;
            var myEndpoint = new EndpointAddress("net.tcp://" + serverName + ":" + port + "/__celeriq_core");
            return new ChannelFactory<ISystemCore>(myBinding, myEndpoint);
        }

        public static UserCredentials GetCredentials(string serverName, string userName, string password)
        {
            try
            {
                using (var factory = SystemCoreInteractDomain.GetFactory(serverName))
                {
                    var server = factory.CreateChannel();
                    var credentials = new UserCredentials();
                    credentials.UserName = userName;
                    credentials.Password = password;
                    credentials.Password = Celeriq.Utilities.SecurityHelper.Encrypt(server.GetPublicKey(), credentials.Password);
                    return credentials;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return null;
            }
        }

        public static List<BaseRemotingObject> GetRepositoryPropertyList(string server, UserCredentials credentials)
        {
            try
            {
                using (var factory = SystemCoreInteractDomain.GetFactory(server))
                {
                    var s = factory.CreateChannel();

                    var paging = new PagingInfo() { PageOffset = 1, RecordsPerPage = 100 };
                    var retval = new List<BaseRemotingObject>();
                    do
                    {
                        var q = s.GetRepositoryPropertyList(credentials, paging);
                        if (q.Count > 0) retval.AddRange(q);
                        if (q.Count < paging.RecordsPerPage) break;
                        paging.PageOffset++;
                    } while (true);
                    return retval;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                throw;
            }
        }

        public static int GetRepositoryCount(string server, UserCredentials credentials, PagingInfo paging)
        {
            try
            {
                using (var factory = SystemCoreInteractDomain.GetFactory(server))
                {
                    var s = factory.CreateChannel();
                    return s.GetRepositoryCount(credentials, paging);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                throw;
            }
        }

        public static List<BaseRemotingObject> GetRepositoryPropertyList(string server, UserCredentials credentials, PagingInfo paging)
        {
            try
            {
                using (var factory = SystemCoreInteractDomain.GetFactory(server))
                {
                    var s = factory.CreateChannel();
                    var retval = new List<BaseRemotingObject>();
                    retval.AddRange(s.GetRepositoryPropertyList(credentials, paging));
                    return retval;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                throw;
            }
        }

        public static List<UserCredentials> GetUserList(string server, UserCredentials credentials)
        {
            try
            {
                using (var factory = SystemCoreInteractDomain.GetFactory(server))
                {
                    var s = factory.CreateChannel();
                    return s.GetUserList(credentials).ToList();
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                throw;
            }
        }

        public static SystemStats GetSystemStats(string server, UserCredentials credentials)
        {
            try
            {
                using (var factory = SystemCoreInteractDomain.GetFactory(server))
                {
                    var s = factory.CreateChannel();
                    return s.GetSystemStats();
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                throw;
            }
        }

    }
}