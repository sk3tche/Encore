using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Trinity.Encore.Framework.Core.Exceptions;
using Trinity.Encore.Framework.Core.Runtime;
using Trinity.Encore.Framework.Core.Security;

namespace Trinity.Encore.Framework.Core.Threading.Actors
{
    public abstract class Actor : RestrictedObject, IActor
    {
        /// <summary>
        /// Used to post messages to run in this Actor's synchronization context. This block always
        /// accepts posted messages, unless the Actor is disposed.
        /// 
        /// Do not call DeclinePermanently on this block. This state is managed by the Actor itself.
        /// </summary>
        public TargetPort<Action> IncomingMessages { get; private set; }

        /// <summary>
        /// Used to pull messages broadcasted by this Actor.
        /// </summary>
        public SourcePort<Action> OutgoingMessages { get; private set; }

        /// <summary>
        /// Gets a CancellationToken that can be used to link cancellation of source or target block
        /// to the cancellation of this Actor.
        /// </summary>
        public CancellationToken CancellationToken
        {
            get { return CancellationTokenSource.Token; }
        }

        public CancellationTokenSource CancellationTokenSource { get; private set; }

        [ContractInvariantMethod]
        private void Invariant()
        {
            Contract.Invariant(IncomingMessages != null);
            Contract.Invariant(OutgoingMessages != null);
            Contract.Invariant(CancellationTokenSource != null);
            Contract.Invariant(_links != null);
        }

        /// <summary>
        /// Creates an Actor instance, linking its cancellation to a given token.
        /// </summary>
        /// <param name="cancellationTokenSource">The CancellationToken to link to.</param>
        protected Actor(CancellationTokenSource cancellationTokenSource)
        {
            Contract.Requires(cancellationTokenSource != null);

            CancellationTokenSource = cancellationTokenSource;

            var options = GetOptions(CancellationToken);
            IncomingMessages = new TargetPort<Action>(new ActionBlock<Action>(new Action<Action>(HandleIncomingMessage), options));
            OutgoingMessages = new SourcePort<Action>(new BroadcastBlock<Action>(x => x, options));
        }

        protected Actor()
            : this(new CancellationTokenSource())
        {
        }

        ~Actor()
        {
            Dispose(false);
        }

        private void HandleIncomingMessage(Action act)
        {
            Contract.Requires(act != null);

            try
            {
                act();
            }
            catch (Exception ex)
            {
                ExceptionManager.RegisterException(ex, this);
                Dispose();
            }
        }

        /// <summary>
        /// Links this Actor instance with the given Actor, so that messages published on this Actor's
        /// OutgoingMessages block will be pipelined to the other Actor's IncomingMessages block.
        /// </summary>
        /// <param name="other">The Actor to link to.</param>
        /// <param name="unlinkAfterOneMsg">Whether or not to unlink after receiving a single message.</param>
        /// <returns>An object that, when disposed, unlinks the other Actor from this Actor.</returns>
        public IDisposable LinkTo(Actor other, bool unlinkAfterOneMsg = false)
        {
            var link = OutgoingMessages.Link(other.IncomingMessages, unlinkAfterOneMsg);
            _links.Add(link);
            return link;
        }

        private readonly List<IDisposable> _links = new List<IDisposable>();

        protected virtual void Dispose(bool disposing)
        {
            if (IsDisposed)
                return;

            // Clear all links.
            foreach (var link in _links)
                link.Dispose();

            _links.Clear();
            CancellationTokenSource.Cancel();
            IsDisposed = true;
        }

        /// <summary>
        /// Disposes of the Actor instance.
        /// 
        /// This method cancels all source and target blocks linked to this Actor's CancellationTokenSource (including
        /// this Actor's IncomingMessages and OutgoingMessages blocks).
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public bool IsDisposed { get; private set; }

        public static DataflowBlockOptions GetOptions(CancellationToken token, int maxDegreeOfParallelism = 1,
            int maxMessagesPerTask = DataflowBlockOptions.UnboundedMessagesPerTask, TaskScheduler scheduler = null)
        {
            Contract.Requires(maxDegreeOfParallelism > 0 || maxDegreeOfParallelism == -1);
            Contract.Requires(maxMessagesPerTask > 0 || maxMessagesPerTask == -1);
            Contract.Ensures(Contract.Result<DataflowBlockOptions>() != null);

            if (scheduler == null)
                scheduler = TaskScheduler.Default;

            return new DataflowBlockOptions(scheduler, maxDegreeOfParallelism, maxMessagesPerTask, token);
        }
    }
}
