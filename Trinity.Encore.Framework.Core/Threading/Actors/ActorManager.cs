using System;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Trinity.Encore.Framework.Core.Threading.Actors
{
    internal static class ActorManager
    {
        private static readonly Scheduler[] _schedulers;

        static ActorManager()
        {
            var procs = Environment.ProcessorCount;
            _schedulers = new Scheduler[procs];

            for (var i = 0; i < procs; i++)
                _schedulers[i] = new Scheduler();
        }

        private static Scheduler PickScheduler()
        {
            Contract.Ensures(Contract.Result<Scheduler>() != null);

            var sched = _schedulers.Aggregate((acc, current) => current.ActorCount < acc.ActorCount ? current : acc);
            Contract.Assume(sched != null);
            return sched;
        }

        public static Scheduler RegisterActor(Actor actor)
        {
            Contract.Requires(actor != null);
            Contract.Ensures(Contract.Result<Scheduler>() != null);

            var scheduler = PickScheduler();
            scheduler.AddActor(actor);
            return scheduler;
        }
    }
}
