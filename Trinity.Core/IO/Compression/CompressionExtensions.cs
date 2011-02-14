using System.Diagnostics.Contracts;
using System.IO;
using SevenZip;

namespace Trinity.Core.IO.Compression
{
    public static class CompressionExtensions
    {
        public static byte[] Code(this ICoder coder, byte[] input, int outputStreamLength, int minimumOutputLength = -1)
        {
            Contract.Requires(coder != null);
            Contract.Requires(input != null);
            Contract.Requires(outputStreamLength >= 0);
            Contract.Requires(minimumOutputLength >= -1);
            Contract.Ensures(Contract.Result<byte[]>() != null);

            using (var inStream = new MemoryStream(input))
            {
                using (var outStream = new MemoryStream(outputStreamLength))
                {
                    coder.Code(inStream, outStream, input.Length, minimumOutputLength, null);

                    var arr = outStream.ToArray();
                    Contract.Assume(arr != null);
                    return arr;
                }
            }
        }
    }
}
