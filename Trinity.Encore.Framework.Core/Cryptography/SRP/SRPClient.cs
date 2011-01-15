using System;
using System.Diagnostics.Contracts;
using System.Security.Cryptography;

namespace Trinity.Encore.Framework.Core.Cryptography.SRP
{
    /// <summary>
    /// The client-side implementation of the Secure Remote Password (SRP) protocol.
    /// </summary>
    public sealed class SRPClient : SRPBase
    {
        public SRPClient(string userName, BigInteger credentials, SRPParameters parameters)
            : base(userName, credentials, parameters)
        {
            Contract.Requires(!string.IsNullOrEmpty(userName));
            Contract.Requires(credentials != null);
            Contract.Requires(parameters != null);
        }

        public override bool IsServer
        {
            get { return false; }
        }

        public override BigInteger PublicEphemeralValueA
        {
            get { return PublicA ?? (PublicA = Parameters.Generator.ModPow(SecretValue, Parameters.Modulus)); }
            set { throw new NotSupportedException("Client cannot manually set A."); }
        }

        public override BigInteger PublicEphemeralValueB
        {
            get
            {
                if (PublicB == null)
                    throw new CryptographicException("The value of B is not set yet.");

                return PublicB;
            }
            set
            {
                var modulus = Parameters.Modulus;
                var b = value % modulus;

                if (b < 0)
                    b += modulus;

                if (b == 0)
                    throw new CryptographicException("The value of B mod N cannot be 0.");

                PublicB = b;
            }
        }

        public override BigInteger SessionKeyRaw
        {
            get
            {
                if (RawSessionKey == null)
                {
                    var modulus = Parameters.Modulus;
                    var a = Parameters.Generator.ModPow(CredentialsHash, modulus);
                    var b = (Parameters.Multiplier * a) % modulus;
                    var c = (PublicEphemeralValueB - b) % modulus;
                    var d = SecretValue + ScramblingParameter * CredentialsHash;
                    var e = c.ModPow(d, modulus);

                    if (e < 0)
                        e += modulus;

                    RawSessionKey = e;
                }

                Contract.Assume(RawSessionKey.ByteLength == Parameters.KeyLength);
                return RawSessionKey;
            }
        }
    }
}
