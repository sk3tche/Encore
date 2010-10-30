using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace Trinity.Encore.Framework.Core.Logging.Loggers
{
    /// <summary>
    /// A logger that writes to the Windows event log.
    /// </summary>
    internal sealed class EventLogger : ILogger
    {
        public EventLogger()
        {
            var src = LogManager.EventLogSource;
            if (!EventLog.SourceExists(src))
                EventLog.CreateEventSource(src, "Application");
        }

        public void WriteInformation(string logString)
        {
            WriteToEventLog(logString, EventLogEntryType.Information);
        }

        public void WriteWarning(string logString)
        {
            WriteToEventLog(logString, EventLogEntryType.Warning);
        }

        public void WriteError(string logString)
        {
            WriteToEventLog(logString, EventLogEntryType.Error);
        }

        private static void WriteToEventLog(string str, EventLogEntryType type)
        {
            Contract.Requires(str != null);

            EventLog.WriteEntry(LogManager.EventLogSource, str, type);
        }
    }
}
