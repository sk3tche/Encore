using System.Diagnostics.Contracts;
using Trinity.Encore.Framework.Game.Network;
using Trinity.Encore.Framework.Game.Network.Handling;
using Trinity.Encore.Framework.Game.Network.Transmission;
using Trinity.Encore.Framework.Network.Connectivity;
using Trinity.Encore.Framework.Game.IO;
using Trinity.Encore.Framework.Core.IO;
using Trinity.Encore.Services.Authentication.Enums;
using System.Text;

namespace Trinity.Encore.Services.Authentication.Handlers
{
    public static class AuthLogonChallengeHandler
    {
        [AuthPacketHandler(GruntClientOpCodes.AuthenticationLogonChallenge)]
        public static void HandleAuthLogonChallenge(IClient client, IncomingAuthPacket packet)
        {
            Contract.Requires(client != null);
            Contract.Requires(packet != null);

            var unk = packet.ReadByte();
            var size = packet.ReadInt16();
            // we can't read it in directly as a string or char array as in C# chars are 16 bits
            var gameName = packet.ReadFourCC();
            var version1 = packet.ReadByte();
            var version2 = packet.ReadByte();
            var version3 = packet.ReadByte();
            var build = packet.ReadInt16();
            var platform = packet.ReadFourCC();
            var os = packet.ReadFourCC();
            var country = packet.ReadFourCC();
            var timezoneBias = packet.ReadInt32();
            var ip = packet.ReadInt32();
            var usernameLength = packet.ReadByte();
            var usernameBytes = packet.ReadBytes(usernameLength);
            var username = Encoding.ASCII.GetString(usernameBytes);
        }

        public static void SendAuthenticationChallengeFailure(IClient client, AuthResult result)
        {
            Contract.Requires(client != null);
            Contract.Requires(result != AuthResult.Success);

            using (var packet = new OutgoingAuthPacket(GruntServerOpCodes.AuthenticationChallenge, 2))
            {
                packet.Write((byte)0x00);
                packet.Write((byte)result);
                client.Send(packet);
            }
        }

        public static void SendAuthenticationChallengeSuccess(IClient client, byte[] publicEphemeral, byte generator, byte[] modulus, byte[] salt, byte[] rand)
        {
            // TODO Fix this method by adding a BigInteger writing extension to BinaryWriter
            Contract.Requires(client != null);
            Contract.Requires(publicEphemeral != null);
            Contract.Requires(modulus != null);
            Contract.Requires(salt != null);
            Contract.Requires(rand != null);
            Contract.Requires(publicEphemeral.Length == 32);
            Contract.Requires(modulus.Length == 32);
            Contract.Requires(salt.Length == 32);
            Contract.Requires(rand.Length == 16);

            using (var packet = new OutgoingAuthPacket(GruntServerOpCodes.AuthenticationChallenge, 118))
            {
                packet.Write((byte)0x00);
                packet.Write((byte)AuthResult.Success);
                packet.Write(publicEphemeral);
                packet.Write((byte)1); // size of g in bytes
                packet.Write(generator);
                packet.Write((byte)modulus.Length);
                packet.Write(modulus);
                packet.Write(salt);
                packet.Write(rand);

                var extraSecurityFlags = (ExtraSecurityFlags)0x00;
                packet.Write((byte)extraSecurityFlags);
                if (extraSecurityFlags.HasFlag(ExtraSecurityFlags.PIN))
                {
                    packet.Write((int)0);

                    // this is supposed to be an array of 16 bytes but there's no purpose in initializing it now
                    packet.Write((long)0);
                    packet.Write((long)0);
                }

                if (extraSecurityFlags.HasFlag(ExtraSecurityFlags.Matrix))
                {
                    for (int i = 0; i < 4; i++)
                        packet.Write((byte)0);
                    packet.Write((long)0);
                }

                if (extraSecurityFlags.HasFlag(ExtraSecurityFlags.SecurityToken))
                    packet.Write((byte)0);

                client.Send(packet);
            }
        }
    }
}
