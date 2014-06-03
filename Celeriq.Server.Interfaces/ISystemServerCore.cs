using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Runtime.Serialization;
using Celeriq.Common;

namespace Celeriq.Server.Interfaces
{
    [ServiceContract]
    public interface ISystemServerCore : ISystemCore
    {
        [DataMember]
        StorageTypeConstants Storage { get; }
    }

    public class RepositoryStatItem
    {
        public long ItemCount { get; set; }
        public long DiskSize { get; set; }
        public long MemorySize { get; set; }
    }
}