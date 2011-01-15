using Trinity.Encore.Framework.Game.Network.Transmission;
using Trinity.Encore.Framework.Network.Connectivity;
using Trinity.Encore.Framework.Network.Handling;

namespace Trinity.Encore.Framework.Game.Network.Handling
{
    public sealed class AuthPacketPropagator : PacketPropagatorBase<AuthPacketHandlerAttribute, IncomingAuthPacket>
    {
        public const int HeaderSize = 1; // Opcode only; length exists only in some packets.

        public const int ChunkSize = 256;

        public override int HeaderLength
        {
            get { return HeaderSize; }
        }

        public override PacketHeader HandleHeader(IClient client, byte[] header)
        {
            var opCode = header[0];

            return new PacketHeader(ChunkSize, opCode);
        }

        protected override IncomingAuthPacket CreatePacket(int opCode, byte[] payload, int length)
        {
            return new IncomingAuthPacket((GruntOpCode)opCode, payload, length);
        }
    }
}
