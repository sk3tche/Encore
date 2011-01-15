using System;
using System.Diagnostics.Contracts;
using System.Security.Cryptography;

namespace Trinity.Encore.Framework.Core.Cryptography.SRP
{
    /// <summary>
    /// The server-side implementation of the Secure Remote Password (SRP) protocol.
    /// </summary>
    public sealed class SRPServer : SRPBase
    {
        public SRPServer(string userName, BigInteger credentials, SRPParameters parameters)
            : base(userName, credentials, parameters)
        {
            Contract.Requires(!string.IsNullOrEmpty(userName));
            Contract.Requires(credentials != null);
            Contract.Requires(parameters != null);
        }

        public override bool IsServer
        {
            get { return true; }
        }

        private BigInteger _verifier;

        public override BigInteger PublicEphemeralValueA
        {
            get
            {
                if (PublicA == null)
                    throw new CryptographicException("A is not set yet.");

                return PublicA;
            }
            set
            {
                var modulus = Parameters.Modulus;
                var a = value % modulus;

                if (a < 0)
                    a += modulus;

                if (a == 0)
                    throw new CryptographicException("The value of A mod N cannot be 0.");

                PublicA = a;
            }
        }

        public override BigInteger PublicEphemeralValueB
        {
            get
            {
                if (PublicB == null)
                {
                    var modulus = Parameters.Modulus;
                    var a = Parameters.Multiplier * Verifier;
                    var b = Parameters.Generator.ModPow(SecretValue, modulus);
                    var c = (a + b) % modulus;

                    if (c < 0)
                        c += modulus;

                    PublicB = c;
                }

                return PublicB;
            }
            set { throw new NotSupportedException("Server cannot manually set B."); }
        }

        public override BigInteger SessionKeyRaw
        {
            get
            {
                Contract.Ensures(Contract.Result<BigInteger>() != null);

                if (RawSessionKey == null)
                {
                    var modulus = Parameters.Modulus;
                    var a = Verifier.ModPow(ScramblingParameter, modulus);
                    var b = a * PublicEphemeralValueA;

                    RawSessionKey = b.ModPow(SecretValue, modulus);
                }

                Contract.Assume(RawSessionKey.ByteLength == Parameters.KeyLength);
                return RawSessionKey;
            }
        }

        /// <summary>
        /// This is v in the specification; v = g ^ x (mod N).
        /// </summary>
        public BigInteger Verifier
        {
            get
            {
                Contract.Ensures(Contract.Result<BigInteger>() != null);

                var modulus = Parameters.Modulus;

                if (_verifier == null)
                    _verifier = Parameters.Generator.ModPow(CredentialsHash, modulus);

                if (_verifier < 0)
                    _verifier += modulus;

                return _verifier;
            }
            set
            {
                Contract.Requires(value != null);

                _verifier = value;
            }
        }
    }
}
