using System;
using System.Diagnostics.Contracts;
using System.Threading;

namespace Trinity.Core.Threading.Actors
{
    internal sealed class ActorWaitHandle : IWaitable
    {
        private readonly AutoResetEvent _event;

        [ContractInvariantMethod]
        private void Invariant()
        {
            Contract.Invariant(_event != null);
        }

        public ActorWaitHandle(AutoResetEvent eventHandle)
        {
            Contract.Requires(eventHandle != null);

            _event = eventHandle;
        }

        public bool Wait()
        {
            return _event.WaitOne();
        }

        public bool Wait(TimeSpan timeout)
        {
            return _event.WaitOne(timeout);
        }

        public bool WaitExitContext(TimeSpan timeout)
        {
            return _event.WaitOne(timeout, true);
        }
    }
}
