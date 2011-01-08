using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Trinity.Encore.Framework.Core;

namespace Trinity.Encore.Tests.Core
{
    [TestClass]
    public sealed class UtilityTest
    {
        [TestMethod]
        public void TestToCamelCase()
        {
            const string str1 = "SOME_C_STYLE_NAME";
            var newStr1 = Utility.ToCamelCase(str1);

            var str2 = string.Empty;
            var newStr2 = Utility.ToCamelCase(str2);

            const string str3 = " ";
            var newStr3 = Utility.ToCamelCase(str3);

            Assert.AreEqual("SomeCStyleName", newStr1);
            Assert.AreEqual(string.Empty, newStr2);
            Assert.AreEqual(string.Empty, newStr3);
        }

        private const string String1 = "000102030405060708090A0B0C0D0E0F";

        private static readonly byte[] _bytes1 =
            {
                0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07,
                0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f,
            };

        private const string String2 = "000102030405060708090A0B0C0D0E0F101112131415161718191A1B1C1D1E1F";

        private static readonly byte[] _bytes2 = _bytes1.Concat(new byte[]
        {
            0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17,
            0x18, 0x19, 0x1a, 0x1b, 0x1c, 0x1d, 0x1e, 0x1f,
        }).ToArray();

        [TestMethod]
        public void TestBinaryToHexString()
        {
            var str1 = Utility.BinaryToHexString(_bytes1);
            var str2 = Utility.BinaryToHexString(_bytes2);

            Assert.AreEqual(String1, str1);
            Assert.AreEqual(String2, str2);
        }

        [TestMethod]
        public void TestHexStringToBinary()
        {
            var bytes1 = Utility.HexStringToBinary(String1);
            var bytes2 = Utility.HexStringToBinary(String2);

            Assert.IsTrue(bytes1.SequenceEqual(_bytes1));
            Assert.IsTrue(bytes2.SequenceEqual(_bytes2));
        }
    }
}
