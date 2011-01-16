using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Threading;
using Trinity.Core.Runtime;

namespace Trinity.Core.Threading.Actors
{
    /// <summary>
    /// Manages registration and execution of Actor instances.
    /// </summary>
    internal sealed class Scheduler : IDisposableResource
    {
        private readonly ConcurrentQueue<Actor> _newActors = new ConcurrentQueue<Actor>();

        private readonly List<Actor> _actors = new List<Actor>();

        private readonly Thread _thread;

        private readonly AutoResetEvent _event = new AutoResetEvent(false);

        private readonly ManualResetEventSlim _processedEvent = new ManualResetEventSlim(true);

        private static volatile int _threadCount;

        private volatile bool _running = true;

        public event EventHandler Disposed;

        /// <summary>
        /// Gets the amount of actors in this Scheduler.
        /// </summary>
        /// <value>The amount of actors managed by this Scheduler.</value>
        public int ActorCount
        {
            get { return _actors.Count; }
        }

        [ContractInvariantMethod]
        private void Invariant()
        {
            Contract.Invariant(_newActors != null);
            Contract.Invariant(_actors != null);
            Contract.Invariant(_thread != null);
            Contract.Invariant(_event != null);
        }

        public Scheduler()
        {
            _thread = new Thread(ThreadBody);
            _thread.Name = "Actor Thread {0}".Interpolate(_threadCount++);
            _thread.IsBackground = true;
            _thread.Start();
        }

        public void AddActor(Actor actor)
        {
            Contract.Requires(actor != null);

            _newActors.Enqueue(actor);
            _event.Set();
        }

        private void TakeNewActors()
        {
            while (_newActors.Count > 0)
            {
                Actor newActor;
                if (!_newActors.TryDequeue(out newActor))
                    continue;

                // Accomodate for the race condition in Actor.Post.
                if (newActor.IsActive)
                    continue;

                newActor.IsActive = true;
                newActor.Scheduler = this;
                _actors.Add(newActor);
            }
        }

        private void ThreadBody()
        {
            while (_running)
            {
                _event.WaitOne();
                TakeNewActors();

                _processedEvent.Reset();

                while (_actors.Count > 0)
                {
                    TakeNewActors();

                    // Process all actors; remove any that break execution/are disposed.
                    _actors.RemoveAll(x =>
                    {
                        if (x.IsDisposed || (!x.ProcessMain() & !x.ProcessMessages()))
                        {
                            x.IsActive = false;
                            return true;
                        }

                        return false;
                    });

                    Thread.Yield();
                }

                _processedEvent.Set();
            }
        }

        ~Scheduler()
        {
            InternalDispose();
        }

        private void InternalDispose()
        {
            _running = false;

            // Wait for processing to stop.
            _processedEvent.Wait();

            // Notify all actors that we're shutting down.
            var evnt = Disposed;
            if (evnt != null)
                evnt(this, EventArgs.Empty);
        }

        [SuppressMessage("Microsoft.Usage", "CA2213", Justification = "_event must not be disposed.")]
        [SuppressMessage("Microsoft.Usage", "CA2213", Justification = "_processedEvent must not be disposed.")]
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
