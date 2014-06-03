using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Celeriq.Common
{
	[Serializable]
	[DataContract]
	public class DataSortItem
	{
		public string Name;
		public SortDirectionConstants Direction;
	}
}
