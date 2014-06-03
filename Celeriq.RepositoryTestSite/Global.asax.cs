using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using Celeriq.Utilities;

namespace Celeriq.RepositoryTestSite
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {

        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs
            var ex = Server.GetLastError();
            if (ex is HttpRequestValidationException)
            {
                Logger.LogError(ex);
                //Response.Redirect("/InvalidInput.aspx");
            }
            else if (ex is HttpException)
            {
                var exception = ex as HttpException;
                if (exception.GetHttpCode() == 404)
                {
                    Logger.LogError(ex);
                    Response.Redirect("/ErrorPage.aspx?error=404");
                }
                else
                {
                    Logger.LogError(ex);
                }
            }
            else if (ex != null)
            {
                Logger.LogError(ex);
            }
        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}