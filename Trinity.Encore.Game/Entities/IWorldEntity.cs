using Mono.GameMath;
using Trinity.Encore.Game.Partitioning;

namespace Trinity.Encore.Game.Entities
{
    public interface IWorldEntity : IEntity
    {
        Vector3 Position { get; }

        QuadTreeNode Node { get; set; }
    }
}
