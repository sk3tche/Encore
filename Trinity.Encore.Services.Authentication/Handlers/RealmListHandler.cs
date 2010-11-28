using System.Diagnostics.Contracts;
using System.Collections.Generic;
using Trinity.Encore.Services.Authentication.Realms;
using Trinity.Encore.Framework.Game.Network;
using Trinity.Encore.Framework.Game.Network.Handling;
using Trinity.Encore.Framework.Game.Network.Transmission;
using Trinity.Encore.Framework.Network.Connectivity;
using Trinity.Encore.Framework.Game.IO;
using Trinity.Encore.Framework.Core.IO;

namespace Trinity.Encore.Services.Authentication.Handlers
{
    public static class RealmListHandler
    {
        [AuthPacketHandler(GruntOpCodes.RealmList)]
        public static void HandleRealmlist(IClient client, IncomingAuthPacket packet)
        {
            Contract.Requires(client != null);
            Contract.Requires(packet != null);

            var unk = packet.ReadInt32(); // ignored

            List<string> realmNames = RealmList.GetRealmNames();

            var realmsSize = 0;
            foreach (string realmName in realmNames)
            {
                Realm realm = RealmList.GetRealm(realmName);
                realmsSize += 3;
                // +1 for the null character at the end
                realmsSize += realm.Name.Length + 1;
                realmsSize += realm.Address.Length + 1;
                realmsSize += 6;
                if ((realm.Color & 4) != 0)
                    realmsSize += 5;
            }

            using (var outPacket = new OutgoingAuthPacket(GruntOpCodes.RealmList, 10 + realmsSize))
            {
                outPacket.Write((short)(6 + realmsSize + 2));
                outPacket.Write(0);
                outPacket.Write((short)realmNames.Count);
                foreach (string realmName in realmNames)
                {
                    Realm realm = RealmList.GetRealm(realmName);
                    var numChars = realm.GetNumChars(client.UserData.SRP.Username);

                    outPacket.Write(realm.Icon);
                    outPacket.Write(realm.Lock);
                    outPacket.Write(realm.Color);
                    outPacket.WriteCString(realm.Name);
                    outPacket.WriteCString(realm.Address);
                    outPacket.Write(realm.PopulationLevel);
                    outPacket.Write(numChars);
                    outPacket.Write(realm.TimeZone);
                    outPacket.Write((byte)0x2C);
                    if ((realm.Color & 0x04) != 0)
                    {
                        outPacket.Write((byte)0);
                        outPacket.Write((byte)0);
                        outPacket.Write((byte)0);
                        outPacket.Write((short)0);
                    }
                }
                outPacket.Write((byte)0x10);
                outPacket.Write((byte)0x00);
                client.Send(outPacket);
            }
        }
    }
}
