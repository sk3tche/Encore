using Trinity.Core.Threading.Actors;

namespace Trinity.Encore.Game.Entities
{
    public interface IEntity : IActor
    {
        EntityGuid Guid { get; }
    }
}
