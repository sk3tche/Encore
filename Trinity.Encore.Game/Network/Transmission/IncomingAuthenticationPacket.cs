using System.Diagnostics.Contracts;
using Trinity.Network.Transmission;

namespace Trinity.Encore.Game.Network.Transmission
{
    public sealed class IncomingAuthenticationPacket : IncomingPacket
    {
        public IncomingAuthenticationPacket(GruntOpCode opCode, byte[] data, int length)
            : base(opCode, data, length, Defines.Protocol.Encoding)
        {
            Contract.Requires(data != null);
            Contract.Requires(length >= 0);
            Contract.Requires(length <= data.Length);
        }

        public new GruntOpCode OpCode
        {
            get { return (GruntOpCode)base.OpCode; }
        }
    }
}
