using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Celeriq.AdminSite.UserControls
{
    public partial class RepositoryGridItemControl : System.Web.UI.UserControl
    {
        private int _itemIndex = 0;

        protected string ClassName
        {
            get { return (_itemIndex % 2 == 0 ? "" : "alternaterow"); }
        }

        public void Populate(Celeriq.Common.BaseRemotingObject dataItem, int itemIndex)
        {
            _itemIndex = itemIndex;

            lblID.Text = dataItem.Repository.ID.ToString();
            lblName.Text = dataItem.Repository.Name;
            lblDisk.Text = Celeriq.AdminSite.Objects.Utilities.ToSizeDisplay(dataItem.DataDiskSize);
            lblMemory.Text = Celeriq.AdminSite.Objects.Utilities.ToSizeDisplay(dataItem.DataMemorySize);
            lblHash.Text = dataItem.Repository.VersionHash.ToString();
            lblCount.Text = dataItem.ItemCount.ToString("###,###,##0");
            lblCreated.Text = dataItem.Repository.CreatedDate.ToString("yyyy-MM-dd HH:mm:ss");

            lblAction.Text = "<a hre='#' item-action='download' item-id='" + dataItem.Repository.ID + "'>Download</a>";

        }

    }
}