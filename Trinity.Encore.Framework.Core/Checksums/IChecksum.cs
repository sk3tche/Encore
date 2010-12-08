using System.Diagnostics.Contracts;

namespace Trinity.Encore.Framework.Core.Checksums
{
    /// <summary>
    /// Represents a checksum algorithm.
    /// </summary>
    [ContractClass(typeof(ChecksumContracts))]
    public interface IChecksum
    {
        /// <summary>
        /// Calculates the checksum of a given input.
        /// </summary>
        /// <param name="input">The input to calculate a checksum for.</param>
        /// <returns>The checksum for the given input.</returns>
        long Calculate(byte[] input);

        /// <summary>
        /// Checks if two inputs have matching checksums.
        /// </summary>
        /// <param name="input1">The first input.</param>
        /// <param name="input2">The second input.</param>
        /// <returns>Whether or not the two inputs have matching checksums.</returns>
        bool Matches(byte[] input1, byte[] input2);
    }

    [ContractClassFor(typeof(IChecksum))]
    public abstract class ChecksumContracts : IChecksum
    {
        public long Calculate(byte[] input)
        {
            Contract.Requires(input != null);

            return 0;
        }

        public bool Matches(byte[] input1, byte[] input2)
        {
            Contract.Requires(input1 != null);
            Contract.Requires(input2 != null);

            return false;
        }
    }
}
