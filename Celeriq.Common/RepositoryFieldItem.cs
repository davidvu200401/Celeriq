using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Celeriq.Common
{
	[Serializable]
	[DataContract()]
	public class RepositoryFieldItem
	{
		[DataMember]
		public string Name { get; set; }
		[DataMember]
		public object Value { get; set; }
	}
}
