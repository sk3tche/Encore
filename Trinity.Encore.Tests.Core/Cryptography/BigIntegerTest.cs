using System;
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

            Assert.AreEqual(absolute, new BigInteger(50));
        }

        [TestMethod]
        public void TestModPow()
        {
            var value = new BigInteger(50);
            var result = value.ModPow(new BigInteger(2), new BigInteger(3));

            Assert.AreEqual(result, new BigInteger(1));
        }

        [TestMethod]
        public void TestGreatestCommonDivisor()
        {
            var value1 = new BigInteger(10);
            var value2 = new BigInteger(20);
            var gcd = value1.GreatestCommonDivisor(value2);

            Assert.AreEqual(gcd, new BigInteger(10));
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

            Assert.AreEqual(result, new BigInteger(4));
        }

        [TestMethod]
        public void TestSqrt()
        {
            var value = new BigInteger(100);
            var sqrt = value.Sqrt();

            Assert.AreEqual(sqrt, new BigInteger(10));
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
    }
}
