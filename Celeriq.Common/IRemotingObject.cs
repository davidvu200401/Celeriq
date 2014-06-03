using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Remoting.Channels;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace Celeriq.Common
{
    [ServiceContract]
    [ServiceKnownType(typeof (BaseRemotingObject))]
    public interface IRemotingObject
    {
        [DataMember]
        RepositorySchema Repository { get; set; }

        [DataMember]
        long DataDiskSize { get; set; }

        [DataMember]
        long DataMemorySize { get; set; }

        [DataMember]
        long ItemCount { get; set; }

        [DataMember]
        long VersionHash { get; set; }

        [DataMember]
        bool IsLoaded { get; set; }

    }
}