using System.Diagnostics.Contracts;
using Trinity.Network.Transmission;

namespace Trinity.Encore.Game.Network.Transmission
{
    public sealed class IncomingWorldPacket : IncomingPacket
    {
        public IncomingWorldPacket(WorldOpCode opCode, byte[] data, int length)
            : base(opCode, data, length, Defines.Protocol.Encoding)
        {
            Contract.Requires(data != null);
            Contract.Requires(length >= 0);
            Contract.Requires(length <= data.Length);
        }

        public new WorldOpCode OpCode
        {
            get { return (WorldOpCode)base.OpCode; }
        }
    }
}
