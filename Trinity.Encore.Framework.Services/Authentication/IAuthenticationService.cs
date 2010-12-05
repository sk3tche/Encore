using System.Net.Security;
using System.ServiceModel;

namespace Trinity.Encore.Framework.Services.Authentication
{
    [ServiceContract(ProtectionLevel = ProtectionLevel.None, SessionMode = SessionMode.Required)]
    public interface IAuthenticationService
    {
        AuthenticationData GetAuthenticationData(string accountName);

        bool IsLoggedIn(string accountName);

        void SetLoggedIn(string accountName, bool loggedIn);
    }
}
