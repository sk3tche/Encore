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
            var filePosition = packet.ReadInt64Field("File Position");
            if (filePosition < 0)
                return InvalidValueRange(client, filePosition, 0, long.MaxValue);

            return true;
        }

        public override void Handle(IClient client)
        {
            // TODO: Handle patching.
        }
    }
}
