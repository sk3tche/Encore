using Trinity.Encore.AuthenticationService.Authentication;
using Trinity.Encore.Game.Cryptography;
using Trinity.Encore.Game.Network;
using Trinity.Encore.Game.Network.Handling;
using Trinity.Encore.Game.Network.Transmission;
using Trinity.Network.Connectivity;

namespace Trinity.Encore.AuthenticationService.Network.Handlers.Authentication
{
    [AuthenticationPacketHandler(GruntOpCode.AuthenticationLogOnProof)]
    public sealed class AuthenticationLogOnProofHandler : AuthenticationPacketHandler
    {
        public override bool Read(IClient client, IncomingAuthenticationPacket packet)
        {
            packet.ReadBigIntegerField("SRP A", WowAuthenticationParameters.KeySize);
            packet.ReadBigIntegerField("SRP Proof", 20);

            // SHA-1 hash of the PublicA and HMAC SHA-1 of the contents of WoW.exe and unicows.dll. HMAC seed is
            // the 16 bytes at the end of the challenge sent by the server. Currently fairly useless for us.
            packet.ReadBytesField("Client File SHA-1 Hash", 20);

            var keyCount = packet.ReadByteField("Key Count");
            if (keyCount > 0)
            {
                for (var i = 0; i < keyCount; i++)
                {
                    packet.ReadInt16Field("Key - Unknown 1");
                    packet.ReadInt32Field("Key - Unknown 2");
                    packet.ReadBytesField("Key - Unknown 3", 4);
                    packet.ReadBytesField("Key - SHA-1 Hash", 20); // SHA-1(PublicA, PublicB, byte[20] unknown)
                }
            }

            var securityFlags = (ExtraSecurityFlags)packet.ReadByteField("Extra Security Flags").Value;

            if (securityFlags.HasFlag(ExtraSecurityFlags.Pin))
            {
                packet.ReadBytesField("PIN Random", 16);
                packet.ReadBytesField("PIN SHA-1 Hash", 20);
            }

            if (securityFlags.HasFlag(ExtraSecurityFlags.Matrix))
            {
                packet.ReadBytesField("Matrix HMAC SHA-1 Hash", 20);
            }

            if (securityFlags.HasFlag(ExtraSecurityFlags.Token))
            {
                var tokenLength = packet.ReadByteField("Security Token Length");
                packet.ReadBytesField("Security Token", tokenLength);
            }

            return true;
        }

        public override void Handle(IClient client)
        {
        }
    }
}
