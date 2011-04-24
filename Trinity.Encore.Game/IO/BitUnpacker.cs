using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.IO;

namespace Trinity.Encore.Game.IO
{
    [SuppressMessage("Microsoft.Performance", "CA1815", Justification = "This type is not equatable.")]
    public struct BitUnpacker
    {
        private readonly BinaryReader _reader;

        private byte _currentValue;

        private int _position;

        [ContractInvariantMethod]
        private void Invariant()
        {
            Contract.Invariant(_reader != null);
        }

        public BitUnpacker(BinaryReader reader, byte? value = null, byte? position = null)
        {
            Contract.Requires(reader != null);

            _reader = reader;
            _currentValue = value ?? 0;
            _position = position ?? 0;

            if (value == null)
                _currentValue = reader.ReadByte();
        }

        [SuppressMessage("Microsoft.Design", "CA1024", Justification = "This method performs too much work to be a property.")]
        public byte GetNextBit()
        {
            if (_position == 8) // If we've read one byte, get a new one.
            {
                _currentValue = _reader.ReadByte();
                _position = 0;
            }

            var bit = _currentValue;
            _currentValue <<= 1;
            _position++;

            return (byte)(bit >> 7);
        }
    }
}
