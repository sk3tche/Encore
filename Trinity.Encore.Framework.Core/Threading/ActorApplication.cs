using System;
using System.Diagnostics.Contracts;
using System.Reflection;
using Trinity.Encore.Framework.Core.Configuration;
using Trinity.Encore.Framework.Core.Exceptions;
using Trinity.Encore.Framework.Core.Initialization;
using Trinity.Encore.Framework.Core.Threading.Actors;

namespace Trinity.Encore.Framework.Core.Threading
{
    public abstract class ActorApplication<T> : Agent
        where T : ActorApplication<T>
    {
        private readonly Lazy<T> _creator;

        private volatile bool _shouldStop;

        public T Instance
        {
            get { return _creator.Value; }
        }

        public ApplicationConfiguration Configuration { get; private set; }

        public override TimeSpan RunInterval
        {
            get { return TimeSpan.FromMilliseconds(50); }
        }

        [ContractInvariantMethod]
        private void Invariant()
        {
            Contract.Invariant(_creator != null);
        }

        protected ActorApplication(Func<T> creator)
        {
            Contract.Requires(creator != null);

            _creator = new Lazy<T>(creator);
        }

        public void Start(string[] args)
        {
            Contract.Requires(args != null);

            GC.Collect();

            var exeFile = Assembly.GetEntryAssembly().Location;
            if (!string.IsNullOrEmpty(exeFile))
            {
                Configuration = new ApplicationConfiguration(exeFile);
                Configuration.ScanAll();
                Configuration.Open();
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

            ScheduleRun();
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

            if (Configuration != null)
                Configuration.Save();

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

        protected override bool Run(TimeSpan diff)
        {
            if (_shouldStop)
                return false;

            OnUpdate(diff);
            return true;
        }

        protected virtual void OnUpdate(TimeSpan diff)
        {
        }
    }
}
