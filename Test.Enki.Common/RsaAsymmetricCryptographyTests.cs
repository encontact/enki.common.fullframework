using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Enki.Common.Cryptography.Tests
{
    [TestClass()]
    public class AsymmetricCryptographyTests
    {
        [TestMethod()]
        public void AsymmetricCryptographyTest()
        {
            var message = "TESTE !@#$#$%$¨%&¨*&**&(257386";
            var crypto = new RsaAsymmetricCryptography();
            var cryptedData = crypto.Encrypt(message);
            var publicKey = crypto.GetPublicKey();
            var privateKey = crypto.GetPrivateKey();
            var crypto2 = new RsaAsymmetricCryptography(privateKey, publicKey);

            // Valida descriptografia
            Assert.AreEqual(message, crypto.Decrypt(cryptedData));
            // Valida descriptografia a partir de outra classe criada com as mesmas chaves
            Assert.AreEqual(message, crypto2.Decrypt(cryptedData));
            // Valida que duas chaves com mesmo texto são criadas diferente
            Assert.AreNotEqual(cryptedData, crypto2.Encrypt(message));


            var messageLong = "{\"ClientId\":1,\"DueDate\":\"2020 - 10 - 10T00: 00:00\",\"EmailLicenses\":1,\"PhoneLicenses\":0,\"ChatLicenses\":12,\"HasVirtualAssistant\":true}";
            try
            {
                var cryptedLongData = crypto.Encrypt(messageLong);
                Assert.Fail("Não poderia decriptogradar uma mensagem tão grande.");
            }
            catch
            {
                Assert.IsTrue(true);
            }
        }
    }
}