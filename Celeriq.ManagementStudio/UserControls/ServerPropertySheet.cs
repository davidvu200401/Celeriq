#pragma warning disable 0168
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Celeriq.ManagementStudio.EventArguments;
using System.Collections;
using Celeriq.ManagementStudio.Objects;
using Celeriq.Common;
using Celeriq.Utilities;

namespace Celeriq.ManagementStudio.UserControls
{
    internal partial class ServerPropertySheet : UserControl
    {
        public event EventHandler<RepositoryListEventArgs> RepositorySelected;
        public event EventHandler<RepositoryListEventArgs> RepositoryDeleted;
        public event EventHandler<MouseEventArgs> MouseUp;
        public event EventHandler<TextEventArgs> StatusChanged;

        private DateTime _lastUpdate = DateTime.MinValue;
        private Timer _timer;
        private List<IRemotingObject> _list = new List<IRemotingObject>();
        private ListViewColumnSorter _sorter = null;

        public PagingInfo PageInfo { get; set; }
        public UserCredentials Credentials { get; set; }
        public string ServerName { get; set; }
        public bool IsConnected { get; set; }

        public List<IRemotingObject> RepositoryList 
        {
            get { return _list; }
        }

        public IRemotingObject SelectedRepository
        {
            get
            {
                if (lvwItem.SelectedItems.Count == 0) return null;
                return lvwItem.SelectedItems[0].Tag as IRemotingObject;
            }
        }

        public List<IRemotingObject> SelectedRepositoryList
        {
            get
            {
                var retval = new List<IRemotingObject>();
                if (lvwItem.SelectedItems.Count == 0) return retval;
                foreach (ListViewItem item in lvwItem.SelectedItems)
                    retval.Add(item.Tag as IRemotingObject);
                return retval;
            }
        }

        private void DelayedReload()
        {
            _lastUpdate = DateTime.Now;
            if (!_timer.Enabled)
                _timer.Start();
        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            if (DateTime.Now.Subtract(_lastUpdate).TotalMilliseconds > 400)
            {
                _timer.Stop();
                this.PageInfo.PageOffset = 1;
                RefreshFromServer();
            }
        }

        public bool RefreshFromServer()
        {
            if (string.IsNullOrEmpty(this.ServerName))
            {
                this.PageInfo = new PagingInfo();
                lvwItem.Items.Clear();
                txtPageNo.Text = "1";
                txtFilter.Text = string.Empty;
                lblDetails.Text = "0 items";
                pnlAction.Enabled = false;
                this.IsConnected = false;
                return false;
            }

            try
            {
                pnlAction.Enabled = true;
                _list.Clear();
                lvwItem.Items.Clear();

                this.PageInfo.SortAsc = (_sorter.Order == SortOrder.Ascending);
                if (_sorter.SortColumn == -1) this.PageInfo.SortField = string.Empty;
                else this.PageInfo.SortField = lvwItem.Columns[_sorter.SortColumn].Tag as string;

                this.PageInfo.Keyword = txtFilter.Text.Trim();
                var repositoryList = SystemCoreInteractDomain.GetRepositoryPropertyList(this.ServerName, this.Credentials, this.PageInfo)
                    .OrderBy(x => x.Repository.Name)
                    .ToList();

                this.PageInfo.TotalItemCount = SystemCoreInteractDomain.GetRepositoryCount(this.ServerName, this.Credentials, this.PageInfo);
                
                _list.AddRange(repositoryList);

                _list.ForEach(x => this.AddPropertyItem(x));

                var lastIndex = this.PageInfo.StartItemIndex + this.PageInfo.RecordsPerPage;
                if (lastIndex >= this.PageInfo.TotalItemCount) lastIndex = this.PageInfo.TotalItemCount;
                lblDetails.Text = (this.PageInfo.StartItemIndex + 1).ToString("###,###,###,##0") + "-" + lastIndex.ToString("###,###,###,##0") + " of " + this.PageInfo.TotalItemCount;

                if (this.PageInfo.TotalItemCount == 0)
                    lblDetails.Text = "0 items";

                txtPageNo.Text = this.PageInfo.PageOffset.ToString("###,###,##0");
                this.IsConnected = true;

                this.OnRepositorySelected(new RepositoryListEventArgs(new List<IRemotingObject>()));
                this.OnStatusChanged(new TextEventArgs());
                //this.EnableMenus();
                return true;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                this.IsConnected = false;
                this.ServerName = string.Empty;
                this.OnStatusChanged(new TextEventArgs() { Text = "Connection Failed!" });
                return false;
            }
            finally
            {
                //this.EnableMenus();
            }
        }

        protected void OnRepositorySelected(RepositoryListEventArgs e)
        {
            if (this.RepositorySelected != null)
                this.RepositorySelected(this, e);
        }

        protected void OnRepositoryDeleted(RepositoryListEventArgs e)
        {
            if (this.RepositoryDeleted != null)
                this.RepositoryDeleted(this, e);
        }

        protected void OnMouseUp(MouseEventArgs e)
        {
            if (this.MouseUp != null)
                this.MouseUp(this, e);
        }

        protected void OnStatusChanged(TextEventArgs e)
        {
            if (this.StatusChanged != null)
                this.StatusChanged(this, e);
        }

        public void AddRepository(IRemotingObject item)
        {
            //TODO
        }

        public void RemoveRepository(IRemotingObject item)
        {
            //TODO
        }

        public void UpdateRepository(IRemotingObject item)
        {
            RefreshFromServer();
        }

        public ServerPropertySheet()
        {
            InitializeComponent();

            _timer = new Timer() { Enabled = false };
            _timer.Tick += _timer_Tick;

            this.PageInfo = new PagingInfo();
            this.PageInfo.RecordsPerPage = 10;
            this.PageInfo.PageOffset = 1;

            cboRPP.Items.Add(10);
            cboRPP.Items.Add(20);
            cboRPP.Items.Add(30);
            cboRPP.Items.Add(40);
            cboRPP.Items.Add(50);
            cboRPP.SelectedIndex = 0;

            this.Resize += new EventHandler(RepositoryPropertySheet_Resize);
            lvwItem.ItemSelectionChanged += lvwItem_ItemSelectionChanged;
            lvwItem.KeyDown += lvwItem_KeyDown;
            lvwItem.MouseUp += lvwItem_MouseUp;
            _sorter = new ListViewColumnSorter(lvwItem);
            lvwItem.ListViewItemSorter = _sorter;
            lvwItem.ColumnClick += lvwItem_ColumnClick;
            txtFilter.TextChanged += txtFilter_TextChanged;

            lvwItem.Columns.Clear();
            lvwItem.Columns.Add(new ColumnHeader() { Text = "ID", Width = 100, Tag = "id" });
            lvwItem.Columns.Add(new ColumnHeader() { Text = "Name", Width = 100, Tag = "name" });
            lvwItem.Columns.Add(new ColumnHeader() { Text = "Disk Size", Width = 100, Tag = "disksize" });
            lvwItem.Columns.Add(new ColumnHeader() { Text = "Memory Size", Width = 100, Tag = "memorysize" });
            lvwItem.Columns.Add(new ColumnHeader() { Text = "Version Hash", Width = 100, Tag = "hash" });
            lvwItem.Columns.Add(new ColumnHeader() { Text = "Count", Width = 100, Tag = "count" });
            lvwItem.Columns.Add(new ColumnHeader() { Text = "Created", Width = 100, Tag = "created" });
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            DelayedReload();
        }

        private void lvwItem_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            // Determine if clicked column is already the column that is being sorted.
            if (_sorter == null) return;

            if (e.Column == _sorter.SortColumn)
            {
                // Reverse the current sort direction for this column.
                if (_sorter.Order == SortOrder.Ascending)
                {
                    _sorter.Order = SortOrder.Descending;
                }
                else
                {
                    _sorter.Order = SortOrder.Ascending;
                }
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending.
                _sorter.SortColumn = e.Column;
                _sorter.Order = SortOrder.Ascending;
            }

            // Perform the sort with these new sort options.
            lvwItem.Sort();
            RefreshFromServer();
        }

        private void lvwItem_MouseUp(object sender, MouseEventArgs e)
        {
            this.OnMouseUp(e);
        }

        private void lvwItem_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (lvwItem.SelectedItems.Count > 0)
                {
                    if (MessageBox.Show("Do you wish to delete the selected items?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        var dList = new List<ListViewItem>();
                        foreach (ListViewItem item in lvwItem.SelectedItems)
                        {
                            dList.Add(item);
                        }
                        this.OnRepositoryDeleted(new RepositoryListEventArgs(dList.Select(x => x.Tag).Cast<IRemotingObject>().ToList()));
                        dList.ForEach(x => lvwItem.Items.Remove(x));
                    }
                }
            }
            else if (e.KeyCode == Keys.A && e.Control)
            {
                foreach (ListViewItem item in lvwItem.Items)
                {
                    item.Selected = true;
                }
            }
        }

        private void lvwItem_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (lvwItem.SelectedItems.Count > 0)
            {
                var dList = new List<ListViewItem>();
                foreach (ListViewItem item in lvwItem.SelectedItems)
                {
                    dList.Add(item);

                }
                this.OnRepositorySelected(new RepositoryListEventArgs(dList.Select(x => x.Tag).Cast<IRemotingObject>().ToList()));
            }
        }

        //private void Reload()
        //{
        //    txtPageNo.Text = this.PageInfo.PageOffset.ToString();
        //    lvwItem.BeginUpdate();
        //    try
        //    {
        //        IRemotingObject selected = null;
        //        if (lvwItem.SelectedItems.Count > 0)
        //        {
        //            selected = lvwItem.SelectedItems[0].Tag as IRemotingObject;
        //        }

        //        lvwItem.Items.Clear();
        //        var filter = txtFilter.Text.Trim().ToLower();
        //        if (string.IsNullOrEmpty(filter))
        //        {
        //            foreach (var item in _list.OrderBy(x => x.Repository.Name))
        //                this.AddPropertyItem(item);
        //        }
        //        else
        //        {
        //            foreach (var item in _list.Where(x => x.Repository.Name.ToLower().Contains(filter) || x.Repository.ID.ToString().ToLower().Contains(filter)).OrderBy(x => x.Repository.Name))
        //                this.AddPropertyItem(item);
        //        }

        //        if (selected != null)
        //        {
        //            var item = ((IEnumerable) lvwItem.Items).OfType<ListViewItem>().ToList().FirstOrDefault(x => x.Tag == selected);
        //            if (item != null)
        //                item.Selected = true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //    finally
        //    {
        //        lvwItem.EndUpdate();
        //    }
        //}

        private void AddPropertyItem(IRemotingObject item)
        {
            var newItem = new ListViewItem();
            newItem.Tag = item;

            //Add in reverse order
            newItem.ImageIndex = (item.IsLoaded ? 0 : 1);
            newItem.Text = item.Repository.ID.ToString();
            newItem.SubItems.Add(item.Repository.Name);
            newItem.SubItems.Add(Celeriq.ManagementStudio.Objects.Utilities.ToSizeDisplay(item.DataDiskSize));
            newItem.SubItems.Add(Celeriq.ManagementStudio.Objects.Utilities.ToSizeDisplay(item.DataMemorySize));
            newItem.SubItems.Add(item.VersionHash.ToString());
            newItem.SubItems.Add(item.ItemCount.ToString("###,###,###,##0"));
            var ds = "(unknown)";
            if (item.Repository.CreatedDate > DateTime.MinValue)
                ds = item.Repository.CreatedDate.ToString("yyyy-MM-dd HH:mm:ss");
            newItem.SubItems.Add(ds);
            lvwItem.Items.Add(newItem);
        }

        private void RepositoryPropertySheet_Resize(object sender, EventArgs e)
        {
            //pnlLeft.Width = this.Width / 2;
        }

        #region Column Sorter

        public class ListViewColumnSorter : IComparer
        {
            /// <summary>
            /// Specifies the column to be sorted
            /// </summary>
            private int ColumnToSort;

            /// <summary>
            /// Specifies the order in which to sort (i.e. 'Ascending').
            /// </summary>
            private SortOrder OrderOfSort;

            /// <summary>
            /// Case insensitive comparer object
            /// </summary>
            private CaseInsensitiveComparer ObjectCompare;

            private ListView _ctrl = null;

            /// <summary>
            /// Class constructor.  Initializes various elements
            /// </summary>
            public ListViewColumnSorter(ListView ctrl)
            {
                _ctrl = ctrl;

                // Initialize the column to '0'
                ColumnToSort = 0;

                // Initialize the sort order to 'none'
                OrderOfSort = SortOrder.None;

                // Initialize the CaseInsensitiveComparer object
                ObjectCompare = new CaseInsensitiveComparer();
            }

            /// <summary>
            /// This method is inherited from the IComparer interface.  It compares the two objects passed using a case insensitive comparison.
            /// </summary>
            /// <param name="x">First object to be compared</param>
            /// <param name="y">Second object to be compared</param>
            /// <returns>The result of the comparison. "0" if equal, negative if 'x' is less than 'y' and positive if 'x' is greater than 'y'</returns>
            public int Compare(object x, object y)
            {
                int compareResult;
                ListViewItem listviewX, listviewY;

                // Cast the objects to be compared to ListViewItem objects
                listviewX = (ListViewItem) x;
                listviewY = (ListViewItem) y;

                var a = listviewX.SubItems[ColumnToSort].Text;
                var b = listviewY.SubItems[ColumnToSort].Text;
                var item1 = listviewX.Tag as IRemotingObject;
                var item2 = listviewY.Tag as IRemotingObject;

                if (this.SortColumn != -1)
                {
                    if (_ctrl.Columns[this.SortColumn].Text == "Disk Size")
                    {
                        a = item1.DataDiskSize.ToString("0000000000");
                        b = item2.DataDiskSize.ToString("0000000000");
                    }
                    else if (_ctrl.Columns[this.SortColumn].Text == "Memory Size")
                    {
                        a = item1.DataMemorySize.ToString("0000000000");
                        b = item2.DataMemorySize.ToString("0000000000");
                    }
                    else if (_ctrl.Columns[this.SortColumn].Text == "Version Hash")
                    {
                        a = item1.VersionHash.ToString("0000000000");
                        b = item2.VersionHash.ToString("0000000000");
                    }
                    else if (_ctrl.Columns[this.SortColumn].Text == "ID")
                    {
                        a = item1.Repository.ID.ToString();
                        b = item2.Repository.ID.ToString();
                    }
                    else if (_ctrl.Columns[this.SortColumn].Text == "Name")
                    {
                        a = item1.Repository.Name;
                        b = item2.Repository.Name;
                    }
                    else if (_ctrl.Columns[this.SortColumn].Text == "Count")
                    {
                        a = item1.ItemCount.ToString("0000000000");
                        b = item2.ItemCount.ToString("0000000000");
                    }
                    else if (_ctrl.Columns[this.SortColumn].Text == "Created")
                    {
                        a = item1.Repository.CreatedDate.ToString("yyyy-MM-dd HH:mm:ss");
                        b = item2.Repository.CreatedDate.ToString("yyyy-MM-dd HH:mm:ss");
                    }
                }

                // Compare the two items
                compareResult = ObjectCompare.Compare(a, b);

                // Calculate correct return value based on object comparison
                if (OrderOfSort == SortOrder.Ascending)
                {
                    // Ascending sort is selected, return normal result of compare operation
                    return compareResult;
                }
                else if (OrderOfSort == SortOrder.Descending)
                {
                    // Descending sort is selected, return negative result of compare operation
                    return (-compareResult);
                }
                else
                {
                    // Return '0' to indicate they are equal
                    return 0;
                }
            }

            /// <summary>
            /// Gets or sets the number of the column to which to apply the sorting operation (Defaults to '0').
            /// </summary>
            public int SortColumn
            {
                set { ColumnToSort = value; }
                get { return ColumnToSort; }
            }

            /// <summary>
            /// Gets or sets the order of sorting to apply (for example, 'Ascending' or 'Descending').
            /// </summary>
            public SortOrder Order
            {
                set { OrderOfSort = value; }
                get { return OrderOfSort; }
            }

        }

        #endregion

        private void cmdPageBack_Click(object sender, EventArgs e)
        {
            if (this.PageInfo.PageOffset > 1)
            {
                this.PageInfo.PageOffset--;
                RefreshFromServer();
            }
        }

        private void cmdPageNext_Click(object sender, EventArgs e)
        {
            if (this.PageInfo.PageOffset < this.PageInfo.PageCount)
            {
                this.PageInfo.PageOffset++;
                RefreshFromServer();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.PageInfo.RecordsPerPage = (int)cboRPP.SelectedItem;
            this.PageInfo.PageOffset = 1;
            RefreshFromServer();
        }

    }
}