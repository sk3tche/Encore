using Microsoft.VisualStudio.TestTools.UnitTesting;
using Trinity.Encore.Framework.Core.Mathematics;

namespace Trinity.Encore.Tests.Core.Mathematics
{
    [TestClass]
    public sealed class FastRandomTest
    {
        private FastRandom _rng;

        [TestInitialize]
        public void Initialize()
        {
            _rng = new FastRandom(0xBADC0DE); // Well, we can't use Environment.TickCount, so...
        }

        [TestMethod]
        public void TestNextMax()
        {
            var value = _rng.Next(1000);

            Assert.IsTrue(value < 1000);
        }

        [TestMethod]
        public void TestNextMinMax()
        {
            var value = _rng.Next(500, 1000);

            Assert.IsTrue(value >= 500);
            Assert.IsTrue(value < 1000);
        }
    }
}
