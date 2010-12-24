using System;
using System.Diagnostics.Contracts;
using System.Net.Security;
using System.ServiceModel;

namespace Trinity.Encore.Framework.Services.Authentication
{
    [ContractClass(typeof(AuthenticationServiceContracts))]
    [ServiceContract(ProtectionLevel = ProtectionLevel.None, SessionMode = SessionMode.Required)]
    public interface IAuthenticationService
    {
        [OperationContract]
        AuthenticationData GetAuthenticationData(string accountName);

        [OperationContract]
        bool IsLoggedIn(string accountName);

        [OperationContract]
        void SetLoggedIn(string accountName, bool loggedIn);
    }

    [ContractClassFor(typeof(IAuthenticationService))]
    public abstract class AuthenticationServiceContracts : IAuthenticationService
    {
        public AuthenticationData GetAuthenticationData(string accountName)
        {
            Contract.Requires(!string.IsNullOrEmpty(accountName));

            return null;
        }

        public bool IsLoggedIn(string accountName)
        {
            Contract.Requires(!string.IsNullOrEmpty(accountName));

            return false;
        }

        public void SetLoggedIn(string accountName, bool loggedIn)
        {
            Contract.Requires(!string.IsNullOrEmpty(accountName));
        }
    }
}
