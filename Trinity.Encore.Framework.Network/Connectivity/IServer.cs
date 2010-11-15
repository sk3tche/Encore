using System;
using System.Diagnostics.Contracts;
using System.Net;

namespace Trinity.Encore.Framework.Network.Connectivity
{
    [ContractClass(typeof(ServerContracts))]
    public interface IServer
    {
        EndPoint EndPoint { get; }

        void Start(string address, int port);

        void Start(EndPoint ep);

        void Stop();

        event EventHandler<ConnectionEventArgs> ClientConnected;

        event EventHandler<ConnectionEventArgs> ClientDisconnected;
    }

    [ContractClassFor(typeof(IServer))]
    public abstract class ServerContracts : IServer
    {
        public abstract EndPoint EndPoint { get; }

        public void Start(string address, int port)
        {
            Contract.Requires(!string.IsNullOrEmpty(address), "An IP address must be specified.");
            Contract.Requires(port >= IPEndPoint.MinPort && port <= IPEndPoint.MaxPort, "An invalid port number was specified.");
        }

        public void Start(EndPoint ep)
        {
            Contract.Requires(ep != null);
        }

        public abstract void Stop();

        public abstract event EventHandler<ConnectionEventArgs> ClientConnected;

        public abstract event EventHandler<ConnectionEventArgs> ClientDisconnected;
    }
}
