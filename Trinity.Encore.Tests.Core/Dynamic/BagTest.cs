using Microsoft.VisualStudio.TestTools.UnitTesting;
using Trinity.Encore.Framework.Core.Dynamic;

namespace Trinity.Encore.Tests.Core.Dynamic
{
    [TestClass]
    public sealed class BagTest
    {
        [TestMethod]
        public void TestCount()
        {
            dynamic bag = new Bag();

            bag.Object1 = new object();
            bag.Object2 = new object();
            bag.Object3 = new object();

            Assert.IsTrue(bag.HasProperties);
            Assert.AreEqual(3, bag.Count);
        }

        [TestMethod]
        public void TestRetrieval()
        {
            dynamic bag = new Bag();

            var obj1 = new object();
            var obj2 = new object();
            var obj3 = new object();

            bag.Object1 = obj1;
            bag.Object2 = obj2;
            bag.Object3 = obj3;

            var retObj1 = bag.Object1;
            var retObj2 = bag.Object2;
            var retObj3 = bag.Object3;

            Assert.AreSame(obj1, retObj1);
            Assert.AreSame(obj2, retObj2);
            Assert.AreSame(obj3, retObj3);
        }
    }
}
