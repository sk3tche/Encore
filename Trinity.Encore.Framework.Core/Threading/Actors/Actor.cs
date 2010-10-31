using System;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Trinity.Encore.Framework.Core.Threading.Actors
{
    public abstract class Actor : IDisposable
    {
        /// <summary>
        /// Used to post messages to run in this Actor's synchronization context.
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
        public CancellationToken CancellationToken
        {
            get { return _cts.Token; }
        }

        private readonly CancellationTokenSource _cts = new CancellationTokenSource();

        protected Actor()
        {
            var options = new DataflowBlockOptions(TaskScheduler.Default, 1, DataflowBlockOptions.UnboundedMessagesPerTask,
                CancellationToken); // DataflowBlockOptions is immutable, so we can safely pass it along to both blocks.

            IncomingMessages = new ActionBlock<Action>(x => HandleIncomingMessage(x), options);
            OutgoingMessages = new BroadcastBlock<Action>(x => x /* Delegates are immutable; no real cloning needed. */, options);
        }

        private void HandleIncomingMessage(Action act)
        {
            try
            {
                act();
            }
            catch (Exception)
            {
                // TODO: Log the exception.
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
            OutgoingMessages.LinkTo(other.IncomingMessages, unlinkAfterOneMsg);
        }

        /// <summary>
        /// Disposes of the Actor instance.
        /// 
        /// This method cancels all source and target blocks linked to this Actor's CancellationToken (including
        /// this Actor's IncomingMessages and OutgoingMessages blocks).
        /// </summary>
        public void Dispose()
        {
            _cts.Cancel();
        }
    }
}
