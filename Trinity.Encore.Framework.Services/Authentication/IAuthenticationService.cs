using System.Net.Security;
using System.ServiceModel;

namespace Trinity.Encore.Framework.Services.Authentication
{
    [ServiceContract(ProtectionLevel = ProtectionLevel.None, SessionMode = SessionMode.Required)]
    public interface IAuthenticationService
    {
        [OperationContract(IsInitiating = false)]
        AuthenticationData GetAuthenticationData(string accountName);

        [OperationContract(IsInitiating = false)]
        bool IsLoggedIn(string accountName);

        [OperationContract(IsInitiating = false)]
        void SetLoggedIn(string accountName, bool loggedIn);
    }
}
