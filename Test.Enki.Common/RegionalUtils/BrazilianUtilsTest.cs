using Enki.Common.RegionalUtils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Enki.Common
{
    [TestClass]
    public class BrazilianUtilsTest
    {
        [TestMethod]
        public void TestValidLicensePlate()
        {
            Assert.AreEqual(true, BrazilianUtils.ValidLicensePlate("AAA0000"));
            Assert.AreEqual(true, BrazilianUtils.ValidLicensePlate("AAA-0000", true));
            Assert.AreEqual(false, BrazilianUtils.ValidLicensePlate("AAA00A0"));
            Assert.AreEqual(false, BrazilianUtils.ValidLicensePlate("0AAA0000"));
            Assert.AreEqual(false, BrazilianUtils.ValidLicensePlate("AAA00000"));
            Assert.AreEqual(false, BrazilianUtils.ValidLicensePlate("AAAA0000"));
        }
    }
}