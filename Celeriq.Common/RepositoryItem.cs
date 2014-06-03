using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Celeriq.Common
{
	[Serializable]
	[DataContract()]
	[KnownType(typeof(GeoCode))]
	[KnownType(typeof(GeoCodeFieldFilter))]
	public class RepositoryItem : IRepositoryItem
	{
		public RepositoryItem()
		{
		}

		[DataMember]
		[OptionalField]
		public long __RecordIndex;

		[DataMember]
		public DateTime __CreateDate;

		long IRepositoryItem.__RecordIndex
		{
			get { return this.__RecordIndex; }
		}

		DateTime IRepositoryItem.__CreateDate
		{
			get { return this.__CreateDate; }
		}

	}
}
