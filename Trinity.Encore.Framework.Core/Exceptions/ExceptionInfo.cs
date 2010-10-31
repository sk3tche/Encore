using System;
using System.Diagnostics.Contracts;
using Trinity.Encore.Framework.Core.Threading.Actors;

namespace Trinity.Encore.Framework.Core.Exceptions
{
    /// <summary>
    /// Holds information describing an exception.
    /// </summary>
    public sealed class ExceptionInfo
    {
        /// <summary>
        /// The exception that occurred.
        /// </summary>
        public Exception Exception { get; private set; }

        /// <summary>
        /// The Actor instance the exception occurred in, if any.
        /// </summary>
        public Actor Actor { get; private set; }

        /// <summary>
        /// The time of the exception.
        /// </summary>
        public DateTime OccurrenceTime { get; private set; }

        [ContractInvariantMethod]
        private void Invariant()
        {
            Contract.Invariant(Exception != null);
        }

        internal ExceptionInfo(Exception exception, Actor actor)
        {
            Contract.Requires(exception != null);

            Exception = exception;
            Actor = actor;
            OccurrenceTime = DateTime.Now;
        }
    }
}
