using System;
using Microsoft.Xna.Framework;
using Trinity.Encore.Framework.Game.Partitioning;

namespace Trinity.Encore.Framework.Game.Entities
{
    [CLSCompliant(false)]
    public interface IWorldEntity : IEntity
    {
        Vector3 Position { get; }

        QuadTreeNode Node { get; set; }
    }
}
