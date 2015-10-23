using System;

namespace Lightcore.Logging
{
    public class LogEntry
    {
        public readonly Exception Exception;
        public readonly string Message;
        public readonly LoggingEvent Severity;

        public LogEntry(LoggingEvent severity, string message, Exception exception = null)
        {
            Requires.IsNotNullOrEmpty(message, "message");

            Severity = severity;
            Message = message;
            Exception = exception;
        }
    }
}