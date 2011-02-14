using System;
using System.Diagnostics.Contracts;
using ICSharpCode.SharpZipLib.Zip.Compression;

namespace Trinity.Core.IO.Compression
{
    public static class ZLibCompressor
    {
        public static byte[] Compress(byte[] input, ZLibCompressionLevel level, bool headerAndFooter = true)
        {
            Contract.Requires(input != null);
            Contract.Ensures(Contract.Result<byte[]>() != null);

            var deflater = new Deflater((int)level, !headerAndFooter);
            var length = input.Length;
            var output = new byte[length];

            deflater.SetInput(input, 0, length);
            var compressedLength = deflater.Deflate(output);

            var realOutput = new byte[compressedLength];
            Buffer.BlockCopy(output, 0, realOutput, 0, compressedLength);
            return realOutput;
        }
    }
}
