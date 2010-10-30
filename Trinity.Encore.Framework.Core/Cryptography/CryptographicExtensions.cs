using System.Diagnostics.Contracts;
using System.IO;
using System.Security.Cryptography;

namespace Trinity.Encore.Framework.Core.Cryptography
{
    public static class CryptographicExtensions
    {
        /// <summary>
        /// Computes a hash from hash data brokers using the given
        /// hashing algorithm.
        /// </summary>
        /// <param name="algorithm">The hashing algorithm to use.</param>
        /// <param name="brokers">The data brokers to hash.</param>
        public static BigInteger FinalizeHash(this HashAlgorithm algorithm, params HashDataBroker[] brokers)
        {
            Contract.Requires(algorithm != null);
            Contract.Requires(brokers != null);
            Contract.Requires(brokers.Length >= 0);
            Contract.Ensures(Contract.Result<BigInteger>() != null);
            Contract.Ensures(Contract.Result<BigInteger>().DataLength == algorithm.HashSize);
            Contract.Ensures(Contract.Result<BigInteger>().ByteLength == algorithm.HashSize / 8);

            using (var buffer = new MemoryStream())
            {
                foreach (var broker in brokers)
                    buffer.Write(broker.RawData, 0, broker.Length);

                buffer.Position = 0;

                var result = new BigInteger(algorithm.ComputeHash(buffer));
                Contract.Assume(result.DataLength == algorithm.HashSize);
                Contract.Assume(result.ByteLength == algorithm.HashSize / 8);
                return result;
            }
        }
    }
}
