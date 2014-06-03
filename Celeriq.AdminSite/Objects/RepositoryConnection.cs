using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Celeriq.Common;

namespace Celeriq.AdminSite.Objects
{
    partial class RepositoryConnection
    {
        private static UserCredentials _credentials = null;
        private static object _locker = new object();

        public static bool SetCredentials(string servername, string username, string password)
        {
            lock (_locker)
            {
                if (_credentials == null)
                {
                    using (var factory = SystemCoreInteractDomain.GetFactory(servername))
                    {
                        var server = factory.CreateChannel();
                        string publicKey;
                        try
                        {
                            publicKey = server.GetPublicKey();
                        }
                        catch (Exception ex)
                        {
                            return false;
                        }

                        _credentials = new UserCredentials();
                        _credentials.UserName = username;
                        _credentials.Password = password;
                        _credentials.Password = Celeriq.Utilities.SecurityHelper.Encrypt(publicKey, password);
                    }
                }
            }
            return (_credentials != null);
        }

        public static UserCredentials Credentials
        {
            get
            {
                lock (_locker)
                {
                    if (_credentials == null)
                    {
                        SetCredentials(SessionHelper.CurrentUser.Server, SessionHelper.CurrentUser.UserName, SessionHelper.CurrentUser.Password);
                    }
                }
                return _credentials;
            }
        }

        public static void Reset()
        {
            _credentials = null;
        }

        public static List<BaseRemotingObject> GetRepositoryPropertyList(PagingInfo paging, out int count)
        {
            try
            {
                using (var factory = SystemCoreInteractDomain.GetFactory(SessionHelper.CurrentUser.Server))
                {
                    var s = factory.CreateChannel();
                    count = s.GetRepositoryCount(Credentials, paging);
                    return s.GetRepositoryPropertyList(Credentials, paging);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}