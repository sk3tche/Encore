using System;
using Trinity.Encore.AuthenticationService.Realms;
using Trinity.Encore.Game.Network;
using Trinity.Encore.Game.Network.Handling;
using Trinity.Encore.Game.Network.Transmission;
using Trinity.Encore.Game.Security;
using Trinity.Network.Connectivity;

namespace Trinity.Encore.AuthenticationService.Handlers.Realms
{
    [AuthenticationPacketHandler(GruntOpCode.RealmList, Permission = typeof(AuthenticatedPermission))]
    public sealed class RealmListHandler : AuthenticationPacketHandler
    {
        public override bool Read(IClient client, IncomingAuthPacket packet)
        {
            packet.ReadInt32Field("Unknown");

            return true;
        }

        public override void Handle(IClient client)
        {
            RealmManager.Instance.PostAsync(mgr => RealmHandler.SendRealmList(client, mgr.GetRealms(x => true)));
        }
    }
}
