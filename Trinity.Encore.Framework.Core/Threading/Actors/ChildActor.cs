using System;
using System.Diagnostics.Contracts;
using Trinity.Encore.Framework.Core.Security;

namespace Trinity.Encore.Framework.Core.Threading.Actors
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

        public void Dispose()
        {
            Post(InternalDispose);
        }

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

        public void Post(Action msg)
        {
            _parent.Post(msg);
        }
    }

    public abstract class ChildActor<TThis> : ChildActor, IActor<TThis>
        where TThis : ChildActor<TThis>
    {
        protected ChildActor(IActor parent)
            : base(parent)
        {
            Contract.Requires(parent != null);
        }

        public void Post(Action<TThis> msg)
        {
            Post(() => msg((TThis)this));
        }
    }
}
