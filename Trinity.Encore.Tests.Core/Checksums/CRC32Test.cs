using Microsoft.VisualStudio.TestTools.UnitTesting;
using Trinity.Core.Checksums;

namespace Trinity.Encore.Tests.Core.Checksums
{
    [TestClass]
    public sealed class CRC32Test : ChecksumTest
    {
        public CRC32Test()
            : base(new CRC32())
        {
        }
    }
}
