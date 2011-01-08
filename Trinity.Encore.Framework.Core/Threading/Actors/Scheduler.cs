using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading;
using Trinity.Encore.Framework.Core.Runtime;

namespace Trinity.Encore.Framework.Core.Threading.Actors
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

        private readonly ManualResetEventSlim processedEvent = new ManualResetEventSlim(true);

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

                processedEvent.Reset();

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

                processedEvent.Set();
            }
        }

        ~Scheduler()
        {
            Dispose(false);
        }

        private void Dispose(bool disposing)
        {
            _running = false;

            // Wait for processing to stop.
            processedEvent.Wait();

            // Notify all actors that we're shutting down.
            var evnt = Disposed;
            if (evnt != null)
                evnt(this, EventArgs.Empty);
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
