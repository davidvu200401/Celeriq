using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Celeriq.AdminSite.Objects;
using Celeriq.Common;
using Celeriq.Utilities;

namespace Celeriq.AdminSite
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    public class MainService : System.Web.Services.WebService
    {
        [WebMethod(EnableSession = true)]
        public bool Login(string servername, string username, string password)
        {
            try
            {
                return SessionHelper.Login(servername, username, password);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        [WebMethod(EnableSession = true)]
        public void Logout()
        {
            SessionHelper.Logout();
        }

        #region Celeriq
        [WebMethod(EnableSession = true)]
        public string[] GetCeleriqHistory(int hours)
        {
            try
            {
                using (var factory = SystemCoreInteractDomain.GetFactory(SessionHelper.CurrentUser.Server))
                {
                    var server = factory.CreateChannel();
                    var data = server.PerformanceCounters(RepositoryConnection.Credentials, DateTime.Now.AddHours(-hours), DateTime.Now);
                    //data = data.Reverse().ToArray();

                    var retval = new List<string>();
                    int index;
                    List<string> dataProjection;

                    #region CPU

                    index = 0;
                    dataProjection = new List<string>();
                    foreach (var item in data)
                    {
                        dataProjection.Add(item.Timestamp.ToString("HH:mm") + "," + item.ProcessorUsage);
                        index++;
                    }
                    retval.Add(string.Join("|", dataProjection));

                    #endregion

                    #region Total In Memory

                    index = 0;
                    dataProjection = new List<string>();
                    foreach (var item in data)
                    {
                        dataProjection.Add(item.Timestamp.ToString("HH:mm") + "," + item.RepositoryInMem);
                        index++;
                    }
                    retval.Add(string.Join("|", dataProjection));

                    #endregion

                    #region Loaded Delta

                    index = 0;
                    dataProjection = new List<string>();
                    foreach (var item in data)
                    {
                        dataProjection.Add(item.Timestamp.ToString("HH:mm") + "," + (item.RepositoryLoadDelta + item.RepositoryUnloadDelta));
                        index++;
                    }
                    retval.Add(string.Join("|", dataProjection));

                    #endregion

                    #region Create Delta

                    index = 0;
                    dataProjection = new List<string>();
                    foreach (var item in data)
                    {
                        dataProjection.Add(item.Timestamp.ToString("HH:mm") + "," + (item.RepositoryCreateDelta));
                        index++;
                    }
                    retval.Add(string.Join("|", dataProjection));

                    #endregion

                    #region Delete Delta

                    index = 0;
                    dataProjection = new List<string>();
                    foreach (var item in data)
                    {
                        dataProjection.Add(item.Timestamp.ToString("HH:mm") + "," + (item.RepositoryDeleteDelta));
                        index++;
                    }
                    retval.Add(string.Join("|", dataProjection));

                    #endregion

                    #region Memory Usage

                    index = 0;
                    dataProjection = new List<string>();
                    foreach (var item in data)
                    {
                        var memValue = ((item.MemoryUsageTotal - item.MemoryUsageAvailable) / (1024 * 1024));
                        dataProjection.Add(item.Timestamp.ToString("HH:mm") + "," + memValue);
                        index++;
                    }
                    retval.Add(string.Join("|", dataProjection));

                    #endregion

                    return retval.ToArray();
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return new string[0];
            }
        }
        #endregion

    }
}
