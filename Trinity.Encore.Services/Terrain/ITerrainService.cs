using System.Net.Security;
using System.ServiceModel;

namespace Trinity.Encore.Services.Terrain
{
    [ServiceContract(ProtectionLevel = ProtectionLevel.None, SessionMode = SessionMode.Required, CallbackContract = typeof(IEmptyCallbackService))]
    public interface ITerrainService
    {
    }
}
