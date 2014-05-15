using System;
using System.Runtime.Serialization;

namespace Celeriq.Common
{
    [Serializable]
    [DataContract()]
    public class SystemCredentials : UserCredentials
    {
        public SystemCredentials()
        {
            this.UserId = Guid.NewGuid();
        }

        [DataMember]
        public Guid UserId { get; set; }
    }
}