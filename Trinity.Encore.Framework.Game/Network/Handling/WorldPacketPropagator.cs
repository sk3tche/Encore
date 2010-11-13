using System;
using System.Diagnostics.Contracts;
using Trinity.Encore.Framework.Core.Exceptions;
using Trinity.Encore.Framework.Core.Logging;
using Trinity.Encore.Framework.Game.Network.Transmission;
using Trinity.Encore.Framework.Network.Connectivity;
using Trinity.Encore.Framework.Network.Handling;

namespace Trinity.Encore.Framework.Game.Network.Handling
{
    public sealed class WorldPacketPropagator : PacketPropagatorBase<WorldPacketHandlerAttribute, IncomingWorldPacket>,
        IPacketPropagator
    {
        private static readonly LogProxy _log = new LogProxy("WorldPacketPropagator");

        public const int HeaderSize = 2 + 4; // Length and opcode.

        public int HeaderLength
        {
            get { return HeaderSize;  }
        }

        public PacketHeader HandleHeader(IClient client, byte[] header)
        {
            Contract.Assume(header.Length == HeaderSize);

            var length = BitConverter.ToInt16(header, 0);
            Contract.Assume(length >= 0);

            var opCode = BitConverter.ToInt32(header, 2);
            Contract.Assume(opCode >= 0);

            return new PacketHeader(length, opCode);
        }

        public void HandlePayload(IClient client, int opCode, byte[] payload, int length)
        {
            var handler = GetHandler(opCode);
            if (handler == null)
            {
                client.Disconnect();
                _log.Warn("Client {0} sent an unhandled opcode {1} - disconnected.", client, opCode.ToString("X8"));
                return;
            }

            var permission = handler.Permission;
            Contract.Assume(permission != null);

            if (!client.HasPermission(permission))
            {
                client.Disconnect();
                _log.Warn("Client {0} sent opcode {1} which requires permission {2} - disconnected.", client,
                    opCode.ToString("X8"), permission.Name);
                return;
            }

            try
            {
                var packet = new IncomingWorldPacket((WorldServerOpCodes)opCode, payload, length);
                handler.Invoke(client, packet);
            }
            catch (Exception ex)
            {
                ExceptionManager.RegisterException(ex, client);
                client.Disconnect();
            }
        }
    }
}
