using System;

namespace Trinity.Encore.Game.Network.Handling
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class AuthenticationPacketHandlerAttribute : PacketHandlerAttribute
    {
        public AuthenticationPacketHandlerAttribute(GruntOpCode opCode)
            : base(opCode)
        {
        }
    }
}
