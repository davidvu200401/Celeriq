using System;
using System.Collections.Generic;
using System.Configuration.Install;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using CeleriqSetup.EventArguments;
using Microsoft.Web.Administration;

namespace CeleriqSetup
{
    internal class WorkThreader
    {
        private Celeriq.DataCore.Install.InstallSetup _setup;

        public event EventHandler Complete;
        public event EventHandler<TextChangedEventArgs> ProgressText;

        protected virtual void OnComplete(EventArgs e)
        {
            if (this.Complete != null)
                this.Complete(this, e);
        }

        protected virtual void OnProgressText(TextChangedEventArgs e)
        {
            if (this.ProgressText != null)
                this.ProgressText(this, e);
        }

        public WorkThreader(Celeriq.DataCore.Install.InstallSetup setup)
        {
            _setup = setup;
        }

        public void Run()
        {
            //Try to stop the service if it is running
            const string SERVICENAME = "Celeriq Core Services";
            if (DoesServiceExist(SERVICENAME))
            {
                var sc = new ServiceController(SERVICENAME);
                sc.Stop();
            }

            if (this.UseDatabase)
            {
                //Database
                try
                {
                    var di = new Celeriq.DataCore.Install.DatabaseInstaller();
                    di.Install(_setup);
                    this.OnProgressText(new TextChangedEventArgs("Database installation completed successfully"));
                }
                catch (Exception ex)
                {
                    this.OnProgressText(new TextChangedEventArgs("Database installation failed: " + ex.Message));
                    this.Error = ex.Message;
                    this.OnComplete(new EventArgs());
                    return;
                }
            }

            if (this.UseService)
            {
                //Service
                try
                {
                    if (!DoesServiceExist(SERVICENAME))
                    {
                        var fi = new FileInfo(System.Reflection.Assembly.GetEntryAssembly().Location);
                        //var filePath = @"C:\Program Files (x86)\Widgetsphere\Celeriq Server\Celeriq.WinService.exe";
                        var filePath = Path.Combine(fi.DirectoryName, @"Celeriq.WinService.exe");
                        ManagedInstallerClass.InstallHelper(new string[] {filePath});
                        this.OnProgressText(new TextChangedEventArgs("Service installation completed successfully"));
                    }
                }
                catch (Exception ex)
                {
                    this.OnProgressText(new TextChangedEventArgs("Service installation failed: " + ex.Message));
                    this.Error = ex.Message;
                    this.OnComplete(new EventArgs());
                    return;
                }
            }

            if (DoesServiceExist(SERVICENAME))
            {
                var sc = new ServiceController(SERVICENAME);
                sc.Start();
            }

            if (this.UseAdminsite)
            {
                //Setup admin site
                //try
                //{
                //    var manager = new ServerManager();
                //    var sitename = "admin.celeriq.local";
                //    var applicationPool = "DefaultAppPool"; // set your deafultpool :4.0 in IIS
                //    var hostName = "admin.celeriq.local";
                //    var address = "localhost";
                //    var bindinginfo = address + ":80:" + hostName;

                //    //check if website name already exists in IIS
                //    var bWebsite = manager.Sites.Any(x => x.Name == sitename);
                //    if (!bWebsite)
                //    {
                //        var mySite = manager.Sites.Add(sitename.ToString(), "http", bindinginfo, "C:\\inetpub\\wwwroot\\" + sitename);
                //        mySite.ApplicationDefaults.ApplicationPoolName = applicationPool;
                //        mySite.TraceFailedRequestsLogging.Enabled = true;
                //        mySite.TraceFailedRequestsLogging.Directory = "C:\\inetpub\\customfolder\\site";
                //        manager.CommitChanges();
                //        this.OnProgressText(new TextChangedEventArgs("Admin site created successfully"));
                //    }
                //}
                //catch (Exception ex)
                //{
                //    this.OnProgressText(new TextChangedEventArgs("Service installation failed: " + ex.Message));
                //    this.Error = ex.Message;
                //    this.OnComplete(new EventArgs());
                //    return;
                //}
            }

            this.OnComplete(new EventArgs());
        }

        private bool DoesServiceExist(string serviceName)
        {
            var services = ServiceController.GetServices();
            var service = services.FirstOrDefault(s => s.ServiceName == serviceName);
            return service != null;
        }

        public string Error { get; private set; }
        public bool UseDatabase { get; set; }
        public bool UseService { get; set; }
        public bool UseAdminsite { get; set; }
    }
}