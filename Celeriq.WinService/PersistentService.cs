using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Description;
using Celeriq.Utilities;
using System.Configuration;
using System.IO;
using Celeriq.Server.Interfaces;

namespace Celeriq.WinService
{
    public partial class PersistentService : ServiceBase
    {
        #region Class Members

        private Celeriq.Common.ISystemCore _core = null;

        #endregion

        #region Constructor

        public PersistentService()
        {
            InitializeComponent();
        }

        #endregion

        #region Service Events

        protected override void OnStart(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            this.Start();
        }

        protected override void OnStop()
        {
            //KillTimer();
            try
            {
                if (_core != null) _core.ShutDown();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }

            Logger.LogInfo("Services Stopped");
        }

        protected override void OnShutdown()
        {
            try
            {
                base.OnShutdown();
                _core.ShutDown();
                Logger.LogInfo("Services ShutDown");
                //KillTimer();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                throw;
            }
        }

        #endregion

        #region Methods

        public void Cleanup()
        {
            try
            {
                _core.ShutDown();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
        }

        public void Start()
        {
            Logger.LogInfo("Services Started Begin");
            try
            {
                #region Load the service into memory

                #region Primary Endpoint

                Celeriq.Server.Interfaces.StorageTypeConstants storage;
                if (!Enum.TryParse<Celeriq.Server.Interfaces.StorageTypeConstants>(ConfigurationManager.AppSettings["Storage"], out storage))
                {
                    throw new Exception("The 'Storage' type in AppSettings must be 'File' or 'Database'");
                }

                //Setup storage type
                Celeriq.Server.Interfaces.ConfigHelper.Storage = storage;
                Celeriq.Server.Interfaces.UserDomain.Storage = storage;

                var setup = new Server.Interfaces.ConfigurationSetup();

                setup.AutoLoad = ConfigurationManager.AppSettings["AutoLoad"].ToBool();
                setup.AutoDataUnloadTime = Convert.ToInt32(ConfigurationManager.AppSettings["AutoDataUnloadTime"]);
                setup.MaxMemory = Convert.ToInt32(ConfigurationManager.AppSettings["MaxMemory"]);
                setup.MaxRunningRepositories = Convert.ToInt32(ConfigurationManager.AppSettings["MaxRunningRepositories"]);
                setup.PrivateKey = ConfigurationManager.AppSettings["PrivateKey"];
                setup.PublicKey = ConfigurationManager.AppSettings["PublicKey"];

                int port;
                if (!int.TryParse(ConfigurationManager.AppSettings["Port"], out port)) port = 1973;
                setup.Port = port;
                Celeriq.Server.Interfaces.ConfigHelper.Port = setup.Port;

                int mailPort;
                if (!int.TryParse(ConfigurationManager.AppSettings["MailServerPort"], out mailPort)) mailPort = 25;
                Celeriq.Server.Interfaces.ConfigHelper.MailServerPort = mailPort;

                Celeriq.Server.Interfaces.ConfigHelper.MailServer = ConfigurationManager.AppSettings["MailServer"];
                Celeriq.Server.Interfaces.ConfigHelper.MailServerUsername = ConfigurationManager.AppSettings["MailServerUsername"];
                Celeriq.Server.Interfaces.ConfigHelper.MailServerPassword = ConfigurationManager.AppSettings["MailServerPassword"];
                Celeriq.Server.Interfaces.ConfigHelper.NotifyEmail = ConfigurationManager.AppSettings["NotifyEmail"];
                Celeriq.Server.Interfaces.ConfigHelper.FromEmail = ConfigurationManager.AppSettings["FromEmail"];
                Celeriq.Server.Interfaces.ConfigHelper.DebugEmail = ConfigurationManager.AppSettings["DebugEmail"];

                bool b;
                if (bool.TryParse(ConfigurationManager.AppSettings["AllowStatistics"], out b))
                    setup.AllowStatistics = b;
                else
                    setup.AllowStatistics = true;

                if (setup.AutoLoad && (setup.MaxRunningRepositories > 0))
                {
                    throw new Exception("MaxRunningRepositories must be 0 when AutoLoad is true.");
                }

                if (setup.MaxMemory < 0)
                {
                    throw new Exception("MaxMemory cannot be less than zero.");
                }

                if (setup.MaxRunningRepositories < 0)
                {
                    throw new Exception("MaxRunningRepositories cannot be less than zero.");
                }

                if (setup.AutoDataUnloadTime < 0)
                {
                    throw new Exception("AutoDataUnloadTime cannot be less than zero.");
                }

                if (storage == Celeriq.Server.Interfaces.StorageTypeConstants.File)
                {
                    //setup.AllowCaching = Convert.ToBoolean(ConfigurationManager.AppSettings["AllowCaching"]);
                    setup.DataPath = ConfigurationManager.AppSettings["DataPath"];
                }

                var primaryAddress = new Uri("net.tcp://localhost:" + setup.Port + "/__celeriq_core");
                var service = new Celeriq.Server.Core.SystemCore(storage, setup);
                var primaryHost = new ServiceHost(service, primaryAddress);

                //Initialize the service
                var netTcpBinding = new NetTcpBinding();
                netTcpBinding.Security.Mode = SecurityMode.None;
                primaryHost.AddServiceEndpoint(typeof(Celeriq.Common.ISystemCore), netTcpBinding, string.Empty);
                primaryHost.Open();

                //Create Core Listener
                var primaryEndpoint = new EndpointAddress(primaryHost.BaseAddresses.First().AbsoluteUri);
                var primaryClient = new ChannelFactory<Celeriq.Common.ISystemCore>(netTcpBinding, primaryEndpoint);
                _core = primaryClient.CreateChannel();

                var settings = service.ServerResourceSettings;
                settings.AutoDataUnloadTime = setup.AutoDataUnloadTime;
                settings.MaxMemory = setup.MaxMemory;
                settings.MaxRunningRepositories = setup.MaxRunningRepositories;

                #endregion

                this.LoadEngine(service);

                Logger.LogInfo("MaxRunningRepositories=" + setup.MaxRunningRepositories + ", MaxMemory=" + setup.MaxMemory + ", AutoDataUnloadTime=" + setup.AutoDataUnloadTime);
                Logger.LogInfo("Services Started End");

                #endregion

            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                throw;
            }
        }

        private void LoadEngine(Celeriq.Server.Core.SystemCore core)
        {
            try
            {
                //Load Server Object
                var baseAddress = new Uri("net.tcp://localhost:" + ConfigHelper.Port + "/__celeriq_engine");
                var serviceInstance = core.Manager;
                var host = new ServiceHost(serviceInstance, baseAddress);

                //Initialize the service
                var myBinding = new NetTcpBinding()
                {
                    MaxBufferSize = int.MaxValue,
                    MaxReceivedMessageSize = int.MaxValue,
                    MaxBufferPoolSize = int.MaxValue,
                    ReaderQuotas = new System.Xml.XmlDictionaryReaderQuotas()
                    {
                        MaxArrayLength = int.MaxValue,
                        MaxBytesPerRead = int.MaxValue,
                        MaxDepth = int.MaxValue,
                        MaxNameTableCharCount = int.MaxValue,
                        MaxStringContentLength = int.MaxValue,
                    }
                };
                myBinding.Security.Mode = SecurityMode.None;
                var endpoint = host.AddServiceEndpoint(typeof(Celeriq.Common.IDataModel), myBinding, host.BaseAddresses.First().AbsoluteUri);

                foreach (var op in endpoint.Contract.Operations)
                {
                    var dataContractBehavior = op.Behaviors.Find<DataContractSerializerOperationBehavior>();
                    if (dataContractBehavior != null)
                    {
                        dataContractBehavior.MaxItemsInObjectGraph = int.MaxValue;
                    }
                }

                //var behavior = host.Description.Behaviors.Find<ServiceDebugBehavior>();
                //behavior.IncludeExceptionDetailInFaults = true;

                host.Open();

            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                throw;
            }

        }

        #endregion

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