using System.Diagnostics.Contracts;
using Trinity.Encore.Framework.Game.Network;
using Trinity.Encore.Framework.Game.Network.Handling;
using Trinity.Encore.Framework.Game.Network.Transmission;
using Trinity.Encore.Framework.Network.Connectivity;
using Trinity.Encore.Framework.Game.IO;
using Trinity.Encore.Framework.Core.IO;
using Trinity.Encore.Services.Authentication.Enums;
using System.Numerics;

namespace Trinity.Encore.Services.Authentication.Handlers
{
    public static class AuthLogonProofHandler
    {
        private struct AuthLogonKey
        {
            public AuthLogonKey(short unk1, int unk2, byte[] unk3, byte[] shaHash)
            {
                Contract.Requires(unk3 != null);
                Contract.Requires(shaHash != null);
                Contract.Requires(unk3.Length == 4);
                Contract.Requires(shaHash.Length == 20);
                this.unk1 = unk1;
                this.unk2 = unk2;
                this.unk3 = unk3;
                this.shaHash = shaHash;
            }

            public readonly short unk1;
            public readonly int unk2;
            public readonly byte[] unk3;
            public readonly byte[] shaHash;
        }

        [AuthPacketHandler(GruntClientOpCodes.AuthenticationLogonProof)]
        public static void HandleAuthLogonProof(IClient client, IncomingAuthPacket packet)
        {
            Contract.Requires(client != null);
            Contract.Requires(packet != null);

            var clientPublicEphemeralBytes = packet.ReadBytes(32);
            var clientResultBytes = packet.ReadBytes(32);
            var crcHashBytes = packet.ReadBytes(20); // these can safely be ignored

            // the client tends to send 0, but just in case it's safer to implement this.
            var numKeys = packet.ReadByte();
            if (numKeys > 0)
            {
                // only initialize the array if we actually HAVE keys
                AuthLogonKey[] keys = new AuthLogonKey[numKeys];
                for (byte key = 0; key < numKeys; key++)
                {
                    var unk1 = packet.ReadInt16();
                    var unk2 = packet.ReadInt32();
                    var unk3 = packet.ReadBytes(4);
                    var shaHash = packet.ReadBytes(20);
                    keys[key] = new AuthLogonKey(unk1, unk2, unk3, shaHash);
                }
            }

            var securityFlags = packet.ReadByte(); // can be safely ignored

            BigInteger clientPublicEphemeral = new BigInteger(clientPublicEphemeralBytes);
            BigInteger clientResult = new BigInteger(clientResultBytes);
        }

        public static void SendAuthenticationLogonProofSuccess(IClient client, BigInteger serverResult)
        {
            Contract.Requires(client != null);
            Contract.Requires(serverResult != null);

            using (var packet = new OutgoingAuthPacket(GruntServerOpCodes.AuthenticationProof, 31))
            {
                packet.Write((byte)AuthResult.Success);
                packet.Write(serverResult);
                packet.Write(0x00800000);
                packet.Write(0x00);
                packet.Write((short)0x00);
                client.Send(packet);
            }
        }

        public static void SendAuthenticationLogonProofFailure(IClient client, AuthResult result)
        {
            Contract.Requires(client != null);

            using (var packet = new OutgoingAuthPacket(GruntServerOpCodes.AuthenticationProof, 3))
            {
                packet.Write((byte)result);
                packet.Write((byte)3);
                packet.Write((byte)0);
                client.Send(packet);
            }
        }
    }
}
