using System.Diagnostics.Contracts;
using System.IO;
using ICSharpCode.SharpZipLib.BZip2;

namespace Trinity.Core.IO.Compression
{
    public static class BZip2Decompressor
    {
        public static byte[] Decompress(byte[] input, int uncompressedLength)
        {
            Contract.Requires(input != null);
            Contract.Requires(uncompressedLength >= 0);
            Contract.Ensures(Contract.Result<byte[]>() != null);
            Contract.Ensures(Contract.Result<byte[]>().Length == uncompressedLength);

            var output = new byte[uncompressedLength];

            using (var stream = new BZip2InputStream(new MemoryStream(input, false)))
                stream.Read(output, 0, uncompressedLength);

            return output;
        }
    }
}
