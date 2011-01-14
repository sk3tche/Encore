using System;
using Trinity.Encore.Framework.Core.Configuration;
using Trinity.Encore.Framework.Services.Authentication;

namespace Trinity.Encore.Services.Authentication.Services
{
    public sealed class AuthenticationService : IAuthenticationService
    {
        [ConfigurationVariable("ipcUri", "net.tcp://127.0.0.1:9501/Encore.AuthenticationService", Static = true)]
        public static string IpcUri { get; set; }

        public AuthenticationData GetAuthenticationData(string accountName)
        {
            // TODO: Implement.
            throw new NotImplementedException();
        }

        public bool IsLoggedIn(string accountName)
        {
            // TODO: Implement.
            throw new NotImplementedException();
        }

        public void SetLoggedIn(string accountName, bool loggedIn)
        {
            // TODO: Implement.
            throw new NotImplementedException();
        }
    }
}
