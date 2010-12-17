using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Trinity.Encore.Framework.Core.Collections;

namespace Trinity.Encore.Tests.Core.Collections
{
    [TestClass]
    public sealed class CollectionExtensionsTest
    {
        [TestMethod]
        public void TestAddRange()
        {
            var obj1 = new object();
            var obj2 = new object();
            var obj3 = new object();

            var col = new List<object>
            {
                obj1,
                obj2,
                obj3,
            };

            var newCol = (ICollection<object>)new List<object>();
            newCol.AddRange(col);
            var list = newCol.ToList();

            Assert.AreSame(obj1, list[0]);
            Assert.AreSame(obj2, list[1]);
            Assert.AreSame(obj3, list[2]);
        }
    }
}
