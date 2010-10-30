using System;
using System.Diagnostics.Contracts;

namespace Trinity.Encore.Framework.Core.Threading
{
    /// <summary>
    /// Guards a scope as write mode with a given ReaderWriterLockSlim object.
    /// 
    /// Intended for use with the C# "using" statement.
    /// </summary>
    internal sealed class WriteScopeGuard : IDisposable
    {
        private readonly ReadWriteLock _lock;

        [ContractInvariantMethod]
        private void Invariant()
        {
            Contract.Invariant(_lock != null);
        }

        public WriteScopeGuard(ReadWriteLock rwLock)
        {
            Contract.Requires(rwLock != null);

            _lock = rwLock;
        }

        public void Guard()
        {
            _lock.EnterWriteLock();
        }

        public void Dispose()
        {
            _lock.ExitWriteLock();
        }
    }
}
