using System;
using System.Linq;
using System.ServiceModel;
using System.Runtime.Serialization;
using Celeriq.Common;

namespace Celeriq.Common
{
    [Serializable]
    [DataContract]
    [KnownType(typeof(DimensionDefinition))]
    [KnownType(typeof(FieldDefinition))]
    [KnownType(typeof(IFieldDefinition))]
    [KnownType(typeof(GeoCode))]
    [KnownType(typeof(GeoCodeFieldFilter))]
    [KnownType(typeof(RepositorySchema))]
    public class BaseRemotingObject : IRemotingObject, ICloneable
    {
        [DataMember]
        public RepositorySchema Repository { get; set; }

        [DataMember]
        public long DataDiskSize { get; set; }

        [DataMember]
        public long DataMemorySize { get; set; }

        [DataMember]
        public long ItemCount { get; set; }

        [DataMember]
        public long VersionHash { get; set; }

        [DataMember]
        public bool IsLoaded { get; set; }

        [DataMember]
        public DateTime? LastUnloadTime { get; set; }

        public override string ToString()
        {
            if (this.Repository == null) return string.Empty;
            return this.Repository.Name;
        }

        #region ICloneable Members

        object ICloneable.Clone()
        {
            var retval = new BaseRemotingObject();
            retval.Repository = this.Repository;
            retval.DataDiskSize = this.DataDiskSize;
            retval.DataMemorySize = this.DataMemorySize;
            retval.ItemCount = this.ItemCount;
            retval.VersionHash = this.VersionHash;
            retval.IsLoaded = this.IsLoaded;
            return retval;
        }

        #endregion
    }
}