using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Trinity.Encore.Framework.Core.Initialization;
using Trinity.Encore.Framework.Core.Logging;
using Trinity.Encore.Framework.Core.Threading.Actors;

namespace Trinity.Encore.Framework.Core.Exceptions
{
    /// <summary>
    /// Class used to log exceptions, so they can be viewed later.
    /// </summary>
    public static class ExceptionManager
    {
        public static event EventHandler<ExceptionEventArgs> ExceptionOccurred;

        private static readonly SynchronizedCollection<ExceptionInfo> _exceptionList =
            new SynchronizedCollection<ExceptionInfo>();

        private static readonly LogProxy _log = new LogProxy("ExceptionManager");

        /// <summary>
        /// Registers an exception that occurred.
        /// </summary>
        /// <param name="ex">The exception that occurred.</param>
        /// <param name="actor">The actor the message occurred in, if any.</param>
        public static void RegisterException(Exception ex, IActor actor = null)
        {
            Contract.Requires(ex != null);

            var actorStr = actor == null ? " " : string.Format(" (actor: {0} ({1}))", actor, actor.GetType().Name);
            _log.Error("{0} caught{1}:", ex.GetType().Name, actorStr);
            PrintException(ex);

            var info = new ExceptionInfo(ex, actor);
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

        public static ExceptionInfo[] GetExceptions(bool clear = false)
        {
            var exceptions = _exceptionList.ToArray();

            if (clear)
                ClearExceptions();

            return exceptions;
        }

        public static void ClearExceptions()
        {
            _exceptionList.Clear();
        }

        [Initializable("ExceptionManager", InitializationPass.Framework)]
        public static void ChangeState(bool init)
        {
            if (!init)
                ClearExceptions();
        }
    }
}
