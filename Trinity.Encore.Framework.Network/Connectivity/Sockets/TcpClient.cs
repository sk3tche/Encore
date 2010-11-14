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

        public const int MaxReceiveBufferSize = 0xffffff; // 2 ^ 24 - 1 (24 bits, 3 bytes).

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

        private int _receivePosition;

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

            var error = args.SocketError;
            if (error != SocketError.Success)
            {
                _log.Warn("Client {0} triggered a socket error ({1}) when trying to fetch a header - disconnected.", this, error);
                Disconnect();
                return;
            }

            var bytesTransferred = args.BytesTransferred;
            if (bytesTransferred == 0)
            {
                _log.Info("Client {0} disconnected gracefully.", this);
                Disconnect();
                return;
            }

            var length = _propagator.HeaderLength;
            if (bytesTransferred > length)
            {
                _log.Warn("Client {0} sent an invalid-length header ({1} bytes, expected {2}) - disconnected.", this,
                    bytesTransferred, length);
                Disconnect();
                return;
            }

            _receivePosition += bytesTransferred;
            if (_receivePosition != length)
            {
                // The header was split; continue receiving...
                args.Completed += OnReceiveHeader;
                args.SetBuffer(_headerBuffer, _receivePosition, length - _receivePosition);

                try
                {
                    if (!_socket.ReceiveAsync(args))
                        OnReceiveHeader(this, args);
                    else
                        return; // Call back once the rest of the header arrives.
                }
                catch (Exception ex)
                {
                    args.Completed -= OnReceiveHeader;
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

            // We have the full header; reset.
            _receivePosition = 0;

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
            Contract.Assume(_headerBuffer.Length == _propagator.HeaderLength); // Make the static checker shut up.
        }

        private void OnReceivePayload(object sender, SocketAsyncEventArgs args)
        {
            args.Completed -= OnReceivePayload;

            var error = args.SocketError;
            if (error != SocketError.Success)
            {
                _log.Warn("Client {0} triggered a socket error ({1}) when trying to fetch a payload - disconnected.", this, error);
                Disconnect();
                return;
            }

            var bytesTransferred = args.BytesTransferred;
            if (bytesTransferred == 0)
            {
                _log.Info("Client {0} disconnected gracefully.", this);
                Disconnect();
                return;
            }

            _receivePosition += bytesTransferred;
            if (_receivePosition != _receiveLength)
            {
                // The payload was split (happens especially for large packets); continue receiving...
                args.Completed += OnReceivePayload;
                args.SetBuffer(_receiveBuffer, _receivePosition, _receiveLength - _receivePosition);

                try
                {
                    if (!_socket.ReceiveAsync(args))
                        OnReceivePayload(this, args);
                    else
                        return; // Call back once the rest of the payload arrives.
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
            }

            _propagator.HandlePayload(this, _receiveOpCode, _receiveBuffer, _receiveLength);
            Contract.Assume(_headerBuffer.Length == _propagator.HeaderLength); // Make the static checker shut up.
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
                // Suck it up. If an exception occurs, it's already been disposed.
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
