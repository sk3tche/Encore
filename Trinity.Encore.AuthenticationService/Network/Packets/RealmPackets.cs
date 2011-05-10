using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Trinity.Core.IO;
using Trinity.Encore.AuthenticationService.Realms;
using Trinity.Encore.Game.Network;
using Trinity.Encore.Game.Network.Transmission;
using Trinity.Encore.Game.Realms;

namespace Trinity.Encore.AuthenticationService.Network.Packets
{
    public static class RealmPackets
    {
        public static OutgoingAuthenticationPacket BuildRealmList(IEnumerable<Realm> realms)
        {
            Contract.Requires(realms != null);
            Contract.Ensures(Contract.Result<OutgoingAuthenticationPacket>() != null);

            var packet = new OutgoingAuthenticationPacket(GruntOpCode.RealmList);

            packet.Pad(0, sizeof(short)); // Packet body length.
            packet.Write(0); // Unknown value.
            packet.Write((short)realms.Count());

            foreach (var realm in realms)
            {
                packet.Write((byte)realm.Type);
                packet.Write((byte)realm.Status);
                packet.Write((byte)realm.Flags);
                packet.WriteCString(realm.Name);
                packet.WriteCString(realm.Location.ToString());
                packet.Write(realm.PopulationLevel);
                packet.Write(0); // Number of characters the client has on this realm.
                packet.Write((byte)realm.Category);
                packet.Write((byte)0x2c); // Probably site ID.

                if (!realm.Flags.HasFlag(RealmFlags.SpecifyBuild))
                    continue;

                packet.Write((byte)realm.ClientVersion.Major);
                packet.Write((byte)realm.ClientVersion.Minor);
                packet.Write((byte)realm.ClientVersion.Build);
                packet.Write((short)realm.ClientVersion.Revision);
            }

            packet.Write((short)0x1000); // No idea what this is...

            // Write the packet length.
            packet.Position = 0;
            packet.Write((short)(packet.Length - packet.HeaderLength - sizeof(short)));

            return packet;
        }
    }
}
