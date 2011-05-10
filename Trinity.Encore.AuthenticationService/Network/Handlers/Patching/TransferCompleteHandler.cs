using Trinity.Encore.Game.Network;
using Trinity.Encore.Game.Network.Handling;
using Trinity.Encore.Game.Security;

namespace Trinity.Encore.AuthenticationService.Network.Handlers.Patching
{
    [AuthenticationPacketHandler(GruntOpCode.TransferComplete, Permission = typeof(AuthenticatedPermission))]
    public sealed class TransferCompleteHandler : AuthenticationPacketHandler
    {
    }
}
