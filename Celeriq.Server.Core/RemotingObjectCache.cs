﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Remoting.Channels;
using System.ServiceModel;
using System.Runtime.Serialization;
using Celeriq.Utilities;
using Celeriq.Common;

namespace Celeriq.Server.Core
{
    [Serializable]
    [DataContract]
    public class RemotingObjectCache : BaseRemotingObject, IRemotingObject
    {
        public RemotingObjectCache()
        {
        }

        internal Celeriq.Server.Interfaces.IRepository ServiceInstance { get; set; }

        public bool GetIsLoaded()
        {
            return (this.ServiceInstance != null && this.ServiceInstance.IsLoaded);
        }

    }
}