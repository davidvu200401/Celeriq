using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Celeriq.Common;

namespace Celeriq.AdminSite.Objects
{
    [Serializable]
    public class CeleriqSettings
    {
        private int _hourIndex = 1;

        public bool IsInitialized { get; private set; }

        public bool InitializeServer(string serverName, string userName, string password)
        {
            try
            {
                PublicKey = string.Empty;
                CeleriqUser = string.Empty;
                CeleriqPassword = string.Empty;
                CeleriqServer = string.Empty;

                using (var factory = SystemCoreInteractDomain.GetFactory(serverName))
                {
                    var server = factory.CreateChannel();
                    PublicKey = server.GetPublicKey();
                    CeleriqUser = userName;
                    CeleriqPassword = password;
                    CeleriqServer = serverName;
                    IsInitialized = true;
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
                //throw;
            }
        }

        public string CeleriqServer { get; private set; }
        public string CeleriqUser { get; private set; }
        public string PublicKey { get; private set; }
        public string CeleriqPassword { get; private set; }
        public string TotalRepositories { get; set; }
        public string CurrentLoaded { get; set; }
        public string CurrentMemory { get; set; }
        public string CurrentDisk { get; set; }
        public long TotalMemory { get; set; }
        public string MachineName { get; set; }
        public string OSVersion { get; set; }
        public int ProcessorCount { get; set; }
        public int TickCount { get; set; }

        public int HourIndex
        {
            get { return _hourIndex; }
            set
            {
                switch (value)
                {
                    case 1:
                    case 2:
                    case 3:
                    case 6:
                    case 12:
                    case 24:
                        _hourIndex = value;
                        break;
                    default:
                        _hourIndex = 1;
                        break;
                }
            }
        }

    }
}