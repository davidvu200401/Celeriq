using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Celeriq.Common;

namespace Celeriq.RepositoryAPI
{
    [Serializable()]
    public class DataItemExtension : Celeriq.Common.DataItem
    {
        public DataItemExtension(Celeriq.Common.DataItem baseItem, RepositorySchema definition)
            : base()
        {
            this.__RecordIndex = baseItem.__RecordIndex;
            this.ItemArray = baseItem.ItemArray;
            this.DimensionValueArray = new List<long>();
            this.DimensionSingularValueArray = new List<long?>();
        }

        public DataItem ToSerialized()
        {
            var retval = new DataItem();
            retval.__RecordIndex = this.__RecordIndex;
            retval.ItemArray = new object[this.ItemArray.Length];
            this.ItemArray.CopyTo(retval.ItemArray, 0);
            return retval;
        }

        /// <summary>
        /// This is a list of all dimension values for this object
        /// it can contain multiple refinments from a single dimenion
        /// </summary>
        public List<long> DimensionValueArray;

        /// <summary>
        /// This is an array of values that matches the dimension definision could
        /// List dimensions are not valid in this array but exclusive dimensions are valid
        /// This allows very fast dimension aggregation to be performed on non-List dimensions
        /// </summary>
        public List<long?> DimensionSingularValueArray;

        /// <summary>
        /// A keyword blob field
        /// </summary>
        public string Keyword;

    }
}