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
    internal partial class MainForm : Form
    {
        #region Class Members

        private bool _isConnected = false;
        private WaitForm _progress = null;
        private string _serverName = null;
        private List<IRemotingObject> _activeRepositoryList = new List<IRemotingObject>();
        private List<IRemotingObject> _currentRepositoryList = new List<IRemotingObject>();
        private UserCredentials _credentials = null;
        private string _publicKey = string.Empty;
        private ApplicationUserSetting _userSettings = new ApplicationUserSetting();

        #endregion

        #region Constructors

        public MainForm()
        {
            InitializeComponent();

            this.Size = new System.Drawing.Size((int) (Screen.PrimaryScreen.WorkingArea.Width*0.8), (int) (Screen.PrimaryScreen.WorkingArea.Height*0.8));

            this.UpdateStatus();
            EnableMenus();

            _userSettings.Load();
            if (_userSettings.WindowSize.Width != 0)
                this.Size = _userSettings.WindowSize;
            this.WindowState = _userSettings.WindowState;
            this.Location = _userSettings.WindowLocation;

            tvwItem.AfterSelect += new TreeViewEventHandler(tvwItem_AfterSelect);
            tvwItem.KeyDown += new KeyEventHandler(tvwItem_KeyDown);
            this.KeyDown += new KeyEventHandler(MainForm_KeyDown);
            this.Move += MainForm_Move;
            this.ResizeEnd += MainForm_ResizeEnd;
            this.FormClosing += MainForm_FormClosing;

            var timer = new Timer();
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = 500;
            timer.Start();
        }

        public MainForm(string[] args)
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

        private TreeNode GetRepositoryNode(IRemotingObject repository)
        {
            if (repository == null) return null;
            var list = this.AllNodes().Where(x => x.Tag is IRemotingObject).ToList();
            if (list == null) return null;
            return list.FirstOrDefault(x => ((IRemotingObject)x.Tag).Repository.ID == repository.Repository.ID);
        }

        private TreeNode SelectedRepositoryNode
        {
            get
            {
                var node = tvwItem.SelectedNode;
                if (node == null) return null;

                var cache = node.Tag as IRemotingObject;
                if (cache != null)
                {
                    return node;
                }
                else if (cache == null && node.Parent != null)
                {
                    cache = node.Parent.Tag as IRemotingObject;
                    if (cache != null)
                        return node.Parent;
                }
                return null;
            }
        }

        private void tvwItem_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (this.SelectedRepositoryNode != null)
                {
                    if (MessageBox.Show("Do you wish to delete this repository and all associated data?", "Delete?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.Yes)
                    {
                        DeleteRepository(this.ActiveRepository);
                    }
                }
            }
            else if (e.KeyCode == Keys.C && e.Control)
            {
                //Copy
                this.CopyRepository();
            }
            else if (e.KeyCode == Keys.V && e.Control)
            {
                //Paste
                this.PasteRepository();
            }
        }

        private List<TreeNode> AllNodes()
        {
            var retval = new List<TreeNode>();
            foreach (TreeNode n in tvwItem.Nodes)
            {
                retval.Add(n);
                retval.AddRange(GetNodes(n));
            }
            return retval;
        }

        private List<TreeNode> GetNodes(TreeNode node)
        {
            var retval = new List<TreeNode>();
            foreach (TreeNode subnode in node.Nodes)
            {
                retval.Add(subnode);
                retval.AddRange(GetNodes(subnode));
            }
            return retval;
        }


        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                this.RefreshFromServer();
            }
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
            if (_isConnected) return false;

            try
            {
                this.UpdateStatus("Connecting...");
                _serverName = serverName;
                if (this.RefreshFromServer())
                {
                    _isConnected = true;
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
                _isConnected = false;
                _serverName = string.Empty;
                this.UpdateStatus("Connection Failed!");
                return false;
            }
        }

        private void ServerDisconnect()
        {
            _isConnected = false;
            _serverName = null;
            tvwItem.Nodes.Clear();
            pnlContent.Controls.Clear();
            lblServer.Text = string.Empty;
            _credentials = null;
            _publicKey = string.Empty;
            this.UpdateStatus("Disconnected");
        }

        private TreeNode ServerRepositoriesNode
        {
            get
            {
                if (tvwItem.Nodes.Count > 0 && tvwItem.Nodes[0].Nodes.Count > 0)
                    return tvwItem.Nodes[0].Nodes[0];
                return null;
            }
        }

        private TreeNode ServerUsersNode
        {
            get
            {
                if (tvwItem.Nodes.Count > 0 && tvwItem.Nodes[0].Nodes.Count > 1)
                    return tvwItem.Nodes[0].Nodes[1];
                return null;
            }
        }
        private bool RefreshFromServer()
        {
            if (string.IsNullOrEmpty(_serverName)) return false;
            try
            {
                pnlContent.Controls.Clear();
                _activeRepositoryList.Clear();
                this.UpdateStatus("Loading...");

                var selText = string.Empty;
                if (this.SelectedRepositoryNode != null)
                    selText = this.SelectedRepositoryNode.Text;

                //Save for later setting
                //var active = this.ActiveRepository;
                var isOnRpositoryList = (tvwItem.SelectedNode != null && tvwItem.SelectedNode.Text == "Repositories");
                tvwItem.Nodes.Clear();
                var repositoryList = SystemCoreInteractDomain.GetRepositoryPropertyList(_serverName, _credentials).OrderBy(x => x.Repository.Name);
                var root = tvwItem.Nodes.Add(_serverName);
                root.ImageIndex = 5;
                root.SelectedImageIndex = root.ImageIndex;

                var repositoryListNode = root.Nodes.Add("Repositories");
                repositoryListNode.Tag = "REPOSITORYLIST";
                repositoryListNode.ImageIndex = 4;
                repositoryListNode.SelectedImageIndex = repositoryListNode.ImageIndex;

                _currentRepositoryList.Clear();
                _currentRepositoryList.AddRange(repositoryList);
                foreach (var item in repositoryList)
                {
                    this.AddTreeNode(item);
                }

                if (isOnRpositoryList)
                    tvwItem.SelectedNode = repositoryListNode;

                root.Expand();
                repositoryListNode.Expand();

                this.RefreshUsers();

                if (!string.IsNullOrEmpty(selText))
                {
                    var active = repositoryListNode.Nodes.Cast<TreeNode>().FirstOrDefault(x => x.Text == selText);
                    this.tvwItem.SelectedNode = active;
                }

                lblCount.Text = "Repositories (Loaded/Total) " + repositoryList.Count(x => x.IsLoaded) + " / " + repositoryList.Count().ToString();

                this.UpdateStatus();
                this.EnableMenus();
                return true;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                _isConnected = false;
                _serverName = string.Empty;
                this.UpdateStatus("Connection Failed!");
                return false;
            }
            finally
            {
                this.EnableMenus();
            }
        }

        private void RefreshUsers()
        {
            if (tvwItem.Nodes.Count == 0) return;
            var userList = SystemCoreInteractDomain.GetUserList(_serverName, _credentials).OrderBy(x => x.UserName);

            #region Users

            var root = tvwItem.Nodes[0];

            //Add the root user node if need be
            if (root.Nodes.Count == 1)
            {
                var userListNode = root.Nodes.Add("Users");
                userListNode.Tag = "USERLIST";
                userListNode.ImageIndex = 4;
                userListNode.SelectedImageIndex = userListNode.ImageIndex;
                userListNode.Nodes.Clear();
            }
            

            foreach (var item in userList)
            {
                this.AddTreeNode(item);
            }

            #endregion

        }

        private void AddTreeNode(UserCredentials item)
        {
            try
            {
                var newNode = new TreeNode();
                newNode.Text = item.UserName;
                newNode.Tag = item;
                newNode.ImageIndex = 6;
                newNode.SelectedImageIndex = newNode.ImageIndex;
                ServerUsersNode.Nodes.Add(newNode);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                throw;
            }
        }

        private void AddTreeNode(IRemotingObject item)
        {
            try
            {
                var newNode = new TreeNode();
                newNode.Text = item.Repository.Name;
                newNode.Tag = item;
                newNode.ImageIndex = (item.IsLoaded ? 0 : 1);
                newNode.SelectedImageIndex = newNode.ImageIndex;
                ServerRepositoriesNode.Nodes.Add(newNode);

                if (item.Repository != null)
                {
                    var dList = new List<FieldDefinition>();
                    foreach (var loop in item.Repository.DimensionList.OrderBy(x => x.Name))
                    {
                        dList.Add(loop);
                        var newSubItem = new TreeNode(loop.Name);
                        newSubItem.ImageIndex = 3;
                        newSubItem.SelectedImageIndex = newSubItem.ImageIndex;
                        newNode.Nodes.Add(newSubItem);
                    }

                    foreach (var loop in item.Repository.FieldList.Where(x => !dList.Contains(x)).OrderBy(x => x.Name))
                    {
                        var newSubItem = new TreeNode(loop.Name);
                        newSubItem.ImageIndex = 2;
                        newSubItem.SelectedImageIndex = newSubItem.ImageIndex;
                        newNode.Nodes.Add(newSubItem);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                throw;
            }
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
            menuFileConnect.Enabled = !_isConnected;
            menuFileDisconnect.Enabled = _isConnected;

            cmdNew.Enabled = _isConnected;
            cmdLoadSchema.Enabled = _isConnected;
            cmdCreateFromTemplate.Enabled = _isConnected;
            cmdCode.Enabled = (_activeRepositoryList.Count == 1);
            cmdRefresh.Enabled = _isConnected;

            menuSep1.Visible = _isConnected && (_currentRepositoryList.Count > 0);
            menuMainServerProp.Enabled = _isConnected;
            menuMainUserList.Enabled = _isConnected;
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

        #region Properties

        public IRemotingObject ActiveRepository
        {
            get
            {
                if (this.SelectedRepositoryNode == null) return null;
                return this.SelectedRepositoryNode.Tag as IRemotingObject;
            }
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
            this.tvwItem.Enabled = false;
            this.pnlContent.Enabled = false;
            _progress.Show();
            Application.DoEvents();
        }

        private void EnableUI()
        {
            if (_progress != null)
                _progress.Close();
            this.MainMenu.Enabled = true;
            this.MainToolbar.Enabled = true;
            this.tvwItem.Enabled = true;
            this.pnlContent.Enabled = true;
        }

        private void OpenConnection()
        {
            var F = new ServerConnectionForm();
            if (F.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                _credentials = F.Credentials;
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
                this.AddTreeNode(item);
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
                        this.AddTreeNode(item);

                        using (var factory = SystemCoreInteractDomain.GetFactory(_serverName))
                        {
                            var server = factory.CreateChannel();
                            var list = SystemCoreInteractDomain.GetRepositoryPropertyList(_serverName, _credentials);
                            if (list.Count(x => x.Repository.ID == item.Repository.ID) > 0)
                            {
                                MessageBox.Show("The repository already exists.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                            server.SaveRepository(item.Repository, _credentials);
                        }
                        var n = GetRepositoryNode(item);
                        n.Text = item.Repository.Name;
                    }

                    RefreshFromServer();
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
                    this.AddTreeNode(item);

                    using (var factory = SystemCoreInteractDomain.GetFactory(_serverName))
                    {
                        var server = factory.CreateChannel();
                        server.SaveRepository(item.Repository, _credentials);
                    }
                    var n = GetRepositoryNode(item);
                    n.Text = item.Repository.Name;

                    RefreshFromServer();
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

            //var F = new ExportDataForm(_serverName);
            //if (F.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //{
            //  this.RefreshFromServer();
            //}
        }

        private void menuMainUserList_Click(object sender, EventArgs e)
        {
            if (!_isConnected) return;
            try
            {
                var F = new UserListForm(_serverName, _credentials, _publicKey);
                if (F.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    RefreshUsers();
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
            if (!_isConnected) return;
            if (_activeRepositoryList.Count != 1) return;
            try
            {
                var repository = _activeRepositoryList.First();
                using (var factory = SystemCoreInteractDomain.GetFactory(_serverName))
                {
                    var server = factory.CreateChannel();
                    var stats = server.GetRepositoryStats(_credentials, repository.Repository.ID, DateTime.Now.AddDays(-1), DateTime.Now);

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
            var F = new ServerInfoForm(_serverName, _credentials, _publicKey, _currentRepositoryList);
            F.ShowDialog();
        }

        private void cmdExportSchema_Click(object sender, EventArgs e)
        {
            try
            {
                if (!_isConnected) return;
                if (_activeRepositoryList.Count == 0) return;
                var active = _activeRepositoryList.First();
                var dialog = new SaveFileDialog();
                dialog.Title = "Export Schema";
                dialog.FileName = active.Repository.Name + ".celeriq";
                dialog.Filter = "Schema Files (*.celeriq)|*.celeriq";
                dialog.FilterIndex = 0;
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    using (var factory = SystemCoreInteractDomain.GetFactory(_serverName))
                    {
                        var server = factory.CreateChannel();
                        server.ExportSchema(active.Repository.ID, _credentials, dialog.FileName);
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
            if (!_isConnected) return;
            if (this.SelectedRepositoryNode != null)
            {
                var active = this.ActiveRepository;
                try
                {
                    Clipboard.SetText(active.Repository.ToXml());
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
            if (!_isConnected) return;
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
                Celeriq.ManagementStudio.Objects.ConnectionCache.CreateRepository(_serverName, xml, _credentials);
                this.RefreshFromServer();
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
                var config = Celeriq.ManagementStudio.Objects.ClientCodeHelper.GenerateClientConfig(active.Repository, _serverName);
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
            this.RefreshFromServer();
        }

        private void tvwItem_AfterSelect(object sender, TreeViewEventArgs e)
        {
            pnlContent.Controls.Clear();
            _activeRepositoryList.Clear();
            if (e.Node != null)
            {
                if (e.Node.Tag == "REPOSITORYLIST")
                {
                    SelectUIRepositoryList(_currentRepositoryList);
                }
                else
                {
                    var node = e.Node;
                    var cache = node.Tag as IRemotingObject;
                    if (cache == null && node.Parent != null)
                        cache = node.Parent.Tag as IRemotingObject;

                    if (cache != null)
                    {
                        _activeRepositoryList.Add(cache);
                        SelectUIRepository(cache);
                    }
                }
            }
            EnableMenus();
        }

        private void SelectUIRepository(IRemotingObject cache)
        {
            pnlContent.Controls.Clear();
            var ctrl = new RepositoryPropertySheet();
            ctrl.Populate(cache);
            pnlContent.Controls.Add(ctrl);
            ctrl.Dock = DockStyle.Fill;
        }

        private void SelectUIRepositoryList(List<IRemotingObject> list)
        {
            pnlContent.Controls.Clear();
            var ctrl = new ServerPropertySheet();
            ctrl.Populate(list);
            pnlContent.Controls.Add(ctrl);
            ctrl.Dock = DockStyle.Fill;
            ctrl.RepositoryDeleted += ctrl_RepositoryDeleted;
            ctrl.RepositorySelected += ctrl_RepositorySelected;
            ctrl.MouseUp += ctrl_MouseUp;
        }

        private void DeleteRepository(IRemotingObject repository)
        {
            try
            {
                using (var factory = SystemCoreInteractDomain.GetFactory(_serverName))
                {
                    var server = factory.CreateChannel();

                    var r = _currentRepositoryList.FirstOrDefault(x => x.Repository.ID == repository.Repository.ID);
                    _currentRepositoryList.Remove(r);
                    if (_activeRepositoryList.Contains(r)) _activeRepositoryList.Remove(r);

                    server.DeleteRepository(repository.Repository, _credentials);
                    var n = GetRepositoryNode(repository);
                    if (n != null) n.Remove();
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

        private void tvwItem_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != System.Windows.Forms.MouseButtons.Right)
                return;

            if (this.SelectedRepositoryNode != null)
            {
                ShowContextMenu(tvwItem, e);
            }
        }

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
                    using (var factory = SystemCoreInteractDomain.GetFactory(_serverName))
                    {
                        var server = factory.CreateChannel();
                        active = server.SaveRepository(active.Repository, _credentials);
                    }

                    var n = GetRepositoryNode(active);
                    n.Tag = active;
                    n.Text = active.Repository.Name;
                    n.ImageIndex = 1; //(active.IsRunning ? 0 : 1);
                    n.SelectedImageIndex  =n.ImageIndex;
                    _currentRepositoryList.Add(active);

                    EnableUI();
                    this.UpdateStatus();
                    tvwItem.SelectedNode = n;

                }

            }
        }

        #endregion

        private void menuMainExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pnlContent_Paint(object sender, PaintEventArgs e)
        {

        }

    }
}