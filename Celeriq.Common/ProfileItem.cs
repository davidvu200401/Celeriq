using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Celeriq.Common
{
    /// <summary>
    /// This is a log item of an action performed by the engine
    /// </summary>
    [Serializable]
    [DataContract()]
    public class ProfileItem
    {
        [DataMember]
        public long ProfileId { get; set; }
        [DataMember]
        public RepositoryActionConstants Action { get; set; }
        [DataMember]
        public Guid RepositoryId { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public DateTime StartTime { get; set; }
        [DataMember]
        public int Duration { get; set; }
        [DataMember]
        public int ItemsAffected { get; set; }
        [DataMember]
        public string Query { get; set; }
    }
}