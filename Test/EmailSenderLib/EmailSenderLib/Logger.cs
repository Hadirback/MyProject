using NLog;
using NLog.Config;
using NLog.Targets;
using System;
using System.IO;

namespace EmailSenderLib
{
    class LoggerNotification
    {
        public Logger Log { get; set; }

        public LoggerNotification()
        {
            var config = new LoggingConfiguration();
            var fileTarget = new FileTarget();
            config.AddTarget("file", fileTarget);
            fileTarget.FileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs\\NotificationLog.txt");
            fileTarget.Layout = "[${date:format=dd\\.MM\\.yyyy HH\\:mm\\:ss}] ${level} [${callsite}] ${message}";
            fileTarget.ArchiveAboveSize = 5242880;
            fileTarget.ArchiveEvery = FileArchivePeriod.Day;
            fileTarget.ArchiveNumbering = ArchiveNumberingMode.Rolling;
            fileTarget.CreateDirs = true;
            fileTarget.KeepFileOpen = true;
            fileTarget.MaxArchiveFiles = 7;
            config.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, fileTarget));
            LogManager.Configuration = config;

            Log = LogManager.GetCurrentClassLogger();
        }
    }
}
