using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Celeriq.AdminSite.Objects
{
    public static class SessionHelper
    {
        public static CeleriqSettings Configuration
        {
            get
            {
                if (HttpContext.Current.Session["Configuration"] == null)
                {
                    HttpContext.Current.Session["Configuration"] = new CeleriqSettings();
                }
                return (CeleriqSettings) HttpContext.Current.Session["Configuration"];
            }
            set { HttpContext.Current.Session["Configuration"] = value; }
        }

        public static bool Login(string servername, string username, string password)
        {
            try
            {
                RepositoryConnection.Reset();

                //Try to login to server. If creds are null then failed
                if (RepositoryConnection.SetCredentials(servername, username, password))
                {
                    var user = new UserAccount()
                    {
                        Server = servername,
                        UserName = username,
                        Password = password,
                    };
                    CurrentUser = user;
                }
                return IsLoggedIn;

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static void Logout()
        {
            HttpContext.Current.Session.Abandon();
            RepositoryConnection.Reset();
        }

        public static bool IsLoggedIn
        {
            get { return (CurrentUser != null); }
        }

        public static UserAccount CurrentUser
        {
            get { return (UserAccount)HttpContext.Current.Session["CurrentUser"]; }
            private set { HttpContext.Current.Session["CurrentUser"] = value; }
        }

    }
}