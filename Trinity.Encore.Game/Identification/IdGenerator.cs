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
        private readonly ConcurrentQueue<long> _recycledIds = new ConcurrentQueue<long>();

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
        public IdGenerator(long begin = 0)
        {
            _lastId = begin;
        }

        /// <summary>
        /// Generates an ID.
        /// 
        /// If the recycling queue isn't empty, the ID returned will be a
        /// recycled ID.
        /// </summary>
        public long GenerateId()
        {
            long id;
            if (!_recycledIds.TryDequeue(out id))
                id = Interlocked.Increment(ref _lastId);

            return id;
        }

        /// <summary>
        /// Queues an ID for recycling.
        /// </summary>
        /// <param name="id">The ID to recycle.</param>
        public void RecycleId(long id)
        {
            _recycledIds.Enqueue(id);
        }

        /// <summary>
        /// Gets the last generated (non-recycled) ID.
        /// </summary>
        public long LastId
        {
            get { return _lastId; }
        }

        /// <summary>
        /// Gets a collection containing all IDs currently queued for recycling.
        /// </summary>
        public IEnumerable<long> RecycledIds
        {
            get
            {
                Contract.Ensures(Contract.Result<IEnumerable<long>>() != null);

                var arr = _recycledIds.ToArray();
                Contract.Assume(arr != null);
                return arr;
            }
        }
    }
}
