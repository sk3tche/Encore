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

        [TestMethod]
        public void TestByteArrayToHex()
        {
            var bytes1 = new byte[]
            {
                0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07,
                0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f,
            };
            var bytes2 = bytes1.Concat(new byte[]
            {
                0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17,
                0x18, 0x19, 0x1a, 0x1b, 0x1c, 0x1d, 0x1e, 0x1f,
            }).ToArray();

            var str1 = Utility.BinaryToHexString(bytes1);
            var str2 = Utility.BinaryToHexString(bytes2);

            Assert.AreEqual("000102030405060708090A0B0C0D0E0F", str1);
            Assert.AreEqual("000102030405060708090A0B0C0D0E0F101112131415161718191A1B1C1D1E1F", str2);
        }

        [TestMethod]
        public void TestHexToByteArray()
        {
            var str1 = "000102030405060708090A0B0C0D0E0F";
            var str2 = "000102030405060708090A0B0C0D0E0F101112131415161718191A1B1C1D1E1F";

            var bytes1 = Utility.HexStringToBinary(bytes1);
            var bytes2 = Utility.HexStringToBinary(bytes2);
            var bytes1Reference = new byte[]
            {
                0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07,
                0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f,
            };

            var bytes2Reference = bytes1Reference.Concat(new byte[]
            {
                0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17,
                0x18, 0x19, 0x1a, 0x1b, 0x1c, 0x1d, 0x1e, 0x1f,
            }).ToArray();
            
            Assert.AreEqual(bytes1.Length, bytes1Reference.Length);
            Assert.AreEqual(bytes2.Length, bytes2Reference.Length);

            for (int i = 0; i < bytes1.Length; i++)
                Assert.AreEqual(bytes1[i], bytes1Reference[i]);

            for (int i = 0; i < bytes2.Length; i++)
                Assert.AreEqual(bytes2[i], bytes2Reference[i]);
        }
    }
}
