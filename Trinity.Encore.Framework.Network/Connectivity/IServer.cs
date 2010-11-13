using System;
using System.Diagnostics.Contracts;
using System.Net;

namespace Trinity.Encore.Framework.Network.Connectivity
{
    [ContractClass(typeof(ServerContracts))]
    public interface IServer
    {
        IPEndPoint EndPoint { get; }

        void Start(string address, int port);

        void Stop();

        event EventHandler<ConnectionEventArgs> ClientConnected;

        event EventHandler<ConnectionEventArgs> ClientDisconnected;
    }

    [ContractClassFor(typeof(IServer))]
    public abstract class ServerContracts : IServer
    {
        public abstract IPEndPoint EndPoint { get; }

        public void Start(string address, int port)
        {
            Contract.Requires(!string.IsNullOrEmpty(address));
            Contract.Requires(port >= IPEndPoint.MinPort && port <= IPEndPoint.MaxPort);
        }

        public abstract void Stop();

        public abstract event EventHandler<ConnectionEventArgs> ClientConnected;

        public abstract event EventHandler<ConnectionEventArgs> ClientDisconnected;
    }
}
