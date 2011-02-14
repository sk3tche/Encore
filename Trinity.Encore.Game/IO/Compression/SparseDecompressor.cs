using System.Diagnostics.Contracts;
using System.IO;
using Trinity.Core.IO;

namespace Trinity.Encore.Game.IO.Compression
{
    // Based on code by Ladislav Zezula and ShadowFlare.
    public static class SparseDecompressor
    {
        public const int CompressionThreshold = 5;

        public static byte[] Decompress(BinaryReader reader, int length)
        {
            Contract.Requires(reader != null);
            Contract.Requires(length >= 0);
            Contract.Ensures(Contract.Result<byte[]>() != null);

            // Don't decompress anything smaller than this.
            if (length < CompressionThreshold)
                return reader.ReadBytes(length);

            var endPos = (int)reader.BaseStream.Position + length;

            var stream = new MemoryStream();
            using (var writer = new BinaryWriter(stream))
            {
                var outputLength = reader.ReadInt32BigEndian();

                if (outputLength < 0)
                    throw new InvalidDataException("Negative output length encountered.");

                writer.Write(outputLength);

                while (reader.BaseStream.Position < endPos)
                {
                    var b = reader.ReadByte();
                    var normalData = (b & 0x80) != 0;

                    var chunkSize = b & 0x7f + (normalData ? 1 : 3);
                    chunkSize = (chunkSize < outputLength) ? chunkSize : outputLength;

                    if (chunkSize < 0)
                        throw new InvalidDataException("Negative length encountered.");

                    var data = normalData ? reader.ReadBytes(chunkSize) : new byte[chunkSize] /* Zero bytes. */;

                    writer.Write(data);
                }

                var arr = stream.ToArray();
                Contract.Assume(arr != null);
                return arr;
            }
        }
    }
}
