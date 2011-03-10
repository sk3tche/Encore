using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using Trinity.Core;

namespace Trinity.Encore.Game.IO.Compression
{
    // Based on code by Ladislav Zezula and Foole.
    // TODO: Figure out if this is reusable enough to be moved to Trinity.Core.
    public static class PkLibDecompressor
    {
        private static readonly byte[] _sLenBits =
        {
            3, 2, 3, 3, 4, 4, 4, 5,
            5, 5, 5, 6, 6, 6, 7, 7,
        };

        private static readonly byte[] _sLenCode =
        {
            5, 3, 1, 6, 10, 2, 12, 20,
            4, 24, 8, 48, 16, 32, 64, 0,
        };

        private static readonly byte[] _sExLenBits = 
        {
            0, 0, 0, 0, 0, 0, 0, 0,
            1, 2, 3, 4, 5, 6, 7, 8,
        };

        private static readonly ushort[] _sLenBase =
        {
            0x0000, 0x0001, 0x0002, 0x0003, 0x0004, 0x0005, 0x0006, 0x0007,
            0x0008, 0x000a, 0x000e, 0x0016, 0x0026, 0x0046, 0x0086, 0x0106,
        };

        private static readonly byte[] _sDistBits =
        {
            2, 4, 4, 5, 5, 5, 5, 6, 6, 6, 6, 6, 6, 6, 6, 6,
            6, 6, 6, 6, 6, 6, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
            7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
            8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8,
        };

        private static readonly byte[] _sDistCode =
        {
            0x03, 0x0d, 0x05, 0x19, 0x09, 0x11, 0x01, 0x3e, 0x1e, 0x2e, 0x0e, 0x36, 0x16, 0x26, 0x06, 0x3a,
            0x1a, 0x2a, 0x0a, 0x32, 0x12, 0x22, 0x42, 0x02, 0x7c, 0x3c, 0x5c, 0x1c, 0x6c, 0x2c, 0x4c, 0x0c,
            0x74, 0x34, 0x54, 0x14, 0x64, 0x24, 0x44, 0x04, 0x78, 0x38, 0x58, 0x18, 0x68, 0x28, 0x48, 0x08,
            0xF0, 0x70, 0xb0, 0x30, 0xd0, 0x50, 0x90, 0x10, 0xe0, 0x60, 0xa0, 0x20, 0xc0, 0x40, 0x80, 0x00,
        };

        private static readonly byte[] _sPosition1 = new Func<byte[]>(() =>
        {
            Contract.Assume(_sDistBits.Length == _sDistCode.Length);
            return GenerateDecodeTable(_sDistBits, _sDistCode);
        })();

        private static readonly byte[] _sPosition2 = new Func<byte[]>(() =>
        {
            Contract.Assume(_sLenBits.Length == _sLenCode.Length);
            return GenerateDecodeTable(_sLenBits, _sLenCode);
        })();

        public static byte[] Decompress(BinaryReader input, int expectedSize)
        {
            Contract.Requires(input != null);
            Contract.Requires(expectedSize >= 0);
            Contract.Ensures(Contract.Result<byte[]>() != null);

            var bitStream = new BitStreamReader(input);

            var compressionType = (PkLibCompressionType)input.ReadByte();

            if (compressionType != PkLibCompressionType.Binary && compressionType != PkLibCompressionType.ASCII)
                throw new InvalidDataException("Invalid compression type: {0}".Interpolate(compressionType));

            var dictSizeBits = input.ReadByte();

            if (dictSizeBits < 4 || dictSizeBits > 6)
                throw new InvalidDataException("Invalid dictionary size: {0}".Interpolate(dictSizeBits));

            var outputBuffer = new byte[expectedSize];
            using (var outputStream = new MemoryStream(outputBuffer))
            {
                int instruction;
                while ((instruction = DecodeLiteral(bitStream, compressionType)) != -1)
                {
                    if (instruction >= 0x100)
                    {
                        // If instruction is greater than 0x100, it means "repeat n - 0xfe bytes".
                        var copyLength = instruction - 0xfe;
                        var moveBack = DecodeDistance(bitStream, copyLength, dictSizeBits);

                        if (moveBack == 0)
                            break;

                        var source = (int)outputStream.Position - moveBack;

                        while (copyLength-- > 0)
                            outputStream.WriteByte(outputBuffer[source++]);
                    }
                    else
                        outputStream.WriteByte((byte)instruction);
                }

                if (outputStream.Position == expectedSize)
                    return outputBuffer;

                return outputStream.ToArray();
            }
        }

        private static int DecodeLiteral(BitStreamReader bitStream, PkLibCompressionType compressionType)
        {
            Contract.Requires(bitStream != null);

            // Return values:
            // 0x000 to 0x0ff: One byte from compressed file.
            // 0x100 to 0x305: Copy previous block (0x100 = 1 byte).
            // -1: End of stream.
            switch (bitStream.ReadBits(1))
            {
                case -1:
                    return -1;
                case 1:
                    // The next bits are positions in buffers.
                    int pos = _sPosition2[bitStream.PeekByte()];

                    // Skip the bits we just used.
                    var numBits = _sLenBits[pos];
                    Contract.Assume(numBits < BitStreamReader.MaxBitCount);
                    if (bitStream.ReadBits(numBits) == -1)
                        return -1;
    
                    var nBits = _sExLenBits[pos];
                    if (nBits != 0)
                    {
                        Contract.Assume(nBits < BitStreamReader.MaxBitCount);
                        var val2 = bitStream.ReadBits(nBits);
                        if (val2 == -1 && (pos + val2 != 0x10e))
                            return -1;

                        pos = _sLenBase[pos] + val2;
                    }

                    return pos + 0x100; // Return number of bytes to repeat.
                case 0:
                    if (compressionType == PkLibCompressionType.Binary)
                        return bitStream.ReadBits(sizeof(byte) * 8);

                    // TODO: Implement ASCII mode.
                    throw new NotImplementedException("ASCII mode is not yet implemented.");
                default:
                    return 0;
            }
        }

        private static int DecodeDistance(BitStreamReader bitStream, int length, int dictSizeBits)
        {
            Contract.Requires(bitStream != null);
            Contract.Requires(length >= 0);
            Contract.Requires(dictSizeBits >= 0);
            Contract.Requires(dictSizeBits < BitStreamReader.MaxBitCount);

            if (bitStream.EnsureBits(8) == false)
                return 0;

            var pos = (int)_sPosition1[bitStream.PeekByte()];
            var skip = _sDistBits[pos]; // Number of bits to skip.

            // Skip the appropriate number of bits
            Contract.Assume(skip < BitStreamReader.MaxBitCount);
            if (bitStream.ReadBits(skip) == -1)
                return 0;

            if (length == 2)
            {
                if (bitStream.EnsureBits(2) == false)
                    return 0;

                pos = (pos << 2) | bitStream.ReadBits(2);
            }
            else
            {
                if (bitStream.EnsureBits(dictSizeBits) == false)
                    return 0;

                pos = ((pos << dictSizeBits)) | bitStream.ReadBits(dictSizeBits);
            }

            return pos + 1;
        }

        private static byte[] GenerateDecodeTable(IList<byte> bits, IList<byte> codes)
        {
            Contract.Requires(bits != null);
            Contract.Requires(codes != null);
            Contract.Requires(bits.Count == codes.Count);
            Contract.Ensures(Contract.Result<byte[]>() != null);

            var result = new byte[byte.MaxValue];

            for (var i = bits.Count - 1; i >= 0; i--)
            {
                uint idx1 = codes[i];
                var idx2 = (uint)1 << bits[i];

                do
                {
                    result[idx1] = (byte)i;
                    idx1 += idx2;
                } while (idx1 < 0x100);
            }

            return result;
        }
    }
}
