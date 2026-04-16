using Microsoft.Extensions.Logging;

namespace HealthClaimsProcessor.Core.Logging
{
    /// <summary>
    /// Bridge helper that wraps ILogger for backward compatibility during migration.
    /// New code should inject ILogger&lt;T&gt; directly.
    /// </summary>
    public static class LoggingHelper
    {
        private static ILoggerFactory _loggerFactory;

        public static void Configure(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
        }

        private static ILogger GetLogger(string category)
        {
            if (_loggerFactory == null)
            {
                _loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
            }
            return _loggerFactory.CreateLogger(category);
        }

        public static void LogInfo(string message, string category = "General")
        {
            GetLogger(category).LogInformation(message);
        }

        public static void LogWarning(string message, string category = "General")
        {
            GetLogger(category).LogWarning(message);
        }

        public static void LogError(string message, Exception exception = null, string category = "General")
        {
            if (exception != null)
                GetLogger(category).LogError(exception, message);
            else
                GetLogger(category).LogError(message);
        }

        public static void LogCritical(string message, Exception exception = null, string category = "General")
        {
            if (exception != null)
                GetLogger(category).LogCritical(exception, message);
            else
                GetLogger(category).LogCritical(message);
        }

        public static void LogVerbose(string message, string category = "General")
        {
            GetLogger(category).LogDebug(message);
        }
    }
}
