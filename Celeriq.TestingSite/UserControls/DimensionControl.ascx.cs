using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using Celeriq.Common;

namespace Celeriq.TestingSite.UserControls
{
	public partial class DimensionControl : System.Web.UI.UserControl
	{
		public void Populate(Celeriq.Common.DimensionItem dimension)
		{
			var sb = new StringBuilder();
			foreach (var refinment in dimension.RefinementList)
			{
				var query = new DataQuery() { PageName = "/GraphResults.aspx" };
				query.DimensionValueList.Add(refinment.DVIdx);
				sb.AppendLine("<a href=\"" + query.ToString() + "\">" + refinment.FieldValue + "</a>");
			}
		}

	}
}