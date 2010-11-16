namespace Trinity.Encore.Framework.Game.Network.Handling
{
    public sealed class AuthPacketHandlerAttribute : PacketHandlerAttribute
    {
        public AuthPacketHandlerAttribute(GruntClientOpCodes opCode)
            : base(opCode)
        {
        }
    }
}
