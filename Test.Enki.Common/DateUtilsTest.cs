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
            var epochRecovered = DateUtils.GetUnixEpoch(dateNow, DateTimeKind.Utc);
            var dateRecovered = DateUtils.FromUnixEpochSeconds(epochRecovered);
            Assert.AreEqual(epochRecovered, epochNow);
            Assert.AreEqual(dateNow.ToString("dd/MM/yyyy HH:mm:ss"), dateRecovered.ToString("dd/MM/yyyy HH:mm:ss"));
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