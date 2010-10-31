using System;
using System.Threading;
using System.Threading.Tasks;

namespace Trinity.Encore.Framework.Core.Threading.Actors
{
    /// <summary>
    /// An Agent is a special Actor that has a "body". It can schedule a method to run
    /// every RunInterval.
    /// </summary>
    public abstract class Agent : Actor
    {
        public abstract TimeSpan RunInterval { get; }

        private DateTime _lastUpdate;

        private void Setup()
        {
            _lastUpdate = DateTime.Now;
            Task.Factory.StartNew(RunInternal, CancellationToken);
        }

        protected Agent(CancellationToken cancellationToken)
            : base(cancellationToken)
        {
            Setup();
        }

        protected Agent()
        {
            Setup();
        }

        private void RunInternal()
        {
            try
            {
                Run(DateTime.Now - _lastUpdate);
            }
            catch (Exception)
            {
                // TODO: Log the exception.
                Dispose();
            }

            _lastUpdate = DateTime.Now;
            Task.Factory.StartNewDelayed(RunInterval.Milliseconds, RunInternal, CancellationToken);
        }

        public abstract void Run(TimeSpan diff);
    }
}
