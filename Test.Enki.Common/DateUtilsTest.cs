using Enki.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Test.Enki.Common
{
    [TestClass]
    public class DateUtilsTest
    {
        [TestMethod]
        public void TestGetUnixEpoch()
        {
            Assert.AreEqual(1382633602, DateUtils.GetUnixEpoch(new DateTime(2013, 10, 24, 16, 53, 22)));
            Assert.AreEqual(1382126175, DateUtils.GetUnixEpoch(new DateTime(2013, 10, 18, 19, 56, 15)));
        }
    }
}