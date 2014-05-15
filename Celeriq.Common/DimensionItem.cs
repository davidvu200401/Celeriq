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
    public class DimensionItem : System.ICloneable, Celeriq.Common.ICloneable<DimensionItem>
    {
        public const string DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

        public DimensionItem()
        {
            this.RefinementList = new List<RefinementItem>();
        }

        [DataMember]
        public string Name;

        [DataMember]
        public int DIdx;

        [DataMember]
        public long? NumericBreak;

        [DataMember]
        public List<RefinementItem> RefinementList;

        [DataMember]
        public DimensionItem Parent;

        public long GetNextDVIdx()
        {
            if (this.RefinementList.Count == 0) return ((this.DIdx - DimensionDefinition.DGROUP) + 1) * DimensionDefinition.DVALUEGROUP;
            else return this.RefinementList.Max(x => x.DVIdx) + 1;
        }

        public override string ToString()
        {
            return this.Name;
        }

        object ICloneable.Clone()
        {
            return ((ICloneable<DimensionItem>)this).Clone(new DimensionItem());
        }

        #region ICloneable<DimensionItem> Members

        DimensionItem ICloneable<DimensionItem>.Clone()
        {
            return ((ICloneable<DimensionItem>)this).Clone(new DimensionItem());
        }

        DimensionItem ICloneable<DimensionItem>.Clone(DimensionItem dest)
        {
            if (dest == null)
                throw new Exception("Object cannot be null.");

            dest.DIdx = this.DIdx;
            dest.Name = this.Name;
            dest.NumericBreak = this.NumericBreak;
            this.RefinementList.ForEach(x => dest.RefinementList.Add(((ICloneable)x).Clone() as RefinementItem));
            return dest;
        }

        #endregion
    }
}