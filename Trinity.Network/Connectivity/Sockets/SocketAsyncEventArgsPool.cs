using System.Collections.Concurrent;
using System.Diagnostics.Contracts;
using System.Net.Sockets;

namespace Trinity.Network.Connectivity.Sockets
{
    public static class SocketAsyncEventArgsPool
    {
        private static readonly ObjectPool<SocketAsyncEventArgs> _objectPool =
            new ObjectPool<SocketAsyncEventArgs>(() => new SocketAsyncEventArgs());

        public static SocketAsyncEventArgs Acquire()
        {
            Contract.Ensures(Contract.Result<SocketAsyncEventArgs>() != null);

            var obj = _objectPool.GetObject();
            Contract.Assume(obj != null);
            return obj;
        }

        public static void Release(SocketAsyncEventArgs arg)
        {
            Contract.Requires(arg != null);

            arg.AcceptSocket = null;
            arg.SetBuffer(null, 0, 0);
            arg.BufferList = null;
            arg.DisconnectReuseSocket = false;
            arg.RemoteEndPoint = null;
            arg.SendPacketsElements = null;
            arg.SendPacketsFlags = 0;
            arg.SendPacketsSendSize = 0;
            arg.SocketError = 0;
            arg.SocketFlags = 0;
            arg.UserToken = null;

            _objectPool.PutObject(arg);
        }
    }
}
