using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Enki.Common.Cryptography.Tests
{
    [TestClass()]
    public class AesSymmetricCryptographyTests
    {
        [TestMethod()]
        public void AesSymmetricCryptographyTest()
        {
            var message = "TESTE !@#$#$%$¨%&¨*&**&(257386";
            var crypto = new AesSymmetricCryptography();
            var cryptedData = crypto.Encrypt(message);
            var key = crypto.GetKey();
            var iv = crypto.GetIV();
            var crypto2 = new AesSymmetricCryptography(key, iv);

            // Valida descriptografia
            Assert.AreEqual(message, crypto.Decrypt(cryptedData));
            // Valida descriptografia a partir de outra classe criada com as mesmas chaves
            Assert.AreEqual(message, crypto2.Decrypt(cryptedData));
            // Valida que duas chaves com mesmo texto são criadas diferente
            Assert.AreEqual(cryptedData, crypto2.Encrypt(message));
            
            var messageLong = "{\"ClientId\":1,\"DueDate\":\"2020 - 10 - 10T00: 00:00\",\"EmailLicenses\":1,\"PhoneLicenses\":0,\"ChatLicenses\":12,\"HasVirtualAssistant\":true}";
            var cryptedLongData = crypto.Encrypt(messageLong);

            Assert.AreEqual(messageLong, crypto.Decrypt(cryptedLongData));
        }
    }
}