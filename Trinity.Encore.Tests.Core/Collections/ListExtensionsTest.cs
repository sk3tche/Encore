using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Trinity.Encore.Framework.Core.Collections;

namespace Trinity.Encore.Tests.Core.Collections
{
    [TestClass]
    public sealed class ListExtensionsTest
    {
        [TestMethod]
        public void TestTryGet()
        {
            var obj1 = new object();
            var obj3 = new object();

            var list = new List<object>
            {
                obj1,
                null,
                obj3,
            };

            var o1 = list.TryGet(0);
            var o2 = list.TryGet(1);
            var o3 = list.TryGet(2);
            var o4 = list.TryGet(3);

            Assert.AreSame(obj1, o1);
            Assert.IsNull(o2);
            Assert.AreSame(obj3, o3);
            Assert.IsNull(o4);
        }

        [TestMethod]
        public void TestSwap()
        {
            var obj1 = new object();
            var obj2 = new object();

            var list = new List<object>
            {
                new object(),
                new object(),
                obj1,
                new object(),
                new object(),
                new object(),
                obj2,
                new object()
            };

            list.Swap(2, 6);

            Assert.AreSame(obj1, list[6]);
            Assert.AreSame(obj2, list[2]);
        }
    }
}
