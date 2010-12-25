using System;
using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Trinity.Encore.Framework.Core.Reflection;

namespace Trinity.Encore.Tests.Core.Reflection
{
    [TestClass]
    public sealed class ReflectionUtilityTest
    {
        private sealed class DummyType
        {
            public DummyType()
            {
            }

            public DummyType(string s)
            {
                String = s;
            }

            public int Integer { get; set; }

            public readonly string String;

            public event EventHandler OnAction;

            public void DoWork()
            {
                OnAction(this, EventArgs.Empty);
            }

            public void DoWork([UsedImplicitly] string s)
            {
            }

            public int GetValue([UsedImplicitly] string s, [UsedImplicitly] int i)
            {
                return 0;
            }
        }

        [TestMethod]
        public void TestConstructorOf()
        {
            var actualCtor1 = typeof(DummyType).GetConstructor(Type.EmptyTypes);
            var actualCtor2 = typeof(DummyType).GetConstructor(new[] { typeof(string) });
            var ctor1 = ReflectionUtility.ConstructorOf(() => new DummyType());
            var ctor2 = ReflectionUtility.ConstructorOf(() => new DummyType(string.Empty));

            Assert.IsNotNull(ctor1);
            Assert.IsNotNull(ctor2);
            Assert.AreEqual(actualCtor1, ctor1);
            Assert.AreEqual(actualCtor2, ctor2);
        }

        [TestMethod]
        public void TestMethodOf()
        {
            var obj = new DummyType();

            var actualMethod1 = typeof(DummyType).GetMethod("DoWork", Type.EmptyTypes);
            var actualMethod2 = typeof(DummyType).GetMethod("DoWork", new[] { typeof(string) });
            var actualMethod3 = typeof(DummyType).GetMethod("GetValue", new[] { typeof(string), typeof(int) });
            var method1 = ReflectionUtility.MethodOf(() => obj.DoWork());
            var method2 = ReflectionUtility.MethodOf(() => obj.DoWork(string.Empty));
            var method3 = ReflectionUtility.MethodOf(() => obj.GetValue(string.Empty, 0));

            Assert.IsNotNull(method1);
            Assert.IsNotNull(method2);
            Assert.IsNotNull(method3);
            Assert.AreEqual(actualMethod1, method1);
            Assert.AreEqual(actualMethod2, method2);
            Assert.AreEqual(actualMethod3, method3);
        }

        [TestMethod]
        public void TestFieldOf()
        {
            var obj = new DummyType();

            var actualField = typeof(DummyType).GetField("String");
            var field = ReflectionUtility.FieldOf(() => obj.String);

            Assert.IsNotNull(field);
            Assert.AreEqual(actualField, field);
        }

        [TestMethod]
        public void TestPropertyOf()
        {
            var obj = new DummyType();

            var actualProp = typeof(DummyType).GetProperty("Integer");
            var prop = ReflectionUtility.PropertyOf(() => obj.Integer);

            Assert.IsNotNull(prop);
            Assert.AreEqual(actualProp, prop);
        }
    }
}
