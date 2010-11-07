using System;
using System.Diagnostics.Contracts;
using Trinity.Encore.Framework.Core.Cryptography;

namespace Trinity.Encore.Framework.Game.Cryptography
{
    [Serializable]
    public sealed class Password
    {
        public BigInteger SHA1Password { get; private set; }

        public BigInteger SHA256Password { get; private set; }

        [ContractInvariantMethod]
        private void Invariant()
        {
            Contract.Invariant(SHA1Password != null);
            Contract.Invariant(SHA1Password.ByteLength == 20);
            Contract.Invariant(SHA256Password != null);
            Contract.Invariant(SHA256Password.ByteLength == 32);
        }

        public Password(BigInteger sha1, BigInteger sha256)
        {
            Contract.Requires(sha1 != null);
            Contract.Requires(sha1.ByteLength == 20);
            Contract.Requires(sha256 != null);
            Contract.Requires(sha256.ByteLength == 32);

            SHA1Password = sha1;
            SHA256Password = sha256;
        }
    }
}
