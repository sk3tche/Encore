using System;
using System.Diagnostics.Contracts;
using System.Net;
using Trinity.Encore.Game.Network.Transmission;
using Trinity.Network.Connectivity;
using Trinity.Network.Handling;
using Trinity.Network.Transmission;

namespace Trinity.Encore.Game.Network.Handling
{
    public sealed class WorldPacketPropagator : PacketPropagatorBase<WorldPacketHandlerAttribute, IncomingWorldPacket>
    {
        public const int IncomingHeaderSize = 2 + 4; // Length and opcode.

        public override int IncomingHeaderLength
        {
            get { return IncomingHeaderSize;  }
        }

        public override PacketHeader HandleHeader(IClient client, byte[] header)
        {
            Contract.Assume(header.Length == IncomingHeaderSize);

            var length = IPAddress.NetworkToHostOrder(unchecked((short)BitConverter.ToUInt16(header, 0)));
            Contract.Assume(length >= 0);
            var opCode = (int)BitConverter.ToUInt32(header, 2);

            return new PacketHeader(length, opCode);
        }

        protected override IncomingWorldPacket CreatePacket(int opCode, byte[] payload, int length)
        {
            return new IncomingWorldPacket((WorldOpCode)opCode, payload, length);
        }

        public override void WriteHeader(OutgoingPacket packet, byte[] buffer)
        {
            var headerIdx = 0;
            var length = packet.Length;
            var large = length > 0x7fff;

            if (large)
                buffer[headerIdx++] = (byte)(0x80 | (0xff & length >> 16));

            buffer[headerIdx++] = (byte)(0xff & length >> 8);
            buffer[headerIdx++] = (byte)(0xff & length);

            var opCode = BitConverter.GetBytes(((IConvertible)packet.OpCode).ToUInt16(null));
            Buffer.BlockCopy(opCode, 0, buffer, headerIdx, 2);
        }
    }
}
