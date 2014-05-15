using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Celeriq.Common
{
    [DataContract]
    [Serializable]
    [KnownType(typeof(GeoCode))]
    [KnownType(typeof(GeoCodeFieldFilter))]
    public class DimensionDefinition : FieldDefinition
    {
        public const int DGROUP = 1000000; // Dimension groups are by 1 Million
        public const int DVALUEGROUP = DGROUP * 10; // Dimension groups are by 10 Million

        public DimensionDefinition()
            : base()
        {
            this.MultivalueComparison = RepositorySchema.MultivalueComparisonContants.Union;
        }

        [DataMember]
        public override bool IsPrimaryKey
        {
            get { return false; }
            set { ; }
        }

        [DataMember]
        public virtual long DIdx { get; set; }

        [DataMember]
        public virtual RepositorySchema.DimensionTypeConstants DimensionType { get; set; }

        [DataMember]
        public virtual long? NumericBreak { get; set; }

        [DataMember]
        public string Parent { get; set; }

        [DataMember]
        public override RepositorySchema.FieldTypeConstants FieldType
        {
            get { return RepositorySchema.FieldTypeConstants.Dimension; }
            set { ;}
        }

        //THIS NEED TO BE FIXED. THE DESERIALIZE DOES NOT WORK IF WE MAKE THIS A REAL PROPERTY!!!!!
        [DataMember(IsRequired = false)]
        public virtual RepositorySchema.MultivalueComparisonContants MultivalueComparison //{ get; set; }
        {
            get { return RepositorySchema.MultivalueComparisonContants.Union; }
            set { ;}
        }

    }
}