using System;
using Mono.GameMath;
using Trinity.Encore.Game.Partitioning;

namespace Trinity.Encore.Game.Entities
{
    public abstract class WorldEntity : Entity, IWorldEntity
    {
        public Vector3 Position
        {
            get { throw new NotImplementedException(); }
        }

        public QuadTreeNode Node
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }
    }
}
