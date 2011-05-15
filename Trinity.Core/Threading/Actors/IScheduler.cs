using System;
using System.Diagnostics.Contracts;
using Trinity.Core.Runtime;

namespace Trinity.Core.Threading.Actors
{
    [ContractClass(typeof(SchedulerContracts))]
    internal interface IScheduler : IDisposableResource
    {
        event EventHandler Disposed;

        int ActorCount { get; }

        void AddActor(Actor actor);
    }

    [ContractClassFor(typeof(IScheduler))]
    public abstract class SchedulerContracts : IScheduler
    {
        public abstract event EventHandler Disposed;

        public int ActorCount
        {
            get
            {
                Contract.Ensures(Contract.Result<int>() >= 0);

                return 0;
            }
        }

        public void AddActor(Actor actor)
        {
            Contract.Requires(actor != null);
        }

        public abstract void Dispose();

        public abstract bool IsDisposed { get; }
    }
}
