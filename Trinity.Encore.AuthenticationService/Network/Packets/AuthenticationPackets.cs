using System.Diagnostics.Contracts;
using Trinity.Core;
using Trinity.Core.Cryptography;
using Trinity.Encore.AuthenticationService.Authentication;
using Trinity.Encore.Game.Cryptography;
using Trinity.Encore.Game.IO;
using Trinity.Encore.Game.Network;
using Trinity.Encore.Game.Network.Transmission;

namespace Trinity.Encore.AuthenticationService.Network.Packets
{
    public static class AuthenticationPackets
    {
        public static OutgoingAuthenticationPacket BuildAuthenticationLogOnChallengeSuccess(BigInteger srpPublicB, BigInteger srpGenerator, BigInteger srpModulus,
            BigInteger srpSalt, ExtraSecurityFlags extraFlags = ExtraSecurityFlags.None, bool immediateDisconnect = false)
        {
            Contract.Requires(srpPublicB != null);
            Contract.Requires(srpPublicB.ByteLength == WowAuthenticationParameters.KeySize);
            Contract.Requires(srpGenerator != null);
            Contract.Requires(srpGenerator.ByteLength == WowAuthenticationParameters.GeneratorSize);
            Contract.Requires(srpModulus != null);
            Contract.Requires(srpModulus.ByteLength == WowAuthenticationParameters.KeySize);
            Contract.Requires(srpSalt != null);
            Contract.Requires(srpSalt.ByteLength == WowAuthenticationParameters.KeySize);
            Contract.Ensures(Contract.Result<OutgoingAuthenticationPacket>() != null);

            var packet = new OutgoingAuthenticationPacket(GruntOpCode.AuthenticationLogOnChallenge);

            packet.Write(immediateDisconnect);
            packet.Write((byte)AuthenticationResult.Success);
            packet.Write(srpPublicB, WowAuthenticationParameters.KeySize);
            packet.Write(srpGenerator, WowAuthenticationParameters.GeneratorSize, true);
            packet.Write(srpModulus, WowAuthenticationParameters.KeySize, true);
            packet.Write(srpSalt, WowAuthenticationParameters.KeySize);
            packet.Write(new byte[Password.MD5Length]); // HMAC seed for client file verification.
            packet.Write((byte)extraFlags);

            if (extraFlags.HasFlag(ExtraSecurityFlags.Pin))
            {
                packet.Write(0); // Factor for determining PIN order.
                packet.Write(new byte[Password.MD5Length]);
            }

            if (extraFlags.HasFlag(ExtraSecurityFlags.Matrix))
            {
                packet.Write((byte)0); // Matrix height.
                packet.Write((byte)0); // Matrix width.
                packet.Write((byte)0); // Minimum digits.
                packet.Write((byte)0); // Maximum digits.
                packet.Write((long)0); // MD5 seed.

                // Let S = MD5(seed, sessionKey). Client uses S for the seed to a HMAC SHA-1 and an ARC4. It
                // then captures key presses, and for every press, processes the entered value with the ARC4.
                // The HMAC SHA-1 is then updated with the resulting value.
            }

            if (extraFlags.HasFlag(ExtraSecurityFlags.Token))
                packet.Write((byte)0); // Not sure what this is...

            return packet;
        }

        public static OutgoingAuthenticationPacket BuildAuthenticationLogOnChallengeFailure(AuthenticationResult result, bool immediateDisconnect = false)
        {
            Contract.Requires(result != AuthenticationResult.Success && result != AuthenticationResult.SuccessSurvey);
            Contract.Ensures(Contract.Result<OutgoingAuthenticationPacket>() != null);

            var packet = new OutgoingAuthenticationPacket(GruntOpCode.AuthenticationLogOnChallenge);

            packet.Write(immediateDisconnect);
            packet.Write((byte)result);

            return packet;
        }

        public static OutgoingAuthenticationPacket BuildAuthenticationLogOnProofSuccess(BigInteger serverSessionKeyProof, bool doHardwareSurvery = false)
        {
            Contract.Requires(serverSessionKeyProof != null);
            Contract.Requires(serverSessionKeyProof.ByteLength == 20);
            Contract.Ensures(Contract.Result<OutgoingAuthenticationPacket>() != null);

            var packet = new OutgoingAuthenticationPacket(GruntOpCode.AuthenticationLogOnProof);

            packet.Write((byte)AuthenticationResult.Success);
            packet.Write(serverSessionKeyProof, Password.SHA1Length);
            // Game account flags. Only These are checked:
            // 0x1 = ?
            // 0x8 = Trial Account
            // 0x800000 = ?
            packet.Write(0x00800000);
            packet.Write(doHardwareSurvery.AsConvertible().ToInt32(null));
            // If 1, client will fire EVENT_ACCOUNT_MESSAGES_AVAILABLE.
            packet.Write((short)0);

            return packet;
        }

        public static OutgoingAuthenticationPacket BuildAuthenticationLogOnProofFailure(AuthenticationResult result)
        {
            Contract.Requires(result != AuthenticationResult.Success && result != AuthenticationResult.SuccessSurvey);
            Contract.Ensures(Contract.Result<OutgoingAuthenticationPacket>() != null);

            var packet = new OutgoingAuthenticationPacket(GruntOpCode.AuthenticationLogOnProof);

            packet.Write((byte)result);

            if (result == AuthenticationResult.FailedUnknownAccount)
                packet.Write((short)0); // Unknown value.

            return packet;
        }

        public static OutgoingAuthenticationPacket BuildReconnectChallengeSuccess(BigInteger reconnectProof,
            AuthenticationResult result = AuthenticationResult.Success)
        {
            Contract.Requires(reconnectProof != null);
            Contract.Requires(reconnectProof.ByteLength == Password.MD5Length);
            Contract.Requires(result == AuthenticationResult.Success || result == AuthenticationResult.SuccessSurvey);
            Contract.Ensures(Contract.Result<OutgoingAuthenticationPacket>() != null);

            var packet = new OutgoingAuthenticationPacket(GruntOpCode.AuthenticationReconnectChallenge);

            packet.Write((byte)result);
            packet.Write(reconnectProof, Password.MD5Length);
            packet.Write(new byte[Password.MD5Length]); // HMAC seed for client file verification.

            return packet;
        }

        public static OutgoingAuthenticationPacket BuildReconnectChallengeFailure(AuthenticationResult result)
        {
            Contract.Requires(result != AuthenticationResult.Success && result != AuthenticationResult.SuccessSurvey);
            Contract.Ensures(Contract.Result<OutgoingAuthenticationPacket>() != null);

            var packet = new OutgoingAuthenticationPacket(GruntOpCode.AuthenticationReconnectChallenge);

            packet.Write((byte)result);

            return packet;
        }

        public static OutgoingAuthenticationPacket BuildReconnectProofSuccess()
        {
            Contract.Ensures(Contract.Result<OutgoingAuthenticationPacket>() != null);

            var packet = new OutgoingAuthenticationPacket(GruntOpCode.AuthenticationReconnectProof);

            packet.Write((byte)AuthenticationResult.Success);
            packet.Write((short)0); // Unknown value.

            return packet;
        }

        public static OutgoingAuthenticationPacket BuildReconnectProofFailure(AuthenticationResult result)
        {
            Contract.Requires(result != AuthenticationResult.Success && result != AuthenticationResult.SuccessSurvey);
            Contract.Ensures(Contract.Result<OutgoingAuthenticationPacket>() != null);

            var packet = new OutgoingAuthenticationPacket(GruntOpCode.AuthenticationReconnectProof);

            packet.Write((byte)result);

            return packet;
        }
    }
}
