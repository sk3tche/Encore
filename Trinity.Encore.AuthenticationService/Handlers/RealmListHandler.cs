using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Trinity.Core.IO;
using Trinity.Encore.AuthenticationService.Realms;
using Trinity.Encore.Game.Network;
using Trinity.Encore.Game.Network.Handling;
using Trinity.Encore.Game.Network.Transmission;
using Trinity.Encore.Game.Realms;
using Trinity.Network.Connectivity;

namespace Trinity.Encore.AuthenticationService.Handlers
{
    public static class RealmListHandler
    {
        [AuthPacketHandler(GruntOpCode.RealmList)]
        public static void HandleRealmList(IClient client, IncomingAuthPacket packet)
        {
            Contract.Requires(client != null);
            Contract.Requires(packet != null);

            packet.ReadInt32(); // unk, ignored

            RealmManager.Instance.PostAsync(mgr => SendRealmList(client, mgr.GetRealms(x => true)));
        }

        public static void SendRealmList(IClient client, IEnumerable<Realm> realms)
        {
            Contract.Requires(client != null);
            Contract.Requires(realms != null);

            var count = realms.Count();

            using (var packet = new OutgoingAuthPacket(GruntOpCode.RealmList,
                2 + 4 + 4 + count * (1 + 1 + 1 + 4 + 4 + 1 + 1))) // estimated packet size
            {
                packet.Write((ushort)0); // packet length
                packet.Write(0); // unk
                packet.Write((ushort)count);

                foreach (var realm in realms)
                {
                    packet.Write((byte)realm.Type);
                    packet.Write((byte)realm.Status);
                    packet.Write((byte)realm.Flags);
                    packet.WriteCString(realm.Name);
                    packet.WriteCString(realm.Location.ToString());
                    packet.Write(realm.PopulationLevel);
                    packet.Write(0); // number of characters the client has on this realm
                    packet.Write((byte)realm.Category);
                    packet.Write((byte)0x2C); // probably site id

                    if (!realm.Flags.HasFlag(RealmFlags.SpecifyBuild))
                        continue;

                    packet.Write((byte)realm.ClientVersion.Major);
                    packet.Write((byte)realm.ClientVersion.Minor);
                    packet.Write((byte)realm.ClientVersion.Build);
                    packet.Write((ushort)realm.ClientVersion.Revision);
                }

                packet.Write((ushort)0x1000);

                // Write the packet length.
                packet.Position = 0;
                packet.Write((ushort)(packet.Length - packet.HeaderLength - 2));

                client.Send(packet);
            }
        }
    }
}
