using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Enki.Common.WebUtils
{
    public class HtmlToText
    {
        public string OriginalHtml { get; private set; }

        public HtmlToText(string html)
        {
            OriginalHtml = html;
        }

        /// <summary>
        /// Recupera o texto do HTML
        /// </summary>
        /// <returns>Texto puro em UTF-8 do Html.</returns>
        public string GetText()
        {
            return GetText(Encoding.UTF8);
        }

        /// <summary>
        /// Recupera o texto do Html
        /// </summary>
        /// <param name="encode">Encode de retorno desejado.</param>
        /// <returns></returns>
        public string GetText(Encoding encode)
        {
            var lineBreakReplaced = Regex.Replace(OriginalHtml, "<br>|<br/>", "\r\n", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            var paragraphReplaced = Regex.Replace(lineBreakReplaced, "<p>|</p>", "\r\n", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            var divReplaced = Regex.Replace(paragraphReplaced, "<div>|</div>", "\r\n", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            var removedStyleAndScript = Regex.Replace(paragraphReplaced, "(<style.+?</style>)|(<script.+?</script>)", "", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            var removedTags = Regex.Replace(removedStyleAndScript, "<[^>]+>", " ", RegexOptions.IgnoreCase);
            var decodedHtml = HttpUtility.HtmlDecode(removedTags);
            var final = EncodeString(decodedHtml, encode);
            return final.Trim();
        }

        /// <summary>
        /// Converte o texto informado para o encode Desejado.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private string EncodeString(string text, Encoding encode)
        {
            byte[] utf = encode.GetBytes(text);
            string strEncoded = encode.GetString(utf);
            return strEncoded;
        }
    }
}
