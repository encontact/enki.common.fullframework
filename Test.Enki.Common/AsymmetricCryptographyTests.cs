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
            var crypto = new AsymmetricCryptography();
            var cryptedData = crypto.Encrypt(message);
            var publicKey = crypto.GetPublicKey();
            var privateKey = crypto.GetPrivateKey();
            var crypto2 = new AsymmetricCryptography(privateKey, publicKey);

            // Valida descriptografia
            Assert.AreEqual(message, crypto.Decrypt(cryptedData));
            // Valida descriptografia a partir de outra classe criada com as mesmas chaves
            Assert.AreEqual(message, crypto2.Decrypt(cryptedData));
            // Valida que duas chaves com mesmo texto são criadas diferente
            Assert.AreNotEqual(cryptedData, crypto2.Encrypt(message));
        }
    }
}