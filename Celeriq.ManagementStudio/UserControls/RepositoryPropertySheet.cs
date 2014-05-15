using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Celeriq.ManagementStudio.Objects;
using Celeriq.Common;

namespace Celeriq.ManagementStudio.UserControls
{
    internal partial class RepositoryPropertySheet : UserControl
    {
        public RepositoryPropertySheet()
        {
            InitializeComponent();
            this.Resize += new EventHandler(RepositoryPropertySheet_Resize);
            lvwItem.Columns.Clear();
            lvwItem.Columns.Add(new ColumnHeader() {Text = "Property", Width = 160});
            lvwItem.Columns.Add(new ColumnHeader() {Text = "Value", Width = 290});
        }

        public void Populate(IRemotingObject repositoryCache)
        {
            //Add in reverse order
            this.AddPropertyItem("ID:", repositoryCache.Repository.ID.ToString());
            this.AddPropertyItem("Name:", repositoryCache.Repository.Name);
            this.AddPropertyItem("Data Disk Size:", Celeriq.ManagementStudio.Objects.Utilities.ToSizeDisplay(repositoryCache.DataDiskSize));
            this.AddPropertyItem("Data Memory Size:", Celeriq.ManagementStudio.Objects.Utilities.ToSizeDisplay(repositoryCache.DataMemorySize));
            this.AddPropertyItem("Version Hash:", repositoryCache.VersionHash.ToString());
            this.AddPropertyItem("Total Items:", repositoryCache.ItemCount.ToString("###,###,###,##0"));
            var ds = "(unknown)";
            if (repositoryCache.Repository.CreatedDate > DateTime.MinValue)
                ds = repositoryCache.Repository.CreatedDate.ToString("yyyy-MM-dd HH:mm");
            this.AddPropertyItem("Created Date:", ds);
        }

        private void AddPropertyItem(string header, string data)
        {
            var newItem = new ListViewItem();
            newItem.Text = header;
            newItem.SubItems.Add(data);
            lvwItem.Items.Add(newItem);

        }

        private void RepositoryPropertySheet_Resize(object sender, EventArgs e)
        {
            //pnlLeft.Width = this.Width / 2;
        }

    }
}