using System.Net.Security;
using System.ServiceModel;

namespace Trinity.Encore.Services.Map
{
    [ServiceContract(ProtectionLevel = ProtectionLevel.None, SessionMode = SessionMode.Required, CallbackContract = typeof(IEmptyCallbackService))]
    public interface IMapService
    {
    }
}
