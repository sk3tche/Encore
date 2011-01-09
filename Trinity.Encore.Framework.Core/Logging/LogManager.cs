using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Text;
using Trinity.Encore.Framework.Core.Configuration;
using Trinity.Encore.Framework.Core.Logging.Loggers;

namespace Trinity.Encore.Framework.Core.Logging
{
    /// <summary>
    /// Takes care of all logging in the entire source base.
    /// 
    /// Do not use this class directly (except for debug logging); use LogProxy objects instead.
    /// </summary>
    public static class LogManager
    {
        private static readonly List<ILogger> _loggers = new List<ILogger>();

        private static readonly StringBuilder _builder = new StringBuilder();

        private static readonly object _lock = new object();

        /// <summary>
        /// Whether or not to use console colors.
        /// </summary>
        [ConfigurationVariable("ConsoleLogColors", true)]
        public static bool UseConsoleColors { get; set; }

        /// <summary>
        /// Whether or not to timestamp console log strings.
        /// </summary>
        [ConfigurationVariable("ConsoleLogTimestamp", true)]
        public static bool UseConsoleTimestamp { get; set; }

        static LogManager()
        {
            // TODO: Make this more customizable.
            AddLogger(new ConsoleLogger());
        }

        public static void AddLogger(ILogger logger)
        {
            Contract.Requires(logger != null);

            _loggers.Add(logger);
        }

        private static string PrepareForOutput(string source, string logString, params object[] args)
        {
            Contract.Requires(!string.IsNullOrEmpty(source));
            Contract.Requires(logString != null);
            Contract.Requires(args != null);
            Contract.Ensures(!string.IsNullOrEmpty(Contract.Result<string>()));

            _builder.Append("[");
            _builder.Append(source);
            _builder.Append("] ");
            _builder.AppendFormat(logString, args);

            var result = _builder.ToString();
            Contract.Assume(!string.IsNullOrEmpty(result));
            _builder.Clear();

            return result;
        }

        internal static void Information(string source, string logString, params object[] args)
        {
            Contract.Requires(!string.IsNullOrEmpty(source));
            Contract.Requires(logString != null);
            Contract.Requires(args != null);

            lock (_lock)
                foreach (var log in _loggers)
                    log.WriteInformation(PrepareForOutput(source, logString, args));
        }

        internal static void Warning(string source, string logString, params object[] args)
        {
            Contract.Requires(!string.IsNullOrEmpty(source));
            Contract.Requires(logString != null);
            Contract.Requires(args != null);

            lock (_lock)
                foreach (var log in _loggers)
                    log.WriteWarning(PrepareForOutput(source, logString, args));
        }

        internal static void Error(string source, string logString, params object[] args)
        {
            Contract.Requires(!string.IsNullOrEmpty(source));
            Contract.Requires(logString != null);
            Contract.Requires(args != null);

            lock (_lock)
                foreach (var log in _loggers)
                    log.WriteError(PrepareForOutput(source, logString, args));
        }

        [Conditional("DEBUG")]
        public static void Debug(string logString, params object[] args)
        {
            Contract.Requires(logString != null);
            Contract.Requires(args != null);

            lock (_lock)
                Console.WriteLine(logString, args);
        }
    }
}
