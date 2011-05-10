using Trinity.Encore.Game.Network;
using Trinity.Encore.Game.Network.Handling;
using Trinity.Encore.Game.Network.Transmission;
using Trinity.Encore.Game.Security;
using Trinity.Network.Connectivity;

namespace Trinity.Encore.AuthenticationService.Network.Handlers.Patching
{
    [AuthenticationPacketHandler(GruntOpCode.TransferResume, Permission = typeof(AuthenticatedPermission))]
    public sealed class TransferResumeHandler : AuthenticationPacketHandler
    {
        public override bool Read(IClient client, IncomingAuthenticationPacket packet)
        {
            packet.ReadInt64Field("File Position");

            return true;
        }
    }
}
