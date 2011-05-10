using System.Diagnostics.Contracts;
using System.Text;
using Trinity.Core.Cryptography;
using Trinity.Core.Cryptography.SRP;
using Trinity.Core.IO;
using Trinity.Core.Mathematics;
using Trinity.Encore.AuthenticationService.Authentication;
using Trinity.Encore.Game.Cryptography;
using Trinity.Encore.Game.IO;
using Trinity.Encore.Game.Network;
using Trinity.Encore.Game.Network.Handling;
using Trinity.Encore.Game.Network.Transmission;
using Trinity.Encore.Game.Security;
using Trinity.Network.Connectivity;

namespace Trinity.Encore.AuthenticationService.Handlers
{
    public static class AuthenticationHandler
    {
        // TODO: Migrate all this stuff to the new handlers...

        /*
        public static void HandleAuthLogOnChallenge(IClient client, IncomingAuthPacket packet)
        {
            Contract.Assume(!string.IsNullOrEmpty(username));
            SRPServer srpData = GetSRPDataForUserName(username);
            if (srpData == null)
            {
                SendAuthenticationChallengeFailure(client, AuthenticationResult.FailedUnknownAccount);
            }
            else
            {
                client.UserData.SRP = srpData;

                // make sure the result is at least 32 bytes long
                var peData = srpData.PublicEphemeralValueB.GetBytes(32);
                var publicEphemeral = new BigInteger(peData);

                var rand = new BigInteger(new FastRandom(), 16 * 8);
                Contract.Assume(srpData.Parameters.Modulus.ByteLength == 32);
                Contract.Assume(srpData.Salt.ByteLength == 32);
                Contract.Assume(rand.ByteLength == 16);
                Contract.Assume(srpData.Parameters.Generator.ByteLength == 1);
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
        /// <param name="userName">The account's username.</param>
        /// <returns>null if no account exists, otherwise an SRPServer instance to be used for this connection.</returns>
        private static SRPServer GetSRPDataForUserName(string userName)
        {
            Contract.Requires(!string.IsNullOrEmpty(userName));
            // TODO this needs to be fetched from the database
            BigInteger credentials = null ?? new BigInteger(0);
            SRPServer srpData = new SRPServer(userName, credentials, new WowAuthParameters());
            srpData.Salt = new BigInteger(new FastRandom(), 32 * 8);
            return srpData;
        }

        public static void HandleAuthLogOnProof(IClient client, IncomingAuthPacket packet)
        {
            SRPServer srpData = client.UserData.SRP;
            Contract.Assume(clientPublicEphemeralA != null);
            srpData.PublicEphemeralValueA = clientPublicEphemeralA;
            Contract.Assume(clientResult != null);
            var success = srpData.Validator.IsClientProofValid(clientResult);
            if (success)
            {
                SendAuthenticationLogOnProofSuccess(client, srpData.Validator.ServerSessionKeyProof);
                client.AddPermission(new AuthenticatedPermission());
            }
            else
                SendAuthenticationLogOnProofFailure(client, AuthenticationResult.FailedUnknownAccount);
        }

        public static void SendAuthenticationLogOnProofFailure(IClient client, AuthenticationResult result)
        {
            Contract.Requires(client != null);

            using (var packet = new OutgoingAuthPacket(GruntOpCode.AuthenticationLogOnProof, 3))
            {
                packet.Write((byte)result);
                if (result == AuthenticationResult.FailedUnknownAccount)
                {
                    // This is only read if the result == 4, and even then its not used. But it does need this to be here, as it does a length check before reading
                    packet.Write((short) 0);
                }
                client.Send(packet);
            }
        }

        public static void HandleReconnectChallenge(IClient client, IncomingAuthPacket packet)
        {
            // TODO fetch this from the database (or some other persistent storage)
            BigInteger sessionKey = null;
            if (sessionKey == null)
            {
                client.Disconnect();
                return;
            }

            //BigInteger rand = new BigInteger(new FastRandom(), 16 * 8);
            //SendReconnectChallengeSuccess(client, rand);
            //client.UserData.ReconnectRand = rand;
            //client.UserData.Username = username;
        }

        public static void HandleReconnectProof(IClient client, IncomingAuthPacket packet)
        {
            SRPServer srpData = client.UserData.SRP;
            string username = client.UserData.Username;
            BigInteger rand = client.UserData.ReconnectRand;

            // TODO fetch this from the database (or some other persistent storage)
            //BigInteger sessionKey = null ?? new BigInteger(0);
            Contract.Assume(username != null);
            Contract.Assume(r1 != null);
            Contract.Assume(rand != null);
            BigInteger hash = srpData.Hash(new HashDataBroker(Encoding.ASCII.GetBytes(username)), r1, rand);
            if (hash == r2)
            {
                SendReconnectProofSuccess(client);
                client.AddPermission(new AuthenticatedPermission());
            }
            else
                client.Disconnect();
        }
        */
    }
}
