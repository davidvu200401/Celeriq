using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace Celeriq.Common
{
    [ServiceContract()]
    public interface IDataModel
    {
        [OperationContract]
        string[] UpdateData(Guid repositoryId, IEnumerable<DataItem> list, UserCredentials credentials);

        [OperationContract]
        string[] DeleteData(Guid repositoryId, IEnumerable<DataItem> item, UserCredentials credentials);

        [OperationContract]
        DataQueryResults Query(Guid repositoryId, DataQuery query);

        [OperationContract]
        string[] Clear(Guid repositoryId, UserCredentials credentials);

        [OperationContract]
        bool IsValidFormat(Guid repositoryId, DataItem item, UserCredentials credentials);

        [OperationContract]
        long GetItemCount(Guid repositoryId, UserCredentials credentials);

    }
}