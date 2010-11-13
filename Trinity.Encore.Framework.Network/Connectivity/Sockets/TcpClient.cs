using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Net;
using System.Net.Sockets;
using Trinity.Encore.Framework.Core.Dynamic;
using Trinity.Encore.Framework.Core.Exceptions;
using Trinity.Encore.Framework.Core.Logging;
using Trinity.Encore.Framework.Core.Security;
using Trinity.Encore.Framework.Core.Threading.Actors;
using Trinity.Encore.Framework.Network.Encryption;
using Trinity.Encore.Framework.Network.Handling;
using Trinity.Encore.Framework.Network.Transmission;

namespace Trinity.Encore.Framework.Network.Connectivity.Sockets
{
    public sealed class TcpClient : Actor, IClient
    {
        private static readonly LogProxy _log = new LogProxy("TcpClient");

        public const int InitialReceiveBufferSize = ushort.MaxValue;

        public const int MaxReceiveBufferSize = 0xFFFFFF; // 2 ^ 24 - 1 (24 bits, 3 bytes).

        private readonly Socket _socket;

        private readonly SocketAsyncEventArgs _eventArgs;

        /// <summary>
        /// Stores incoming packet headers. This buffer is fixed-size.
        /// </summary>
        private readonly byte[] _headerBuffer;

        /// <summary>
        /// The buffer in which incoming data is stored. This will be reallocated as necessary, should a larger
        /// size be required.
        /// </summary>
        private byte[] _receiveBuffer;

        private int _receiveOpCode;

        private int _receiveLength;

        private readonly IPacketPropagator _propagator;

        [ContractInvariantMethod]
        private void Invariant()
        {
            Contract.Invariant(_socket != null);
            Contract.Invariant(_eventArgs != null);
            Contract.Invariant(_headerBuffer != null);
            Contract.Invariant(_headerBuffer.Length == _propagator.HeaderLength);
            Contract.Invariant(_receiveBuffer != null);
            Contract.Invariant(_receiveBuffer.Length >= InitialReceiveBufferSize);
            Contract.Invariant(_receiveOpCode >= 0);
            Contract.Invariant(_receiveLength >= 0);
            Contract.Invariant(_receiveLength < _receiveBuffer.Length);
            Contract.Invariant(_propagator != null);
            Contract.Invariant(Server != null);
            Contract.Invariant((object)UserData != null);
        }

        internal TcpClient(Socket socket, TcpServer server, IPacketPropagator propagator)
        {
            Contract.Requires(socket != null);
            Contract.Requires(server != null);
            Contract.Requires(propagator != null);
            Contract.Ensures(Server == server);

            _socket = socket;
            _eventArgs = SocketAsyncEventArgsPool.Acquire();
            _headerBuffer = new byte[propagator.HeaderLength];
            _receiveBuffer = new byte[InitialReceiveBufferSize]; // Start out with a 2 ^ 16 - 1 buffer.
            _propagator = propagator;
            Server = server;
            UserData = new Bag();

            // Start receiving packets...
            Receive();

            Contract.Assume(Server == server);
        }

        private void Receive()
        {
            _eventArgs.SetBuffer(_headerBuffer, 0, _propagator.HeaderLength);
            _eventArgs.Completed += OnReceiveHeader;

            try
            {
                if (!_socket.ReceiveAsync(_eventArgs))
                    OnReceiveHeader(this, _eventArgs);
            }
            catch (Exception ex)
            {
                _eventArgs.Completed -= OnReceiveHeader;
                Disconnect();

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

        private void OnReceiveHeader(object sender, SocketAsyncEventArgs args)
        {
            args.Completed -= OnReceiveHeader;

            var length = _propagator.HeaderLength;

            // Indicates a disconnection (and possibly, error).
            var bytesTransferred = args.BytesTransferred;
            if (bytesTransferred != length)
            {
                _log.Warn("Client {0} sent an invalid-length header ({1} bytes, expected {2}) - disconnected.", this,
                    bytesTransferred, length);
                Disconnect();
                return;
            }

            // Decrypt if some sort of encryption scheme has been set.
            if (Crypt != null)
                Crypt.Decrypt(_headerBuffer, 0, length);

            // Try to extract header information; if anything goes wrong, it's definitely a malicious client.
            int recvLength;

            try
            {
                var header = _propagator.HandleHeader(this, _headerBuffer);
                recvLength = header.Length;
                _receiveOpCode = header.OpCode;
            }
            catch (Exception ex)
            {
                ExceptionManager.RegisterException(ex);
                Disconnect();
                return;
            }

            if (_receiveLength > MaxReceiveBufferSize)
            {
                _log.Warn("Client {0} sent an invalid packet length in a header ({1} bytes) - disconnected.", this, _receiveLength);
                Disconnect();
                return;
            }

            if (_receiveLength == 0)
            {
                // For empty packets, we just return the buffer, but with no actual region.
                _propagator.HandlePayload(this, _receiveOpCode, _receiveBuffer, 0);
                Receive();
                return;
            }

            _receiveLength = recvLength;

            // If the client is trying to send a packet larger than our receive buffer, reallocate it.
            if (recvLength > _receiveBuffer.Length)
                _receiveBuffer = new byte[recvLength];

            _eventArgs.SetBuffer(_receiveBuffer, 0, recvLength);
            args.Completed += OnReceivePayload;

            try
            {
                if (!_socket.ReceiveAsync(args))
                    OnReceivePayload(this, args);
            }
            catch (Exception ex)
            {
                args.Completed -= OnReceivePayload;
                Disconnect();

                if (ex is ObjectDisposedException)
                    return;

                if (ex is SocketException)
                {
                    ExceptionManager.RegisterException(ex);
                    return;
                }

                throw;
            }

            Contract.Assume(_receiveLength < _receiveBuffer.Length);
            Contract.Assume(_headerBuffer.Length == _propagator.HeaderLength);
        }

        private void OnReceivePayload(object sender, SocketAsyncEventArgs args)
        {
            args.Completed -= OnReceivePayload;

            // Indicates a disconnection (and possibly, error).
            var bytesTransferred = args.BytesTransferred;
            if (bytesTransferred != _receiveLength)
            {
                _log.Warn("Client {0} sent an invalid-length payload ({1} bytes, expected {2}) - disconnected.", this,
                    bytesTransferred, _receiveLength);
                Disconnect();
                return;
            }

            _propagator.HandlePayload(this, _receiveOpCode, _receiveBuffer, _receiveLength);

            Contract.Assume(_headerBuffer.Length == _propagator.HeaderLength);
        }

        public void Send(OutgoingPacket packet)
        {
            var headerLength = packet.HeaderLength;
            var length = packet.Length;
            var totalLength = headerLength + length;

            // Insert the header.
            var bytes = new byte[totalLength];
            packet.WriteHeader(bytes);

            // Insert the payload.
            packet.Position = 0;
            packet.BaseStream.Read(bytes, headerLength, length);

            // Encrypt if some sort of encryption scheme has been set.
            if (Crypt != null)
                Crypt.Encrypt(bytes, 0, headerLength);

            var args = SocketAsyncEventArgsPool.Acquire();
            args.SetBuffer(bytes, 0, totalLength);
            args.Completed += OnSend;

            try
            {
                if (!_socket.SendAsync(args))
                    OnSend(this, args);
            }
            catch (Exception ex)
            {
                args.Completed -= OnSend;
                SocketAsyncEventArgsPool.Release(args);
                Disconnect();

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

        private void OnSend(object sender, SocketAsyncEventArgs args)
        {
            args.Completed -= OnSend;
            SocketAsyncEventArgsPool.Release(args);
        }

        public void Disconnect()
        {
            try
            {
                _socket.Disconnect(false);
                _socket.Shutdown(SocketShutdown.Both);
                _socket.Dispose();
            }
            catch (Exception)
            {
                // Suck it up... If an exception occurs, it's already been disposed.
            }

            SocketAsyncEventArgsPool.Release(_eventArgs);
        }

        public IPacketCrypt Crypt { get; set; }

        public IPEndPoint EndPoint
        {
            get { return _socket.RemoteEndPoint.ToIPEndPoint(); }
        }

        public dynamic UserData { get; private set; }

        public TcpServer Server { get; private set; }

        public override string ToString()
        {
            return EndPoint.ToString();
        }
    }
}
