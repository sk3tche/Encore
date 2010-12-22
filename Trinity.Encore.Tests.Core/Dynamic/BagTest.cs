using Microsoft.VisualStudio.TestTools.UnitTesting;
using Trinity.Encore.Framework.Core.Dynamic;

namespace Trinity.Encore.Tests.Core.Dynamic
{
    [TestClass]
    public sealed class BagTest
    {
        private dynamic _bag;

        [TestInitialize]
        public void Initialize()
        {
            _bag = new Bag();
        }

        [TestMethod]
        public void TestCount()
        {
            _bag.Object1 = new object();
            _bag.Object2 = new object();
            _bag.Object3 = new object();

            Assert.IsTrue(_bag.HasProperties);
            Assert.AreEqual(3, _bag.Count);
        }

        [TestMethod]
        public void TestRetrieval()
        {
            var obj1 = new object();
            var obj2 = new object();
            var obj3 = new object();

            _bag.Object1 = obj1;
            _bag.Object2 = obj2;
            _bag.Object3 = obj3;

            var retObj1 = _bag.Object1;
            var retObj2 = _bag.Object2;
            var retObj3 = _bag.Object3;

            Assert.AreSame(obj1, retObj1);
            Assert.AreSame(obj2, retObj2);
            Assert.AreSame(obj3, retObj3);
        }
    }
}
