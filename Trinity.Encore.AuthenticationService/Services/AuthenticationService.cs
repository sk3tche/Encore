using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.ServiceModel;
using System.Threading;
using Trinity.Core.Configuration;
using Trinity.Encore.AuthenticationService.Realms;
using Trinity.Encore.AuthenticationService.Sessions;
using Trinity.Encore.Game.Realms;
using Trinity.Encore.Services.Authentication;

namespace Trinity.Encore.AuthenticationService.Services
{
    public sealed class AuthenticationService : IAuthenticationService
    {
        [ConfigurationVariable("IpcUri", "net.tcp://127.0.0.1:9502", Static = true)]
        [SuppressMessage("Microsoft.Design", "CA1056", Justification = "This is a configuration variable.")]
        public static string IpcUri { get; set; }

        public AuthenticationData GetAuthenticationData(string accountName)
        {
            SessionInfo session = null;
            SessionManager.Instance.PostWait(mgr => session = mgr.GetSession(accountName)).Wait();
            return session != null ? session.Serialize() : null;
        }

        public bool IsLoggedIn(string accountName)
        {
            var loggedIn = false;
            SessionManager.Instance.PostWait(mgr => loggedIn = mgr.IsActive(accountName)).Wait();
            return loggedIn;
        }

        public void SetActiveState(string accountName, bool loggedIn)
        {
            SessionManager.Instance.PostAsync(mgr => mgr.SetActive(accountName, loggedIn));
        }

        public void RegisterWorldServer(string name, Uri location, RealmFlags flags, RealmCategory category, RealmType type, RealmStatus status,
            int characterCount, int characterCapacity, Version clientVersion)
        {
            var id = OperationContext.Current.SessionId;
            Contract.Assume(!string.IsNullOrEmpty(id));

            RealmManager.Instance.PostAsync(mgr => mgr.AddRealm(new Realm(id)
            {
                Name = name,
                Location = location,
                Flags = flags,
                Category = category,
                Type = type,
                Status = status,
                Population = characterCount,
                Capacity = characterCapacity,
                ClientVersion = clientVersion,
            }));
        }

        public void UpdateWorldServer(string name, Uri location, RealmFlags flags, RealmCategory category, RealmType type, RealmStatus status,
            int characterCount, int characterCapacity, Version clientVersion)
        {
            var id = OperationContext.Current.SessionId;
            Contract.Assume(!string.IsNullOrEmpty(id));

            Realm realm = null;
            RealmManager.Instance.PostWait(mgr => realm = mgr.GetRealm(id)).Wait();

            if (realm == null)
                throw new InvalidOperationException("A realm has not registered before updating itself.");

            RealmManager.Instance.PostAsync(() =>
            {
                realm.Name = name;
                realm.Location = location;
                realm.Flags = flags;
                realm.Category = category;
                realm.Type = type;
                realm.Status = status;
                realm.Population = characterCount;
                realm.Capacity = characterCapacity;
                realm.ClientVersion = clientVersion;
            });
        }

        public void UnregisterWorldServer()
        {
            var id = OperationContext.Current.SessionId;
            Contract.Assume(!string.IsNullOrEmpty(id));

            RealmManager.Instance.PostAsync(mgr => mgr.RemoveRealm(id));
        }
    }
}
