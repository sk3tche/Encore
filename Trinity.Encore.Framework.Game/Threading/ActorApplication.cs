using System;
using System.Diagnostics.Contracts;
using System.Reflection;
using System.Threading;
using Trinity.Encore.Framework.Core.Configuration;
using Trinity.Encore.Framework.Core.Exceptions;
using Trinity.Encore.Framework.Core.Initialization;
using Trinity.Encore.Framework.Core.Logging;
using Trinity.Encore.Framework.Core.Threading.Actors;

namespace Trinity.Encore.Framework.Game.Threading
{
    public abstract class ActorApplication<T> : SingletonActor<T>
        where T : ActorApplication<T>
    {
        private static readonly LogProxy _log = new LogProxy("ActorApplication");

        public const int UpdateDelay = 50;

        public event EventHandler Shutdown;

        private ActorTimer _updateTimer;

        private DateTime _lastUpdate;

        private bool _shouldStop;

        private ApplicationConfiguration _configuration;

        [ContractInvariantMethod]
        private void Invariant()
        {
            Contract.Invariant(_updateTimer != null);
        }

        protected ActorApplication()
        {
            _updateTimer = new ActorTimer(this, UpdateCallback, TimeSpan.FromMilliseconds(UpdateDelay), UpdateDelay);
            _lastUpdate = DateTime.Now;
        }

        protected override void Dispose(bool disposing)
        {
            _updateTimer.Dispose();
            _updateTimer = null;

            base.Dispose(disposing);
        }

        public void Start(string[] args)
        {
            Contract.Requires(args != null);

            GC.Collect();

            var asmPath = Assembly.GetEntryAssembly().Location;
            Contract.Assume(!string.IsNullOrEmpty(asmPath));
            _configuration = new ApplicationConfiguration(asmPath);
            _configuration.ScanAll();
            _configuration.Open();

            InitializationManager.InitializeAll();

            try
            {
                OnStart(args);
            }
            catch (Exception ex)
            {
                ExceptionManager.RegisterException(ex);
            }

            _log.Info("{0} initialized.", GetType().Name);
        }

        public void Stop()
        {
            try
            {
                var shutdownEvent = Shutdown;
                if (shutdownEvent != null)
                    shutdownEvent(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                ExceptionManager.RegisterException(ex);
            }

            try
            {
                OnStop();
            }
            catch (Exception ex)
            {
                ExceptionManager.RegisterException(ex);
            }

            InitializationManager.TeardownAll();

            _configuration.Save();

            GC.Collect();

            _shouldStop = true;
        }

        protected virtual void OnStart(string[] args)
        {
            Contract.Requires(args != null);
        }

        protected virtual void OnStop()
        {
        }

        private void UpdateCallback()
        {
            if (_shouldStop)
            {
                _updateTimer.Change(TimeSpan.FromMilliseconds(Timeout.Infinite));
                return;
            }

            var now = DateTime.Now;
            var diff = now - _lastUpdate;
            _lastUpdate = now;

            OnUpdate(diff);
        }

        protected virtual void OnUpdate(TimeSpan diff)
        {
        }
    }
}
