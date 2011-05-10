using Trinity.Encore.AuthenticationService.Handlers;
using Trinity.Encore.AuthenticationService.Network.Packets;
using Trinity.Encore.AuthenticationService.Realms;
using Trinity.Encore.Game.Network;
using Trinity.Encore.Game.Network.Handling;
using Trinity.Encore.Game.Network.Transmission;
using Trinity.Encore.Game.Security;
using Trinity.Network.Connectivity;

namespace Trinity.Encore.AuthenticationService.Network.Handlers.Realms
{
    [AuthenticationPacketHandler(GruntOpCode.RealmList, Permission = typeof(AuthenticatedPermission))]
    public sealed class RealmListHandler : AuthenticationPacketHandler
    {
        public override bool Read(IClient client, IncomingAuthenticationPacket packet)
        {
            packet.ReadInt32Field("Unknown");

            return true;
        }

        public override void Handle(IClient client)
        {
            RealmManager.Instance.PostAsync(mgr =>
            {
                var realms = mgr.GetRealms(x => true);
                client.PostAsync(() => client.Send(RealmPackets.BuildRealmList(realms)));
            });
        }
    }
}
