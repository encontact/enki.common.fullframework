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

        public string GetText()
        {
            var removedStyleAndScript = Regex.Replace(OriginalHtml, "(<style.+?</style>)|(<script.+?</script>)", "", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            var removedTags = Regex.Replace(removedStyleAndScript, "<[^>]+>", " ", RegexOptions.IgnoreCase);
            var decodedHtml = HttpUtility.HtmlDecode(removedTags);
            return decodedHtml.Trim();
        }
    }
}
