using System.Diagnostics.Contracts;
using ICSharpCode.SharpZipLib.Zip.Compression;

namespace Trinity.Core.IO.Compression
{
    public static class ZLibDecompressor
    {
        public static byte[] Decompress(byte[] input, int uncompressedLength, bool header = true)
        {
            Contract.Requires(input != null);
            Contract.Requires(uncompressedLength >= 0);
            Contract.Ensures(Contract.Result<byte[]>() != null);
            Contract.Ensures(Contract.Result<byte[]>().Length == uncompressedLength);

            var inflater = new Inflater(!header);
            var output = new byte[uncompressedLength];

            inflater.SetInput(input, 0, input.Length);
            inflater.Inflate(output);

            return output;
        }
    }
}
