using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Celeriq.ManagementStudio.UserControls;
using Celeriq.ManagementStudio.EventArguments;
using Celeriq.Common;
using Celeriq.ManagementStudio.Objects;
using Celeriq.Common;
using Celeriq.Utilities;

namespace Celeriq.ManagementStudio
{
    internal partial class MainForm2 : Form
    {
        #region Class Members

        private WaitForm _progress = null;
        private List<IRemotingObject> _activeRepositoryList = new List<IRemotingObject>();
        private List<IRemotingObject> _currentRepositoryList = new List<IRemotingObject>();
        private string _publicKey = string.Empty;
        private ApplicationUserSetting _userSettings = new ApplicationUserSetting();

        #endregion

        #region Constructors

        public MainForm2()
        {
            InitializeComponent();

            this.Size = new System.Drawing.Size((int) (Screen.PrimaryScreen.WorkingArea.Width*0.8), (int) (Screen.PrimaryScreen.WorkingArea.Height*0.8));

            this.UpdateStatus();
            EnableMenus();

            serverPropertySheet1.MouseUp += serverPropertySheet1_MouseUp;
            serverPropertySheet1.RepositorySelected += serverPropertySheet1_RepositorySelected;
            serverPropertySheet1.RepositoryDeleted += ctrl_RepositoryDeleted;
            this.KeyUp += MainForm2_KeyUp;

            _userSettings.Load();
            if (_userSettings.WindowSize.Width != 0)
                this.Size = _userSettings.WindowSize;
            this.WindowState = _userSettings.WindowState;
            this.Location = _userSettings.WindowLocation;

            this.Move += MainForm_Move;
            this.ResizeEnd += MainForm_ResizeEnd;
            this.FormClosing += MainForm_FormClosing;

            var timer = new Timer();
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = 500;
            timer.Start();
        }

        private void MainForm2_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                serverPropertySheet1.RefreshFromServer();
            }
        }

        private void serverPropertySheet1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                ShowContextMenu(sender as Control, e);
            }
        }

        private void serverPropertySheet1_RepositorySelected(object sender, RepositoryListEventArgs e)
        {
            _activeRepositoryList.Clear();
            _activeRepositoryList.AddRange(e.RepositoryList);
            this.EnableMenus();
        }

        public MainForm2(string[] args)
            : this()
        {
            if (args.Length == 3)
            {

            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _userSettings.Save();
        }

        private void MainForm_ResizeEnd(object sender, EventArgs e)
        {
            ResetWindowState();
        }

        private void MainForm_Move(object sender, EventArgs e)
        {
            ResetWindowState();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            var timer = sender as Timer;
            if (timer != null)
                timer.Stop();

            this.OpenConnection();
        }

        #endregion

        #region Methods

        private bool ServerConnect(string serverName)
        {
            if (serverPropertySheet1.IsConnected) return false;

            try
            {
                this.UpdateStatus("Connecting...");
                serverPropertySheet1.ServerName = serverName;
                if (serverPropertySheet1.RefreshFromServer())
                {
                    serverPropertySheet1.IsConnected = true;
                    this.UpdateStatus();
                    this.UpdateStatus("Connected");
                    lblServer.Text = "Server: " + serverName.ToUpper();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                serverPropertySheet1.IsConnected = false;
                serverPropertySheet1.ServerName = string.Empty;
                this.UpdateStatus("Connection Failed!");
                return false;
            }
        }

        private void ServerDisconnect()
        {
            serverPropertySheet1.IsConnected = false;
            serverPropertySheet1.ServerName = null;
            lblServer.Text = string.Empty;
            serverPropertySheet1.Credentials = null;
            _publicKey = string.Empty;
            serverPropertySheet1.RefreshFromServer();
            this.UpdateStatus("Disconnected");
        }

        private void UpdateStatus()
        {
            UpdateStatus(string.Empty);
        }

        private void UpdateStatus(string text)
        {
            if (string.IsNullOrEmpty(text))
                text = "Ready!";
            lblStatus.Text = text;
        }

        private void EnableMenus()
        {
            menuFileConnect.Enabled = !serverPropertySheet1.IsConnected;
            menuFileDisconnect.Enabled = serverPropertySheet1.IsConnected;

            cmdNew.Enabled = serverPropertySheet1.IsConnected;
            cmdLoadSchema.Enabled = serverPropertySheet1.IsConnected;
            cmdCreateFromTemplate.Enabled = serverPropertySheet1.IsConnected;
            cmdCode.Enabled = (serverPropertySheet1.SelectedRepositoryList.Count == 1);
            cmdRefresh.Enabled = serverPropertySheet1.IsConnected;

            menuSep1.Visible = serverPropertySheet1.IsConnected && (_currentRepositoryList.Count > 0);
            menuMainServerProp.Enabled = serverPropertySheet1.IsConnected;
            menuMainUserList.Enabled = serverPropertySheet1.IsConnected;
        }

        private void ResetWindowState()
        {
            if (this.WindowState == FormWindowState.Normal)
                _userSettings.WindowSize = this.Size;
            if (this.WindowState != FormWindowState.Minimized)
                _userSettings.WindowState = this.WindowState;
            if (this.WindowState == FormWindowState.Normal)
                _userSettings.WindowLocation = this.Location;
        }

        #endregion

        #region Repository Event Handlers

        private delegate void UpdateUICallback(object sender, RepositoryEventArgs e);

        private delegate void ErrorCallback(object sender, RepositoryErrorEventArgs e);

        private void DisableUI()
        {
            _progress = new WaitForm();
            this.MainMenu.Enabled = false;
            this.MainToolbar.Enabled = false;
            this.serverPropertySheet1.Enabled = false;
            _progress.Show();
            Application.DoEvents();
        }

        private void EnableUI()
        {
            if (_progress != null)
                _progress.Close();
            this.MainMenu.Enabled = true;
            this.MainToolbar.Enabled = true;
            this.serverPropertySheet1.Enabled = true;
        }

        private void OpenConnection()
        {
            var F = new ServerConnectionForm();
            if (F.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                serverPropertySheet1.Credentials = F.Credentials;
                _publicKey = F.PublicKey;
                this.ServerConnect(F.ServerName);
                EnableMenus();
            }
        }

        #endregion

        #region Menu Event Handlers

        private void menuHelpAbout_Click(object sender, EventArgs e)
        {
            var F = new SplashForm();
            F.ShowDialog();
            System.Windows.Forms.Application.DoEvents();
        }

        private void menuFileConnect_Click(object sender, EventArgs e)
        {
            OpenConnection();
        }

        private void menuFileDisconnect_Click(object sender, EventArgs e)
        {
            this.ServerDisconnect();
            EnableMenus();
        }

        private void cmdNew_Click(object sender, EventArgs e)
        {
            try
            {
                var item = new BaseRemotingObject();
                item.Repository = new Common.RepositorySchema(){ Name="[New Repository]"};
                item.Repository.FieldList.Add(new Common.FieldDefinition() {DataType = Common.RepositorySchema.DataTypeConstants.Int, Name = "ID", IsPrimaryKey = true});                
                serverPropertySheet1.AddRepository(item);
                ShowDesignWindow(item);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                throw;
            }
        }

        private void cmdLoadSchema_Click(object sender, EventArgs e)
        {
            try
            {
                var dialog = new OpenFileDialog();
                dialog.Title = "Load Schema";
                dialog.FileName = "*.celeriq";
                dialog.Filter = "Schema Files (*.celeriq)|*.celeriq";
                dialog.FilterIndex = 0;
                dialog.Multiselect = true;
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    foreach (var fileName in dialog.FileNames)
                    {
                        var item = new BaseRemotingObject();
                        item.Repository = new RepositorySchema();
                        item.Repository.Load(fileName);
                        serverPropertySheet1.AddRepository(item);

                        using (var factory = SystemCoreInteractDomain.GetFactory(serverPropertySheet1.ServerName))
                        {
                            var server = factory.CreateChannel();
                            var matchList = SystemCoreInteractDomain.GetRepositoryPropertyList(serverPropertySheet1.ServerName, serverPropertySheet1.Credentials,
                                new Common.PagingInfo { PageOffset = 1, RecordsPerPage = 1, Keyword = item.Repository.ID.ToString() });

                            if (matchList.Count > 0)
                            {
                                MessageBox.Show("The repository already exists.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                            server.SaveRepository(item.Repository, serverPropertySheet1.Credentials);
                        }
                    }

                    serverPropertySheet1.RefreshFromServer();
                    EnableUI();
                    this.UpdateStatus();

                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                MessageBox.Show("An error occurred!\r\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmdCreateFromTemplate_Click(object sender, EventArgs e)
        {
            try
            {
                var dialog = new OpenFileDialog();
                dialog.Title = "Load Template";
                dialog.FileName = "*.celeriq";
                dialog.Filter = "Schema Files (*.celeriq)|*.celeriq";
                dialog.FilterIndex = 0;
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    var item = new BaseRemotingObject();
                    item.Repository = new RepositorySchema();
                    item.Repository.Load(dialog.FileName);
                    item.Repository.ID = Guid.NewGuid();
                    item.Repository.CreatedDate = DateTime.Now;
                    serverPropertySheet1.AddRepository(item);

                    using (var factory = SystemCoreInteractDomain.GetFactory(serverPropertySheet1.ServerName))
                    {
                        var server = factory.CreateChannel();
                        server.SaveRepository(item.Repository, serverPropertySheet1.Credentials);
                    }

                    serverPropertySheet1.RefreshFromServer();
                    EnableUI();
                    this.UpdateStatus();

                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                MessageBox.Show("An error occurred!\r\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmdExport_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This functionality has not yet been implemented.", "Not Implemented", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //if (this.SelecetedRepositoryNode == null) return;
            //var active = this.ActiveRepository;
            //if (active == null) return;

            //var F = new ExportDataForm(serverPropertySheet1.ServerName);
            //if (F.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //{
            //  serverPropertySheet1.RefreshFromServer();
            //}
        }

        private void menuMainUserList_Click(object sender, EventArgs e)
        {
            if (!serverPropertySheet1.IsConnected) return;
            try
            {
                var F = new UserListForm(serverPropertySheet1.ServerName, serverPropertySheet1.Credentials, _publicKey);
                if (F.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                //Do Nothing
            }
        }

        public void cmdStats_Click(object sender, EventArgs e)
        {
            if (!serverPropertySheet1.IsConnected) return;
            if (_activeRepositoryList.Count != 1) return;
            try
            {
                var repository = _activeRepositoryList.First();
                using (var factory = SystemCoreInteractDomain.GetFactory(serverPropertySheet1.ServerName))
                {
                    var server = factory.CreateChannel();
                    var stats = server.GetRepositoryStats(serverPropertySheet1.Credentials, repository.Repository.ID, DateTime.Now.AddDays(-1), DateTime.Now);

                    MessageBox.Show("Total queries: " + stats.ItemCount +
                        "\r\nAverage time per query: " + stats.Elapsed.ToString("###,###,##0.0") + " ms", "Last 24-hours", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                //Do Nothing
            }
        }

        private void menuMainServerProp_Click(object sender, EventArgs e)
        {
            var F = new ServerInfoForm(serverPropertySheet1.ServerName, serverPropertySheet1.Credentials, _publicKey, _currentRepositoryList);
            F.ShowDialog();
        }

        private void cmdExportSchema_Click(object sender, EventArgs e)
        {
            try
            {
                if (!serverPropertySheet1.IsConnected) return;
                if (_activeRepositoryList.Count == 0) return;
                var active = _activeRepositoryList.First();
                var dialog = new SaveFileDialog();
                dialog.Title = "Export Schema";
                dialog.FileName = active.Repository.Name + ".celeriq";
                dialog.Filter = "Schema Files (*.celeriq)|*.celeriq";
                dialog.FilterIndex = 0;
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    using (var factory = SystemCoreInteractDomain.GetFactory(serverPropertySheet1.ServerName))
                    {
                        var server = factory.CreateChannel();
                        server.ExportSchema(active.Repository.ID, serverPropertySheet1.Credentials, dialog.FileName);
                        MessageBox.Show("Operation complete", "Finished", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                MessageBox.Show("An error occurred!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CopyRepository()
        {
            if (!serverPropertySheet1.IsConnected) return;
            if (serverPropertySheet1.SelectedRepository != null)
            {
                try
                {
                    Clipboard.SetText(serverPropertySheet1.SelectedRepository.Repository.ToXml());
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex);
                    //Do Nothing
                }
            }
        }

        private void PasteRepository()
        {
            if (!serverPropertySheet1.IsConnected) return;
            var xml = string.Empty;
            try
            {
                xml = Clipboard.GetText();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return;
            }

            if (!string.IsNullOrEmpty(xml))
            {
                Celeriq.ManagementStudio.Objects.ConnectionCache.CreateRepository(serverPropertySheet1.ServerName, xml, serverPropertySheet1.Credentials);
                serverPropertySheet1.RefreshFromServer();
            }
        }

        private void cmdCode_Click(object sender, EventArgs e)
        {
            try
            {
                if (_activeRepositoryList.Count == 0) return;
                var active = _activeRepositoryList.First();
                if (active == null) return;

                var code = Celeriq.ManagementStudio.Objects.ClientCodeHelper.GenerateClientCode(active.Repository);
                var config = Celeriq.ManagementStudio.Objects.ClientCodeHelper.GenerateClientConfig(active.Repository, serverPropertySheet1.ServerName);
                var F = new CodeWindow(code, config);
                F.ShowDialog();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                throw;
            }
        }

        private void cmdRefresh_Click(object sender, EventArgs e)
        {
            serverPropertySheet1.RefreshFromServer();
        }

        private void DeleteRepository(IRemotingObject repository)
        {
            try
            {
                using (var factory = SystemCoreInteractDomain.GetFactory(serverPropertySheet1.ServerName))
                {
                    var server = factory.CreateChannel();

                    var r = _currentRepositoryList.FirstOrDefault(x => x.Repository.ID == repository.Repository.ID);
                    _currentRepositoryList.Remove(r);
                    if (_activeRepositoryList.Contains(r)) _activeRepositoryList.Remove(r);

                    server.DeleteRepository(repository.Repository, serverPropertySheet1.Credentials);
                    serverPropertySheet1.RemoveRepository(repository);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                throw;
            }
        }

        private void ShowContextMenu(Control ctrl, MouseEventArgs e)
        {
            if (_activeRepositoryList.Count > 0)
            {
                var menu = new ContextMenu();
                MenuItem newItem = null;

                newItem = new MenuItem();
                newItem.Text = "Design...";
                newItem.Click += new EventHandler(DesignMenuClick);
                newItem.Enabled = (_activeRepositoryList.Count == 1);
                menu.MenuItems.Add(newItem);

                newItem = new MenuItem();
                newItem.Text = "Design Code...";
                newItem.Click += new EventHandler(cmdCode_Click);
                newItem.Enabled = (_activeRepositoryList.Count == 1);
                menu.MenuItems.Add(newItem);

                menu.MenuItems.Add(new MenuItem() {Text = "-"});

                newItem = new MenuItem();
                newItem.Text = "Export Schema...";
                newItem.Click += new EventHandler(cmdExportSchema_Click);
                newItem.Enabled = (_activeRepositoryList.Count == 1);
                menu.MenuItems.Add(newItem);

                //newItem = new MenuItem();
                //newItem.Text = "Export data...";
                //newItem.Click += new EventHandler(cmdExport_Click);
                //menu.MenuItems.Add(newItem);

                newItem = new MenuItem();
                newItem.Text = "Stats";
                newItem.Click += new EventHandler(cmdStats_Click);
                newItem.Enabled = (_activeRepositoryList.Count == 1);
                menu.MenuItems.Add(newItem);

                menu.Show(ctrl, e.Location);

            }
        }

        private void ctrl_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                ShowContextMenu(sender as Control, e);
            }
        }

        private void ctrl_RepositorySelected(object sender, RepositoryListEventArgs e)
        {
            _activeRepositoryList.Clear();
            _activeRepositoryList.AddRange(e.RepositoryList);
        }

        private void ctrl_RepositoryDeleted(object sender, RepositoryListEventArgs e)
        {
            try
            {
                e.RepositoryList.ForEach(x => DeleteRepository(x));
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                throw;
            }
        }

        #endregion

        #region Tree Event Handlers

        private void DesignMenuClick(object sender, EventArgs e)
        {
            if (_activeRepositoryList.Count == 0) return;
            var active = _activeRepositoryList.First();
            ShowDesignWindow(active);
        }

        #endregion

        #region ShowDesignWindow

        private void ShowDesignWindow(IRemotingObject active)
        {
            if (active != null)
            {
                var F = new DesignRepositoryForm();
                F.Populate(active);
                if (F.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    using (var factory = SystemCoreInteractDomain.GetFactory(serverPropertySheet1.ServerName))
                    {
                        var server = factory.CreateChannel();
                        active = server.SaveRepository(active.Repository, serverPropertySheet1.Credentials);
                    }
                    _currentRepositoryList.Add(active);
                    serverPropertySheet1.UpdateRepository(active);
                    EnableUI();
                    this.UpdateStatus();
                }
            }
        }

        #endregion

        private void menuMainExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}