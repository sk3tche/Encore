using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Trinity.Encore.Framework.Core.Collections;

namespace Trinity.Encore.Tests.Core.Collections
{
    [TestClass]
    public sealed class EnumerableExtensionsTest
    {
        [TestMethod]
        public void TestConversions()
        {
            var list = new List<int>
            {
                0,
                1,
                2,
            };

            var queue = list.ToQueue();
            var stack = list.ToStack();
            var prioQueue = list.ToPriorityQueue();

            Assert.IsNotNull(queue);
            Assert.AreEqual(queue.Count, 3);
            Assert.IsNotNull(stack);
            Assert.AreEqual(stack.Count, 3);
            Assert.IsNotNull(prioQueue);
            Assert.AreEqual(prioQueue.Count, 3);
        }
    }
}
