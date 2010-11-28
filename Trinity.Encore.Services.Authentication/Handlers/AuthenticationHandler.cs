using System.Diagnostics.Contracts;
using Trinity.Encore.Framework.Game.Network;
using Trinity.Encore.Framework.Game.Network.Handling;
using Trinity.Encore.Framework.Game.Network.Transmission;
using Trinity.Encore.Framework.Network.Connectivity;
using Trinity.Encore.Framework.Game.IO;
using Trinity.Encore.Framework.Core.IO;
using Trinity.Encore.Services.Authentication.Enums;
using System.Text;
using Trinity.Encore.Framework.Core.Cryptography;
using Trinity.Encore.Framework.Game.Cryptography;
using Trinity.Encore.Framework.Core.Cryptography.SRP;
using Trinity.Encore.Framework.Core.Mathematics;
using System.Security.Cryptography;
using Trinity.Encore.Framework.Game.Security;

namespace Trinity.Encore.Services.Authentication.Handlers
{
    public static class AuthenticationHandler
    {
        [AuthPacketHandler(GruntOpCodes.AuthenticationLogonChallenge)]
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
            SRPServer srpData = GetSRPDataForUsername(username);
            if (srpData == null)
            {
                SendAuthenticationChallengeFailure(client, AuthResult.FailUnknownAccount);
            }
            else
            {
                client.UserData.SRP = srpData;

                // make sure the result is at least 32 bytes long
                var peData = srpData.PublicEphemeralValueB.GetBytes(32);
                var publicEphemeral = new BigInteger(peData);

                var rand = new BigInteger(new FastRandom(), 16 * 8);
                SendAuthenticationChallengeSuccess(client,
                                                   publicEphemeral,
                                                   srpData.Parameters.Generator,
                                                   srpData.Parameters.Modulus,
                                                   srpData.Salt,
                                                   rand);
            }
        }

        /// <summary>
        /// Calculates SRP information based on the username of the account used.
        /// If this account does not exist in the database, return null.
        /// </summary>
        /// <param name="username">The account's username.</param>
        /// <returns>null if no account exists, otherwise an SRPServer instance to be used for this connection.</returns>
        private static SRPServer GetSRPDataForUsername(string username)
        {
            // TODO this needs to be fetched from the database
            BigInteger credentials = null ?? new BigInteger(0);
            SRPServer srpData = new SRPServer(username, credentials, WowAuthParameters.Default);
            BigInteger g = srpData.Parameters.Generator;
            BigInteger n = srpData.Parameters.Modulus;
            srpData.Salt = new BigInteger(new FastRandom(), 32 * 8);
            return srpData;
        }

        public static void SendAuthenticationChallengeFailure(IClient client, AuthResult result)
        {
            Contract.Requires(client != null);
            Contract.Requires(result != AuthResult.Success);

            using (var packet = new OutgoingAuthPacket(GruntOpCodes.AuthenticationLogonChallenge, 2))
            {
                packet.Write((byte)0x00);
                packet.Write((byte)result);
                client.Send(packet);
            }
        }

        public static void SendAuthenticationChallengeSuccess(IClient client, BigInteger publicEphemeral, BigInteger generator, BigInteger modulus, BigInteger salt, BigInteger rand)
        {
            Contract.Requires(client != null);
            Contract.Requires(publicEphemeral != null);
            Contract.Requires(publicEphemeral.ByteLength == 32);
            Contract.Requires(modulus != null);
            Contract.Requires(modulus.ByteLength == 32);
            Contract.Requires(salt != null);
            Contract.Requires(salt.ByteLength == 32);
            Contract.Requires(rand != null);
            Contract.Requires(rand.ByteLength == 16);

            using (var packet = new OutgoingAuthPacket(GruntOpCodes.AuthenticationLogonChallenge, 118))
            {
                packet.Write((byte)0x00);
                packet.Write((byte)AuthResult.Success);
                packet.Write(publicEphemeral, 32);
                packet.Write(generator, 1, true);
                packet.Write(modulus, 32, true);
                packet.Write(salt, 32);
                packet.Write(rand, 16);

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

        [AuthPacketHandler(GruntOpCodes.AuthenticationLogonProof)]
        public static void HandleAuthLogonProof(IClient client, IncomingAuthPacket packet)
        {
            Contract.Requires(client != null);
            Contract.Requires(packet != null);

            var clientPublicEphemeralBytes = packet.ReadBytes(32);
            var clientResultBytes = packet.ReadBytes(20);
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
            SRPServer srpData = client.UserData.SRP;
            srpData.PublicEphemeralValueA = clientPublicEphemeral;
            var success = srpData.Validator.IsClientProofValid(clientResult);
            if (success)
            {
                SendAuthenticationLogonProofSuccess(client, srpData.Validator.ServerSessionKeyProof);
                // TODO now we need to update the database
                client.AddPermission(new AuthenticatedPermission());
            }
            else
                SendAuthenticationLogonProofFailure(client, AuthResult.FailUnknownAccount);
        }

        public static void SendAuthenticationLogonProofSuccess(IClient client, BigInteger serverResult)
        {
            Contract.Requires(client != null);
            Contract.Requires(serverResult != null);

            using (var packet = new OutgoingAuthPacket(GruntOpCodes.AuthenticationLogonProof, 31))
            {
                packet.Write((byte)AuthResult.Success);
                packet.Write(serverResult, 20);
                packet.Write((int)0x00800000);
                packet.Write((int)0x00);
                packet.Write((short)0x00);
                client.Send(packet);
            }
        }

        public static void SendAuthenticationLogonProofFailure(IClient client, AuthResult result)
        {
            Contract.Requires(client != null);

            using (var packet = new OutgoingAuthPacket(GruntOpCodes.AuthenticationLogonProof, 3))
            {
                packet.Write((byte)result);
                packet.Write((byte)3);
                packet.Write((byte)0);
                client.Send(packet);
            }
        }

        [AuthPacketHandler(GruntOpCodes.AuthenticationReconnectChallenge)]
        public static void HandleReconnectChallenge(IClient client, IncomingAuthPacket packet)
        {
            // structure is the same as AuthenticationLogonChallenge
            Contract.Requires(client != null);
            Contract.Requires(packet != null);

            var unk = packet.ReadByte();
            var size = packet.ReadInt16();
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

            // TODO fetch this from the database (or some other persistent storage)
            BigInteger sessionKey = null;
            if (sessionKey == null) {
                client.Disconnect();
                return;
            }

            BigInteger rand = new BigInteger(new FastRandom(), 16 * 8);
            SendReconnectChallengeSuccess(client, rand);
            client.UserData.ReconnectRand = rand;
            client.UserData.Username = username;
        }

        private static void SendReconnectChallengeSuccess(IClient client, BigInteger rand)
        {
            Contract.Requires(client != null);
            Contract.Requires(rand != null);
            Contract.Requires(rand.ByteLength == 16);

            using (var packet = new OutgoingAuthPacket(GruntOpCodes.AuthenticationReconnectChallenge, 34))
            {
                packet.Write((byte)AuthResult.Success);
                packet.Write(rand, 16);

                // this should be a byte[] but there's no point in initializing it as we always send 0s
                packet.Write((ulong)0);
                packet.Write((ulong)0);
            }
        }

        [AuthPacketHandler(GruntOpCodes.AuthenticationReconnectProof)]
        public static void HandleReconnectProof(IClient client, IncomingAuthPacket packet)
        {
            var r1Data = packet.ReadBytes(16);
            BigInteger r1 = new BigInteger(r1Data);
            var r2Data = packet.ReadBytes(20);
            BigInteger r2 = new BigInteger(r2Data);
            var r3Data = packet.ReadBytes(20);
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

            SRPServer srpData = client.UserData.SRP;
            string username = client.UserData.Username;
            BigInteger rand = client.UserData.ReconnectRand;

            // TODO fetch this from the database (or some other persistent storage)
            BigInteger sessionKey = null ?? new BigInteger(0);
            BigInteger hash = srpData.Hash(new HashDataBroker(Encoding.ASCII.GetBytes(username)), r1, rand);
            if (hash == r2)
            {
                SendReconnectProofSuccess(client);
                // TODO now we need to update the database
                client.AddPermission(new AuthenticatedPermission());
            }
            else
            {
                client.Disconnect();
            }
        }

        private static void SendReconnectProofSuccess(IClient client)
        {
            using (var packet = new OutgoingAuthPacket(GruntOpCodes.AuthenticationReconnectProof, 3))
            {
                packet.Write((byte)AuthResult.Success);
                packet.Write((short)0); // two unknown bytes
            }
        }
    }
}
