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

        public QuadTreeNode(BoundingBox bounds, CancellationToken ct)
            : base(ct)
        {
            Bounds = bounds;

            var cpus = Environment.ProcessorCount;
            Contract.Assume(cpus > 0);
            var options = GetOptions(ct, cpus);

            AddEntityChannel = new ActionBlock<Tuple<IWorldEntity, Action<bool>>>(new Action<Tuple<IWorldEntity, Action<bool>>>(AddEntity), options);
            RemoveEntityChannel = new ActionBlock<Tuple<IWorldEntity, Action<bool>>>(new Action<Tuple<IWorldEntity, Action<bool>>>(RemoveEntity), options);
            FindEntityChannel = new ActionBlock<Tuple<Func<IWorldEntity, bool>, Action<IWorldEntity>>>(new Action<Tuple<Func<IWorldEntity, bool>, Action<IWorldEntity>>>(FindEntity), options);
            FindEntitiesChannel = new ActionBlock<Tuple<Func<IWorldEntity, bool>, Action<IEnumerable<IWorldEntity>>>>(new Action<Tuple<Func<IWorldEntity, bool>, Action<IEnumerable<IWorldEntity>>>>(FindEntities), options);
        }

        /// <summary>
        /// Sends a message instructing the QuadTreeNode to add an IWorldEntity.
        /// </summary>
        public ITargetBlock<Tuple<IWorldEntity, Action<bool>>> AddEntityChannel { get; private set; }

        /// <summary>
        /// Sends a message instructing the QuadTreeNode to remove an IWorldEntity.
        /// </summary>
        public ITargetBlock<Tuple<IWorldEntity, Action<bool>>> RemoveEntityChannel { get; private set; }

        /// <summary>
        /// Sends a message instructing the QuadTreeNode to find a specific IWorldEntity and call back once done.
        /// 
        /// The returned IWorldEntity may be null (if it was not found).
        /// </summary>
        public ITargetBlock<Tuple<Func<IWorldEntity, bool>, Action<IWorldEntity>>> FindEntityChannel { get; private set; }

        /// <summary>
        /// Sends a message instructing the QuadTreeNode to find a list of IWorldEntity instances and call back once done.
        /// 
        /// The returned sequence may be empty (but not null) if no results were found.
        /// </summary>
        public ITargetBlock<Tuple<Func<IWorldEntity, bool>, Action<IEnumerable<IWorldEntity>>>> FindEntitiesChannel { get; private set; }

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

        private void AddEntity(Tuple<IWorldEntity, Action<bool>> tuple)
        {
            Contract.Requires(tuple != null);
            Contract.Requires(tuple.Item1 != null);

            var entity = tuple.Item1;
            var callback = tuple.Item2;

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

                    Contract.Assume(tuple.Item1 != null);
                    node.AddEntity(tuple);
                }
            }

            if (callback != null)
                callback(false);
        }

        private void RemoveEntity(Tuple<IWorldEntity, Action<bool>> tuple)
        {
            Contract.Requires(tuple != null);
            Contract.Requires(tuple.Item1 != null);

            var entity = tuple.Item1;
            var callback = tuple.Item2;

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

                    Contract.Assume(tuple.Item1 != null);
                    node.RemoveEntity(tuple);
                }
            }

            if (callback != null)
                callback(false);
        }

        private void FindEntities(Tuple<Func<IWorldEntity, bool>, Action<IEnumerable<IWorldEntity>>> tuple)
        {
            Contract.Requires(tuple != null);
            Contract.Requires(tuple.Item1 != null);
            Contract.Requires(tuple.Item2 != null);

            var results = RecursiveSearch(tuple.Item1, null);
            tuple.Item2(results); // Call back with the found entities.
        }

        private void FindEntity(Tuple<Func<IWorldEntity, bool>, Action<IWorldEntity>> tuple)
        {
            Contract.Requires(tuple != null);
            Contract.Requires(tuple.Item1 != null);
            Contract.Requires(tuple.Item2 != null);

            // We really just do the same as for multi-entity searches, but return a single (or no) result.
            var result = RecursiveSearch(tuple.Item1, null).FirstOrDefault();
            tuple.Item2(result); // Call back with the found entity.
        }

        private IEnumerable<IWorldEntity> RecursiveSearch(Func<IWorldEntity, bool> criteria, ICollection<IWorldEntity> results)
        {
            Contract.Requires(criteria != null);
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
                    var node = _children[i, j];
                    node.RecursiveSearch(criteria, results);
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

                var ct = CancellationToken;

                _children[South, West] = new QuadTreeNode(new BoundingBox(new Vector3(minX, minY, minZ),
                    new Vector3(minXWidth, minYHeight, maxZ)), ct);

                _children[North, West] = new QuadTreeNode(new BoundingBox(new Vector3(minX, minYHeight, minZ),
                    new Vector3(minXWidth, maxY, maxZ)), ct);

                _children[South, East] = new QuadTreeNode(new BoundingBox(new Vector3(minXWidth, minY, minZ),
                    new Vector3(maxX, minYHeight, maxZ)), ct);

                _children[North, East] = new QuadTreeNode(new BoundingBox(new Vector3(minXWidth, minYHeight, minZ),
                    new Vector3(maxX, maxY, maxZ)), ct);

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
