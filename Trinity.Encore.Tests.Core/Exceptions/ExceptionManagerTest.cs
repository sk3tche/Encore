using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Trinity.Encore.Framework.Core.Exceptions;

namespace Trinity.Encore.Tests.Core.Exceptions
{
    [TestClass]
    public sealed class ExceptionManagerTest
    {
        [TestMethod]
        public void TestThrowCatchRegister()
        {
            try
            {
                object obj = null;
                // ReSharper disable PossibleNullReferenceException
                obj.GetType(); // Trigger an NRE.
                // ReSharper restore PossibleNullReferenceException
            }
            catch (Exception ex)
            {
                ExceptionManager.RegisterException(ex);
            }
        }

        [TestMethod]
        public void TestGetExceptions()
        {
            var ex1 = new NullReferenceException();
            var ex2 = new ArgumentException();
            var ex3 = new InvalidOperationException();

            ExceptionManager.RegisterException(ex1);
            ExceptionManager.RegisterException(ex2);
            ExceptionManager.RegisterException(ex3);

            var exceptions = ExceptionManager.GetExceptions();

            Assert.AreEqual(ex1, exceptions[0].Exception);
            Assert.AreEqual(ex2, exceptions[1].Exception);
            Assert.AreEqual(ex3, exceptions[2].Exception);
        }
    }
}
