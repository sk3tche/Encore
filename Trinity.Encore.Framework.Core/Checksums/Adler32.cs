using System.Diagnostics.Contracts;

namespace Trinity.Encore.Framework.Core.Checksums
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

        public long Calculate(byte[] input)
        {
            _adler.Reset();
            _adler.Update(input);

            return _adler.Value;
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
