using System.Diagnostics.Contracts;
using System.IO;
using ICSharpCode.SharpZipLib.BZip2;

namespace Trinity.Core.IO.Compression
{
    public static class BZip2Compressor
    {
        public static byte[] Compress(byte[] input)
        {
            Contract.Requires(input != null);
            Contract.Ensures(Contract.Result<byte[]>() != null);

            var length = input.Length;
            var ms = new MemoryStream(length);

            using (var stream = new BZip2OutputStream(ms))
                stream.Write(input, 0, length);

            return ms.ToArray();
        }
    }
}
