using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Trinity.Encore.Framework.Core.Reflection;

namespace Trinity.Encore.Tests.Core.Reflection
{
    [TestClass]
    public sealed class TypeExtensionsTest
    {
        [TestMethod]
        public void TestIsAssignableTo()
        {
            var assignable1 = typeof(List<int>).IsAssignableTo(typeof(IList<int>));
            var assignable2 = typeof(int).IsAssignableTo(typeof(ValueType));
            var assignable3 = typeof(int).IsAssignableTo(typeof(object));
            var assignable4 = typeof(Action).IsAssignableTo(typeof(Delegate));

            Assert.IsTrue(assignable1);
            Assert.IsTrue(assignable2);
            Assert.IsTrue(assignable3);
            Assert.IsTrue(assignable4);
        }

        [TestMethod]
        public void TestIsSimple()
        {
            // Just test types we don't test in the tests for IsInteger and IsFloatingPoint.
            var isSimple1 = typeof(PlatformID).IsSimple();
            var isSimple2 = typeof(char).IsSimple();
            var isSimple3 = typeof(bool).IsSimple();
            var isSimple4 = typeof(string).IsSimple();

            Assert.IsTrue(isSimple1);
            Assert.IsTrue(isSimple2);
            Assert.IsTrue(isSimple3);
            Assert.IsTrue(isSimple4);
        }

        [TestMethod]
        public void TestIsInteger()
        {
            var isInt1 = typeof(byte).IsInteger();
            var isInt2 = typeof(sbyte).IsInteger();
            var isInt3 = typeof(short).IsInteger();
            var isInt4 = typeof(ushort).IsInteger();
            var isInt5 = typeof(int).IsInteger();
            var isInt6 = typeof(uint).IsInteger();
            var isInt7 = typeof(long).IsInteger();
            var isInt8 = typeof(ulong).IsInteger();

            Assert.IsTrue(isInt1);
            Assert.IsTrue(isInt2);
            Assert.IsTrue(isInt3);
            Assert.IsTrue(isInt4);
            Assert.IsTrue(isInt5);
            Assert.IsTrue(isInt6);
            Assert.IsTrue(isInt7);
            Assert.IsTrue(isInt8);
        }

        [TestMethod]
        public void TestIsFloatingPoint()
        {
            var isFp1 = typeof(float).IsFloatingPoint();
            var isFp2 = typeof(double).IsFloatingPoint();
            var isFp3 = typeof(decimal).IsFloatingPoint();

            Assert.IsTrue(isFp1);
            Assert.IsTrue(isFp2);
            Assert.IsTrue(isFp3);
        }
    }
}
