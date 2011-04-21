using System.Diagnostics.Contracts;
using System.IO;
using Trinity.Core.IO;

namespace Trinity.Encore.Game.IO
{
    public sealed class BitStreamReader
    {
        public const int MaxBitCount = 16;

        private readonly BinaryReader _reader;

        private int _current;

        private int _bitCount;

        [ContractInvariantMethod]
        private void Invariant()
        {
            Contract.Invariant(_reader != null);
        }

        public BitStreamReader(BinaryReader reader)
        {
            Contract.Requires(reader != null);

            _reader = reader;
        }

        public int ReadBits(int bitCount)
        {
            Contract.Requires(bitCount >= 0);
            Contract.Requires(bitCount < MaxBitCount);

            if (!EnsureBits(bitCount))
                throw new EndOfStreamException("Not enough bits available.");

            var result = _current & (ushort.MaxValue >> (MaxBitCount - bitCount));
            WasteBits(bitCount);

            return result;
        }

        public int PeekByte()
        {
            if (!EnsureBits(sizeof(byte) * 8))
                return -1;

            return _current & 0xff;
        }

        public bool EnsureBits(int bitCount)
        {
            Contract.Requires(bitCount >= 0);
            Contract.Requires(bitCount < MaxBitCount);

            if (bitCount <= _bitCount)
                return true;

            if (_reader.BaseStream.IsRead())
                return false;

            var nextValue = _reader.ReadByte();

            _current |= nextValue << _bitCount;
            _bitCount += sizeof(byte) * 8;

            return true;
        }

        private void WasteBits(int bitCount)
        {
            Contract.Requires(bitCount >= 0);
            Contract.Requires(bitCount < MaxBitCount);

            _current >>= bitCount;
            _bitCount -= bitCount;
        }
    }
}
