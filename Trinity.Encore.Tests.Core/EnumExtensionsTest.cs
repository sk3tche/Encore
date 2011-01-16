using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Trinity.Core;

namespace Trinity.Encore.Tests.Core
{
    [TestClass]
    public sealed class EnumExtensionsTest
    {
        [Flags]
        [Serializable]
        private enum DummyFlags : byte
        {
            None = 0x0,
            Flag1 = 0x1,
            Flag2 = 0x2,
            Flag3 = 0x4,
            Flag4 = 0x8,
        }

        [TestMethod]
        public void TestHasAnyFlag()
        {
            const DummyFlags value1 = DummyFlags.Flag1 | DummyFlags.Flag2 | DummyFlags.Flag3 | DummyFlags.Flag4;
            const DummyFlags value2 = DummyFlags.Flag2 | DummyFlags.Flag4;

            var hasFlags1 = value1.HasAnyFlag(DummyFlags.Flag1 | DummyFlags.Flag2 | DummyFlags.Flag3 | DummyFlags.Flag4);
            var hasFlags2 = value1.HasAnyFlag(DummyFlags.Flag1 | DummyFlags.Flag3);
            var hasFlags3 = value1.HasAnyFlag(DummyFlags.Flag2);
            var hasFlags4 = value1.HasAnyFlag(DummyFlags.None);
            var hasFlags5 = value2.HasAnyFlag(DummyFlags.Flag4);
            var hasFlags6 = value2.HasAnyFlag(DummyFlags.Flag3 | DummyFlags.Flag1);

            Assert.IsTrue(hasFlags1);
            Assert.IsTrue(hasFlags2);
            Assert.IsTrue(hasFlags3);
            Assert.IsFalse(hasFlags4);
            Assert.IsTrue(hasFlags5);
            Assert.IsFalse(hasFlags6);
        }

        [TestMethod]
        public void TestIsValid()
        {
            const DummyFlags value1 = DummyFlags.None;
            const DummyFlags value2 = DummyFlags.Flag3;
            const DummyFlags value3 = (DummyFlags)0x7;

            var isValid1 = value1.IsValid();
            var isValid2 = value2.IsValid();
            var isValid3 = value3.IsValid();

            Assert.IsTrue(isValid1);
            Assert.IsTrue(isValid2);
            Assert.IsFalse(isValid3);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestEnsureValid()
        {
            const DummyFlags value = (DummyFlags)0x10;

            value.EnsureValid();
        }
    }
}
