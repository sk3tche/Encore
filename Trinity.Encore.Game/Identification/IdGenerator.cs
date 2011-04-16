using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading;

namespace Trinity.Encore.Game.Identification
{
    /// <summary>
    /// A thread-safe ID generator. Generates IDs in the form of Int64 variables.
    /// </summary>
    public sealed class IdGenerator
    {
        private readonly ConcurrentQueue<ulong> _recycledIds = new ConcurrentQueue<ulong>();

        private long _lastId;

        [ContractInvariantMethod]
        private void Invariant()
        {
            Contract.Invariant(_recycledIds != null);
        }

        /// <summary>
        /// Constructs a new instance of the IdGenerator class.
        /// </summary>
        /// <param name="begin">The ID to begin generating at.</param>
        [CLSCompliant(false)]
        public IdGenerator(ulong begin = (ulong)0)
        {
            _lastId = (long)begin;
        }

        /// <summary>
        /// Generates an ID.
        /// 
        /// If the recycling queue isn't empty, the ID returned will be a
        /// recycled ID.
        /// </summary>
        [CLSCompliant(false)]
        public ulong GenerateId()
        {
            ulong id;
            if (_recycledIds.TryDequeue(out id))
                return id;

            return (ulong)Interlocked.Increment(ref _lastId);
        }

        /// <summary>
        /// Queues an ID for recycling.
        /// </summary>
        /// <param name="id">The ID to recycle.</param>
        [CLSCompliant(false)]
        public void RecycleId(ulong id)
        {
            _recycledIds.Enqueue(id);
        }

        /// <summary>
        /// Gets the last generated (non-recycled) ID.
        /// </summary>
        [CLSCompliant(false)]
        public ulong LastId
        {
            get { return (ulong)_lastId; }
        }

        /// <summary>
        /// Gets a collection containing all IDs currently queued for recycling.
        /// </summary>
        public IEnumerable<ulong> RecycledIds
        {
            get
            {
                Contract.Ensures(Contract.Result<IEnumerable<ulong>>() != null);

                var arr = _recycledIds.ToArray();
                Contract.Assume(arr != null);
                return arr;
            }
        }
    }
}
