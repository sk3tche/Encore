using System;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;
using Trinity.Encore.Framework.Core.Exceptions;

namespace Trinity.Encore.Framework.Core.Threading.Actors
{
    /// <summary>
    /// An Agent is a special Actor that has a "body". It can schedule a method to run
    /// every RunInterval.
    /// </summary>
    public abstract class Agent : Actor
    {
        /// <summary>
        /// The interval at which to run the agent body.
        /// </summary>
        public abstract TimeSpan RunInterval { get; }

        private volatile int _state;

        private DateTime _lastUpdate;

        private void StartExecution()
        {
            _state = (int)AgentState.Running;
            _lastUpdate = DateTime.Now;
            Task.Factory.StartNew(RunInternal, CancellationToken);
        }

        protected Agent(CancellationToken cancellationToken)
            : base(cancellationToken)
        {
            StartExecution();
        }

        protected Agent()
        {
            StartExecution();
        }

        private void RunInternal()
        {
            var runAgain = false;

            try
            {
                runAgain = Run(DateTime.Now - _lastUpdate);
            }
            catch (Exception ex)
            {
                ExceptionManager.RegisterException(ex, this);
                Dispose();
            }

            // No more processing is needed. Note that this doesn't necessarily imply cancellation.
            if (!runAgain)
            {
                _state = (int)AgentState.Stopped;
                return;
            }

            _lastUpdate = DateTime.Now;
            Task.Factory.StartNewDelayed(RunInterval.Milliseconds, RunInternal, CancellationToken);
        }

        /// <summary>
        /// The Agent's body method. This will be called every RunInterval.
        /// </summary>
        /// <param name="diff">Time since last call.</param>
        /// <returns>A value indicating whether or not any further calls to this method are desired.</returns>
        protected abstract bool Run(TimeSpan diff);

        /// <summary>
        /// Instructs the Agent to continue the processing loop, if it was previously stopped.
        /// </summary>
        protected void ScheduleRun()
        {
            if (_state == (int)AgentState.Running)
                throw new InvalidOperationException("The agent is already running.");

            StartExecution();
        }
    }
}
