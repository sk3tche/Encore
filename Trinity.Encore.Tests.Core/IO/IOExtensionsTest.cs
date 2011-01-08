using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Trinity.Encore.Framework.Core.IO;

namespace Trinity.Encore.Tests.Core.IO
{
    [TestClass]
    public sealed class IOExtensionsTest
    {
        private MemoryStream _stream;

        private BinaryReader _reader;

        private BinaryWriter _writer;

        private void Reset()
        {
            _stream.Position = 0;
        }

        [TestInitialize]
        public void Initialize()
        {
            _stream = new MemoryStream(new byte[4096]);
            _reader = new BinaryReader(_stream);
            _writer = new BinaryWriter(_stream);
        }

        [TestMethod]
        public void TestCString()
        {
            _writer.WriteCString("ENCR");
            _writer.WriteCString("ENCR", Encoding.UTF8);

            Reset();

            var newStr = _reader.ReadCString();
            var newUtfStr = _reader.ReadCString();

            Assert.AreEqual("ENCR", newStr);
            Assert.AreEqual("ENCR", newUtfStr);
        }

        [TestMethod]
        public void TestFourCC()
        {
            _writer.WriteFourCC("ENCR");

            Reset();

            var new4CC = _reader.ReadFourCC();

            Assert.AreEqual("ENCR", new4CC);
        }

        [TestMethod]
        public void TestIsRead()
        {
            _reader.ReadBytes(4096);
            var isRead = _reader.BaseStream.IsRead();

            Assert.IsTrue(isRead);
        }

        [TestMethod]
        public void TestBigEndian()
        {
            _writer.WriteBigEndian(short.MaxValue);
            _writer.WriteBigEndian(ushort.MaxValue);
            _writer.WriteBigEndian(int.MaxValue);
            _writer.WriteBigEndian(uint.MaxValue);
            _writer.WriteBigEndian(long.MaxValue);
            _writer.WriteBigEndian(ulong.MaxValue);

            Reset();

            var shortMax = _reader.ReadInt16BigEndian();
            var ushortMax = _reader.ReadUInt16BigEndian();
            var intMax = _reader.ReadInt32BigEndian();
            var uintMax = _reader.ReadUInt32BigEndian();
            var longMax = _reader.ReadInt64BigEndian();
            var ulongMax = _reader.ReadUInt64BigEndian();

            Assert.AreEqual(short.MaxValue, shortMax);
            Assert.AreEqual(ushort.MaxValue, ushortMax);
            Assert.AreEqual(int.MaxValue, intMax);
            Assert.AreEqual(uint.MaxValue, uintMax);
            Assert.AreEqual(long.MaxValue, longMax);
            Assert.AreEqual(ulong.MaxValue, ulongMax);
        }
    }
}
