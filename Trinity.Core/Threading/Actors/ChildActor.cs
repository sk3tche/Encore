using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using Trinity.Core.Security;

namespace Trinity.Core.Threading.Actors
{
    public abstract class ChildActor : RestrictedObject, IActor
    {
        protected ChildActor(IActor parent)
        {
            Contract.Requires(parent != null);

            _parent = parent;
        }

        [ContractInvariantMethod]
        private void Invariant()
        {
            Contract.Invariant(_parent != null);
        }

        private readonly IActor _parent;

        ~ChildActor()
        {
            Dispose(false);
        }

        [SuppressMessage("Microsoft.Usage", "CA1816", Justification = "Behavior intended.")]
        [SuppressMessage("Microsoft.Design", "CA1063", Justification = "Behavior intended.")]
        public void Dispose()
        {
            PostAsync(InternalDispose);
        }

        [SuppressMessage("Microsoft.Usage", "CA1816", Justification = "Behavior intended.")]
        private void InternalDispose()
        {
            if (IsDisposed)
                return;

            Dispose(true);
            IsDisposed = true;
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
        }

        public bool IsDisposed { get; private set; }

        public void Join()
        {
            _parent.Join();
        }

        public void PostAsync(Action msg)
        {
            _parent.PostAsync(msg);
        }

        public IWaitable PostWait(Action msg)
        {
            return _parent.PostWait(msg);
        }
    }

    [SuppressMessage("Microsoft.Design", "CA1063", Justification = "IDisposable is part of IActor.")]
    public abstract class ChildActor<TThis> : ChildActor, IActor<TThis>
        where TThis : ChildActor<TThis>
    {
        protected ChildActor(IActor parent)
            : base(parent)
        {
            Contract.Requires(parent != null);
        }

        public void PostAsync(Action<TThis> msg)
        {
            PostAsync(() => msg((TThis)this));
        }

        public IWaitable PostWait(Action<TThis> msg)
        {
            return PostWait(() => msg((TThis)this));
        }
    }
}
