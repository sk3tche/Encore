using System.Diagnostics.Contracts;
using System.Threading;
using Microsoft.Xna.Framework;

namespace Trinity.Encore.Framework.Game.Partitioning
{
    /// <summary>
    /// Represents a quad tree data structure. This class is fully parallelized.
    /// </summary>
    public sealed class QuadTree : QuadTreeNode
    {
        /// <summary>
        /// The default depth threshold for partitioning.
        /// </summary>
        public const int DefaultDepthThreshold = 6;

        public QuadTree(BoundingBox bounds, int depthThreshold = DefaultDepthThreshold)
            : base(bounds, new CancellationTokenSource())
        {
            Contract.Requires(depthThreshold > 0);

            Partition(depthThreshold, 0);
        }
    }
}
