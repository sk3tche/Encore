using System.Diagnostics.CodeAnalysis;
using Trinity.Core.Configuration;
using Trinity.Core.Services;
using Trinity.Encore.Game.Services;
using Trinity.Encore.Game.Threading;
using Trinity.Encore.Services;
using Trinity.Encore.Services.Account;
using Trinity.Encore.Services.Authentication;

namespace Trinity.Encore.AuthenticationService
{
    public sealed class AuthenticationApplication : ActorApplication<AuthenticationApplication>
    {
        [ConfigurationVariable("AccountIpcUri", "net.tcp://127.0.0.1:9501", Static = true)]
        [SuppressMessage("Microsoft.Design", "CA1056", Justification = "This is a configuration variable.")]
        public static string AccountIpcUri { get; set; }

        private AuthenticationApplication()
        {
        }

        public IpcDevice<IAccountService, EmptyCallbackService> AccountService { get; private set; }

        private ServiceHost<IAuthenticationService, Services.AuthenticationService> _authenticationHost;

        protected override void OnStart(string[] args)
        {
            AccountService = new IpcDevice<IAccountService, EmptyCallbackService>(() =>
                new DuplexServiceClient<IAccountService, EmptyCallbackService>(new EmptyCallbackService(), AccountIpcUri));

            _authenticationHost = new ServiceHost<IAuthenticationService, Services.AuthenticationService>(new Services.AuthenticationService(),
                Services.AuthenticationService.IpcUri);
        }

        protected override void OnStop()
        {
            _authenticationHost.Close();
        }
    }
}
