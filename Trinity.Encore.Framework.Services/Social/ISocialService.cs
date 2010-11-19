using System.Net.Security;
using System.ServiceModel;
using Trinity.Encore.Framework.Game.Services;

namespace Trinity.Encore.Framework.Services.Social
{
    [ServiceContract(ProtectionLevel = ProtectionLevel.None, SessionMode = SessionMode.Required)]
    public interface ISocialService : IAuthenticatableService
    {
    }
}
