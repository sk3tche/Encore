using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Mono.GameMath;
using Trinity.Core.Collections;
using Trinity.Core.Threading.Actors;
using Trinity.Encore.Game.Entities;

namespace Trinity.Encore.Game.Partitioning
{
    public class QuadTreeNode : Actor<QuadTreeNode>
    {
        public const float MinNodeLength = 333.0f;

        private const int West = 0;

        private const int East = 1;

        private const int North = 0;

        private const int South = 1;

        public QuadTreeNode(BoundingBox bounds)
        {
            Bounds = bounds;
        }

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
        private Dictionary<EntityGuid, IWorldEntity> _entities;

        public bool AddEntity(IWorldEntity entity)
        {
            Contract.Requires(entity != null);

            if (IsLeaf)
            {
                Contract.Assume(_entities != null);
                _entities.Add(entity.Guid, entity);
                entity.PostAsync(() => entity.Node = this);

                return true;
            }

            var pos = entity.Position;
            for (var i = 0; i < 2; i++)
            {
                for (var j = 0; j < 2; j++)
                {
                    var node = _children[i, j];
                    if (node.Bounds.Contains(pos) != ContainmentType.Contains)
                        continue;

                    return node.AddEntity(entity);
                }
            }

            return false;
        }

        public bool RemoveEntity(IWorldEntity entity)
        {
            Contract.Requires(entity != null);

            if (IsLeaf)
            {
                Contract.Assume(_entities != null);
                _entities.Remove(entity.Guid);
                entity.PostAsync(() => entity.Node = null);

                return true;
            }

            var pos = entity.Position;
            for (var i = 0; i < 2; i++)
            {
                for (var j = 0; j < 2; j++)
                {
                    var node = _children[i, j];
                    if (node.Bounds.Contains(pos) != ContainmentType.Contains)
                        continue;

                    return node.RemoveEntity(entity);
                }
            }

            return false;
        }

        public IEnumerable<IWorldEntity> FindEntities(Func<IWorldEntity, bool> criteria, BoundingBox searchArea,
            int maxCount = QuadTree.NoMaxCount)
        {
            Contract.Requires(criteria != null);
            Contract.Requires(maxCount >= QuadTree.NoMaxCount);
            Contract.Ensures(Contract.Result<IEnumerable<IWorldEntity>>() != null);

            return RecursiveSearch(criteria, null, maxCount, x => searchArea.Contains(x.Bounds) == ContainmentType.Contains);
        }

        public IEnumerable<IWorldEntity> FindEntities(Func<IWorldEntity, bool> criteria, BoundingSphere searchArea,
            int maxCount = QuadTree.NoMaxCount)
        {
            Contract.Requires(criteria != null);
            Contract.Requires(maxCount >= QuadTree.NoMaxCount);
            Contract.Ensures(Contract.Result<IEnumerable<IWorldEntity>>() != null);

            return RecursiveSearch(criteria, null, maxCount, x => searchArea.Contains(x.Bounds) == ContainmentType.Contains);
        }

        public IEnumerable<IWorldEntity> FindEntities(Func<IWorldEntity, bool> criteria, int maxCount = QuadTree.NoMaxCount)
        {
            Contract.Requires(criteria != null);
            Contract.Requires(maxCount >= QuadTree.NoMaxCount);
            Contract.Ensures(Contract.Result<IEnumerable<IWorldEntity>>() != null);

            return RecursiveSearch(criteria, null, maxCount);
        }

        public IWorldEntity FindEntity(Func<IWorldEntity, bool> criteria, BoundingBox searchArea)
        {
            Contract.Requires(criteria != null);

            return RecursiveSearch(criteria, null, 1, x => searchArea.Contains(x.Bounds) ==
                ContainmentType.Contains).SingleOrDefault();
        }

        public IWorldEntity FindEntity(Func<IWorldEntity, bool> criteria, BoundingSphere searchArea)
        {
            Contract.Requires(criteria != null);

            return RecursiveSearch(criteria, null, 1, x => searchArea.Contains(x.Bounds) ==
                ContainmentType.Contains).SingleOrDefault();
        }

        public IWorldEntity FindEntity(Func<IWorldEntity, bool> criteria)
        {
            Contract.Requires(criteria != null);

            return FindEntities(criteria, 1).SingleOrDefault();
        }

        private IEnumerable<IWorldEntity> RecursiveSearch(Func<IWorldEntity, bool> criteria, ICollection<IWorldEntity> results,
            int maxCount, Func<QuadTreeNode, bool> nodeInclusionFunc = null)
        {
            Contract.Requires(criteria != null);
            Contract.Requires(maxCount >= QuadTree.NoMaxCount);
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
                    if (maxCount != QuadTree.NoMaxCount && results.Count >= maxCount)
                        return results;

                    var node = _children[i, j];
                    if (nodeInclusionFunc != null && !nodeInclusionFunc(node))
                        continue;

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

                _children[South, West] = new QuadTreeNode(new BoundingBox(new Vector3(minX, minY, minZ),
                    new Vector3(minXWidth, minYHeight, maxZ)));

                _children[North, West] = new QuadTreeNode(new BoundingBox(new Vector3(minX, minYHeight, minZ),
                    new Vector3(minXWidth, maxY, maxZ)));

                _children[South, East] = new QuadTreeNode(new BoundingBox(new Vector3(minXWidth, minY, minZ),
                    new Vector3(maxX, minYHeight, maxZ)));

                _children[North, East] = new QuadTreeNode(new BoundingBox(new Vector3(minXWidth, minYHeight, minZ),
                    new Vector3(maxX, maxY, maxZ)));

                startDepth++;

                for (var i = 0; i < 2; i++)
                    for (var j = 0; j < 2; j++)
                        _children[i, j].Partition(maxDepth, startDepth);
            }
            else
                _entities = new Dictionary<EntityGuid, IWorldEntity>();
        }

        public bool IsLeaf
        {
            get { return _children == null; }
        }

        public bool IsEmpty
        {
            get
            {
                if (!IsLeaf)
                    throw new InvalidOperationException("This node is not a leaf.");

                Contract.Assume(_entities != null);
                return !_entities.Any();
            }
        }
    }
}
