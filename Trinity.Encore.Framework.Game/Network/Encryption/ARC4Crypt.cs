using System.Diagnostics.Contracts;
using System.Security.Cryptography;
using Mono.Security.Cryptography;
using Trinity.Encore.Framework.Network.Encryption;

namespace Trinity.Encore.Framework.Game.Network.Encryption
{
    public sealed class ARC4Crypt : IPacketCrypt
    {
        #region Encryption keys

        private static readonly byte[] _serverEncClientDec =
            {
                0xCC, 0x98, 0xAE, 0x04, 0xE8, 0x97, 0xEA, 0xCA,
                0x12, 0xDD, 0xC0, 0x93, 0x42, 0x91, 0x53, 0x57
            };

        private static readonly byte[] _serverDecClientEnc =
            {
                0xC2, 0xB3, 0x72, 0x3C, 0xC6, 0xAE, 0xD9, 0xB5,
                0x34, 0x3C, 0x53, 0xEE, 0x2F, 0x43, 0x67, 0xCE
            };

        #endregion

        /// <summary>
        /// The amount of bytes to drop from the encryption/decryption streams on creation.
        /// </summary>
        public const int DropN = 1024;

        /// <summary>
        /// Used to encrypt packets sent from the server to the client.
        /// </summary>
        public static byte[] ServerToClientKey
        {
            get { return _serverEncClientDec; }
        }

        /// <summary>
        /// Used to encrypt packets sent from the client to the server.
        /// </summary>
        public static byte[] ClientToServerKey
        {
            get { return _serverDecClientEnc; }
        }

        private readonly ARC4Managed _encrypt;

        private readonly ARC4Managed _decrypt;

        [ContractInvariantMethod]
        private void Invariant()
        {
            Contract.Invariant(_encrypt != null);
            Contract.Invariant(_decrypt != null);
        }

        public ARC4Crypt(HashAlgorithm hash, bool isServer)
        {
            Contract.Requires(hash != null);

            var encKey = isServer ? _serverEncClientDec : _serverDecClientEnc;
            var decKey = isServer ? _serverDecClientEnc : _serverEncClientDec;

            _encrypt = new ARC4Managed();
            _decrypt = new ARC4Managed();

            _encrypt.Key = hash.ComputeHash(encKey);
            _decrypt.Key = hash.ComputeHash(decKey);

            var buffer = new byte[DropN];
            var length = buffer.Length;

            // Drop the first N bytes in the stream, to prevent the FMS attack.
            _encrypt.TransformFinalBlock(buffer, 0, length);
            _decrypt.TransformFinalBlock(buffer, 0, length);
        }

        public int Encrypt(byte[] buffer, int start, int count)
        {
            // Use TransformBlock instead of TransformFinalBlock to avoid too many array allocations.
            return _encrypt.TransformBlock(buffer, start, count, buffer, start);
        }

        public int Decrypt(byte[] buffer, int start, int count)
        {
            // Use TransformBlock instead of TransformFinalBlock to avoid too many array allocations.
            return _decrypt.TransformBlock(buffer, start, count, buffer, start);
        }
    }
}
