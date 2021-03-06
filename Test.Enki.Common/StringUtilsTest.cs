﻿using Enki.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Test.Enki.Common
{
    [TestClass]
    public class StringUtilsTest
    {
        [TestMethod]
        public void TestCurrencyFrom()
        {
            Assert.AreEqual(1250.33, StringUtils.CurrencyFrom("1250,33"));
            Assert.AreEqual(1250.22, StringUtils.CurrencyFrom("1250.22"));
            Assert.AreEqual(1250.55, StringUtils.CurrencyFrom("1.250,55"));
            Assert.AreEqual(1250.66, StringUtils.CurrencyFrom("1,250.66"));
            Assert.AreEqual(1250, StringUtils.CurrencyFrom("1250"));
            Assert.AreEqual(50.60, StringUtils.CurrencyFrom("50.6"));
            Assert.AreEqual(1001.34, StringUtils.CurrencyFrom("1.001,34"));
            Assert.AreEqual(1024.00, StringUtils.CurrencyFrom("1.024"));
            Assert.AreEqual(1104.00, StringUtils.CurrencyFrom("1,104"));
            Assert.AreEqual(1764.30, StringUtils.CurrencyFrom("1,764.30"));
            Assert.AreEqual(1402.95, StringUtils.CurrencyFrom("1,402.95"));
            Assert.AreEqual(85.13, StringUtils.CurrencyFrom("R$ 85.13"));
            Assert.AreEqual(0.0, StringUtils.CurrencyFrom("0"));
            Assert.AreEqual(0.9, StringUtils.CurrencyFrom(".9"));
            Assert.AreEqual(1, StringUtils.CurrencyFrom("1"));
            Assert.AreEqual(1, StringUtils.CurrencyFrom("01"));
            Assert.AreEqual(0.95, StringUtils.CurrencyFrom(".95"));
            Assert.AreEqual(12, StringUtils.CurrencyFrom("12"));
            try
            {
                StringUtils.CurrencyFrom("83,65 (2012) e 96,81 (2011)");
                Assert.Fail();
            }
            catch (InvalidOperationException e)
            {
                Assert.AreEqual("O valor 83,65 (2012) e 96,81 (2011) não pode ser convertido para double.", e.Message);
            }
        }

        [TestMethod]
        public void TestFormat()
        {
            Assert.AreEqual("1.250,33", StringUtils.format(StringUtils.CurrencyFrom("1250,33").ToString(), "###.###.###,##"));
            Assert.AreEqual("1.250,22", StringUtils.format(StringUtils.CurrencyFrom("1250.22").ToString(), "###.###.###,##"));
            Assert.AreEqual("1.250,55", StringUtils.format(StringUtils.CurrencyFrom("1.250,55").ToString(), "###.###.###,##"));
            Assert.AreEqual("1.250,66", StringUtils.format(StringUtils.CurrencyFrom("1,250.66").ToString(), "###.###.###,##"));
            Assert.AreEqual("1.250,00", StringUtils.format(StringUtils.CurrencyFrom("1250").ToString("0.00"), "###.###.###,##"));
            Assert.AreEqual("50,60", StringUtils.format(StringUtils.CurrencyFrom("50.6").ToString("0.00"), "###.###.###,##"));
            Assert.AreEqual("1.001,34", StringUtils.format(StringUtils.CurrencyFrom("1.001,34").ToString(), "###.###.###,##"));
            Assert.AreEqual("1.024,00", StringUtils.format(StringUtils.CurrencyFrom("1.024").ToString("0.00"), "###.###.###,##"));
            Assert.AreEqual("1.104,00", StringUtils.format(StringUtils.CurrencyFrom("1,104").ToString("0.00"), "###.###.###,##"));
            Assert.AreEqual("1.764,30", StringUtils.format(StringUtils.CurrencyFrom("1,764.30").ToString("0.00"), "###.###.###,##"));
            Assert.AreEqual("1.402,95", StringUtils.format(StringUtils.CurrencyFrom("1,402.95").ToString(), "###.###.###,##"));
            Assert.AreEqual("85,13", StringUtils.format(StringUtils.CurrencyFrom("R$ 85.13").ToString(), "###.###.###,##"));
        }

        [TestMethod]
        public void RemoveAccentsTest()
        {
            Assert.AreEqual("A", StringUtils.RemoveAccents("Á"));
            Assert.AreEqual("a", StringUtils.RemoveAccents("á"));
            Assert.AreEqual("A", StringUtils.RemoveAccents("Â"));
            Assert.AreEqual("a", StringUtils.RemoveAccents("â"));
            Assert.AreEqual("A", StringUtils.RemoveAccents("Â"));
            Assert.AreEqual("a", StringUtils.RemoveAccents("â"));
            Assert.AreEqual("A", StringUtils.RemoveAccents("À"));
            Assert.AreEqual("a", StringUtils.RemoveAccents("à"));
            Assert.AreEqual("A", StringUtils.RemoveAccents("Å"));
            Assert.AreEqual("a", StringUtils.RemoveAccents("å"));
            Assert.AreEqual("A", StringUtils.RemoveAccents("Ã"));
            Assert.AreEqual("a", StringUtils.RemoveAccents("ã"));
            Assert.AreEqual("A", StringUtils.RemoveAccents("Ä"));
            Assert.AreEqual("a", StringUtils.RemoveAccents("ä"));

            Assert.AreEqual("E", StringUtils.RemoveAccents("É"));
            Assert.AreEqual("e", StringUtils.RemoveAccents("é"));
            Assert.AreEqual("E", StringUtils.RemoveAccents("Ê"));
            Assert.AreEqual("e", StringUtils.RemoveAccents("ê"));
            Assert.AreEqual("E", StringUtils.RemoveAccents("È"));
            Assert.AreEqual("e", StringUtils.RemoveAccents("è"));
            Assert.AreEqual("E", StringUtils.RemoveAccents("Ë"));
            Assert.AreEqual("e", StringUtils.RemoveAccents("ë"));

            Assert.AreEqual("I", StringUtils.RemoveAccents("Í"));
            Assert.AreEqual("i", StringUtils.RemoveAccents("í"));
            Assert.AreEqual("I", StringUtils.RemoveAccents("Î"));
            Assert.AreEqual("i", StringUtils.RemoveAccents("î"));
            Assert.AreEqual("I", StringUtils.RemoveAccents("Ì"));
            Assert.AreEqual("i", StringUtils.RemoveAccents("ì"));
            Assert.AreEqual("I", StringUtils.RemoveAccents("Ï"));
            Assert.AreEqual("i", StringUtils.RemoveAccents("ï"));

            Assert.AreEqual("O", StringUtils.RemoveAccents("Ó"));
            Assert.AreEqual("o", StringUtils.RemoveAccents("ó"));
            Assert.AreEqual("O", StringUtils.RemoveAccents("Ô"));
            Assert.AreEqual("o", StringUtils.RemoveAccents("ô"));
            Assert.AreEqual("O", StringUtils.RemoveAccents("Ò"));
            Assert.AreEqual("o", StringUtils.RemoveAccents("ò"));
            Assert.AreEqual("O", StringUtils.RemoveAccents("Õ"));
            Assert.AreEqual("o", StringUtils.RemoveAccents("õ"));
            Assert.AreEqual("O", StringUtils.RemoveAccents("Ö"));
            Assert.AreEqual("o", StringUtils.RemoveAccents("ö"));

            Assert.AreEqual("U", StringUtils.RemoveAccents("Ú"));
            Assert.AreEqual("u", StringUtils.RemoveAccents("ú"));
            Assert.AreEqual("U", StringUtils.RemoveAccents("Û"));
            Assert.AreEqual("u", StringUtils.RemoveAccents("û"));
            Assert.AreEqual("U", StringUtils.RemoveAccents("Ù"));
            Assert.AreEqual("u", StringUtils.RemoveAccents("ù"));
            Assert.AreEqual("U", StringUtils.RemoveAccents("Ü"));
            Assert.AreEqual("u", StringUtils.RemoveAccents("ü"));

            Assert.AreEqual("C", StringUtils.RemoveAccents("Ç"));
            Assert.AreEqual("c", StringUtils.RemoveAccents("ç"));

            Assert.AreEqual("N", StringUtils.RemoveAccents("Ñ"));
            Assert.AreEqual("n", StringUtils.RemoveAccents("ñ"));

            Assert.AreEqual("Y", StringUtils.RemoveAccents("Ý"));
            Assert.AreEqual("y", StringUtils.RemoveAccents("ý"));
        }

        [TestMethod]
        public void ExtractEmailAddressTest()
        {
            // Validações verdadeiras.
            Assert.AreEqual("nomecomprido@enkilabs.com.br", StringUtils.ExtractEmailAddress("nomecomprido@enkilabs.com.br"));
            Assert.AreEqual("nomecomprido_cs@hotmail.com", StringUtils.ExtractEmailAddress("nomecomprido_cs@hotmail.com"));
            Assert.AreEqual("nomecomprido.coelho@hotmail.com", StringUtils.ExtractEmailAddress("nomecomprido.coelho@hotmail.com"));
            Assert.AreEqual("nomecomprido123@hotmail.com", StringUtils.ExtractEmailAddress("nomecomprido123@hotmail.com"));
            Assert.AreEqual("123nomecomprido@teste.com", StringUtils.ExtractEmailAddress("123nomecomprido@teste.com"));
            Assert.AreEqual("relacoesinstitucionais@williamfreire.com.br", StringUtils.ExtractEmailAddress("relacoesinstitucionais@williamfreire.com.br"));
            Assert.AreEqual("sac@sede.embrapa", StringUtils.ExtractEmailAddress("sac@sede.embrapa"));
            Assert.AreEqual("nomecomprido@enkilabs.com.br", StringUtils.ExtractEmailAddress("nomecomprido Coelho Sartorelli <nomecomprido@enkilabs.com.br>"));
            Assert.AreEqual("nomecomprido@enkilabs.com.br", StringUtils.ExtractEmailAddress("nomecomprido Coelho Sartorelli <   nomecomprido@enkilabs.com.br >"));
            Assert.AreEqual("nomecomprido@enkilabs.software", StringUtils.ExtractEmailAddress("nomecomprido@enkilabs.software"));
            Assert.AreEqual("nomecomprido@123enkilabs.software", StringUtils.ExtractEmailAddress("nomecomprido@123enkilabs.software"));
            Assert.AreEqual("nomecomprido@gov.br", StringUtils.ExtractEmailAddress("nomecomprido@gov.br"));
            Assert.AreEqual("nome.sobrenome@br.ey.com", StringUtils.ExtractEmailAddress("nome.sobrenome@br.ey.com"));
            Assert.AreEqual("livia.pereira@am.sebrae.com.br", StringUtils.ExtractEmailAddress("livia.pereira@am.sebrae.com.br"));
            Assert.AreEqual("meunome@teste.com", StringUtils.ExtractEmailAddress("Meu nome(meunome@teste.com) <meunome@teste.com>"));

            // Validações falsas.
            Assert.AreEqual("", StringUtils.ExtractEmailAddress("nomecomprido@"));
            Assert.AreEqual("", StringUtils.ExtractEmailAddress("@tagged.com"));
            Assert.AreEqual("", StringUtils.ExtractEmailAddress("MAILER-DAEMON(MailDeliverySystem"));
            Assert.AreEqual("", StringUtils.ExtractEmailAddress("nomecomprido@_enkilabs.software"));
            Assert.AreEqual("", StringUtils.ExtractEmailAddress("scan11@com.br."));
        }

        [TestMethod]
        public void ExtractEmailNameTest()
        {
            // Validações verdadeiras.
            Assert.AreEqual("nomecomprido Coelho Sartorelli", StringUtils.ExtractEmailName("nomecomprido Coelho Sartorelli <nomecomprido@enkilabs.com.br>"));
            Assert.AreEqual("nomecomprido Coelho Sartorelli", StringUtils.ExtractEmailName("nomecomprido Coelho Sartorelli <   nomecomprido@enkilabs.com.br >"));
            Assert.AreEqual("Meu nome(meunome@teste.com)", StringUtils.ExtractEmailName("Meu nome(meunome@teste.com) <meunome@teste.com>"));

            // Validações falsas.
            Assert.AreEqual("", StringUtils.ExtractEmailName("nomecomprido@"));
            Assert.AreEqual("", StringUtils.ExtractEmailName("@tagged.com"));
            Assert.AreEqual("", StringUtils.ExtractEmailName("MAILER-DAEMON(MailDeliverySystem"));
            Assert.AreEqual("", StringUtils.ExtractEmailName("nomecomprido@_enkilabs.software"));
            Assert.AreEqual("", StringUtils.ExtractEmailName("scan11@com.br."));
            Assert.AreEqual("", StringUtils.ExtractEmailName("nomecomprido@enkilabs.com.br"));
            Assert.AreEqual("", StringUtils.ExtractEmailName("nomecomprido_cs@hotmail.com"));
            Assert.AreEqual("", StringUtils.ExtractEmailName("nomecomprido.coelho@hotmail.com"));
            Assert.AreEqual("", StringUtils.ExtractEmailName("nomecomprido123@hotmail.com"));
            Assert.AreEqual("", StringUtils.ExtractEmailName("123nomecomprido@teste.com"));
            Assert.AreEqual("", StringUtils.ExtractEmailName("relacoesinstitucionais@williamfreire.com.br"));
            Assert.AreEqual("", StringUtils.ExtractEmailName("sac@sede.embrapa"));
            Assert.AreEqual("", StringUtils.ExtractEmailName("nomecomprido@enkilabs.software"));
            Assert.AreEqual("", StringUtils.ExtractEmailName("nomecomprido@123enkilabs.software"));
            Assert.AreEqual("", StringUtils.ExtractEmailName("nomecomprido@gov.br"));
            Assert.AreEqual("", StringUtils.ExtractEmailName("nome.sobrenome@br.ey.com"));
            Assert.AreEqual("", StringUtils.ExtractEmailName("livia.pereira@am.sebrae.com.br"));
        }
    }
}