using Xunit;

namespace Enki.Common.Tests
{
    public class ValidateUtilsTests
    {
        [Theory]
        [InlineData("nomecomprido@enkilabs.com.br")]
        [InlineData("nomecomprido_cs@hotmail.com")]
        [InlineData("nomecomprido.coelho@hotmail.com")]
        [InlineData("nomecomprido123@hotmail.com")]
        [InlineData("123nomecomprido@teste.com")]
        [InlineData("nomecomprido@eNkIlAbS.cOm.Br")]
        [InlineData("relacoesinstitucionais@williamfreire.com.br")]
        [InlineData("sac@sede.embrapa")]
        [InlineData("nomecomprido Coelho Sartorelli <nomecomprido@enkilabs.com.br>")]
        [InlineData("nomecomprido Coelho Sartorelli <   nomecomprido@enkilabs.com.br >")]
        [InlineData("nomecomprido@enkilabs.software")]
        [InlineData("nomecomprido@123enkilabs.software")]
        [InlineData("nomecomprido@gov.br")]
        [InlineData("nome.sobrenome@br.ey.com")]
        [InlineData("livia.pereira@am.sebrae.com.br")]
        [InlineData("teste.email@meu-dominio.com.br")]
        [InlineData("teste.email@meu-dominio-dois.com.br")]
        [InlineData("Meu nome(meunome@teste.com) <meunome@teste.com>")]
        [InlineData("teste.email@meu.dominio-internacional.com")]
        [InlineData("'nomecomprido Coelho Sartorelli' <   nomecomprido@enkilabs.com.br >")]
        [InlineData("`nomecomprido Coelho Sartorelli` <   nomecomprido@enkilabs.com.br >")]
        [InlineData("´nomecomprido Coelho Sartorelli´ <   nomecomprido@enkilabs.com.br >")]
        [InlineData("CLIENT_UNDERLINE_&E@Client.com")]
        [InlineData("Client underline and & <CLIENT_UNDERLINE_&E@Client.com>")]
        [InlineData("clientemail@i.client.com")]
        [InlineData("Client underline and & <clientemail@i.client.com>")]
        public void TestValidEmailCases(string validEmail) => Assert.True(ValidateUtils.ValidaEmail(validEmail));

        [Theory]
        [InlineData("nomecomprido@")]
        [InlineData("@tagged.com")]
        [InlineData("@taggedmail.com")]
        [InlineData("MAILER-DAEMON(MailDeliverySystem")]
        [InlineData("Nutrição")]
        [InlineData("@neomerkato.com.br")]
        [InlineData("roy")]
        [InlineData("nomecomprido@_enkilabs.software")]
        [InlineData("nomecomprido@-enkilabs.software")]
        [InlineData("nomecomprido@enkilabs-.software")]
        [InlineData("scan11@.com.br")]
        [InlineData("scan11@com..br")]
        [InlineData("scan11@com.br.")]
        [InlineData("invalidemail@domain..com.br")]
        [InlineData("teste2.@enkilabs.com.br")]
        public void TestInvalidEmailCases(string invalidEmail) => Assert.False(ValidateUtils.ValidaEmail(invalidEmail));
    }
}