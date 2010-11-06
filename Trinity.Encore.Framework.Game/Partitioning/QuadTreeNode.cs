using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading;
using System.Threading.Tasks.Dataflow;
using Microsoft.Xna.Framework;
using Trinity.Encore.Framework.Core.Collections;
using Trinity.Encore.Framework.Core.Threading.Actors;
using Trinity.Encore.Framework.Game.Entities;

namespace Trinity.Encore.Framework.Game.Partitioning
{
    public sealed class AddEntityArguments : Tuple<IWorldEntity, Action<bool>>
    {
        public AddEntityArguments(IWorldEntity entity, Action<bool> callback)
            : base(entity, callback)
        {
        }
    }

    public sealed class RemoveEntityArguments : Tuple<IWorldEntity, Action<bool>>
    {
        public RemoveEntityArguments(IWorldEntity entity, Action<bool> callback)
            : base(entity, callback)
        {
        }
    }

    public sealed class FindEntityArguments : Tuple<Func<IWorldEntity, bool>, Action<IWorldEntity>>
    {
        public FindEntityArguments(Func<IWorldEntity, bool> predicate, Action<IWorldEntity> callback)
            : base(predicate, callback)
        {
        }
    }

    public sealed class FindEntitiesArguments : Tuple<Func<IWorldEntity, bool>, int, Action<IEnumerable<IWorldEntity>>>
    {
        public FindEntitiesArguments(Func<IWorldEntity, bool> predicate, int maxCount, Action<IEnumerable<IWorldEntity>> callback)
            : base(predicate, maxCount, callback)
        {
        }
    }

    public class QuadTreeNode : Actor
    {
        public const float MinNodeLength = 250.0f;

        private const int West = 0;

        private const int East = 1;

        private const int North = 0;

        private const int South = 1;

        [ContractInvariantMethod]
        private void Invariant()
        {
            Contract.Invariant(AddEntityChannel != null);
            Contract.Invariant(RemoveEntityChannel != null);
            Contract.Invariant(FindEntityChannel != null);
            Contract.Invariant(FindEntitiesChannel != null);
        }

        public QuadTreeNode(BoundingBox bounds, CancellationTokenSource cts)
            : base(cts)
        {
            Contract.Requires(cts != null);

            Bounds = bounds;

            var cpus = Environment.ProcessorCount;
            Contract.Assume(cpus > 0);
            var options = GetOptions(cts.Token, cpus);

            AddEntityChannel = new TargetPort<AddEntityArguments>(new ActionBlock<AddEntityArguments>(new Action<AddEntityArguments>(AddEntity), options));
            RemoveEntityChannel = new TargetPort<RemoveEntityArguments>(new ActionBlock<RemoveEntityArguments>(new Action<RemoveEntityArguments>(RemoveEntity), options));
            FindEntityChannel = new TargetPort<FindEntityArguments>(new ActionBlock<FindEntityArguments>(new Action<FindEntityArguments>(FindEntity), options));
            FindEntitiesChannel = new TargetPort<FindEntitiesArguments>(new ActionBlock<FindEntitiesArguments>(new Action<FindEntitiesArguments>(FindEntities), options));
        }

        /// <summary>
        /// Sends a message instructing the QuadTreeNode to add an IWorldEntity.
        /// </summary>
        public TargetPort<AddEntityArguments> AddEntityChannel { get; private set; }

        /// <summary>
        /// Sends a message instructing the QuadTreeNode to remove an IWorldEntity.
        /// </summary>
        public TargetPort<RemoveEntityArguments> RemoveEntityChannel { get; private set; }

        /// <summary>
        /// Sends a message instructing the QuadTreeNode to find a specific IWorldEntity and call back once done.
        /// 
        /// The returned IWorldEntity may be null (if it was not found).
        /// </summary>
        public TargetPort<FindEntityArguments> FindEntityChannel { get; private set; }

        /// <summary>
        /// Sends a message instructing the QuadTreeNode to find a list of IWorldEntity instances and call back once done.
        /// 
        /// The returned sequence may be empty (but not null) if no results were found.
        /// </summary>
        public TargetPort<FindEntitiesArguments> FindEntitiesChannel { get; private set; }

        public BoundingBox Bounds { get; private set; }

        public float Length
        {
            get { return Bounds.Max.X - Bounds.Min.X; }
        }

        public float Width
        {
            get { return Bounds.Max.Y - Bounds.Min.Y; }
        }

        /// <summary>
        /// Children nodes (if IsLeaf == false; otherwise, null).
        /// </summary>
        private QuadTreeNode[,] _children;

        /// <summary>
        /// Contained entities (if IsLeaf == true; otherwise, null).
        /// </summary>
        private ConcurrentDictionary<EntityGuid, IWorldEntity> _entities;

        private void AddEntity(AddEntityArguments args)
        {
            Contract.Requires(args != null);
            Contract.Requires(args.Item1 != null);

            var entity = args.Item1;
            var callback = args.Item2;

            if (IsLeaf)
            {
                Contract.Assume(_entities != null);
                _entities.Add(entity.Guid, entity);
                entity.Node = this;

                if (callback != null)
                    callback(true);
            }

            var pos = entity.Position;
            for (var i = 0; i < 2; i++)
            {
                for (var j = 0; j < 2; j++)
                {
                    var node = _children[i, j];
                    if (node.Bounds.Contains(pos) != ContainmentType.Contains)
                        continue;

                    Contract.Assume(args.Item1 != null);
                    node.AddEntity(args);
                }
            }

            if (callback != null)
                callback(false);
        }

        private void RemoveEntity(RemoveEntityArguments args)
        {
            Contract.Requires(args != null);
            Contract.Requires(args.Item1 != null);

            var entity = args.Item1;
            var callback = args.Item2;

            if (IsLeaf)
            {
                Contract.Assume(_entities != null);
                _entities.Remove(entity.Guid);
                entity.Node = null;

                if (callback != null)
                    callback(true);
            }

            var pos = entity.Position;
            for (var i = 0; i < 2; i++)
            {
                for (var j = 0; j < 2; j++)
                {
                    var node = _children[i, j];
                    if (node.Bounds.Contains(pos) != ContainmentType.Contains)
                        continue;

                    Contract.Assume(args.Item1 != null);
                    node.RemoveEntity(args);
                }
            }

            if (callback != null)
                callback(false);
        }

        private void FindEntities(FindEntitiesArguments args)
        {
            Contract.Requires(args != null);
            Contract.Requires(args.Item1 != null);
            Contract.Requires(args.Item2 >= 0);
            Contract.Requires(args.Item3 != null);

            var results = RecursiveSearch(args.Item1, null, args.Item2);
            args.Item3(results); // Call back with the found entities.
        }

        private void FindEntity(FindEntityArguments args)
        {
            Contract.Requires(args != null);
            Contract.Requires(args.Item1 != null);
            Contract.Requires(args.Item2 != null);

            // We really just do the same as for multi-entity searches, but return a single (or no) result.
            var result = RecursiveSearch(args.Item1, null, 1).FirstOrDefault();
            args.Item2(result); // Call back with the found entity.
        }

        private IEnumerable<IWorldEntity> RecursiveSearch(Func<IWorldEntity, bool> criteria, ICollection<IWorldEntity> results,
            int maxCount)
        {
            Contract.Requires(criteria != null);
            Contract.Requires(maxCount >= 0);
            Contract.Ensures(Contract.Result<IEnumerable<IWorldEntity>>() != null);

            if (results == null)
                results = new List<IWorldEntity>();

            if (IsLeaf)
            {
                results.AddRange(_entities.Values.Where(criteria));
                return results; // We cannot go any further in this branch of the tree.
            }

            // Recurse into the tree...
            for (var i = 0; i < 2; i++)
            {
                for (var j = 0; j < 2; j++)
                {
                    // Break out early if we've hit the max amount of results.
                    if (maxCount > 0 && results.Count >= maxCount)
                        return results;

                    var node = _children[i, j];
                    node.RecursiveSearch(criteria, results, maxCount);
                }
            }

            return results;
        }

        protected void Partition(int maxDepth, int startDepth)
        {
            Contract.Requires(maxDepth > 0);
            Contract.Requires(startDepth >= 0);

            var width = Length / 2;
            var height = Width / 2;

            if (startDepth < maxDepth && width > MinNodeLength && height > MinNodeLength)
            {
                _children = new QuadTreeNode[2, 2];

                var min = Bounds.Min;
                var max = Bounds.Max;

                var minX = min.X;
                var minY = min.Y;
                var minZ = min.Z;
                var maxX = max.X;
                var maxY = max.Y;
                var maxZ = max.Z;

                var minXWidth = minX + width;
                var minYHeight = minY + height;

                var cts = CancellationTokenSource;

                _children[South, West] = new QuadTreeNode(new BoundingBox(new Vector3(minX, minY, minZ),
                    new Vector3(minXWidth, minYHeight, maxZ)), cts);

                _children[North, West] = new QuadTreeNode(new BoundingBox(new Vector3(minX, minYHeight, minZ),
                    new Vector3(minXWidth, maxY, maxZ)), cts);

                _children[South, East] = new QuadTreeNode(new BoundingBox(new Vector3(minXWidth, minY, minZ),
                    new Vector3(maxX, minYHeight, maxZ)), cts);

                _children[North, East] = new QuadTreeNode(new BoundingBox(new Vector3(minXWidth, minYHeight, minZ),
                    new Vector3(maxX, maxY, maxZ)), cts);

                startDepth++;

                for (var i = 0; i < 2; i++)
                    for (var j = 0; j < 2; j++)
                        _children[i, j].Partition(maxDepth, startDepth);
            }
            else
                _entities = new ConcurrentDictionary<EntityGuid, IWorldEntity>();
        }

        public bool IsLeaf
        {
            get { return _children == null; }
        }

        public bool IsEmpty
        {
            get { return IsLeaf && _entities.Count == 0; }
        }
    }
}
