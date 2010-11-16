using System.Diagnostics.Contracts;
using Trinity.Encore.Framework.Network.Transmission;

namespace Trinity.Encore.Framework.Game.Network.Transmission
{
    public sealed class IncomingWorldPacket : IncomingPacket
    {
        public IncomingWorldPacket(WorldServerOpCodes opCode, byte[] data, int length)
            : base(opCode, data, length, Defines.Protocol.Encoding)
        {
            Contract.Requires(data != null);
            Contract.Requires(length >= 0);
            Contract.Requires(length <= data.Length);
        }

        public new WorldServerOpCodes OpCode
        {
            get { return (WorldServerOpCodes)base.OpCode; }
        }
    }
}
