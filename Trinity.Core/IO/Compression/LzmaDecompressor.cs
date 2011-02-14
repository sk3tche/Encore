using System.Diagnostics.Contracts;
using System.IO;
using SevenZip.Compression.LZMA;

namespace Trinity.Core.IO.Compression
{
    public static class LzmaDecompressor
    {
        public static byte[] Decompress(byte[] input, int uncompressedLength)
        {
            Contract.Requires(input != null);
            Contract.Requires(uncompressedLength >= 0);
            Contract.Ensures(Contract.Result<byte[]>() != null);
            Contract.Ensures(Contract.Result<byte[]>().Length == uncompressedLength);

            var decoder = new Decoder();
            var result = decoder.Code(input, uncompressedLength, uncompressedLength);
            Contract.Assume(result.Length == uncompressedLength);
            return result;
        }
    }
}
