using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ServiceModel;
using Celeriq.Common;

namespace Celeriq.Common
{
    [ServiceContract]
    public interface ISystemCore
    {
        //[OperationContract]
        //BaseRemotingObject StartRepository(Guid repositoryId, UserCredentials credentials);

        //[OperationContract]
        //BaseRemotingObject StopRepository(Guid repositoryId, UserCredentials credentials);

        [OperationContract]
        BaseRemotingObject DataLoadRepository(Guid repositoryId, UserCredentials credentials);

        [OperationContract]
        int GetRepositoryCount(UserCredentials credentials, PagingInfo paging);

        [OperationContract]
        List<BaseRemotingObject> GetRepositoryPropertyList(UserCredentials credentials, PagingInfo paging);

        [OperationContract]
        BaseRemotingObject SaveRepository(RepositorySchema repository, UserCredentials credentials);

        [OperationContract]
        bool RepositoryExists(Guid repositoryId, UserCredentials credentials);

        [OperationContract]
        void DeleteRepository(RepositorySchema repository, UserCredentials credentials);

        [OperationContract]
        void ShutDown();

        [OperationContract]
        bool ExportSchema(Guid repositoryId, UserCredentials credentials, string backupFile);

        [OperationContract]
        string GetPublicKey();

        [OperationContract]
        bool IsValidCredentials(UserCredentials credentials);

        [OperationContract]
        ServerResourceSettings GetServerResourceSetting(UserCredentials credentials);

        [OperationContract]
        bool SaveServerResourceSetting(UserCredentials credentials, ServerResourceSettings settings);

        [OperationContract]
        ProfileItem[] GetProfile(Guid repositoryId, UserCredentials credentials, long lastProfileId);

        [OperationContract]
        bool AddSystemUser(UserCredentials credentials, UserCredentials user);

        [OperationContract]
        bool DeleteSystemUser(UserCredentials credentials, UserCredentials user);

        [OperationContract]
        UserCredentials[] GetUserList(UserCredentials credentials);

        [OperationContract]
        void NotifyLoad(Guid repositoryId, int elapsed, int itemsAffected);

        [OperationContract]
        void NotifyUnload(Guid repositoryId, int elapsed, int itemsAffected);

        [OperationContract]
        RealtimeStats[] PerformanceCounters(UserCredentials credentials, DateTime minDate, DateTime maxDate);

        [OperationContract]
        void LogRepositoryPerf(RepositorySummmaryStats stat);

        [OperationContract]
        RepositorySummmaryStats GetRepositoryStats(UserCredentials credentials, Guid repositoryId, DateTime minDate, DateTime maxDate);

        [OperationContract]
        SystemStats GetSystemStats();

        [OperationContract]
        bool IsInitialized();

    }
}