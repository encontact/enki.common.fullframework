using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Enki.Common.Tests {
    [TestClass()]
    public class ValidateUtilsTests {

        [TestMethod()]
        public void ValidaEmailTest() {

            // Validações verdadeiras.
            Assert.IsTrue(ValidateUtils.ValidaEmail("nomecomprido@enkilabs.com.br"));
            Assert.IsTrue(ValidateUtils.ValidaEmail("nomecomprido_cs@hotmail.com"));
            Assert.IsTrue(ValidateUtils.ValidaEmail("nomecomprido.coelho@hotmail.com"));
            Assert.IsTrue(ValidateUtils.ValidaEmail("nomecomprido123@hotmail.com"));
            Assert.IsTrue(ValidateUtils.ValidaEmail("123nomecomprido@teste.com"));
            Assert.IsTrue(ValidateUtils.ValidaEmail("nomecomprido@eNkIlAbS.cOm.Br"));
            Assert.IsTrue(ValidateUtils.ValidaEmail("relacoesinstitucionais@williamfreire.com.br"));
            Assert.IsTrue(ValidateUtils.ValidaEmail("sac@sede.embrapa"));
            Assert.IsTrue(ValidateUtils.ValidaEmail("nomecomprido Coelho Sartorelli <nomecomprido@enkilabs.com.br>"));
            Assert.IsTrue(ValidateUtils.ValidaEmail("nomecomprido Coelho Sartorelli <   nomecomprido@enkilabs.com.br >"));
            Assert.IsTrue(ValidateUtils.ValidaEmail("nomecomprido@enkilabs.software"));
            Assert.IsTrue(ValidateUtils.ValidaEmail("nomecomprido@123enkilabs.software"));

            // Validações falsas.
            Assert.IsFalse(ValidateUtils.ValidaEmail("nomecomprido@"));
            Assert.IsFalse(ValidateUtils.ValidaEmail("@tagged.com"));
            Assert.IsFalse(ValidateUtils.ValidaEmail("@taggedmail.com"));
            Assert.IsFalse(ValidateUtils.ValidaEmail("MAILER-DAEMON(MailDeliverySystem"));
            Assert.IsFalse(ValidateUtils.ValidaEmail("Nutrição"));
            Assert.IsFalse(ValidateUtils.ValidaEmail("@neomerkato.com.br"));
            Assert.IsFalse(ValidateUtils.ValidaEmail("roy"));
            Assert.IsFalse(ValidateUtils.ValidaEmail("nomecomprido@_enkilabs.software"));
            Assert.IsFalse(ValidateUtils.ValidaEmail("scan11@.com.br"));

        }
    }
}
