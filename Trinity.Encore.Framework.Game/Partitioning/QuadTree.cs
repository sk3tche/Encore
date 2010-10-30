using Microsoft.Xna.Framework;
using Trinity.Encore.Framework.Game.Entities;

namespace Trinity.Encore.Framework.Game.Partitioning
{
    public sealed class QuadTree : QuadTreeNode
    {
        public const int DepthThreshold = 6;

        public QuadTree(BoundingBox bounds)
            : base(bounds)
        {
            Partition(null, DepthThreshold, 0);
        }
    }
}
