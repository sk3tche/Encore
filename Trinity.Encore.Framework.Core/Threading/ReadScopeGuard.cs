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
    internal sealed class ReadScopeGuard : IDisposableResource
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

        ~ReadScopeGuard()
        {
            Dispose(false);
        }

        public void Guard()
        {
            this.ThrowIfDisposed();

            _lock.EnterReadLock();
        }

        public void Dispose(bool disposing)
        {
            _lock.ExitReadLock();
        }

        public void Dispose()
        {
            if (IsDisposed)
                return;

            IsDisposed = true;
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public bool IsDisposed { get; private set; }
    }
}
