using System;
using Trinity.Encore.Game.Network.Transmission;
using Trinity.Network.Connectivity;
using Trinity.Network.Handling;
using Trinity.Network.Transmission;

namespace Trinity.Encore.Game.Network.Handling
{
    public sealed class AuthenticationPacketPropagator : PacketPropagatorBase<AuthenticationPacketHandlerAttribute, IncomingAuthPacket,
        AuthenticationPacketHandler>
    {
        public const int IncomingHeaderSize = 1; // Opcode only; length exists only in some packets.

        public const int EstimatedBodySize = 256;

        public override int IncomingHeaderLength
        {
            get { return IncomingHeaderSize; }
        }

        public override PacketHeader HandleHeader(IClient client, byte[] header)
        {
            var opCode = header[0];

            return new PacketHeader(EstimatedBodySize, opCode);
        }

        protected override IncomingAuthPacket CreatePacket(int opCode, byte[] payload, int length)
        {
            return new IncomingAuthPacket((GruntOpCode)opCode, payload, length);
        }

        public override void WriteHeader(OutgoingPacket packet, byte[] buffer)
        {
            buffer[0] = ((IConvertible)packet.OpCode).ToByte(null);
        }
    }
}
