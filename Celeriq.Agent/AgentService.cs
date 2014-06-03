using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Celeriq.Common;
using Celeriq.Server.Interfaces;
using Celeriq.Utilities;

namespace Celeriq.Agent
{
    public partial class AgentService : ServiceBase
    {
        private static string MachineName = "";
        private static UserCredentials _credentials = null;
        private System.Timers.Timer _timer = null;
        private int _serviceDownCount = 0;
        private int _failureCount = 0;

        public AgentService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            this.Start();
        }

        protected override void OnStop()
        {
            Cleanup();
        }

        public void Cleanup()
        {
            if (_timer != null)
            {
                _timer.Stop();
                _timer = null;
            }

            #region Email
            EmailDomain.SendMail(new EmailSettings
            {
                Body = "Celeriq Agent Stopped [" + Environment.MachineName + "]",
                From = ConfigHelper.FromEmail,
                Subject = "Celeriq Agent Stopped [" + Environment.MachineName + "]",
                To = ConfigHelper.NotifyEmail,
            });
            #endregion

        }

        public void Start()
        {
            Logger.LogDebug("Services Started Begin");
            try
            {
                //Setup configuration
                Celeriq.Server.Interfaces.ConfigHelper.Storage = StorageTypeConstants.File;
                Celeriq.Server.Interfaces.ConfigHelper.MailServer = ConfigurationManager.AppSettings["MailServer"];
                int mailPort;
                if (!int.TryParse(ConfigurationManager.AppSettings["MailServerPort"], out mailPort)) mailPort = 25;
                Celeriq.Server.Interfaces.ConfigHelper.MailServerPort = mailPort;
                Celeriq.Server.Interfaces.ConfigHelper.MailServerUsername = ConfigurationManager.AppSettings["MailServerUsername"];
                Celeriq.Server.Interfaces.ConfigHelper.MailServerPassword = ConfigurationManager.AppSettings["MailServerPassword"];
                Celeriq.Server.Interfaces.ConfigHelper.NotifyEmail = ConfigurationManager.AppSettings["NotifyEmail"];
                Celeriq.Server.Interfaces.ConfigHelper.FromEmail = ConfigurationManager.AppSettings["FromEmail"];
                MachineName = ConfigurationManager.AppSettings["MonitorServer"];

                int port;
                if (!int.TryParse(ConfigurationManager.AppSettings["Port"], out port)) port = 1973;
                Celeriq.Server.Interfaces.ConfigHelper.Port = port;

                _timer = new System.Timers.Timer(60000);
                _timer.Elapsed += _timer_Elapsed;
                _timer.Start();

                #region Email
                EmailDomain.SendMail(new EmailSettings
                {
                    Body = "Celeriq Agent Started [" + Environment.MachineName + "]",
                    From = ConfigHelper.FromEmail,
                    Subject = "Celeriq Agent Started [" + Environment.MachineName + "]",
                    To = ConfigHelper.NotifyEmail,
                });
                #endregion

            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                throw;
            }
        }

        private void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (_timer != null) _timer.Stop();
            try
            {
                _credentials = GetCredentials();
                if (_credentials == null)
                {
                    SendNotification();
                }
                else
                {
                    CheckService();
                    if (_failureCount > 2)
                    {
                        SendNotification();
                    }
                }
            }
            catch(Exception ex)
            {
                Logger.LogError(ex);
                throw;
            }
            finally
            {
                if (_timer != null) _timer.Start();
            }
        }

        private void SendNotification()
        {
            try
            {
                //Logger.LogInfo("Restarting Celeriq Service");

                //{
                //    //Stop Service
                //    var process = new System.Diagnostics.Process();
                //    process.StartInfo.FileName = "net";
                //    process.StartInfo.Arguments = "stop \"Celeriq Core Services\"";
                //    process.StartInfo.UseShellExecute = false;
                //    process.StartInfo.CreateNoWindow = false;
                //    process.Start();
                //    process.WaitForExit();
                //}

                //{
                //    //Start Service
                //    var process = new System.Diagnostics.Process();
                //    process.StartInfo.FileName = "net";
                //    process.StartInfo.Arguments = "start \"Celeriq Core Services\"";
                //    process.StartInfo.UseShellExecute = false;
                //    process.StartInfo.CreateNoWindow = false;
                //    process.Start();
                //    process.WaitForExit();
                //}

                //Right now just email
                #region Email
                EmailDomain.SendMail(new EmailSettings
                {
                    Body = "The Celeriq Agent failed " + _failureCount + " time(s) to connect to the Celeriq Server",
                    From = ConfigHelper.FromEmail,
                    Subject = "Celeriq Agent Connection Error [" + Environment.MachineName + "]",
                    To = ConfigHelper.NotifyEmail,
                });
                #endregion


                _failureCount = 0;
                _serviceDownCount = 0;
                Logger.LogInfo("Celeriq service connection error");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                throw;
            }
        }

        public void CheckService()
        {
            try
            {
                using (var factory = SystemCoreInteractDomain.GetFactory(MachineName, Celeriq.Server.Interfaces.ConfigHelper.Port))
                {
                    var server = factory.CreateChannel();
                    var rList = server.GetRepositoryPropertyList(_credentials, new PagingInfo() { PageOffset = 1, RecordsPerPage = 10 });
                    var repository = rList.FirstOrDefault();
                    if (repository != null)
                    {
                        if (!IsValidRepository(repository.Repository.ID))
                        {
                            _failureCount++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _failureCount++;
                //Logger.LogError(ex);
                //throw;
            }
        }

        private bool IsValidRepository(Guid repositoryId)
        {
            try
            {
                using (var factory = GetFactory(MachineName))
                {
                    var service = factory.CreateChannel();
                    service.IsValidFormat(repositoryId, new DataItem(), _credentials);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private static System.ServiceModel.ChannelFactory<Celeriq.Common.IDataModel> GetFactory(string serverName)
        {
            try
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
                var endpoint = new EndpointAddress("net.tcp://" + serverName + ":" + Celeriq.Server.Interfaces.ConfigHelper.Port + "/__celeriq_engine");
                return new System.ServiceModel.ChannelFactory<Celeriq.Common.IDataModel>(myBinding, endpoint);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                throw;
            }
        }

        //private static DataQueryResults QueryData(Guid repositoryId, Celeriq.Common.IDataModel service)
        //{
        //    try
        //    {
        //        var t = service.Query(repositoryId, new DataQuery());
        //        if (t.ErrorList != null && t.ErrorList.Length > 0)
        //        {
        //            throw new Exception(t.ErrorList.First());
        //        }
        //        return t;
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.LogError(ex);
        //        throw;
        //    }
        //}

        private static UserCredentials GetCredentials()
        {
            try
            {
                if (_credentials == null)
                {
                    _credentials = new UserCredentials();
                    using (var factory = SystemCoreInteractDomain.GetFactory(MachineName, Celeriq.Server.Interfaces.ConfigHelper.Port))
                    {
                        var server = factory.CreateChannel();
                        _credentials.UserName = "root";
                        _credentials.Password = "password";
                        _credentials.Password = Celeriq.Utilities.SecurityHelper.Encrypt(server.GetPublicKey(), _credentials.Password);
                    }
                }
                return _credentials;
            }
            catch (Exception ex)
            {
                //Logger.LogError(ex);
                return null;
            }
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                Logger.LogError(e.ExceptionObject as System.Exception);
            }
            catch (Exception ex)
            {
                //Do Nothing
            }
        }

    }
}