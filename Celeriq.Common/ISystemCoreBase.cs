using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace Celeriq.Common
{
	[ServiceContract]
	public interface ISystemCoreBase
	{
		[OperationContract]
		string GetStorePath();
	}
}
