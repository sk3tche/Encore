using System;
using System.Diagnostics.Contracts;
using Trinity.Core.Runtime;

namespace Trinity.Core.Threading
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
            InternalDispose();
        }

        public void Guard()
        {
            _lock.EnterReadLock();
        }

        private void InternalDispose()
        {
            _lock.ExitReadLock();
        }

        public void Dispose()
        {
            if (IsDisposed)
                return;

            InternalDispose();
            IsDisposed = true;
            GC.SuppressFinalize(this);
        }

        public bool IsDisposed { get; private set; }
    }
}
