using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Celeriq.AdminSite.Objects;
using Celeriq.Common;

namespace Celeriq.AdminSite.AuthUser
{
    public partial class ServerSettings : System.Web.UI.Page
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            cmdSave.Click += cmdSave_Click;
            this.Populate();
        }

        private void RefreshServerState()
        {
            try
            {
                using (var factory = SystemCoreInteractDomain.GetFactory(SessionHelper.CurrentUser.Server))
                {
                    var server = factory.CreateChannel();
                    var repositoryList = SystemCoreInteractDomain.GetRepositoryPropertyList(SessionHelper.CurrentUser.Server, RepositoryConnection.Credentials)
                        .OrderBy(x => x.Repository.Name)
                        .ToList();

                    long diskSize = 0;
                    long memorySize = 0;
                    long loadedCount = 0;

                    if (repositoryList.Count > 0)
                    {
                        diskSize = repositoryList.Sum(x => x.DataDiskSize);
                        var loadedList = repositoryList.Where(x => x.IsLoaded).ToList();
                        if (loadedList.Count > 0) memorySize = loadedList.Sum(x => x.DataMemorySize);
                        loadedCount = repositoryList.Count(x => x.IsLoaded);
                    }

                    SessionHelper.Configuration.TotalRepositories = repositoryList.Count.ToString("#,##0");
                    SessionHelper.Configuration.CurrentDisk = ToSizeDisplay(diskSize);
                    SessionHelper.Configuration.CurrentLoaded = loadedCount.ToString("#,##0");
                    SessionHelper.Configuration.CurrentMemory = ToSizeDisplay(memorySize);

                    var systemStats = SystemCoreInteractDomain.GetSystemStats(SessionHelper.CurrentUser.Server, RepositoryConnection.Credentials);
                    SessionHelper.Configuration.MachineName = systemStats.MachineName;
                    SessionHelper.Configuration.OSVersion = systemStats.OSVersion;
                    SessionHelper.Configuration.ProcessorCount = systemStats.ProcessorCount;
                    SessionHelper.Configuration.TickCount = systemStats.TickCount;
                    SessionHelper.Configuration.TotalMemory = systemStats.TotalMemory;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public string ToSizeDisplay(long size)
        {
            const long KB = 1024;
            const long MB = 1024 * KB;
            const long GB = 1024 * MB;

            if (size > GB)
            {
                return (size / (GB * 1.0)).ToString("###,###,###,##0.0") + " GB";
            }
            else if (size > MB)
            {
                return (size / (MB * 1.0)).ToString("###,###,###,##0.0") + " MB";
            }
            else
            {
                return (size / (KB * 1.0)).ToString("###,###,###,##0.0") + " KB";
            }
        }

        private void cmdSave_Click(object sender, EventArgs e)
        {
            int maxLoaded;
            int maxMemory;
            int unloadTime;

            if (!int.TryParse(txtLoaded.Text, out maxLoaded) || (maxLoaded < 0))
                this.Page.Validators.Add(new CustomValidator { IsValid = false, ErrorMessage = "The max loaded must be greater than or equal 0." });
            if (!int.TryParse(txtMemory.Text, out maxMemory) || (maxMemory < 0))
                this.Page.Validators.Add(new CustomValidator { IsValid = false, ErrorMessage = "The max memory must be greater than or equal 0." });
            if (!int.TryParse(txtTime.Text, out unloadTime) || (unloadTime < 0))
                this.Page.Validators.Add(new CustomValidator { IsValid = false, ErrorMessage = "The unload time must be greater than or equal 0." });

            if (!this.Page.IsValid) return;

            try
            {
                using (var factory = SystemCoreInteractDomain.GetFactory(SessionHelper.CurrentUser.Server))
                {
                    var server = factory.CreateChannel();
                    var settings = server.GetServerResourceSetting(RepositoryConnection.Credentials);
                    settings.MaxRunningRepositories = maxLoaded;
                    settings.MaxMemory = maxMemory * (1024 * 1024);
                    settings.AutoDataUnloadTime = unloadTime;
                    server.SaveServerResourceSetting(RepositoryConnection.Credentials, settings);
                }
            }
            catch (Exception ex)
            {
                return;
            }
        }

        private void Populate()
        {
            try
            {
                this.RefreshServerState();

                using (var factory = SystemCoreInteractDomain.GetFactory(SessionHelper.CurrentUser.Server))
                {
                    var server = factory.CreateChannel();
                    var settings = server.GetServerResourceSetting(RepositoryConnection.Credentials);
                    txtLoaded.Text = settings.MaxRunningRepositories.ToString();
                    txtMemory.Text = (settings.MaxMemory / (1024 * 1024)).ToString();
                    txtTime.Text = settings.AutoDataUnloadTime.ToString();

                    lblRepoTotal.Text = SessionHelper.Configuration.TotalRepositories;
                    lblDisk.Text = SessionHelper.Configuration.CurrentDisk;
                    lblLoaded.Text = SessionHelper.Configuration.CurrentLoaded;
                    lblCurrentMemory.Text = SessionHelper.Configuration.CurrentMemory;
                }

                lblHistoryHours.Text = "<a href='#' hourindex='1'>1 Hour</a>" +
                                       "<a href='#' hourindex='2'>2 Hour</a>" +
                                       "<a href='#' hourindex='3'>3 Hour</a>" +
                                       "<a href='#' hourindex='6'>6 Hour</a>" +
                                       "<a href='#' hourindex='12'>12 Hour</a>" +
                                       "<a href='#' hourindex='24'>24 Hour</a>";

                int hour;
                int.TryParse(this.Request.QueryString["hours"], out hour);
                if (hour == 0) hour = 1;
                this.ClientScript.RegisterHiddenField("__hourindex", hour.ToString());

                lblMachineName.Text = SessionHelper.Configuration.MachineName;
                lblOSVersion.Text = SessionHelper.Configuration.OSVersion;
                lblProcessors.Text = SessionHelper.Configuration.ProcessorCount.ToString();

                var reboot = DateTime.Now.AddMilliseconds(-SessionHelper.Configuration.TickCount);
                var ts = TimeSpan.FromMilliseconds(SessionHelper.Configuration.TickCount);
                var timeago = string.Format("{0}d:{1:D2}h:{2:D2}m:{3:D2}s",
                    ts.Days,
                    ts.Hours,
                    ts.Minutes,
                    ts.Seconds);

                lblReboot.Text = reboot.ToString("yyyy-MM-dd HH:mm:ss") + " (" + timeago + ")";
                lblTotalMemory.Text = (SessionHelper.Configuration.TotalMemory / (1024 * 1024)).ToString("#,##0") + " MB";

            }
            catch (Exception ex)
            {
                return;
            }
        }
    }

}