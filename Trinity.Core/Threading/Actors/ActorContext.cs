using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Trinity.Core.Runtime;

namespace Trinity.Core.Threading.Actors
{
    public sealed class ActorContext : IDisposableResource
    {
        private static readonly Lazy<ActorContext> _globalLazy = new Lazy<ActorContext>(() =>
        {
            var count = Environment.ProcessorCount;
            var schedulers = new SchedulerType[count];

            // The global context should use its own threads to avoid interfering with other
            // threading facilities in the framework.
            for (var i = 0; i < count; i++)
                schedulers[i] = SchedulerType.SingleThread;

            return new ActorContext(schedulers);
        });

        /// <summary>
        /// Gets the AppDomain-global ActorContext instance.
        /// </summary>
        public static ActorContext Global
        {
            get
            {
                Contract.Ensures(Contract.Result<ActorContext>() != null);

                var ctx = _globalLazy.Value;
                Contract.Assume(ctx != null);
                return ctx;
            }
        }

        private readonly List<IScheduler> _schedulers = new List<IScheduler>();

        public ActorContext(params SchedulerType[] schedulers)
        {
            Contract.Requires(schedulers != null);
            Contract.Requires(schedulers.Length >= 1);

            foreach (var type in schedulers)
            {
                IScheduler scheduler;

                switch (type)
                {
                    case SchedulerType.SingleThread:
                        scheduler = new SingleThreadScheduler();
                        break;
                    case SchedulerType.ThreadPool:
                        throw new NotImplementedException();
                    case SchedulerType.TaskParallelLibrary:
                        throw new NotImplementedException();
                    default:
                        throw new ArgumentException("An invalid scheduler type was given.", "schedulers");
                }

                _schedulers.Add(scheduler);
            }
        }

        private IScheduler PickScheduler()
        {
            Contract.Ensures(Contract.Result<IScheduler>() != null);

            // There is an obvious race condition here, but we ignore it, as it would take way too much
            // locking to deal with it.
            var sched = _schedulers.Aggregate((acc, current) => current.ActorCount < acc.ActorCount ? current : acc);
            Contract.Assume(sched != null);
            return sched;
        }

        internal IScheduler RegisterActor(Actor actor)
        {
            Contract.Requires(actor != null);
            Contract.Ensures(Contract.Result<IScheduler>() != null);

            var scheduler = PickScheduler();
            scheduler.AddActor(actor);
            return scheduler;
        }

        ~ActorContext()
        {
            InternalDispose();
        }

        private void InternalDispose()
        {
            foreach (var scheduler in _schedulers)
                scheduler.Dispose();
        }

        public void Dispose()
        {
            if (IsDisposed)
                return;

            InternalDispose();
            IsDisposed = true;
            GC.SuppressFinalize(this);
        }

        public bool IsDisposed { get; private set; }
    }
}
