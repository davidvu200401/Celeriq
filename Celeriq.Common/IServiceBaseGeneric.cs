using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace Celeriq.Common
{
	[ServiceContract]
	public interface IServiceBaseGeneric : IServiceBase
	{
		[OperationContract]
		QueryResults Query(BaseListingQuery query);

		[OperationContract]
		void UpdateItemList(IEnumerable<RepositoryItem> list);

		[OperationContract]
		void UpdateItem(RepositoryItem item);

		[OperationContract]
		void DeleteItem(int primaryKey);

		[OperationContract]
		void ClearIndex();
	}
}