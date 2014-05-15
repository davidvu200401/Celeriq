using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using Celeriq.Common;
using Celeriq.Utilities;

namespace Celeriq.RepositoryTestSite.Objects
{
    internal static class SessionHelper
    {
        private static object _locker = new object();

        public static bool IsConnected
        {
            get { return (SessionHelper.Credentials != null); }
        }

        public static string CeleriqServer
        {
            get
            {
                if (HttpContext.Current.Session["CeleriqServer"] == null) return string.Empty;
                return (string)HttpContext.Current.Session["CeleriqServer"];
            }
            private set { HttpContext.Current.Session["CeleriqServer"] = value; }
        }

        public static string RepositoryKey
        {
            get
            {
                if (HttpContext.Current.Session["RepositoryKey"] == null) return string.Empty;
                return (string)HttpContext.Current.Session["RepositoryKey"];
            }
            private set { HttpContext.Current.Session["RepositoryKey"] = value; }
        }

        public static UserCredentials Credentials
        {
            get
            {
                lock (_locker)
                {
                    return (UserCredentials)HttpContext.Current.Session["Credentials"];
                }
            }
            private set
            {
                lock (_locker)
                {
                    HttpContext.Current.Session["Credentials"] = value;
                }
            }
        }

        public static bool Login(string server, string user, string password)
        {
            using (var factory = SystemCoreInteractDomain.GetFactory(server))
            {
                var channel = factory.CreateChannel();
                string publicKey;
                try
                {
                    publicKey = channel.GetPublicKey();
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex);
                    return false;
                }

                CeleriqServer = server;

                var credentials = new UserCredentials();
                credentials.UserName = user;
                credentials.Password = password;
                credentials.Password = Celeriq.Utilities.SecurityHelper.Encrypt(publicKey, credentials.Password);
                SessionHelper.Credentials = credentials;
                System.Web.Security.FormsAuthentication.SetAuthCookie(MembershipRoleProvider.ROLE_STANDARD, false);
                return true;
            }
        }

        public static void Logout()
        {
            FormsAuthentication.SignOut();
            HttpContext.Current.Session.Abandon();
            HttpContext.Current.Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", String.Empty));
        }

    }
}