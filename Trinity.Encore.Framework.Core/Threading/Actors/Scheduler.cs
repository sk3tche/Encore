using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading;

namespace Trinity.Encore.Framework.Core.Threading.Actors
{
    internal sealed class Scheduler
    {
        private readonly ConcurrentQueue<Actor> _newActors = new ConcurrentQueue<Actor>();

        private readonly List<Actor> _actors = new List<Actor>();

        private readonly Thread _thread;

        private readonly AutoResetEvent _event = new AutoResetEvent(false);

        private static volatile int _threadCount;

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
            _thread.Name = "Actor Thread " + _threadCount++;
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
            Actor newActor;
            while (_newActors.Count > 0)
            {
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
            while (true)
            {
                _event.WaitOne();
                TakeNewActors();

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
            }
        }
    }
}
