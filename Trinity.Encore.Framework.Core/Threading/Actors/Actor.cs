using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading;
using Trinity.Encore.Framework.Core.Exceptions;
using Trinity.Encore.Framework.Core.Security;

namespace Trinity.Encore.Framework.Core.Threading.Actors
{
    public class Actor : RestrictedObject, IActor
    {
        private IEnumerator<Operation> _msgIterator;

        private IEnumerator<Operation> _mainIterator;

        private readonly ConcurrentQueue<Action> _msgQueue = new ConcurrentQueue<Action>();

        private readonly AutoResetEvent _disposeEvent;

        public bool IsDisposed { get; private set; }

        internal bool IsActive { get; set; }

        internal Scheduler Scheduler { get; set; }

        public ActorContext Context { get; private set; }

        [ContractInvariantMethod]
        private void Invariant()
        {
            Contract.Invariant(Context != null);
            Contract.Invariant(_msgQueue != null);
            Contract.Invariant(_disposeEvent != null);
            // Don't add IsDisposed here. It would cause major cancellation issues in an asynchronous environment.
        }

        public Actor()
            : this(ActorContext.Global)
        {
        }

        public Actor(ActorContext context)
        {
            Contract.Requires(context != null);

            Context = context;
            _disposeEvent = new AutoResetEvent(false);

            _msgIterator = EnumerateMessages();
            _mainIterator = Main();

            Scheduler = Context.RegisterActor(this);
            Scheduler.Disposed += OnDisposed;
        }

        private void OnDisposed(object sender, EventArgs args)
        {
            Scheduler.Disposed -= OnDisposed;

            // No guarantee is made about where an actor is disposed!
            InternalDispose();
        }

        ~Actor()
        {
            Dispose(false);
        }

        public void Join()
        {
            _disposeEvent.WaitOne();
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
            _disposeEvent.Set();

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
        }

        internal bool ProcessMessages()
        {
            _msgIterator.MoveNext();

            return _msgQueue.Count > 0;
        }

        internal bool ProcessMain()
        {
            var result = _mainIterator.MoveNext();

            // Happens if a yield break occurs.
            if (!result)
                return false;

            var operation = _mainIterator.Current;

            if (operation == Operation.Dispose)
                Dispose();

            return operation == Operation.Continue;
        }

        public void Post(Action msg)
        {
            _msgQueue.Enqueue(msg);

            Action tmp;
            while (!_msgQueue.TryPeek(out tmp))
                if (_msgQueue.Count == 0)
                    return; // The message was processed immediately, and we can just return.

            if (msg == tmp)
                Scheduler.AddActor(this); // The message was sent while the actor was idle; restart it to continue processing.
        }

        protected virtual IEnumerator<Operation> Main()
        {
            Contract.Ensures(Contract.Result<IEnumerator<Operation>>() != null);

            yield break; // No main by default.
        }

        private IEnumerator<Operation> EnumerateMessages()
        {
            Contract.Ensures(Contract.Result<IEnumerator<Operation>>() != null);

            while (true)
            {
                Action msg;
                if (_msgQueue.TryDequeue(out msg))
                {
                    var op = OnMessage(msg);
                    if (op != null)
                        yield return (Operation)op;
                }

                yield return Operation.Continue;
            }
        }

        protected virtual Operation? OnMessage(Action msg)
        {
            try
            {
                msg();
            }
            catch (Exception ex)
            {
                ExceptionManager.RegisterException(ex);
                return Operation.Dispose;
            }

            return null;
        }
    }

    public abstract class Actor<TThis> : Actor, IActor<TThis>
        where TThis : Actor<TThis>
    {
        protected Actor()
        {
        }

        protected Actor(ActorContext context)
            : base(context)
        {
            Contract.Requires(context != null);
        }

        public void Post(Action<TThis> msg)
        {
            Post(() => msg((TThis)this));
        }
    }
}
