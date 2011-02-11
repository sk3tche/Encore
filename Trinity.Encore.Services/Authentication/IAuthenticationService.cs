using System;
using System.Diagnostics.Contracts;
using System.Net.Security;
using System.ServiceModel;
using Trinity.Encore.Game.Realms;

namespace Trinity.Encore.Services.Authentication
{
    [ContractClass(typeof(AuthenticationServiceContracts))]
    [ServiceContract(ProtectionLevel = ProtectionLevel.None, SessionMode = SessionMode.Required, CallbackContract = typeof(IEmptyCallbackService))]
    public interface IAuthenticationService
    {
        [OperationContract]
        AuthenticationData GetAuthenticationData(string accountName);

        [OperationContract]
        bool IsLoggedIn(string accountName);

        [OperationContract]
        void SetActiveState(string accountName, bool loggedIn);

        [OperationContract]
        void RegisterWorldServer(string name, Uri location, RealmFlags flags, RealmCategory category, RealmType type, RealmStatus status,
            int characterCount, int characterCapacity, Version clientVersion);

        [OperationContract]
        void UpdateWorldServer(string name, Uri location, RealmFlags flags, RealmCategory category, RealmType type, RealmStatus status,
            int characterCount, int characterCapacity, Version clientVersion);

        [OperationContract]
        void UnregisterWorldServer();
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

        public void SetActiveState(string accountName, bool loggedIn)
        {
            Contract.Requires(!string.IsNullOrEmpty(accountName));
        }

        public void RegisterWorldServer(string name, Uri location, RealmFlags flags, RealmCategory category, RealmType type, RealmStatus status,
            int characterCount, int characterCapacity, Version clientVersion)
        {
            Contract.Requires(!string.IsNullOrEmpty(name));
            Contract.Requires(location != null);
            Contract.Requires(characterCount >= 0);
            Contract.Requires(characterCapacity >= 0);
            Contract.Requires(clientVersion != null);
        }

        public void UpdateWorldServer(string name, Uri location, RealmFlags flags, RealmCategory category, RealmType type, RealmStatus status,
            int characterCount, int characterCapacity, Version clientVersion)
        {
            Contract.Requires(!string.IsNullOrEmpty(name));
            Contract.Requires(location != null);
            Contract.Requires(characterCount >= 0);
            Contract.Requires(characterCapacity >= 0);
            Contract.Requires(clientVersion != null);
        }

        public abstract void UnregisterWorldServer();
    }
}
