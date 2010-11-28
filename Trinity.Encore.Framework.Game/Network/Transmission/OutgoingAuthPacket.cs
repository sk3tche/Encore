using System;
using System.Diagnostics.Contracts;
using Trinity.Encore.Framework.Network.Transmission;

namespace Trinity.Encore.Framework.Game.Network.Transmission
{
    public sealed class OutgoingAuthPacket : OutgoingPacket
    {
        public OutgoingAuthPacket(GruntOpCodes opCode, int capacity = 0)
            : base(opCode, Defines.Protocol.Encoding, capacity)
        {
            Contract.Requires(capacity >= 0);
        }

        public new GruntOpCodes OpCode
        {
            get { return (GruntOpCodes)base.OpCode; }
        }

        public override int HeaderLength
        {
            get { return 1; /* Opcode only; length exists in a select few packets. */ }
        }

        public override void WriteHeader(byte[] buffer)
        {
            buffer[0] = ((IConvertible)OpCode).ToByte(null);
        }
    }
}
