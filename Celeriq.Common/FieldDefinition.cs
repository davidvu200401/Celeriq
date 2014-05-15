using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Celeriq.Common
{
    [DataContract]
    [Serializable]
    [KnownType(typeof(DimensionDefinition))]
    public class FieldDefinition : IFieldDefinition
    {
        public FieldDefinition()
        {
            this.Length = 100;
            this.DataType = RepositorySchema.DataTypeConstants.String;
        }

        [DataMember]
        public virtual string Name { get; set; }

        [DataMember]
        public virtual RepositorySchema.FieldTypeConstants FieldType { get; set; }

        [DataMember]
        public virtual int Length { get; set; }

        [DataMember]
        public virtual RepositorySchema.DataTypeConstants DataType { get; set; }

        [DataMember]
        public virtual bool IsPrimaryKey { get; set; }

        [DataMember]
        public virtual bool AllowTextSearch { get; set; }

        public bool SortingSupported
        {
            get { return (this.DataType != RepositorySchema.DataTypeConstants.GeoCode) && (this.DataType != RepositorySchema.DataTypeConstants.List); }
        }

        public bool FilteringSupported
        {
            //get { return (this.DataType != RepositorySchema.DataTypeConstants.List); }
            get { return true; }
        }

        public override string ToString()
        {
            return this.Name + ", " + this.DataType.ToString();
        }

    }
}