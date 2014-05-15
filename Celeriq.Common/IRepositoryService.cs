using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace Celeriq.Common
{
	[ServiceContract]
	public interface IRepositoryService
	{
		[OperationContract]
		int StartUp(ServiceStartup startup);

		[OperationContract]
		void ShutDown();

		[OperationContract]
		void Reset(Celeriq.Common.ServiceStartup startup);

		[OperationContract]
		Guid GetID();

		[OperationContract]
		Guid GetInstanceID();

	}
}
