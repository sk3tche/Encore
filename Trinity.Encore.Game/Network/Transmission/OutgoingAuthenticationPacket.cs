using System;
using System.Diagnostics.Contracts;
using Trinity.Network.Transmission;

namespace Trinity.Encore.Game.Network.Transmission
{
    public class OutgoingAuthenticationPacket : OutgoingPacket
    {
        public OutgoingAuthenticationPacket(GruntOpCode opCode, int capacity = 0)
            : base(opCode, Defines.Protocol.Encoding, capacity)
        {
            Contract.Requires(capacity >= 0);
        }

        public new GruntOpCode OpCode
        {
            get { return (GruntOpCode)base.OpCode; }
        }

        public const int HeaderSize = 1; // Opcode only; length exists only in some packets.

        public override int HeaderLength
        {
            get { return HeaderSize; }
        }
    }
}
