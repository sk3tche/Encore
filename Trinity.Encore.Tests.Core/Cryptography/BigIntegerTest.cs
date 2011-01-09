using Microsoft.VisualStudio.TestTools.UnitTesting;
using Trinity.Encore.Framework.Core.Cryptography;

namespace Trinity.Encore.Tests.Core.Cryptography
{
    [TestClass]
    public sealed class BigIntegerTest
    {
        [TestMethod]
        public void TestAbs()
        {
            var value = new BigInteger(-50);
            var absolute = value.Abs();

            Assert.AreEqual(new BigInteger(50), absolute);
        }

        [TestMethod]
        public void TestModPow()
        {
            var value = new BigInteger(50);
            var result = value.ModPow(new BigInteger(2), new BigInteger(3));

            Assert.AreEqual(new BigInteger(1), result);
        }

        [TestMethod]
        public void TestGreatestCommonDivisor()
        {
            var value1 = new BigInteger(10);
            var value2 = new BigInteger(20);
            var gcd = value1.GreatestCommonDivisor(value2);

            Assert.AreEqual(new BigInteger(10), gcd);
        }

        [TestMethod]
        public void TestMin()
        {
            var value1 = new BigInteger(10);
            var value2 = new BigInteger(20);
            var min = value1.Min(value2);

            Assert.AreEqual(value1, min);
        }

        [TestMethod]
        public void TestMax()
        {
            var value1 = new BigInteger(10);
            var value2 = new BigInteger(20);
            var max = value1.Max(value2);

            Assert.AreEqual(value2, max);
        }

        [TestMethod]
        public void TestModInverse()
        {
            var value = new BigInteger(4);
            var modulus = new BigInteger(5);
            var result = value.ModInverse(modulus);

            Assert.AreEqual(new BigInteger(4), result);
        }

        [TestMethod]
        public void TestSqrt()
        {
            var value = new BigInteger(100);
            var sqrt = value.Sqrt();

            Assert.AreEqual(new BigInteger(10), sqrt);
        }

        [TestMethod]
        public void TestLengths()
        {
            var value1 = new BigInteger();
            var value2 = new BigInteger(0x0);
            var value3 = new BigInteger(0xFFFFFFFF);
            var value4 = new BigInteger((ulong)0x0);
            var value5 = new BigInteger(0xFFFFFFFFFFFFFFFF);

            Assert.AreEqual(0, value1.ByteLength);
            Assert.AreEqual(sizeof(uint) / 4, value1.DataLength);
            Assert.AreEqual(0, value1.BitCount);
            Assert.AreEqual(0, value2.ByteLength);
            Assert.AreEqual(sizeof(uint) / 4, value2.DataLength);
            Assert.AreEqual(0, value2.BitCount);
            Assert.AreEqual(sizeof(uint), value3.ByteLength);
            Assert.AreEqual(sizeof(uint) / 4, value3.DataLength);
            Assert.AreEqual(sizeof(uint) * 8, value3.BitCount);
            Assert.AreEqual(0, value4.ByteLength);
            Assert.AreEqual(sizeof(uint) / 4, value4.DataLength);
            Assert.AreEqual(0, value4.BitCount);
            Assert.AreEqual(sizeof(ulong), value5.ByteLength);
            Assert.AreEqual(sizeof(ulong) / 4, value5.DataLength);
            Assert.AreEqual(sizeof(ulong) * 8, value5.BitCount);
        }

        [TestMethod]
        public void TestAddition()
        {
            var value1 = new BigInteger(10);
            var value2 = new BigInteger(20);
            var sum1 = value1 + value2;
            var sum2 = value2 + 30;
            var sum3 = value2 + -50;

            Assert.AreEqual(new BigInteger(30), sum1);
            Assert.AreEqual(new BigInteger(50), sum2);
            Assert.AreEqual(new BigInteger(-30), sum3);
        }

        [TestMethod]
        public void TestSubtraction()
        {
            var value1 = new BigInteger(80);
            var value2 = new BigInteger(35);
            var result1 = value1 - value2;
            var result2 = value2 - 10;
            var result3 = value2 - -50;

            Assert.AreEqual(new BigInteger(45), result1);
            Assert.AreEqual(new BigInteger(25), result2);
            Assert.AreEqual(new BigInteger(85), result3);
        }

        [TestMethod]
        public void TestBitwiseOr()
        {
            var value1 = new BigInteger(0x20);
            var value2 = new BigInteger(0x40);
            var result = value1 | value2;

            Assert.AreEqual(new BigInteger(0x60), result);
        }

        [TestMethod]
        public void TestBitwiseAnd()
        {
            var value = new BigInteger(0x60);
            var result1 = value & new BigInteger(0x20);
            var result2 = value & new BigInteger(0x60);
            var result3 = value & new BigInteger(0x100);

            Assert.AreEqual(new BigInteger(0x20), result1);
            Assert.AreEqual(new BigInteger(0x60), result2);
            Assert.AreEqual(new BigInteger(0x00), result3);
        }

        [TestMethod]
        public void TestUnaryOperators()
        {
            var value1 = new BigInteger(1);
            value1++;
            var value2 = new BigInteger(2);
            value2--;
            var value3 = new BigInteger(-1);
            ++value3;
            var value4 = new BigInteger(0);
            --value4;

            Assert.AreEqual(new BigInteger(2), value1);
            Assert.AreEqual(new BigInteger(1), value2);
            Assert.AreEqual(new BigInteger(0), value3);
            Assert.AreEqual(new BigInteger(-1), value4);
        }
    }
}
