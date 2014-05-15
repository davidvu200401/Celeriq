using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using Celeriq.Common;

namespace Celeriq.Utilities
{
	[Serializable]
	[DataContract()]
	public class DiagnosticItem
	{
		[DataMember]
		public RepositoryItem LastInserted { get; set; }
		[DataMember]
		public long LastRecordIndex { get; set; }
		[DataMember]
		public long RecordCount { get; set; }
	}
}