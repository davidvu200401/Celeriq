using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Celeriq.Common
{
    [Serializable]
    [DataContract]
    [KnownType(typeof(GeoCode))]
    [KnownType(typeof(GeoCodeFieldFilter))]
    [KnownType(typeof(string[]))]
    [KnownType(typeof(FieldFilter))]
    [KnownType(typeof(FieldSort))]
    [KnownType(typeof(NamedItem))]
    [KnownType(typeof(NamedItemList))]
    [KnownType(typeof(UserCredentials))]
    public class DataQuery : BaseListingQuery
    {
        public DataQuery()
            : base()
        {
        }
    }
}