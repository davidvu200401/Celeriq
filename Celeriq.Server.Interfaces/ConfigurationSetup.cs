using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Celeriq.Server.Interfaces
{
    public enum StorageTypeConstants
    {
        Database = 0,
        File = 1,
    }

    public class ConfigurationSetup
    {
        public ConfigurationSetup()
        {
            this.Port = 1973;
        }

        public string PublicKey { get; set; }
        public string PrivateKey { get; set; }
        public int MaxRunningRepositories { get; set; }
        public long MaxMemory { get; set; }
        public int AutoDataUnloadTime { get; set; }
        //public bool AllowCaching { get; set; }
        public bool AllowStatistics { get; set; }
        public bool AutoLoad { get; set; }
        public string DataPath { get; set; }
        public bool IsDebug { get; set; }
        public int Port { get; set; }
    }
}
