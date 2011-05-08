using System;
using Trinity.Encore.Game.Network;
using Trinity.Encore.Game.Network.Handling;
using Trinity.Encore.Game.Network.Transmission;
using Trinity.Encore.Game.Security;
using Trinity.Network.Connectivity;

namespace Trinity.Encore.AuthenticationService.Handlers.Patching
{
    [AuthenticationPacketHandler(GruntOpCode.TransferComplete, Permission = typeof(AuthenticatedPermission))]
    public sealed class TransferCompleteHandler : AuthenticationPacketHandler
    {
    }
}
