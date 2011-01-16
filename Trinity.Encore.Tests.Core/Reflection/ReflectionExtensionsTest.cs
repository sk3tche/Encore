using System;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Trinity.Core.Reflection;

namespace Trinity.Encore.Tests.Core.Reflection
{
    [TestClass]
    public sealed class ReflectionExtensionsTest
    {
        [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
        private sealed class DummyAttribute : Attribute
        {
        }

        [Dummy]
        private sealed class DummyType1
        {
            [Dummy]
            [UsedImplicitly]
            public void DummyMethod()
            {
            }
        }

        [Dummy]
        [Dummy]
        private sealed class DummyType2
        {
            [Dummy]
            [Dummy]
            [UsedImplicitly]
            public void DummyMethod()
            {
            }
        }

        private Type _type1;

        private MethodInfo _method1;

        private Type _type2;

        private MethodInfo _method2;

        [TestInitialize]
        public void Initialize()
        {
            _type1 = typeof(DummyType1);
            _method1 = _type1.GetMethod("DummyMethod");
            _type2 = typeof(DummyType2);
            _method2 = _type2.GetMethod("DummyMethod");
        }

        [TestMethod]
        public void TestGetCustomAttribute()
        {
            var attr = _type1.GetCustomAttribute<DummyAttribute>();
            var methodAttr = _method1.GetCustomAttribute<DummyAttribute>();

            Assert.IsNotNull(attr);
            Assert.IsNotNull(methodAttr);
        }

        [TestMethod]
        public void TestGetCustomAttributes()
        {
            var attrs = _type2.GetCustomAttributes<DummyAttribute>();
            var methodAttrs = _method2.GetCustomAttributes<DummyAttribute>();

            Assert.IsNotNull(attrs);
            Assert.AreEqual(attrs.Length, 2);
            Assert.IsNotNull(methodAttrs);
            Assert.AreEqual(methodAttrs.Length, 2);
        }
    }
}
