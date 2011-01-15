using System;
using System.Diagnostics.Contracts;
using Trinity.Encore.Framework.Network.Transmission;

namespace Trinity.Encore.Framework.Game.Network.Transmission
{
    public sealed class OutgoingWorldPacket : OutgoingPacket
    {
        public OutgoingWorldPacket(WorldOpCode opCode, int capacity = 0)
            : base(opCode, Defines.Protocol.Encoding, capacity)
        {
            Contract.Requires(capacity >= 0);
        }

        public new WorldOpCode OpCode
        {
            get { return (WorldOpCode)base.OpCode; }
        }

        public override int HeaderLength
        {
            get { return 2 + (Length > Defines.Protocol.LargePacketThreshold ? 1 : 0) + 2; /* Length and opcode. */ }
        }

        public override void WriteHeader(byte[] buffer)
        {
            var headerIdx = 0;
            var large = Length > 0x7fff;

            if (large)
                buffer[headerIdx++] = (byte)(0x80 | (0xff & Length >> 16));

            buffer[headerIdx++] = (byte)(0xff & Length >> 8);
            buffer[headerIdx++] = (byte)(0xff & Length);

            var opCode = BitConverter.GetBytes(((IConvertible)OpCode).ToUInt16(null));
            Buffer.BlockCopy(opCode, 0, buffer, headerIdx, 2);
        }
    }
}
