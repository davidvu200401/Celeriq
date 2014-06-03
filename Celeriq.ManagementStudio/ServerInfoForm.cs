#pragma warning disable 0168
using System;
using System.Linq;
using System.Windows.Forms;
using Celeriq.Common;
using System.Collections.Generic;

namespace Celeriq.ManagementStudio
{
    public partial class ServerInfoForm : Form
    {
        private string _serverName;
        private UserCredentials _credentials;
        private string _publicKey;

        public ServerInfoForm()
        {
            InitializeComponent();
        }

        public ServerInfoForm(string serverName, UserCredentials credentials, string publicKey, List<IRemotingObject> repositoryList)
            : this()
        {
            _serverName = serverName;
            _credentials = credentials;
            _publicKey = publicKey;

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

            var systemStats = SystemCoreInteractDomain.GetSystemStats(serverName, credentials);
            txtLoaded.Text = systemStats.RepositoryCount.ToString("#,##0") + " / " + systemStats.InMemoryCount.ToString("#,##0");
            txtMachine.Text = systemStats.MachineName;
            txtOSVersion.Text = systemStats.OSVersion;
            txtProcessors.Text = systemStats.ProcessorCount.ToString();

            var reboot = DateTime.Now.AddMilliseconds(-systemStats.TickCount);
            var ts = TimeSpan.FromMilliseconds(systemStats.TickCount);
            var timeago = string.Format("{0}d:{1:D2}h:{2:D2}m:{3:D2}s",
                ts.Days,
                ts.Hours,
                ts.Minutes,
                ts.Seconds);

            txtReboot.Text = reboot.ToString("yyyy-MM-dd HH:mm:ss") + " (" + timeago + ")";
            txtTotalMemory.Text = Celeriq.ManagementStudio.Objects.Utilities.ToSizeDisplay(systemStats.TotalMemory);

            if (systemStats.UsedDisk == -1) txtDiskSize.Text = "N/A";
            else txtDiskSize.Text = Celeriq.ManagementStudio.Objects.Utilities.ToSizeDisplay(systemStats.UsedDisk);
            txtMemorySize.Text = Celeriq.ManagementStudio.Objects.Utilities.ToSizeDisplay(systemStats.UsedMemory);

            txtPublicKey.Text = publicKey;
            txtName.Text = serverName;

            try
            {
                using (var factory = SystemCoreInteractDomain.GetFactory(serverName))
                {
                    var server = factory.CreateChannel();
                    var settings = server.GetServerResourceSetting(credentials);
                    udLoaded.Value = settings.MaxRunningRepositories;
                    udMemory.Value = settings.MaxMemory/(1024*1024);
                    udTime.Value = settings.AutoDataUnloadTime;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred connecting to server.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

        }

        private void cmdClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmdUsers_Click(object sender, EventArgs e)
        {
            var F = new UserListForm();
            F.ShowDialog();
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            try
            {
                using (var factory = SystemCoreInteractDomain.GetFactory(_serverName))
                {
                    var server = factory.CreateChannel();
                    var settings = server.GetServerResourceSetting(_credentials);
                    settings.MaxRunningRepositories = (int) udLoaded.Value;
                    settings.MaxMemory = (int) udMemory.Value*(1024*1024);
                    settings.AutoDataUnloadTime = (int) udTime.Value;
                    server.SaveServerResourceSetting(_credentials, settings);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred connecting to server.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            this.Close();

        }

    }
}