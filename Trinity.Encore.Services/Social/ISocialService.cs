using System.Net.Security;
using System.ServiceModel;

namespace Trinity.Encore.Services.Social
{
    [ServiceContract(ProtectionLevel = ProtectionLevel.None, SessionMode = SessionMode.Required)]
    public interface ISocialService
    {
    }
}
