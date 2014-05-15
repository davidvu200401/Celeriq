using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Celeriq.Common
{
	[Serializable]
	[DataContract]
	public class DataQueryItem
	{
		public string Name;
		public Celeriq.Common.ComparisonConstants Comparer;
		public object Value1;
		public object Value2;
	}
}
