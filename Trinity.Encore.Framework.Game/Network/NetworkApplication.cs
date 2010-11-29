using System;
using System.Diagnostics.Contracts;
using System.Net;
using Trinity.Encore.Framework.Core.Threading.Actors;
using Trinity.Encore.Framework.Game.Threading;
using Trinity.Encore.Framework.Network.Connectivity;

namespace Trinity.Encore.Framework.Game.Network
{
    [ContractClass(typeof(NetworkApplicationContracts<>))]
    public abstract class NetworkApplication<T> : ActorApplication<T>
        where T : NetworkApplication<T>
    {
        public IServer Server { get; private set; }

        [ContractInvariantMethod]
        private void Invariant()
        {
            Contract.Invariant(Server != null);
        }

        protected NetworkApplication()
        {
            Server = CreateServer();
            Server.ClientConnected += OnClientConnected;
            Server.ClientDisconnected += OnClientDisconnected;
        }

        protected override void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                Server.ClientConnected -= OnClientConnected;
                Server.ClientDisconnected -= OnClientDisconnected;
            }

            base.Dispose(disposing);
        }

        protected virtual void OnClientConnected(object sender, ConnectionEventArgs args)
        {
            Contract.Requires(sender != null);
            Contract.Requires(args != null);
        }

        protected virtual void OnClientDisconnected(object sender, ConnectionEventArgs args)
        {
            Contract.Requires(sender != null);
            Contract.Requires(args != null);
        }

        protected override void OnStart(string[] args)
        {
            Server.Start(EndPoint);
        }

        protected override void OnStop()
        {
            Server.Stop();
        }

        protected abstract IServer CreateServer();

        public abstract IPEndPoint EndPoint { get; }
    }

    [ContractClassFor(typeof(NetworkApplication<>))]
    public abstract class NetworkApplicationContracts<T> : NetworkApplication<T>
        where T : NetworkApplicationContracts<T>
    {
        protected override IServer CreateServer()
        {
            Contract.Ensures(Contract.Result<IServer>() != null);

            return null;
        }

        public override IPEndPoint EndPoint
        {
            get
            {
                Contract.Ensures(Contract.Result<IPEndPoint>() != null);

                return null;
            }
        }
    }
}
