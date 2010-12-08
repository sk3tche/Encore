using System;
using System.Diagnostics.Contracts;
using System.Security.Cryptography;
using System.Text;
using Trinity.Encore.Framework.Core.Cryptography;

namespace Trinity.Encore.Framework.Game.Cryptography
{
    [Serializable]
    public sealed class Password
    {
        public const int SHA1Length = 20;

        public const int SHA256Length = 32;

        public BigInteger SHA1Password { get; private set; }

        public BigInteger SHA256Password { get; private set; }

        [ContractInvariantMethod]
        private void Invariant()
        {
            Contract.Invariant(SHA1Password != null);
            Contract.Invariant(SHA1Password.ByteLength == SHA1Length);
            Contract.Invariant(SHA256Password != null);
            Contract.Invariant(SHA256Password.ByteLength == SHA256Length);
        }

        public Password(BigInteger sha1, BigInteger sha256)
        {
            Contract.Requires(sha1 != null);
            Contract.Requires(sha1.ByteLength == SHA1Length);
            Contract.Requires(sha256 != null);
            Contract.Requires(sha256.ByteLength == SHA256Length);

            SHA1Password = sha1;
            SHA256Password = sha256;
        }

        /// <summary>
        /// Generates a hash for an account's credentials (username:password).
        /// </summary>
        /// <param name="algo">The hash algorithm to be used.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="caseSensitive">Whether or not username/password should
        /// be case-sensitive.</param>
        public static byte[] GenerateCredentialsHash(HashAlgorithm algo, string username, string password,
            bool caseSensitive = true)
        {
            Contract.Requires(!string.IsNullOrEmpty(username));
            Contract.Requires(!string.IsNullOrEmpty(password));
            Contract.Ensures(Contract.Result<byte[]>() != null);

            var user = caseSensitive ? username : username.ToUpper();
            var pass = caseSensitive ? password : password.ToUpper();
            var str = string.Format("{0}:{1}", user, pass);

            return algo.ComputeHash(Encoding.UTF8.GetBytes(str));
        }
    }
}
