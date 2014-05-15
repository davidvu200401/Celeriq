using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace Celeriq.Common
{
	public enum RepositoryStateConstants
	{
		Ready,
		Loading,
		Compressing,
	}

	[ServiceContract]
	public interface IServiceBase
	{
		[OperationContract]
		void EnsureLoaded();
	}
}