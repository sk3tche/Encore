using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Trinity.Core.Collections;

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

        [TestMethod]
        public void TestPad()
        {
            var list = new List<int>
            {
                0,
                1,
                2,
                3,
                4,
            };

            var last = 5;
            var newSeq = list.Pad(10, () => last++);
            var count = newSeq.Count();

            Assert.AreEqual(10, count);
        }
    }
}
