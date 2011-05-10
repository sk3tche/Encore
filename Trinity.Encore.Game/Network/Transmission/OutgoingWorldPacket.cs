using System;
using System.Diagnostics.Contracts;
using Trinity.Network.Transmission;

namespace Trinity.Encore.Game.Network.Transmission
{
    public class OutgoingWorldPacket : OutgoingPacket
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

        public const int MinimumOutgoingHeaderSize = 2; // Length and opcode. May be 3 for large packets.

        public override int HeaderLength
        {
            get { return 2 + (Length > Defines.Protocol.LargePacketThreshold ? 1 : 0) + 2; }
        }
    }
}
