using System;
using System.Diagnostics.Contracts;
using Trinity.Encore.Framework.Core.Runtime;

namespace Trinity.Encore.Framework.Core.Threading
{
    /// <summary>
    /// Guards a scope as write mode with a given ReaderWriterLockSlim object.
    /// 
    /// Intended for use with the C# "using" statement.
    /// </summary>
    internal sealed class WriteScopeGuard : IDisposableResource
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

        ~WriteScopeGuard()
        {
            Dispose(false);
        }

        public void Guard()
        {
            _lock.EnterWriteLock();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Dispose(bool disposing)
        {
            if (IsDisposed)
                return;

            _lock.ExitWriteLock();
            IsDisposed = true;
        }

        public bool IsDisposed { get; private set; }
    }
}
