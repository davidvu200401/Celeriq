using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Celeriq.Common
{
    [DataContract]
    [Serializable]
    public class FieldSort : Celeriq.Common.IFieldSort
    {
        public FieldSort()
        {
        }

        public FieldSort(string name)
            : this()
        {
            this.Name = name;
        }

        [DataMember]
        public Celeriq.Common.SortDirectionConstants SortDirection { get; set; }

        [DataMember]
        public string Name { get; set; }

        public override int GetHashCode()
        {
            return Utilities.EncryptionDomain.Hash(this.SortDirection.ToString() + "·" + this.Name);
        }

        public override string ToString()
        {
            return this.Name;
        }

    }
}