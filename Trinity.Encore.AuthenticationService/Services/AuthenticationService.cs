using System;
using System.Diagnostics.CodeAnalysis;
using Trinity.Core.Configuration;
using Trinity.Encore.Services.Authentication;

namespace Trinity.Encore.AuthenticationService.Services
{
    public sealed class AuthenticationService : IAuthenticationService
    {
        [ConfigurationVariable("ipcUri", "net.tcp://127.0.0.1:9501/Encore.AuthenticationService", Static = true)]
        [SuppressMessage("Microsoft.Design", "CA1056", Justification = "This is a configuration variable.")]
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
