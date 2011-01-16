using System.Diagnostics.CodeAnalysis;
using Trinity.Core.Configuration;
using Trinity.Core.Services;
using Trinity.Encore.Game.Threading;
using Trinity.Encore.Services.Account;
using Trinity.Encore.Services.Authentication;

namespace Trinity.Encore.AuthenticationService
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
            _authenticationHost = new ServiceHost<IAuthenticationService>(new Services.AuthenticationService(), Services.AuthenticationService.IpcUri);
        }

        protected override void OnStop()
        {
            _authenticationHost.Close();
            _accountClient.Close();
        }
    }
}
