using Microsoft.VisualStudio.TestTools.UnitTesting;
using Trinity.Core.Runtime.InteropServices;

namespace Trinity.Encore.Tests.Core.Runtime
{
    [TestClass]
    public sealed class UnionTest
    {
        [TestMethod]
        public unsafe void TestSize()
        {
            // The size of Union needs to be exactly sizeof(decimal), as that's the largest field in the struct.
            var size = sizeof(Union);

            Assert.AreEqual(sizeof(decimal), size);
        }

        [TestMethod]
        public unsafe void TestIntegerValues()
        {
            var union = new Union();
            var val = new byte[] { 0xff, 0x00, 0xff, 0x00, 0xff, 0x00, 0xff, 0x00 };

            // Don't do this at home, kids.
            for (var i = 0; i < val.Length; i++)
                union.Bytes[i] = val[i]; // These are stored as little-endian.

            Assert.AreEqual<byte>(0xff, union.Byte);
            Assert.AreEqual<sbyte>(-0x1, union.SByte);
            Assert.AreEqual<ushort>(0x00ff, union.UInt16);
            Assert.AreEqual<short>(0x00ff, union.Int16);
            Assert.AreEqual<uint>(0x00ff00ff, union.UInt32);
            Assert.AreEqual(0x00ff00ff, union.Int32);
        }
    }
}
