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

        public long Calculate(byte[] input)
        {
            _crc.Reset();
            _crc.Update(input);

            return _crc.Value;
        }

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
