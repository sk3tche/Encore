using System.Net.Security;
using System.ServiceModel;
using Trinity.Encore.Framework.Game.Services;

namespace Trinity.Encore.Framework.Services.Terrain
{
    [ServiceContract(ProtectionLevel = ProtectionLevel.None, SessionMode = SessionMode.Required)]
    public interface ITerrainService : IAuthenticatableService
    {
    }
}
