using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Celeriq.Common
{
    [Serializable]
    public class RealtimeStats
    {
        [XmlElement]
        [DataMember]
        public DateTime Timestamp { get; set; }

        [XmlElement]
        [DataMember]
        public long MemoryUsageTotal { get; set; }

        [XmlElement]
        [DataMember]
        public long MemoryUsageAvailable { get; set; }

        [XmlElement]
        [DataMember]
        public long MemoryUsageProcess { get; set; }

        [XmlElement]
        [DataMember]
        public int RepositoryInMem { get; set; }

        [XmlElement]
        [DataMember]
        public int RepositoryLoadDelta { get; set; }

        [XmlElement]
        [DataMember]
        public int RepositoryUnloadDelta { get; set; }

        [XmlElement]
        [DataMember]
        public int RepositoryTotal { get; set; }

        [XmlElement]
        [DataMember]
        public int RepositoryCreateDelta { get; set; }

        [XmlElement]
        [DataMember]
        public int RepositoryDeleteDelta { get; set; }

        [XmlElement]
        [DataMember]
        public int ProcessorUsage { get; set; }
    }
}
