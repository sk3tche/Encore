using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using Trinity.Encore.Framework.Core.Exceptions;
using Trinity.Encore.Framework.Core.Logging;
using Trinity.Encore.Framework.Network.Handling;
using Trinity.Encore.Framework.Network.Security;

namespace Trinity.Encore.Framework.Network.Connectivity.Sockets
{
    public sealed class TcpServer : IServer
    {
        private static readonly LogProxy _log = new LogProxy("TcpServer");

        private readonly List<TcpClient> _clients = new List<TcpClient>();

        private readonly Socket _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        private readonly IPacketPropagator _propagator;

        [ContractInvariantMethod]
        private void Invariant()
        {
            Contract.Invariant(_clients != null);
            Contract.Invariant(_socket != null);
            Contract.Invariant(_propagator != null);
            Contract.Invariant(MaximumPendingConnections > 0);
        }

        /// <summary>
        /// Maximum backlog for pending connections.
        /// </summary>
        public int MaximumPendingConnections { get; private set; }

        /// <summary>
        /// Indicates whether or not multiple connections from the same
        /// address are allowed.
        /// </summary>
        public bool AllowMultipleConnections { get; set; }

        /// <summary>
        /// Whether or not to use the Nagle algorithm.
        /// </summary>
        public bool NoDelayAlgorithm { get; private set; }

        public TcpServer(IPacketPropagator propagator, int backlog, bool multipleConnections,
            bool nagleAlgo)
        {
            Contract.Requires(propagator != null);
            Contract.Requires(backlog > 0);

            _propagator = propagator;
            MaximumPendingConnections = backlog;
            AllowMultipleConnections = multipleConnections;
            NoDelayAlgorithm = nagleAlgo;

            // Start accepting incoming connections.
            Accept(null);
        }

        public IPEndPoint EndPoint
        {
            get { return _socket.LocalEndPoint.ToIPEndPoint(); }
        }

        public void Start(string address, int port)
        {
            var ip = IPAddress.Parse(address);
            Contract.Assume(ip != null);
            var ep = new IPEndPoint(ip, port);

            _socket.NoDelay = NoDelayAlgorithm;
            _socket.Bind(ep);
            _socket.Listen(MaximumPendingConnections);

            _log.Info("Socket bound to {0}.", ep);
        }

        public void Stop()
        {
            foreach (var client in _clients)
                client.Disconnect();

            _clients.Clear();

            _log.Info("Stopped listening at {0}.", EndPoint);

            _socket.Dispose();
        }

        private void AddClient(TcpClient client)
        {
            Contract.Requires(client != null);

            lock (_clients)
                _clients.Add(client);

            var cc = ClientConnected;
            if (cc != null)
                cc(this, new ConnectionEventArgs(client));
        }

        internal void RemoveClient(TcpClient client)
        {
            Contract.Requires(client != null);

            lock (_clients)
                _clients.Remove(client);

            var cd = ClientDisconnected;
            if (cd != null)
                cd(this, new ConnectionEventArgs(client));
        }

        public event EventHandler<ConnectionEventArgs> ClientConnected;

        public event EventHandler<ConnectionEventArgs> ClientDisconnected;

        /// <summary>
        /// Starts accepting incoming clients.
        /// </summary>
        /// <param name="args">The async event args to use, if any (passing null
        /// will create a new object).</param>
        private void Accept(SocketAsyncEventArgs args)
        {
            if (args == null)
            {
                args = SocketAsyncEventArgsPool.Acquire();
                args.Completed += OnAccept;
            }
            else
                args.AcceptSocket = null;

            try
            {
                if (!_socket.AcceptAsync(args))
                    OnAccept(this, args);
            }
            catch (Exception ex)
            {
                args.Completed -= OnAccept;
                SocketAsyncEventArgsPool.Release(args);

                if (ex is ObjectDisposedException)
                    return;

                if (ex is SocketException)
                {
                    ExceptionManager.RegisterException(ex);
                    return;
                }

                throw;
            }
        }

        /// <summary>
        /// Called when a connection is accepted.
        /// </summary>
        private void OnAccept(object sender, SocketAsyncEventArgs args)
        {
            var sock = args.AcceptSocket;
            if (!sock.Connected)
                return;

            // Wrap stuff in a try block, in case the socket is disposed of.
            try
            {
                var accept = true;

                // Do we accept multiple connections from the same address?
                if (!AllowMultipleConnections)
                    lock (_clients)
                        accept = _clients.All(cl => !cl.EndPoint.Equals(sock.RemoteEndPoint.ToIPEndPoint()));

                if (!accept)
                {
                    _log.Warn("Disconnecting client from {0}; already connected.", sock.RemoteEndPoint);

                    sock.Shutdown(SocketShutdown.Both);
                    sock.Dispose();
                }
                else
                {
                    // Add the client and thus start receiving.
                    var client = new TcpClient(sock, this, _propagator);
                    client.AddPermission(new ConnectedPermission());
                    AddClient(client);
                }
            }
            catch (Exception ex)
            {
                if (ex is SocketException)
                    ExceptionManager.RegisterException(ex);

                if (!(ex is ObjectDisposedException))
                    throw;

                throw;
            }

            // Continue accepting with the event args we were using before.
            Accept(args);
        }

        public override string ToString()
        {
            return EndPoint.ToString();
        }
    }
}
