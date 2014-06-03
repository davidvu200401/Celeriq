using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Celeriq.Common;

namespace Celeriq.Profiler
{
    public partial class MainForm : Form
    {
        private Timer _timer = new Timer();
        private string _serverName = string.Empty;
        private long _lastProfileId = 0;
        private bool _isRunning = false;
        private BaseRemotingObject _repository = null;

        public MainForm()
        {
            PublicKey = string.Empty;
            Credentials = null;
            InitializeComponent();

            this.AutoScroll = true;

            lvwItem.SelectedIndexChanged += lvwItem_SelectedIndexChanged;

            _timer.Stop();

            lvwItem.Columns.Add("Action", 100);
            lvwItem.Columns.Add("Start time", 120);
            lvwItem.Columns.Add("Duration", 100);
            lvwItem.Columns.Add("Items affected", 100, HorizontalAlignment.Right);
            lvwItem.Columns.Add("Query", 400);
        }

        public void InitSize()
        {
            this.Size = new System.Drawing.Size((int)(this.MdiParent.Size.Width * 0.8), (int)(this.MdiParent.Size.Height * .8));
        }

        public void ClearAll()
        {
            lvwItem.Items.Clear();
        }

        private void lvwItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.PopulateItems();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            using (var factory = SystemCoreInteractDomain.GetFactory(_serverName))
            {
                var server = factory.CreateChannel();
                var list = server.GetProfile(this.Repository.Repository.ID, this.Credentials, _lastProfileId).ToList();
                if (list.Count > 0)
                    _lastProfileId = list.Max(x => x.ProfileId);

                foreach (var item in list)
                {
                    var lvItem = new ListViewItem(item.Action.ToString());
                    lvItem.SubItems.Add(item.StartTime.ToString());
                    lvItem.SubItems.Add(item.Duration.ToString());
                    lvItem.SubItems.Add(item.ItemsAffected.ToString());
                    lvItem.SubItems.Add(item.Query);
                    lvwItem.Items.Add(lvItem);
                }

                if (this.AutoScroll && list.Count > 0 && lvwItem.Items.Count > 0)
                {
                    lvwItem.SelectedItems.Clear();
                    lvwItem.Items[lvwItem.Items.Count - 1].Selected = true;
                    lvwItem.Items[lvwItem.Items.Count - 1].EnsureVisible();
                }

                this.PopulateItems();
            }
        }

        public bool AutoScroll { get; set; }

        public BaseRemotingObject Repository
        {
            get { return _repository; }
            set
            {
                _repository = value;
                if (value != null && value.Repository != null)
                    this.Text = value.Repository.Name;
                else
                    this.Text = string.Empty;
            }
        }

        public UserCredentials Credentials { get; set; }
        public string PublicKey { get; set; }

        public bool IsRunning
        {
            get { return _isRunning; }
            set
            {
                _isRunning = value;
                if (_isRunning)
                    _timer.Start();
                else
                    _timer.Stop();
            }
        }

        public void StartProfiling()
        {
            _timer = new Timer();
            _timer.Tick += new EventHandler(timer_Tick);
            _timer.Interval = 1000;
            _timer.Start();
        }

        public string ServerName
        {
            get { return _serverName; }
            set
            {
                _serverName = value;
                toolConnection.Text = _serverName;
            }
        }

        private void PopulateItems()
        {
            if (lvwItem.SelectedItems.Count == 0)
                toolLine.Text = string.Empty;
            else
                toolLine.Text = "Line: " + lvwItem.SelectedItems[0].Index.ToString("###,###,##0");

            toolRows.Text = "Rows: " + lvwItem.Items.Count.ToString("###,###,##0");
        }

    }
}