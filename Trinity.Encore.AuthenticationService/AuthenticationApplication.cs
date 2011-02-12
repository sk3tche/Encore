using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Net;
using Trinity.Core.Configuration;
using Trinity.Core.Services;
using Trinity.Encore.Game.Network;
using Trinity.Encore.Game.Network.Handling;
using Trinity.Encore.Game.Services;
using Trinity.Encore.Game.Threading;
using Trinity.Encore.Services;
using Trinity.Encore.Services.Account;
using Trinity.Encore.Services.Authentication;
using Trinity.Network.Connectivity;
using Trinity.Network.Connectivity.Sockets;

namespace Trinity.Encore.AuthenticationService
{
    public sealed class AuthenticationApplication : NetworkApplication<AuthenticationApplication>
    {
        [ConfigurationVariable("AccountIpcUri", "net.tcp://127.0.0.1:9501", Static = true)]
        [SuppressMessage("Microsoft.Design", "CA1056", Justification = "This is a configuration variable.")]
        public static string AccountIpcUri { get; set; }

        [ConfigurationVariable("ListenHost", "127.0.0.1", Static = true)]
        public static string ListenHost { get; set; }

        [ConfigurationVariable("ListenPort", "3724", Static = true)]
        public static int ListenPort { get; set; }

        private AuthenticationApplication()
        {
        }

        public IpcDevice<IAccountService, EmptyCallbackService> AccountService { get; private set; }

        private ServiceHost<IAuthenticationService, Services.AuthenticationService> _authenticationHost;

        protected override void OnStart(string[] args)
        {
            AccountService = new IpcDevice<IAccountService, EmptyCallbackService>(() =>
                new DuplexServiceClient<IAccountService, EmptyCallbackService>(new EmptyCallbackService(), AccountIpcUri));

            Contract.Assume(!string.IsNullOrEmpty(Services.AuthenticationService.IpcUri));
            _authenticationHost = new ServiceHost<IAuthenticationService, Services.AuthenticationService>(new Services.AuthenticationService(),
                Services.AuthenticationService.IpcUri);
        }

        protected override void OnStop()
        {
            _authenticationHost.Close();
        }

        protected override IServer CreateServer()
        {
            return new TcpServer(new AuthPacketPropagator());
        }

        public override IPEndPoint EndPoint
        {
            get { return new IPEndPoint(IPAddress.Parse(ListenHost), ListenPort); }
        }
    }
}
