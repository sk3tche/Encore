using System;
using System.Collections.Concurrent;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Trinity.Encore.Framework.Core.Collections;

namespace Trinity.Encore.Tests.Core.Collections
{
    [TestClass]
    public sealed class ConcurrentDictionaryExtensionsTest
    {
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestDuplicateAdd()
        {
            var dict = new ConcurrentDictionary<int, string>();

            dict.Add(3, string.Empty);
            dict.Add(3, string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestDuplicateRemove()
        {
            var dict = new ConcurrentDictionary<int, string>();

            dict.Add(3, string.Empty);

            dict.Remove(3);
            dict.Remove(3);
        }
    }
}
