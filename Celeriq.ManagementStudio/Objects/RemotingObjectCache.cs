using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Remoting.Channels;
using System.ServiceModel;
using System.Runtime.Serialization;
using Celeriq.Utilities;
using Celeriq.Common;

namespace Celeriq.ManagementStudio.Objects
{
	[Serializable]
	[DataContract]
	internal class RemotingObjectCache : Celeriq.Server.Interfaces.IRemotingObject
	{
		public RemotingObjectCache()
		{
		}

		[DataMember]
		public RepositoryDefinition Repository { get; set; }

		[DataMember]
		public bool IsRunning { get; set; }

		[DataMember]
		public long DataDiskSize { get; set; }

		[DataMember]
		public long DataMemorySize { get; set; }

		[DataMember]
		public long ItemCount { get; set; }

		[DataMember]
		public long VersionHash { get; set; }

		[DataMember]
		public bool IsLoaded;

	}
}
