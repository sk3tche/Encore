using Microsoft.VisualStudio.TestTools.UnitTesting;
using Trinity.Core.Checksums;

namespace Trinity.Encore.Tests.Core.Checksums
{
    [TestClass]
    public abstract class ChecksumTest
    {
        protected ChecksumTest(IChecksum provider)
        {
            _provider = provider;
        }

        private readonly IChecksum _provider;

        protected readonly byte[] Bytes1 =
            {
                0x0, 0x1, 0x2, 0x3, 0x4,
                0x5, 0x6, 0x7, 0x8, 0x9,
            };

        protected readonly byte[] Bytes2 =
            {
                0x9, 0x8, 0x7, 0x6, 0x5,
                0x4, 0x3, 0x2, 0x1, 0x0,
            };

        [TestMethod]
        public void TestCalculate()
        {
            var value1 = _provider.Calculate(Bytes1);
            var value2 = _provider.Calculate(Bytes2);
            var value3 = _provider.Calculate(Bytes1);

            Assert.AreNotEqual(value1, value2);
            Assert.AreEqual(value1, value3);
        }

        [TestMethod]
        public void TestMatches()
        {
            var matches1 = _provider.Matches(Bytes1, Bytes2);
            var matches2 = _provider.Matches(Bytes1, Bytes1);
            var matches3 = _provider.Matches(Bytes2, Bytes2);

            Assert.IsFalse(matches1);
            Assert.IsTrue(matches2);
            Assert.IsTrue(matches3);
        }
    }
}
