using Microsoft.VisualStudio.TestTools.UnitTesting;
using Trinity.Core.Mathematics;

namespace Trinity.Encore.Tests.Core.Mathematics
{
    [TestClass]
    public sealed class MathExtensionsTest
    {
        [TestMethod]
        public void TestSingleRound()
        {
            var value1 = 10.4f.Round();
            var value2 = 10.5f.Round();
            var value3 = 10.6f.Round();

            Assert.AreEqual(10.0f, value1);
            Assert.AreEqual(11.0f, value2);
            Assert.AreEqual(11.0f, value3);
        }

        [TestMethod]
        public void TestDoubleRound()
        {
            var value1 = 10.4d.Round();
            var value2 = 10.5d.Round();
            var value3 = 10.6d.Round();

            Assert.AreEqual(10.0d, value1);
            Assert.AreEqual(11.0d, value2);
            Assert.AreEqual(11.0d, value3);
        }

        [TestMethod]
        public void TestDecimalRound()
        {
            var value1 = 10.4m.Round();
            var value2 = 10.5m.Round();
            var value3 = 10.6m.Round();

            Assert.AreEqual(10.0m, value1);
            Assert.AreEqual(11.0m, value2);
            Assert.AreEqual(11.0m, value3);
        }
    }
}
