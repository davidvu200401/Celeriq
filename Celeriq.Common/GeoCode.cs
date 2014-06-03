using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Celeriq.Common
{
	[Serializable()]
	[DataContract()]
	public class GeoCode
	{
		[DataMember]
		public double Latitude;

		[DataMember]
		public double Longitude;

		[DataMember]
		public double? Distance;
	}
}