using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Trinity.Encore.Game.IO
{
    public sealed class PatternScanner
    {
        private readonly byte[] _data;

        [ContractInvariantMethod]
        private void Invariant()
        {
            Contract.Invariant(_data != null);
        }

        public PatternScanner(byte[] data)
        {
            Contract.Requires(data != null);

            _data = data;
        }

        public int? Find(byte?[] pattern)
        {
            Contract.Requires(pattern != null);

            for (var i = 0; i < _data.Length; i++)
                if (CompareSequences(pattern, i))
                    return i;

            return null;
        }

        /// <summary>
        /// Compares a byte sequence with a sequence within memory, tolerating mismatches
        /// if an entry in the given byte sequence is null.
        /// </summary>
        /// <param name="seq">The sequence to compare with memory.</param>
        /// <param name="offset">The offset in memory to start comparing at.</param>
        private bool CompareSequences(IEnumerable<byte?> seq, int offset)
        {
            Contract.Requires(seq != null);

            return !seq.Where((b, index) => b != null && _data[offset + index] != b).Any();
        }
    }
}
