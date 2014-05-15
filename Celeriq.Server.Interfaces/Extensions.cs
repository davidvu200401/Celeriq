using System;
using System.Linq;
using System.Text;
using Celeriq.Common;

namespace Celeriq.Server.Interfaces
{
    public static class Extensions
    {
        public static void Initialize(this ServerResourceSettings item)
        {
            item.AutoDataUnloadTime = ConfigHelper.AutoDataUnloadTime;
            item.MaxMemory = ConfigHelper.MaxMemory;
            item.MaxRunningRepositories = ConfigHelper.MaxRunningRepositories;
        }

        public static void Save(this ServerResourceSettings item)
        {
            ConfigHelper.AutoDataUnloadTime = item.AutoDataUnloadTime;
            ConfigHelper.MaxMemory = item.MaxMemory;
            ConfigHelper.MaxRunningRepositories = item.MaxRunningRepositories;
        }

        public static bool ToBool(this string s)
        {
            if (string.IsNullOrEmpty(s)) return false;
            s = s.ToLower();
            return (s == "true" || s == "1");
        }

    }
}
