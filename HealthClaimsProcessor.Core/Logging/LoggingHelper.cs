using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace HealthClaimsProcessor.Core.Logging
{
    public static class LoggingHelper
    {
        public static void LogInfo(string message, string category = "General")
        {
            var logEntry = new LogEntry
            {
                Message = message,
                Categories = new List<string> { category },
                Priority = 5,
                Severity = TraceEventType.Information,
                TimeStamp = DateTime.Now
            };
            Logger.Write(logEntry);
        }

        public static void LogWarning(string message, string category = "General")
        {
            var logEntry = new LogEntry
            {
                Message = message,
                Categories = new List<string> { category },
                Priority = 7,
                Severity = TraceEventType.Warning,
                TimeStamp = DateTime.Now
            };
            Logger.Write(logEntry);
        }

        public static void LogError(string message, Exception exception = null, string category = "General")
        {
            var logEntry = new LogEntry
            {
                Message = message,
                Categories = new List<string> { category },
                Priority = 10,
                Severity = TraceEventType.Error,
                TimeStamp = DateTime.Now
            };

            if (exception != null)
            {
                logEntry.ExtendedProperties.Add("Exception", exception.ToString());
                logEntry.ExtendedProperties.Add("ExceptionType", exception.GetType().FullName);
                logEntry.ExtendedProperties.Add("StackTrace", exception.StackTrace);
            }

            Logger.Write(logEntry);
        }

        public static void LogCritical(string message, Exception exception = null, string category = "General")
        {
            var logEntry = new LogEntry
            {
                Message = message,
                Categories = new List<string> { category },
                Priority = 15,
                Severity = TraceEventType.Critical,
                TimeStamp = DateTime.Now
            };

            if (exception != null)
            {
                logEntry.ExtendedProperties.Add("Exception", exception.ToString());
                logEntry.ExtendedProperties.Add("ExceptionType", exception.GetType().FullName);
                logEntry.ExtendedProperties.Add("StackTrace", exception.StackTrace);
            }

            Logger.Write(logEntry);
        }

        public static void LogVerbose(string message, string category = "General")
        {
            var logEntry = new LogEntry
            {
                Message = message,
                Categories = new List<string> { category },
                Priority = 1,
                Severity = TraceEventType.Verbose,
                TimeStamp = DateTime.Now
            };
            Logger.Write(logEntry);
        }
    }
}
