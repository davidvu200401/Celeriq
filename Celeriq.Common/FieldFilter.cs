using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Celeriq.Common
{
    [DataContract]
    [Serializable]
    public class FieldFilter : Celeriq.Common.IFieldFilter, System.ICloneable
    {
        public FieldFilter()
        {
        }

        public FieldFilter(string name)
            : this()
        {
            this.Name = name;
            this.Comparer = ComparisonConstants.Equals;
        }

        [DataMember]
        public Celeriq.Common.ComparisonConstants Comparer { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        object Celeriq.Common.IFieldFilter.Value { get; set; }

        [DataMember]
        object Celeriq.Common.IFieldFilter.Value2 { get; set; }

        public override int GetHashCode()
        {
            var h = Utilities.EncryptionDomain.Hash(this.Comparer.ToString()) + "." + this.Name + ".";
            if (((Celeriq.Common.IFieldFilter)this).Value == null)
                h += "NULL";
            else
                h += ((Celeriq.Common.IFieldFilter)this).Value.ToString();

            if (((Celeriq.Common.IFieldFilter)this).Value2 == null)
                h += "NULL";
            else
                h += ((Celeriq.Common.IFieldFilter)this).Value2.ToString();

            return Utilities.EncryptionDomain.Hash(h);
        }

        public override string ToString()
        {
            return this.Name;
        }

        object ICloneable.Clone()
        {
            var retval = new FieldFilter();
            retval.Comparer = this.Comparer;
            retval.Name = this.Name;
            ((Celeriq.Common.IFieldFilter)retval).Value = ((Celeriq.Common.IFieldFilter)this).Value;
            ((Celeriq.Common.IFieldFilter)retval).Value2 = ((Celeriq.Common.IFieldFilter)this).Value2;
            return retval;
        }

    }
}