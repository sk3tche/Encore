using System.Diagnostics.CodeAnalysis;
using Trinity.Encore.Framework.Core.Configuration;
using Trinity.Encore.Framework.Core.Services;
using Trinity.Encore.Framework.Game.Threading;
using Trinity.Encore.Framework.Services.Account;
using Trinity.Encore.Framework.Services.Authentication;
using Trinity.Encore.Services.Authentication.Services;

namespace Trinity.Encore.Services.Authentication
{
    public sealed class AuthenticationApplication : ActorApplication<AuthenticationApplication>
    {
        [ConfigurationVariable("ipcUri", "net.tcp://127.0.0.1:9501/Encore.AccountService", Static = true)]
        [SuppressMessage("Microsoft.Design", "CA1056", Justification = "This is a configuration variable.")]
        public static string AccountIpcUri { get; set; }

        private AuthenticationApplication()
        {
        }

        private ServiceClient<IAccountService> _accountClient;

        private ServiceHost<IAuthenticationService> _authenticationHost;

        protected override void OnStart(string[] args)
        {
            _accountClient = new ServiceClient<IAccountService>(AccountIpcUri);
            _authenticationHost = new ServiceHost<IAuthenticationService>(new AuthenticationService(), AuthenticationService.IpcUri);
        }

        protected override void OnStop()
        {
            _authenticationHost.Close();
            _accountClient.Close();
        }
    }
}
