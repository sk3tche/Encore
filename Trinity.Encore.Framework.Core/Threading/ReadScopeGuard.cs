using System;
using System.Diagnostics.Contracts;

namespace Trinity.Encore.Framework.Core.Threading
{
    /// <summary>
    /// Guards a scope as write mode with a given ReaderWriterLockSlim object.
    /// 
    /// Intended for use with the C# "using" statement.
    /// </summary>
    internal sealed class ReadScopeGuard : IDisposable
    {
        private readonly ReadWriteLock _lock;

        [ContractInvariantMethod]
        private void Invariant()
        {
            Contract.Invariant(_lock != null);
        }

        public ReadScopeGuard(ReadWriteLock rwLock)
        {
            Contract.Requires(rwLock != null);

            _lock = rwLock;
        }

        public void Guard()
        {
            _lock.EnterReadLock();
        }

        public void Dispose()
        {
            _lock.ExitReadLock();
        }
    }
}
