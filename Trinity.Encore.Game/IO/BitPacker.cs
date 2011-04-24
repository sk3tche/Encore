using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.IO;

namespace Trinity.Encore.Game.IO
{
    [SuppressMessage("Microsoft.Performance", "CA1815", Justification = "This type is not equatable.")]
    public struct BitPacker
    {
        // This type, along with BitUnpacker, is a struct for performance reasons; it will only ever live on
        // the stack, and will be created often. So, we avoid GC overhead by making it have value semantics.

        private readonly BinaryWriter _writer;

        public byte Bits;

        public int Position;

        [ContractInvariantMethod]
        private void Invariant()
        {
            Contract.Invariant(_writer != null);
        }

        public BitPacker(BinaryWriter writer)
        {
            Contract.Requires(writer != null);

            _writer = writer;
            Bits = 0;
            Position = 0;
        }

        private void FlushBuffer(int shiftNum, int newPos = 0)
        {
            _writer.Write(Bits << shiftNum);
            Bits = 0;
            Position = (byte)newPos;
        }

        public void WriteBits()
        {
            var shiftNum = (7 - ((Position & 0x7) - 1));
            var num = 8 - Position;
            var newPos = shiftNum - num;

            if (Position == 7 - (shiftNum - 1))
            {
                FlushBuffer(shiftNum);
                return;
            }

            if (newPos >= 0)
            {
                FlushBuffer(num, newPos);
                return;
            }

            Bits <<= shiftNum;
            Position += (byte)shiftNum;
        }
    }
}
