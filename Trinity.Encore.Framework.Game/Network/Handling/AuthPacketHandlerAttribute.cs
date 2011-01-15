using System;

namespace Trinity.Encore.Framework.Game.Network.Handling
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class AuthPacketHandlerAttribute : PacketHandlerAttribute
    {
        public AuthPacketHandlerAttribute(GruntOpCode opCode)
            : base(opCode)
        {
        }
    }
}
