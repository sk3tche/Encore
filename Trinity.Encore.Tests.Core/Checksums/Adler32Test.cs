using Microsoft.VisualStudio.TestTools.UnitTesting;
using Trinity.Encore.Framework.Core.Checksums;

namespace Trinity.Encore.Tests.Core.Checksums
{
    [TestClass]
    public sealed class Adler32Test : ChecksumTest
    {
        public Adler32Test()
            : base(new Adler32())
        {
        }
    }
}
