using System;
using System.Diagnostics.Contracts;

namespace Trinity.Encore.Framework.Core.Exceptions
{
    public sealed class ExceptionEventArgs : EventArgs
    {
        /// <summary>
        /// The information object of the exception that occurred.
        /// </summary>
        public ExceptionInfo Info { get; private set; }

        [ContractInvariantMethod]
        private void Invariant()
        {
            Contract.Invariant(Info != null);
        }

        public ExceptionEventArgs(ExceptionInfo info)
        {
            Contract.Requires(info != null);

            Info = info;
        }
    }
}
