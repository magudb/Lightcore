using System;

namespace Lightcore.Logging
{
    public static class LoggerExtensions
    {
        public static void Debug(this ILogger logger, string message)
        {
            logger.Log(new LogEntry(LoggingEvent.Debug, message));
        }

        public static void Info(this ILogger logger, string message)
        {
            logger.Log(new LogEntry(LoggingEvent.Information, message));
        }

        public static void Warn(this ILogger logger, string message)
        {
            logger.Log(new LogEntry(LoggingEvent.Warning, message));
        }

        public static void Error(this ILogger logger, Exception exception)
        {
            logger.Log(new LogEntry(LoggingEvent.Exception, exception.Message, exception));
        }

        public static void Error(this ILogger logger, string message)
        {
            logger.Log(new LogEntry(LoggingEvent.Error, message));
        }
    }
}