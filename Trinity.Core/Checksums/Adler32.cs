using System.Diagnostics.Contracts;

namespace Trinity.Core.Checksums
{
    /// <summary>
    /// Implements the Adler 32 checksum algorithm.
    /// </summary>
    public sealed class Adler32 : IChecksum
    {
        private readonly ICSharpCode.SharpZipLib.Checksums.Adler32 _adler = new ICSharpCode.SharpZipLib.Checksums.Adler32();

        [ContractInvariantMethod]
        private void Invariant()
        {
            Contract.Invariant(_adler != null);
        }

        /// <summary>
        /// Calculates the Adler 32 checksum of a given input.
        /// </summary>
        /// <param name="input">The input to calculate a checksum for.</param>
        /// <returns>The checksum for the given input.</returns>
        public long Calculate(byte[] input)
        {
            _adler.Reset();
            _adler.Update(input);

            return _adler.Value;
        }

        /// <summary>
        /// Checks if two inputs have matching Adler 32 checksums.
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
