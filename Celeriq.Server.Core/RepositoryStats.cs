using Celeriq.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;

namespace Celeriq.Server.Core
{
    [Serializable]
    public class RepositoryStats : RepositorySummmaryStats
    {
        public override string ToString()
        {
            return this.RepositoryId + " | " + this.ActionType.ToString() + " | " + this.Elapsed + " | " + this.ItemCount;
        }

        [XmlElement]
        [DataMember]
        public DateTime Timestamp { get; set; }

    }
}