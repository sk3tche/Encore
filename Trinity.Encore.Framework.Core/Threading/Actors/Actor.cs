using System;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Trinity.Encore.Framework.Core.Exceptions;

namespace Trinity.Encore.Framework.Core.Threading.Actors
{
    public abstract class Actor : IDisposable
    {
        /// <summary>
        /// Used to post messages to run in this Actor's synchronization context. This block always
        /// accepts posted messages, unless the Actor is disposed.
        /// 
        /// Do not call DeclinePermanently on this block. This state is managed by the Actor itself.
        /// </summary>
        public ITargetBlock<Action> IncomingMessages { get; private set; }

        /// <summary>
        /// Used to pull messages broadcasted by this Actor.
        /// </summary>
        public ISourceBlock<Action> OutgoingMessages { get; private set; }

        /// <summary>
        /// Gets a CancellationToken that can be used to link cancellation of source or target block
        /// to the cancellation of this Actor.
        /// </summary>
        public CancellationToken CancellationToken { get; private set; }

        private readonly CancellationTokenSource _cts;

        [ContractInvariantMethod]
        private void Invariant()
        {
            Contract.Invariant(IncomingMessages != null);
            Contract.Invariant(OutgoingMessages != null);
        }

        private void Setup()
        {
            var options = new DataflowBlockOptions(TaskScheduler.Default, 1, DataflowBlockOptions.UnboundedMessagesPerTask,
                CancellationToken); // DataflowBlockOptions is immutable, so we can safely pass it along to both blocks.

            IncomingMessages = new ActionBlock<Action>(x => HandleIncomingMessage(x), options);
            OutgoingMessages = new BroadcastBlock<Action>(x => x /* Delegates are immutable; no real cloning needed. */, options);
        }

        /// <summary>
        /// Creates an Actor instance, linking its cancellation to a given token.
        /// </summary>
        /// <param name="cancellationToken">The CancellationToken to link to.</param>
        protected Actor(CancellationToken cancellationToken)
        {
            CancellationToken = cancellationToken;

            Setup();
        }

        protected Actor()
        {
            _cts = new CancellationTokenSource();
            CancellationToken = _cts.Token;

            Setup();
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
            Contract.Requires(other != null);

            return OutgoingMessages.LinkTo(other.IncomingMessages, unlinkAfterOneMsg);
        }

        /// <summary>
        /// Disposes of the Actor instance.
        /// 
        /// This method cancels all source and target blocks linked to this Actor's CancellationToken (including
        /// this Actor's IncomingMessages and OutgoingMessages blocks).
        /// </summary>
        public void Dispose()
        {
            // If we have _cts set, it means we aren't linked to some CancellationToken handed to us from the
            // outside, and that we can just cancel our CancellationTokenSource. By doing so, we also cancel
            // anything linked to it (such as other Actor instances).
            if (_cts != null)
                _cts.Cancel();
            else
                IncomingMessages.DeclinePermanently(); // Manually stop the incoming messages block.

            // Note that we do NOT set IncomingMessages and OutgoingMessages to null. This could cause problems
            // in other areas, as, in an asynchronous architecture, all components cannot know when another
            // has been canceled.
        }
    }
}
