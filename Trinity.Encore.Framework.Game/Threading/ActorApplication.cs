using System;
using System.Diagnostics.Contracts;
using System.Reflection;
using System.Threading;
using Trinity.Encore.Framework.Core.Configuration;
using Trinity.Encore.Framework.Core.Exceptions;
using Trinity.Encore.Framework.Core.Initialization;
using Trinity.Encore.Framework.Core.Threading.Actors;

namespace Trinity.Encore.Framework.Game.Threading
{
    public abstract class ActorApplication<T> : SingletonActor<T>
        where T : ActorApplication<T>
    {
        public const int UpdateDelay = 50;

        private ActorTimer _updateTimer;

        private DateTime _lastUpdate;

        private bool _shouldStop;

        private ApplicationConfiguration _configuration;

        [ContractInvariantMethod]
        private void Invariant()
        {
            Contract.Invariant(_updateTimer != null);
        }

        protected ActorApplication(Func<T> creator)
            : base(creator)
        {
            Contract.Requires(creator != null);

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

            var exeFile = Assembly.GetEntryAssembly().Location;
            if (!string.IsNullOrEmpty(exeFile))
            {
                _configuration = new ApplicationConfiguration(exeFile);
                _configuration.ScanAll();
                _configuration.Open();
            }

            InitializationManager.InitializeAll();

            try
            {
                OnStart(args);
            }
            catch (Exception ex)
            {
                ExceptionManager.RegisterException(ex);
            }
        }

        public void Stop()
        {
            try
            {
                OnStop();
            }
            catch (Exception ex)
            {
                ExceptionManager.RegisterException(ex);
            }

            InitializationManager.TeardownAll();

            if (_configuration != null)
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
