namespace Trinity.Encore.Framework.Game.Network.Handling
{
    public sealed class WorldPacketHandlerAttribute : PacketHandlerAttribute
    {
        public WorldPacketHandlerAttribute(WorldServerOpCodes opCode)
            : base(opCode)
        {
        }
    }
}
