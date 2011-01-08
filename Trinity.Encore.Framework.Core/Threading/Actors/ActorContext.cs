using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading;
using Trinity.Encore.Framework.Core.Runtime;

namespace Trinity.Encore.Framework.Core.Threading.Actors
{
    public sealed class ActorContext : IDisposableResource
    {
        private static readonly Lazy<ActorContext> _globalLazy = new Lazy<ActorContext>(() => new ActorContext(Environment.ProcessorCount));

        /// <summary>
        /// Gets the AppDomain-global ActorContext instance.
        /// </summary>
        public static ActorContext Global
        {
            get
            {
                Contract.Ensures(Contract.Result<ActorContext>() != null);

                return _globalLazy.Value;
            }
        }

        private readonly Scheduler[] _schedulers;

        public ActorContext(int? schedulerCount = null)
        {
            var count = schedulerCount ?? Environment.ProcessorCount;
            _schedulers = new Scheduler[count];

            for (var i = 0; i < count; i++)
                _schedulers[i] = new Scheduler();
        }

        private Scheduler PickScheduler()
        {
            Contract.Ensures(Contract.Result<Scheduler>() != null);

            // There is an obvious race condition here, but we ignore it, as it would take way too much
            // locking to deal with it.
            var sched = _schedulers.Aggregate((acc, current) => current.ActorCount < acc.ActorCount ? current : acc);
            Contract.Assume(sched != null);
            return sched;
        }

        internal Scheduler RegisterActor(Actor actor)
        {
            Contract.Requires(actor != null);
            Contract.Ensures(Contract.Result<Scheduler>() != null);

            var scheduler = PickScheduler();
            scheduler.AddActor(actor);
            return scheduler;
        }

        ~ActorContext()
        {
            Dispose(false);
        }

        private void Dispose(bool disposing)
        {
            foreach (var scheduler in _schedulers)
                scheduler.Dispose();
        }

        public void Dispose()
        {
            if (IsDisposed)
                return;

            Dispose(true);
            IsDisposed = true;
            GC.SuppressFinalize(this);
        }

        public bool IsDisposed { get; private set; }
    }
}
