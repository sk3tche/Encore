using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Trinity.Encore.Framework.Core.Collections;

namespace Trinity.Encore.Tests.Core.Collections
{
    [TestClass]
    public sealed class DictionaryExtensionsTest
    {
        [TestMethod]
        public void TestTryGet()
        {
            var dict = new Dictionary<int, string>
            {
                { 1, string.Empty },
                { 2, string.Empty },
                { 4, string.Empty },
            };

            var val1 = dict.TryGet(1);
            var val2 = dict.TryGet(2);
            var val3 = dict.TryGet(3);
            var val4 = dict.TryGet(4);

            Assert.IsNotNull(val1);
            Assert.IsNotNull(val2);
            Assert.IsNull(val3);
            Assert.IsNotNull(val4);
        }
    }
}
