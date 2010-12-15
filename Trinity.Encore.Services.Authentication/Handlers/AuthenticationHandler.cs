using System.Diagnostics.Contracts;
using System.Text;
using Trinity.Encore.Framework.Core.Cryptography;
using Trinity.Encore.Framework.Core.Cryptography.SRP;
using Trinity.Encore.Framework.Core.IO;
using Trinity.Encore.Framework.Core.Mathematics;
using Trinity.Encore.Framework.Game.Cryptography;
using Trinity.Encore.Framework.Game.IO;
using Trinity.Encore.Framework.Game.Network;
using Trinity.Encore.Framework.Game.Network.Handling;
using Trinity.Encore.Framework.Game.Network.Transmission;
using Trinity.Encore.Framework.Game.Security;
using Trinity.Encore.Framework.Network.Connectivity;
using Trinity.Encore.Services.Authentication.Enums;

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
            SRPServer srpData = new SRPServer(username, credentials, new WowAuthParameters());
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
            Contract.Requires(generator != null);
            Contract.Requires(generator.ByteLength == 1);

            using (var packet = new OutgoingAuthPacket(GruntOpCodes.AuthenticationLogonChallenge, 118))
            {
                packet.Write((byte)0x00); // If this is > 0, the client fails immediately
                packet.Write((byte)AuthResult.Success);
                packet.Write(publicEphemeral, 32);
                packet.Write(generator, 1, true);
                packet.Write(modulus, 32, true);
                packet.Write(salt, 32);
                packet.Write(rand, 16);

                var extraSecurityFlags = ExtraSecurityFlags.None;
                packet.Write((byte)extraSecurityFlags);
                if (extraSecurityFlags.HasFlag(ExtraSecurityFlags.PIN))
                {
                    packet.Write(0); // Used as the factor for determining PIN order

                    // this is supposed to be an array of 16 bytes but there's no purpose in initializing it now
                    packet.Write((long)0);
                    packet.Write((long)0);
                }

                if (extraSecurityFlags.HasFlag(ExtraSecurityFlags.Matrix))
                {
                    packet.Write((byte) 0); // height
                    packet.Write((byte) 0); // width
                    packet.Write((byte) 0); // minDigits
                    packet.Write((byte) 0); // maxDigits
                    packet.Write((long) 0); // seed for md5

                    // Client MD5's the seed + sessionkey, and uses it as the seed to an HMAC-SHA1
                    // Client then uses the MD5 as a seed to an RC4 context
                    // On every keypress, the client processes the entered value with the RC4, then updates the HMAC with that value
                    // The HMAC result is sent in the auth proof
                }

                if (extraSecurityFlags.HasFlag(ExtraSecurityFlags.SecurityToken))
                {
                    packet.Write((byte) 0);
                }

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

            var clientPublicEphemeralA = packet.ReadBigInteger(32);
            // Client Proof.
            // SHA1 of { SHA1(Modulus) ^ SHA1(Generator), SHA1(USERNAME), salt, PublicA, PublicB, SessionKey }
            var clientResult = packet.ReadBigInteger(20);
            // SHA1 hash of the PublicA and HMACSHA1 of the contents of WoW.exe and unicows.dll. HMAC seed is the 16 bytes at the end of the challenge sent by the server.
            var clientFileHash = packet.ReadBytes(20); // these can safely be ignored

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
                    // SHA of { PublicA, PublicB, byte[20] unknown data }
                    var shaHash = packet.ReadBytes(20);
                    Contract.Assume(unk3.Length == 4);
                    Contract.Assume(shaHash.Length == 20);
                    keys[key] = new AuthLogonKey(unk1, unk2, unk3, shaHash);
                }
            }

            var securityFlags = (ExtraSecurityFlags)packet.ReadByte(); // can be safely ignored

            if (securityFlags.HasFlag(ExtraSecurityFlags.PIN))
            {
                var pinRandom = packet.ReadBytes(16);
                var pinSHA = packet.ReadBytes(20);
            }
            if (securityFlags.HasFlag(ExtraSecurityFlags.Matrix))
            {
                var matrixHMACResult = packet.ReadBytes(20);
            }
            if (securityFlags.HasFlag(ExtraSecurityFlags.SecurityToken))
            {
                var tokenLength = packet.ReadByte();
                var token = packet.ReadBytes(tokenLength);
            }

            SRPServer srpData = client.UserData.SRP;
            srpData.PublicEphemeralValueA = clientPublicEphemeralA;
            var success = srpData.Validator.IsClientProofValid(clientResult);
            if (success)
            {
                SendAuthenticationLogonProofSuccess(client, srpData.Validator.ServerSessionKeyProof);
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
                // Flags. Only These are checked
                // 0x1 = ?
                // 0x8 = Trial Account
                // 0x800000 = ?
                packet.Write(0x00800000);
                // If 1, the client will do a hardware survey
                packet.Write(0x00);
                // If 1, client will fire EVENT_ACCOUNT_MESSAGES_AVAILABLE
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
                if (result == AuthResult.FailUnknownAccount)
                {
                    // This is only read if the result == 4, and even then its not used. But it does need this to be here, as it does a length check before reading
                    packet.Write((short) 0);
                }
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
