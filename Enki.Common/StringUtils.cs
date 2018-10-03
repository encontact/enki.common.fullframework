using Enki.Common.RegionalUtils;
using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Enki.Common
{
    public static class StringUtils
    {
        public const string EmailRegExp = @"(^[\w\+\=\-\.\&]+@[a-zA-Z0-9]{1}(?:[a-zA-Z0-9\-]*?|[\.]{1})[a-zA-Z0-9]{1,}(?:\.{1}[a-zA-Z0-9\-]{2,})+?$)|^([^<>]*?)<(\s*[\w\+\=\-\.\&]+@[a-zA-Z0-9]{1}(?:[a-zA-Z0-9\-]*?|[\.]{1})[a-zA-Z0-9]{1,}(?:\.{1}[a-zA-Z0-9]{2,})+?\s*)>$";

        /// <summary>
        /// Formata uma string para estrutura de CNPJ
        /// </summary>
        /// <param name="cnpj"></param>
        /// <returns></returns>
        [Obsolete("Será descontinuado por ser específico. Utilizar BrazilianUtilities.FormatCnpj")]
        public static string formatCnpj(string cnpj)
        {
            return BrazilianUtils.FormatCnpj(cnpj);
        }

        /// <summary>
        /// Formata uma string para estrutura de CPF
        /// </summary>
        /// <param name="cpf"></param>
        /// <returns></returns>
        [Obsolete("Será descontinuado por ser específico. Utilizar BrazilianUtilities.FormatCpf")]
        public static string formatCpf(string cpf)
        {
            return BrazilianUtils.FormatCpf(cpf);
        }

        /// <summary>
        /// Formata uma string para estrutura de CEP
        /// </summary>
        /// <param name="cpf"></param>
        /// <returns></returns>
        [Obsolete("Será descontinuado por ser específico. Utilizar BrazilianUtilities.FormatCep")]
        public static string formatCep(string cep)
        {
            return BrazilianUtils.FormatCep(cep);
        }

        /// <summary>
        /// Formata Strings de acordo com o informado. Ex: ##/##/#### ou ##.###,##
        /// </summary>
        /// <param name="valor"></param>
        /// <param name="mascara"></param>
        /// <returns></returns>
        public static string format(string valor, string mascara)
        {
            StringBuilder dado = new StringBuilder();
            // remove caracteres nao numericos
            foreach (char c in valor)
            {
                if (char.IsNumber(c))
                    dado.Append(c);
            }
            int indMascara = mascara.Length;
            int indCampo = dado.Length;
            for (; indCampo > 0 && indMascara > 0;)
            {
                if (mascara[--indMascara] == '#')
                    indCampo--;
            }
            StringBuilder saida = new StringBuilder();
            for (; indMascara < mascara.Length; indMascara++)
            {
                saida.Append((mascara[indMascara] == '#') ? dado[indCampo++] : mascara[indMascara]);
            }
            return saida.ToString();
        }

        /// <summary>
        /// Encoda uma string qualquer em BASE64
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string base64Encode(string data)
        {
            try
            {
                byte[] encData_byte = new byte[data.Length];
                encData_byte = Encoding.UTF8.GetBytes(data);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch (Exception e)
            {
                throw new Exception("Error in base64Encode" + e.Message);
            }
        }

        /// <summary>
        /// Decoda uma string em BASE64 para a String original
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string base64Decode(string data)
        {
            try
            {
                UTF8Encoding encoder = new UTF8Encoding();
                Decoder utf8Decode = encoder.GetDecoder();

                byte[] todecode_byte = Convert.FromBase64String(data);
                int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
                char[] decoded_char = new char[charCount];
                utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
                string result = new String(decoded_char);
                return result;
            }
            catch (Exception e)
            {
                throw new Exception("Error in base64Decode" + e.Message);
            }
        }

        [Obsolete("Utilizar ao invés ExtractEmailAddress()")]
        public static string getEmailAddressFrom(string stringWithEmail)
        {
            return ExtractEmailAddress(stringWithEmail);
        }

        /// <summary>
        /// Recupera o e-mail contido num texto, por exemplo "Reinaldo Coelho Sartorelli <reinaldo@enkiconsultoira.com.br>" retorna "reinaldo@enkiconsultoria.com.br"
        /// </summary>
        /// <param name="text">Texto a ser processado.</param>
        /// <returns>Email encontrado.</returns>
        public static string ExtractEmailAddress(string text)
        {
            string emailTo = text.Trim();
            var emailGroups = Regex.Match(emailTo.ToLower(), EmailRegExp).Groups;
            var emailAddress = !string.IsNullOrEmpty(emailGroups[1]?.Value) ? emailGroups[1]?.Value : emailGroups[3]?.Value;
            return emailAddress.Trim();
        }

        [Obsolete("Utilizar ao invés ExtractEmailName()")]
        public static string getEmailNameFrom(string stringWithEmail)
        {
            return ExtractEmailName(stringWithEmail);
        }

        /// <summary>
        /// Recupera o nome contido num texto de email, por exemplo "Reinaldo Coelho Sartorelli <reinaldo@enkiconsultoira.com.br>" retorna "Reinaldo Coelho Sartorelli"
        /// </summary>
        /// <param name="text">Texto a ser processado.</param>
        /// <returns>Nome encontrado.</returns>
        public static string ExtractEmailName(string text)
        {
            string emailTo = text.Trim();
            var emailGroups = Regex.Match(emailTo, EmailRegExp).Groups;
            var name = emailGroups[2]?.Value ?? "";
            return name.Trim();
        }

        /// <summary>
        /// Remove acentos de uma string qualquer.
        /// </summary>
        /// <param name="text">Texto para remover os acentos.</param>
        /// <returns>Mesma string sem os acentos.</returns>
        public static string RemoveAccents(string text)
        {
            try
            {
                StringBuilder sbReturn = new StringBuilder();
                var arrayText = text.Normalize(NormalizationForm.FormD).ToCharArray();

                foreach (char letter in arrayText)
                {
                    if (CharUnicodeInfo.GetUnicodeCategory(letter) != UnicodeCategory.NonSpacingMark)
                        sbReturn.Append(letter);
                }
                return sbReturn.ToString();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Remove qualquer caractere do texto que não seja um texto de A-Z/a-z ou numérico de 0-9
        /// </summary>
        /// <param name="text">Texto original</param>
        /// <returns>Texto tratado</returns>
        public static string GetOnlyLettersAndNumbers(string text)
        {
            Regex rgx = new Regex("[^a-zA-Z0-9]");
            return rgx.Replace(text, "");
        }

        /// <summary>
        /// A partir de um texto representando um valor monetário, exemplo 99.655,00 ou 99,655.00 ou 99.5, recupera o Double equivalente.
        /// </summary>
        /// <param name="text">Texto representando o valor monetário que se quer transformar</param>
        /// <param name="DecimalSeparator">Separador de casa decimal a ser utilizada.</param>
        /// <param name="GroupSeparator">Separador de grupo numério.</param>
        /// <returns>Double equivalente ao valor do texto.</returns>
        public static double CurrencyFrom(string text, string DecimalSeparator = ",", string GroupSeparator = ".")
        {
            var numberFormat = new NumberFormatInfo();
            numberFormat.NumberDecimalSeparator = DecimalSeparator;
            numberFormat.NumberGroupSeparator = GroupSeparator;

            var originalText = text;
            text = "000" + text.Replace("R$", "").Trim();
            // Processa logica de conversão
            var finalText = "";
            var decimalPoint = text.Substring(text.Length - 3, 1);
            if (decimalPoint != "." && decimalPoint != ",")
            {
                var lastPointIndex = text.Substring(text.Length - 3).LastIndexOf('.');
                var lastCommaIndex = text.Substring(text.Length - 3).LastIndexOf(',');
                if (lastPointIndex == -1 && lastCommaIndex == -1) decimalPoint = "";
                else if ((lastCommaIndex != -1 && lastPointIndex != -1) && (lastCommaIndex > lastPointIndex)) decimalPoint = ",";
                else if ((lastCommaIndex != -1 && lastPointIndex != -1) && (lastPointIndex > lastCommaIndex)) decimalPoint = ".";
                else if (lastCommaIndex != -1 && lastPointIndex == -1) decimalPoint = ",";
                else if (lastCommaIndex == -1 && lastPointIndex != -1) decimalPoint = ".";
            }
            switch (decimalPoint)
            {
                case ".":
                    finalText = text.Replace(",", "").Replace(".", ",");
                    break;

                case ",":
                    finalText = text.Replace(".", "");
                    break;

                default:
                    finalText = text.Replace(".", "").Replace(",", "");
                    break;
            }
            var result = 0.00;
            if (!Double.TryParse(finalText, out result))
            {
                throw new System.InvalidOperationException("O valor " + originalText + " não pode ser convertido para double.");
            }
            return Convert.ToDouble(finalText, numberFormat);
        }
    }
}