using System.Diagnostics.Contracts;
using ICSharpCode.SharpZipLib.Checksums;

namespace Trinity.Core.Checksums
{
    /// <summary>
    /// Implements the CRC32-IEEE 802.3 checksum algorithm.
    /// </summary>
    public sealed class CRC32 : IChecksum
    {
        private readonly Crc32 _crc = new Crc32();

        [ContractInvariantMethod]
        private void Invariant()
        {
            Contract.Invariant(_crc != null);
        }

        /// <summary>
        /// Calculates the CRC32-IEEE 802.3 checksum of a given input.
        /// </summary>
        /// <param name="input">The input to calculate a checksum for.</param>
        /// <returns>The checksum for the given input.</returns>
        public long Calculate(byte[] input)
        {
            _crc.Reset();
            _crc.Update(input);

            return _crc.Value;
        }

        /// <summary>
        /// Checks if two inputs have matching CRC32-IEEE 802.3 checksums.
        /// </summary>
        /// <param name="input1">The first input.</param>
        /// <param name="input2">The second input.</param>
        /// <returns>Whether or not the two inputs have matching checksums.</returns>
        public bool Matches(byte[] input1, byte[] input2)
        {
            // Let's optimize it a bit.
            if (input1.Length != input2.Length)
                return false;

            var v1 = Calculate(input1);
            var v2 = Calculate(input2);

            return v1 == v2;
        }
    }
}
