#pragma warning disable 0168
using System;
using System.Linq;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace Celeriq.Utilities
{
    /// <summary />
    public static class Logger
    {
        #region Class Members

        private const string _eventSource = "Celeriq Core Services";
        private const string _eventLog = "Application";
        private static readonly NLog.Logger _logger = null;

        #endregion

        #region Constructor

        static Logger()
        {
            var config = new LoggingConfiguration();

            //Fiel Target 
            var fileTarget = new FileTarget();
            fileTarget.FileName = "${basedir}/logs/${shortdate}.txt";
            fileTarget.Layout = "${level} | ${longdate}  | ${message} | ${exception:format=Message,ShortType,StackTrace}";
            fileTarget.KeepFileOpen = false;
            fileTarget.Encoding = System.Text.Encoding.ASCII;
            config.AddTarget("file", fileTarget);

            //Add File Rule
            var rule1 = new LoggingRule("*", LogLevel.Debug, fileTarget);
            config.LoggingRules.Add(rule1);

            if (System.Diagnostics.Debugger.IsAttached)
            {
                //Console Target
                var consoleTarget = new ColoredConsoleTarget();
                config.AddTarget("console", consoleTarget);
                consoleTarget.Layout = @"${date:format=HH\:MM\:ss} ${logger} ${message}";

                //Add Console Rule
                var rule2 = new LoggingRule("*", LogLevel.Debug, consoleTarget);
                config.LoggingRules.Add(rule2);
            }

            LogManager.Configuration = config;

            _logger = NLog.LogManager.GetLogger(string.Empty);
        }

        #endregion

        #region Logging

        /// <summary />
        public static void LogError(string message)
        {
            try
            {
                if (_logger != null)
                    _logger.Error(message);
                //System.Diagnostics.EventLog.WriteEntry(_eventSource, message, System.Diagnostics.EventLogEntryType.Error);
            }
            catch (Exception ex)
            {
                //Do Nothing
            }
        }

        /// <summary />
        public static void LogError(Exception exception, string message)
        {
            try
            {
                if (_logger != null)
                    _logger.Error(message + "\n" + exception.ToString());
                //System.Diagnostics.EventLog.WriteEntry(_eventSource, message + "\n" + exception.ToString(), System.Diagnostics.EventLogEntryType.Error);
            }
            catch (Exception ex)
            {
                //Do Nothing
            }
        }

        /// <summary />
        public static void LogError(Exception exception)
        {
            try
            {
                if (_logger != null)
                    _logger.Error(exception.ToString());
                //System.Diagnostics.EventLog.WriteEntry(_eventSource, exception.ToString(), System.Diagnostics.EventLogEntryType.Error);
            }
            catch (Exception ex)
            {
                //Do Nothing
            }
        }

        /// <summary />
        public static void LogDebug(string message)
        {
            try
            {
                if (_logger != null)
                    _logger.Debug(message);
            }
            catch (Exception ex)
            {
                //Do Nothing
            }
        }

        /// <summary />
        public static void LogInfo(string message)
        {
            try
            {
                if (_logger != null)
                    _logger.Info(message);
                //System.Diagnostics.EventLog.WriteEntry(_eventSource, message, System.Diagnostics.EventLogEntryType.Information);
            }
            catch (Exception ex)
            {
                //Do Nothing
            }
        }

        /// <summary />
        public static void LogWarning(string message)
        {
            try
            {
                if (_logger != null)
                    _logger.Warn(message);
                //System.Diagnostics.EventLog.WriteEntry(_eventSource, message, System.Diagnostics.EventLogEntryType.Warning);
            }
            catch (Exception ex)
            {
                //Do Nothing
            }
        }

        #endregion

    }
}