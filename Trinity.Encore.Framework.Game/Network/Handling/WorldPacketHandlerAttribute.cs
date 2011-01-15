using System;

namespace Trinity.Encore.Framework.Game.Network.Handling
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class WorldPacketHandlerAttribute : PacketHandlerAttribute
    {
        public WorldPacketHandlerAttribute(WorldOpCode opCode)
            : base(opCode)
        {
        }
    }
}
