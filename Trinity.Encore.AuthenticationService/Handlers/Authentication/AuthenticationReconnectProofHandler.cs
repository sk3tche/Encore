using Trinity.Encore.Game.IO;
using Trinity.Encore.Game.Network;
using Trinity.Encore.Game.Network.Handling;
using Trinity.Encore.Game.Network.Transmission;
using Trinity.Network.Connectivity;

namespace Trinity.Encore.AuthenticationService.Handlers.Authentication
{
    [AuthenticationPacketHandler(GruntOpCode.AuthenticationReconnectProof)]
    public sealed class AuthenticationReconnectProofHandler : AuthenticationPacketHandler
    {
        public override bool Read(IClient client, IncomingAuthPacket packet)
        {
            packet.ReadBigIntegerField("Reconnect Proof R1", 16); // MD5(AccountName, byte[16] random)
            packet.ReadBigIntegerField("Reconnect Proof R2", 20); // SHA-1(AccountName, R1, ReconnectProof, SessionKey)
            packet.ReadBigIntegerField("Reconnect Proof R3", 20); // SHA-1(R1, byte[16] zeros)

            var keyCount = packet.ReadByteField("Key Count");
            if (keyCount > 0)
            {
                for (var i = 0; i < keyCount; i++)
                {
                    packet.ReadInt16Field("Key - Unknown 1");
                    packet.ReadInt32Field("Key - Unknown 2");
                    packet.ReadBytesField("Key - Unknown 3", 4);
                    packet.ReadBytesField("Key - SHA-1 Hash", 20); // SHA(PublicA, PublicB, byte[20] unknown)
                }
            }

            return true;
        }

        public override void Handle(IClient client)
        {
        }
    }
}
