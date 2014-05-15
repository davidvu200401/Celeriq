using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Celeriq.Common
{
    public enum RepositoryActionConstants
    {
        Query = 1,
        LoadData = 2,
        SaveData = 3,
        UnloadData = 4,
        Reset = 5,
        ExportSchema = 6,
        //Backup = 7,
        //Restore = 8,
        //Compress = 9,
        Shutdown = 10,
        DeleteData = 11,
    }

    [Serializable]
    public class RepositorySummmaryStats
    {
        public override string ToString()
        {
            return this.RepositoryId + " | " + this.ActionType.ToString() + " | " + this.Elapsed + " | " + this.ItemCount;
        }

        [XmlElement]
        [DataMember]
        public Guid RepositoryId { get; set; }

        [XmlElement]
        [DataMember]
        public RepositoryActionConstants ActionType { get; set; }

        [XmlElement]
        [DataMember]
        public int Elapsed { get; set; }

        [XmlElement]
        [DataMember]
        public int ItemCount { get; set; }
    }

}
