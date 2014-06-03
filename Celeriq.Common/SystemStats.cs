using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Celeriq.Common
{
    [Serializable]
    public class SystemStats
    {
        [XmlElement]
        [DataMember]
        public long TotalMemory { get; set; }

        [XmlElement]
        [DataMember]
        public string MachineName { get; set; }

        [XmlElement]
        [DataMember]
        public string OSVersion { get; set; }

        [XmlElement]
        [DataMember]
        public int ProcessorCount { get; set; }

        [XmlElement]
        [DataMember]
        public int RepositoryCount { get; set; }

        [XmlElement]
        [DataMember]
        public int InMemoryCount { get; set; }

        [XmlElement]
        [DataMember]
        public int TickCount { get; set; }

        [XmlElement]
        [DataMember]
        public long UsedMemory { get; set; }

        [XmlElement]
        [DataMember]
        public long UsedDisk { get; set; }

    }
}
