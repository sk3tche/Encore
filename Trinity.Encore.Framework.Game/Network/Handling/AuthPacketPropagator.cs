using System;
using Trinity.Encore.Framework.Game.Network.Transmission;
using Trinity.Encore.Framework.Network.Connectivity;
using Trinity.Encore.Framework.Network.Handling;

namespace Trinity.Encore.Framework.Game.Network.Handling
{
    public sealed class AuthPacketPropagator : PacketPropagatorBase<AuthPacketHandlerAttribute, IncomingAuthPacket>,
        IPacketPropagator
    {
        public const int HeaderSize = 1; // Opcode only; length exists only in some packets.

        public int HeaderLength
        {
            get { return HeaderSize; }
        }

        public PacketHeader HandleHeader(IClient client, byte[] header)
        {
            // TODO: Do some ugly switching on the opcode and figure out a size... Sigh.
            throw new NotImplementedException();
        }

        public void HandlePayload(IClient client, int opCode, byte[] payload, int length)
        {
        }
    }
}
