using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Trinity.Core.Time;

namespace Trinity.Encore.Tests.Core.Time
{
    [TestClass]
    public sealed class TimeUtilityTest
    {
        [TestMethod]
        public void TestUnixTimeToDateTime()
        {
            var unixTime = TimeUtility.UnixTimeToDateTime(1293031362); // December 22, 2010, 15:22:42

            Assert.AreEqual(new DateTime(2010, 12, 22, 15, 22, 42), unixTime);
        }

        [TestMethod]
        public void TestDateTimeToUnixTime()
        {
            var dateTime = TimeUtility.DateTimeToUnixTime(new DateTime(2010, 12, 22, 15, 36, 45)); // December 22, 2010, 15:36:45

            Assert.AreEqual(1293032205, dateTime);
        }
    }
}
