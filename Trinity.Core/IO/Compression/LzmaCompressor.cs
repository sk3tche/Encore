using System.Diagnostics.Contracts;
using System.IO;
using SevenZip.Compression.LZMA;

namespace Trinity.Core.IO.Compression
{
    public static class LzmaCompressor
    {
        public static byte[] Compress(byte[] input)
        {
            Contract.Requires(input != null);
            Contract.Ensures(Contract.Result<byte[]>() != null);

            var encoder = new Encoder();
            return encoder.Code(input, input.Length);
        }
    }
}
