using System.Net.Security;
using System.ServiceModel;

namespace Trinity.Encore.Services.Social
{
    [ServiceContract(ProtectionLevel = ProtectionLevel.None, SessionMode = SessionMode.Required, CallbackContract = typeof(IEmptyCallbackService))]
    public interface ISocialService
    {
    }
}
