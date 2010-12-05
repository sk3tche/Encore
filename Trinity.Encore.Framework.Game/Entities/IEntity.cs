using Trinity.Encore.Framework.Core.Threading.Actors;

namespace Trinity.Encore.Framework.Game.Entities
{
    public interface IEntity : IActor
    {
        EntityGuid Guid { get; }
    }
}
