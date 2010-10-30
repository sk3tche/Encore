using System.Diagnostics.Contracts;

namespace Trinity.Encore.Framework.Core.Checksums
{
    [ContractClass(typeof(ChecksumContracts))]
    public interface IChecksum
    {
        long Calculate(byte[] input);

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
