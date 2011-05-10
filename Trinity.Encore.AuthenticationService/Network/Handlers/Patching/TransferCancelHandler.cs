using Trinity.Encore.Game.Network;
using Trinity.Encore.Game.Network.Handling;
using Trinity.Encore.Game.Security;

namespace Trinity.Encore.AuthenticationService.Network.Handlers.Patching
{
    [AuthenticationPacketHandler(GruntOpCode.TransferCancel, Permission = typeof(AuthenticatedPermission))]
    public sealed class TransferCancelHandler : AuthenticationPacketHandler
    {
    }
}
