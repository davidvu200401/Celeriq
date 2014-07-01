using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Celeriq.DataCore.EFDAL;
using Celeriq.DataCore.EFDAL.Entity;
using Celeriq.Utilities;

namespace Celeriq.Server.Interfaces
{
    public static class ConfigHelper
    {
        private static DateTime _lastUpdate = DateTime.MinValue;
        private static Dictionary<string, string> _settings = new Dictionary<string, string>();
        private static readonly object _syncObject = new object();

        #region Constructors

        public static void Refresh()
        {
            try
            {
                lock (_syncObject)
                {
                    if (Storage == StorageTypeConstants.Database)
                    {
                        //Load all settings from database
                        using (var context = new DataCoreEntities())
                        {
                            _settings = context.ConfigurationSetting
                                .ToList()
                                .ToDictionary(x => x.Name.ToLower(), x => x.Value);
                            _lastUpdate = DateTime.Now;
                        }
                    }
                    else if (Storage == StorageTypeConstants.File)
                    {
                        //Do Nothing
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
        }

        #endregion

        public static StorageTypeConstants Storage { get; set; }

        #region Setting Methods

        public static Dictionary<string, string> AllSettings
        {
            get
            {
                lock (_syncObject)
                {
                    if (DateTime.Now.Subtract(_lastUpdate).TotalSeconds > 20)
                    {
                        Refresh();
                    }
                    return _settings;
                }
            }
        }

        private static string GetValue(string name)
        {
            return GetValue(name, string.Empty);
        }

        private static string GetValue(string name, string defaultValue)
        {
            if (AllSettings.ContainsKey(name.ToLower()))
                return AllSettings[name.ToLower()];
            return defaultValue;
        }

        private static int GetValue(string name, int defaultValue)
        {
            int retVal;
            if (int.TryParse(GetValue(name, string.Empty), out retVal))
                return retVal;
            return defaultValue;
        }

        private static bool GetValue(string name, bool defaultValue)
        {
            bool retVal;
            if (bool.TryParse(GetValue(name, string.Empty), out retVal))
                return retVal;
            return defaultValue;
        }

        private static void SetValue(string name, string value)
        {
            lock (_syncObject)
            {
                if (Storage == StorageTypeConstants.Database)
                {
                    using (var context = new DataCoreEntities())
                    {
                        var item = context.ConfigurationSetting.FirstOrDefault(x => x.Name == name);
                        if (item == null)
                        {
                            item = new ConfigurationSetting() { Name = name };
                            context.AddItem(item);
                        }
                        item.Value = value;
                        context.SaveChanges();
                        Refresh();
                    }
                }
                else if (Storage == StorageTypeConstants.File)
                {
                    var key = name.ToLower();
                    if (_settings.ContainsKey(key))
                        _settings[key] = value;
                    else
                        _settings.Add(key, value);

                    //Write back to config file if can
                    try
                    {
                        var config = System.Configuration.ConfigurationManager.OpenExeConfiguration(System.Configuration.ConfigurationUserLevel.None);
                        var setting = config.AppSettings.Settings[name];
                        if (setting == null || setting.Value != value)
                        {
                            config.AppSettings.Settings.Remove(name);
                            config.AppSettings.Settings.Add(name, value);
                            config.Save(System.Configuration.ConfigurationSaveMode.Modified);
                            System.Configuration.ConfigurationManager.RefreshSection("appSettings");
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.LogWarning(ex.Message);
                    }

                }
            }
        }

        private static void SetValue(string name, long value)
        {
            SetValue(name, value.ToString());
        }

        private static void SetValue(string name, bool value)
        {
            SetValue(name, value.ToString().ToLower());
        }

        #endregion

        #region Properties

        public static string PublicKey
        {
            get { return GetValue("PublicKey", string.Empty); }
            set { SetValue("PublicKey", value); }
        }

        public static string PrivateKey
        {
            get { return GetValue("PrivateKey", string.Empty); }
            set { SetValue("PrivateKey", value); }
        }

        public static int MaxRunningRepositories
        {
            get { return GetValue("MaxRunningRepositories", 0); }
            set { SetValue("MaxRunningRepositories", value); }
        }

        public static long MaxMemory
        {
            get { return GetValue("MaxMemory", 0); }
            set { SetValue("MaxMemory", value); }
        }

        public static int AutoDataUnloadTime
        {
            get { return GetValue("AutoDataUnloadTime", 0); }
            set { SetValue("AutoDataUnloadTime", value); }
        }

        public static KeyPair MasterKeys
        {
            get { return new KeyPair { PublicKey = ConfigHelper.PublicKey, PrivateKey = ConfigHelper.PrivateKey }; }
        }

        public static bool AllowCaching
        {
            get { return GetValue("AllowCaching", true); }
            set { SetValue("AllowCaching", value); }
        }

        public static bool AllowStatistics
        {
            get { return GetValue("AllowStatistics", true); }
            set { SetValue("AllowStatistics", value); }
        }

        public static bool AutoLoad
        {
            get { return GetValue("AutoLoad", true); }
            set { SetValue("AutoLoad", value); }
        }

        public static string DataPath
        {
            get { return GetValue("DataPath", string.Empty); }
            set { SetValue("DataPath", value); }
        }

        public static int Port
        {
            get { return GetValue("Port", 1973); }
            set { SetValue("Port", value); }
        }

        public static string MailServer
        {
            get { return GetValue("MailServer", string.Empty); }
            set { SetValue("MailServer", value); }
        }

        public static int MailServerPort
        {
            get { return GetValue("MailServerPort", 25); }
            set { SetValue("MailServerPort", value); }
        }

        public static string MailServerUsername
        {
            get { return GetValue("MailServerUsername", string.Empty); }
            set { SetValue("MailServerUsername", value); }
        }

        public static string MailServerPassword
        {
            get { return GetValue("MailServerPassword", string.Empty); }
            set { SetValue("MailServerPassword", value); }
        }

        public static string NotifyEmail
        {
            get { return GetValue("NotifyEmail", string.Empty); }
            set { SetValue("NotifyEmail", value); }
        }

        public static string DebugEmail
        {
            get { return GetValue("DebugEmail", string.Empty); }
            set { SetValue("DebugEmail", value); }
        }

        public static string FromEmail
        {
            get { return GetValue("FromEmail", string.Empty); }
            set { SetValue("FromEmail", value); }
        }

        public static bool KeywordSearchLiteral
        {
            get { return GetValue("KeywordSearchLiteral", false); }
            set { SetValue("KeywordSearchLiteral", value); }
        }

        #endregion

    }
}