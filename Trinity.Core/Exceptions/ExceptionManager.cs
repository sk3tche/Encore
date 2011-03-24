using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Trinity.Core.Logging;

namespace Trinity.Core.Exceptions
{
    /// <summary>
    /// Class used to log exceptions, so they can be viewed later.
    /// </summary>
    public static class ExceptionManager
    {
        /// <summary>
        /// Triggered when an exception is registered.
        /// </summary>
        public static event EventHandler<ExceptionEventArgs> ExceptionOccurred;

        private static readonly SynchronizedCollection<ExceptionInfo> _exceptionList =
            new SynchronizedCollection<ExceptionInfo>();

        private static readonly LogProxy _log = new LogProxy("ExceptionManager");

        /// <summary>
        /// Registers an exception that occurred.
        /// </summary>
        /// <param name="ex">The exception that occurred.</param>
        public static void RegisterException(Exception ex)
        {
            Contract.Requires(ex != null);

            _log.Error("{0} caught:", ex.GetType().Name);
            PrintException(ex);

            var info = new ExceptionInfo(ex);
            _exceptionList.Add(info);

            var evnt = ExceptionOccurred;
            if (evnt != null)
                evnt(null, new ExceptionEventArgs(info));
        }

        private static void PrintException(Exception ex)
        {
            Contract.Requires(ex != null);

            _log.Error("Message: {0}", ex.Message);
            _log.Error("Stack trace: {0}", ex.StackTrace);

            var inner = ex.InnerException;
            if (inner != null)
                PrintException(inner);
        }

        /// <summary>
        /// Gets all cached exceptions.
        /// </summary>
        /// <param name="clear">A value indicating whether to clear the internal cache after cloning it.</param>
        /// <returns>A clone of the exception cache.</returns>
        public static ExceptionInfo[] GetExceptions(bool clear = false)
        {
            var exceptions = _exceptionList.ToArray();

            if (clear)
                ClearExceptions();

            return exceptions;
        }

        /// <summary>
        /// Clears the internal exception cache.
        /// </summary>
        public static void ClearExceptions()
        {
            _exceptionList.Clear();
        }
    }
}
