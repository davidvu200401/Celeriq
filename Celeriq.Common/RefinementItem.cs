using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Celeriq.Common
{
    [Serializable()]
    [DataContract()]
    [KnownType(typeof(GeoCode))]
    [KnownType(typeof(GeoCodeFieldFilter))]
    public class RefinementItem : System.ICloneable, Celeriq.Common.ICloneable<RefinementItem>
    {
        [DataMember]
        public string FieldValue;

        [DataMember]
        public long? MinValue;

        [DataMember]
        public long? MaxValue;

        [DataMember]
        public long DVIdx;

        [DataMember]
        public int Count;

        [DataMember]
        [NonSerialized]
        public long? AggCount;

        public override string ToString()
        {
            return this.FieldValue;
        }

        object ICloneable.Clone()
        {
            return ((ICloneable<RefinementItem>)this).Clone(new RefinementItem());
        }

        #region ICloneable<RefinementItem> Members

        RefinementItem ICloneable<RefinementItem>.Clone()
        {
            return ((ICloneable<RefinementItem>)this).Clone(new RefinementItem());
        }

        RefinementItem ICloneable<RefinementItem>.Clone(RefinementItem dest)
        {
            if (dest == null)
                throw new Exception("Object cannot be null.");

            dest.Count = this.Count;
            dest.DVIdx = this.DVIdx;
            dest.FieldValue = this.FieldValue;
            dest.MaxValue = this.MaxValue;
            dest.MinValue = this.MinValue;
            return dest;
        }

        #endregion

    }
}