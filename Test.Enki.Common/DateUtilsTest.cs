using Enki.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Test.Enki.Common
{
    [TestClass]
    public class DateUtilsTest
    {
        [TestMethod]
        public void MustGetUnixEpochFromUtcDate()
        {
            Assert.AreEqual(1382633602, DateUtils.GetUnixEpoch(new DateTime(2013, 10, 24, 16, 53, 22), DateTimeKind.Utc));
            Assert.AreEqual(1382126175, DateUtils.GetUnixEpoch(new DateTime(2013, 10, 18, 19, 56, 15), DateTimeKind.Utc));
        }

        [TestMethod]
        public void MustGetDateTimeUtcNowEpoch()
        {
            var dateNow = DateTime.UtcNow;
            var epochNow = DateUtils.GetGMTUnixEpochNow();
            Assert.AreEqual(DateUtils.GetUnixEpoch(dateNow, DateTimeKind.Utc), epochNow);
        }

        [TestMethod]
        public void MustGetDateTimeNowEpoch()
        {
            var dateNow = DateTime.Now;
            var epochNow = DateUtils.GetLocalUnixEpochNow();
            Assert.AreEqual(DateUtils.GetUnixEpoch(dateNow, DateTimeKind.Local), epochNow);
        }
    }
}