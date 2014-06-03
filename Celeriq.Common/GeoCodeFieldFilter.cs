using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Celeriq.Common
{
    [DataContract]
    [Serializable]
    public class GeoCodeFieldFilter : FieldFilter, System.ICloneable
    {
        public GeoCodeFieldFilter()
        {
        }

        public GeoCodeFieldFilter(string name)
            : this()
        {
            this.Name = name;
        }

        [DataMember]
        public double Latitude;

        [DataMember]
        public double Longitude;

        [DataMember]
        public double Radius;

        public override int GetHashCode()
        {
            return Utilities.EncryptionDomain.Hash(this.Comparer.ToString() + "·" + this.Latitude + "·" + this.Longitude + "·" + this.Radius);
        }

        object ICloneable.Clone()
        {
            var retval = new GeoCodeFieldFilter();
            retval.Comparer = this.Comparer;
            retval.Name = this.Name;
            ((Celeriq.Common.IFieldFilter)retval).Value = ((Celeriq.Common.IFieldFilter)this).Value;
            ((Celeriq.Common.IFieldFilter)retval).Value2 = ((Celeriq.Common.IFieldFilter)this).Value2;
            retval.Latitude = this.Latitude;
            retval.Longitude = this.Longitude;
            retval.Radius = this.Radius;
            return retval;
        }

    }

}