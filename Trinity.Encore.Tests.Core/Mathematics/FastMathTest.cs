using Microsoft.VisualStudio.TestTools.UnitTesting;
using Trinity.Encore.Framework.Core.Mathematics;

namespace Trinity.Encore.Tests.Core.Mathematics
{
    [TestClass]
    public sealed class FastMathTest
    {
        [TestMethod]
        public void TestByteMinMax()
        {
            var value1 = FastMath.MinMax((byte)10, (byte)5, (byte)15);
            var value2 = FastMath.MinMax((byte)3, (byte)5, (byte)15);
            var value3 = FastMath.MinMax((byte)17, (byte)5, (byte)15);

            Assert.AreEqual(10, value1);
            Assert.AreEqual(5, value2);
            Assert.AreEqual(15, value3);
        }

        [TestMethod]
        public void TestSByteMinMax()
        {
            var value1 = FastMath.MinMax((sbyte)10, (sbyte)5, (sbyte)15);
            var value2 = FastMath.MinMax((sbyte)3, (sbyte)5, (sbyte)15);
            var value3 = FastMath.MinMax((sbyte)17, (sbyte)5, (sbyte)15);

            Assert.AreEqual(10, value1);
            Assert.AreEqual(5, value2);
            Assert.AreEqual(15, value3);
        }

        [TestMethod]
        public void TestInt16MinMax()
        {
            var value1 = FastMath.MinMax((short)10, (short)5, (short)15);
            var value2 = FastMath.MinMax((short)3, (short)5, (short)15);
            var value3 = FastMath.MinMax((short)17, (short)5, (short)15);

            Assert.AreEqual(10, value1);
            Assert.AreEqual(5, value2);
            Assert.AreEqual(15, value3);
        }

        [TestMethod]
        public void TestUInt16MinMax()
        {
            var value1 = FastMath.MinMax((ushort)10, (ushort)5, (ushort)15);
            var value2 = FastMath.MinMax((ushort)3, (ushort)5, (ushort)15);
            var value3 = FastMath.MinMax((ushort)17, (ushort)5, (ushort)15);

            Assert.AreEqual(10, value1);
            Assert.AreEqual(5, value2);
            Assert.AreEqual(15, value3);
        }

        [TestMethod]
        public void TestInt32MinMax()
        {
            var value1 = FastMath.MinMax(10, 5, 15);
            var value2 = FastMath.MinMax(3, 5, 15);
            var value3 = FastMath.MinMax(17, 5, 15);

            Assert.AreEqual(10, value1);
            Assert.AreEqual(5, value2);
            Assert.AreEqual(15, value3);
        }

        [TestMethod]
        public void TestUInt32MinMax()
        {
            var value1 = FastMath.MinMax((uint)10, 5, 15);
            var value2 = FastMath.MinMax((uint)3, 5, 15);
            var value3 = FastMath.MinMax((uint)17, 5, 15);

            Assert.AreEqual((uint)10, value1);
            Assert.AreEqual((uint)5, value2);
            Assert.AreEqual((uint)15, value3);
        }

        [TestMethod]
        public void TestInt64MinMax()
        {
            var value1 = FastMath.MinMax((long)10, 5, 15);
            var value2 = FastMath.MinMax((long)3, 5, 15);
            var value3 = FastMath.MinMax((long)17, 5, 15);

            Assert.AreEqual(10, value1);
            Assert.AreEqual(5, value2);
            Assert.AreEqual(15, value3);
        }

        [TestMethod]
        public void TestUInt64MinMax()
        {
            var value1 = FastMath.MinMax((ulong)10, 5, 15);
            var value2 = FastMath.MinMax((ulong)3, 5, 15);
            var value3 = FastMath.MinMax((ulong)17, 5, 15);

            Assert.AreEqual((ulong)10, value1);
            Assert.AreEqual((ulong)5, value2);
            Assert.AreEqual((ulong)15, value3);
        }

        [TestMethod]
        public void TestSingleMinMax()
        {
            var value1 = FastMath.MinMax(10.0f, 5.0f, 15.0f);
            var value2 = FastMath.MinMax(3.0f, 5.0f, 15.0f);
            var value3 = FastMath.MinMax(17.0f, 5.0f, 15.0f);

            Assert.AreEqual(10.0f, value1);
            Assert.AreEqual(5.0f, value2);
            Assert.AreEqual(15.0f, value3);
        }

        [TestMethod]
        public void TestDoubleMinMax()
        {
            var value1 = FastMath.MinMax(10.0d, 5.0d, 15.0d);
            var value2 = FastMath.MinMax(3.0d, 5.0d, 15.0d);
            var value3 = FastMath.MinMax(17.0d, 5.0d, 15.0d);

            Assert.AreEqual(10.0d, value1);
            Assert.AreEqual(5.0d, value2);
            Assert.AreEqual(15.0d, value3);
        }

        [TestMethod]
        public void TestDecimalMinMax()
        {
            var value1 = FastMath.MinMax(10.0m, 5.0m, 15.0m);
            var value2 = FastMath.MinMax(3.0m, 5.0m, 15.0m);
            var value3 = FastMath.MinMax(17.0m, 5.0m, 15.0m);

            Assert.AreEqual(10.0m, value1);
            Assert.AreEqual(5.0m, value2);
            Assert.AreEqual(15.0m, value3);
        }
    }
}
