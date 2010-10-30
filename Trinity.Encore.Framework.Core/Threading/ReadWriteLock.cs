using System;
using System.Diagnostics.Contracts;
using System.Threading;

namespace Trinity.Encore.Framework.Core.Threading
{
    public sealed class ReadWriteLock : ReaderWriterLockSlim
    {
        private readonly ReadScopeGuard _readGuard;

        private readonly WriteScopeGuard _writeGuard;

        public ReadWriteLock(LockRecursionPolicy recursionPolicy = LockRecursionPolicy.NoRecursion)
            : base(recursionPolicy)
        {
            _writeGuard = new WriteScopeGuard(this);
            _readGuard = new ReadScopeGuard(this);
        }

        [ContractInvariantMethod]
        private void Invariant()
        {
            Contract.Invariant(_readGuard != null);
            Contract.Invariant(_writeGuard != null);
        }

        public IDisposable GuardRead()
        {
            Contract.Ensures(Contract.Result<IDisposable>() != null);

            _readGuard.Guard();
            return _readGuard;
        }

        public IDisposable GuardWrite()
        {
            Contract.Ensures(Contract.Result<IDisposable>() != null);

            _writeGuard.Guard();
            return _writeGuard;
        }
    }
}
