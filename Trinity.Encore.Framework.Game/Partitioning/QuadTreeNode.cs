using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Microsoft.Xna.Framework;
using Trinity.Encore.Framework.Core.Collections;
using Trinity.Encore.Framework.Game.Entities;

namespace Trinity.Encore.Framework.Game.Partitioning
{
    public class QuadTreeNode
    {
        public const float MinNodeLength = 250.0f;

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

        private QuadTreeNode[,] _children;

        private ConcurrentDictionary<EntityGuid, IWorldEntity> _entities;

        public bool AddEntity(IWorldEntity entity)
        {
            Contract.Requires(entity != null);

            if (IsLeaf)
            {
                Contract.Assume(_entities != null);
                _entities.Add(entity.Guid, entity);
                entity.Node = this;
                return true;
            }

            var pos = entity.Position;
            for (var i = 0; i < 2; i++)
            {
                for (var j = 0; j < 2; j++)
                {
                    var node = _children[i, j];
                    if (node.Bounds.Contains(pos) == ContainmentType.Contains)
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
                return _entities.Remove(entity.Guid) == entity;
            }

            var pos = entity.Position;
            for (var i = 0; i < 2; i++)
            {
                for (var j = 0; j < 2; j++)
                {
                    var node = _children[i, j];
                    if (node.Bounds.Contains(pos) == ContainmentType.Contains)
                        return node.RemoveEntity(entity);
                }
            }

            return false;
        }

        public void Partition(int maxDepth, int startDepth)
        {
            Contract.Requires(maxDepth > 0);
            Contract.Requires(startDepth >= 0);

            var width = Length / 2;
            var height = Width / 2;

            if (startDepth < maxDepth && width > MinNodeLength && height > MinNodeLength)
            {
                _children = new QuadTree[2, 2];

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
