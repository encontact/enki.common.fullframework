using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;

namespace Enki.Common.WebUtils.Tests
{
    [TestClass()]
    public class HtmlToTextTests
    {
        [TestMethod()]
        public void MustProcessHtmlWithComment()
        {
            var html = @"<html xmlns:v=""urn:schemas-microsoft-com:vml"" xmlns:o=""urn:schemas-microsoft-com:office:office"" xmlns:w=""urn:schemas-microsoft-com:office:word"" xmlns:m=""http://schemas.microsoft.com/office/2004/12/omml"" xmlns=""http://www.w3.org/TR/REC-html40""><head><meta http-equiv=Content-Type content=""text/html; charset=iso-8859-1""><meta name=Generator content=""Microsoft Word 15 (filtered medium)""><!--[if !mso]><style>v\:* {behavior:url(#default#VML);}
o\:* {behavior:url(#default#VML);}
w\:* {behavior:url(#default#VML);}
.shape {behavior:url(#default#VML);}
</style><![endif]--><style><!--
/* Font Definitions */
@font-face
	{font-family:Calibri;
	panose-1:2 15 5 2 2 2 4 3 2 4;}
/* Style Definitions */
p.MsoNormal, li.MsoNormal, div.MsoNormal
	{margin:0cm;
	margin-bottom:.0001pt;
	font-size:11.0pt;
	font-family:""Calibri"",sans-serif;
	mso-fareast-language:EN-US;}
a:link, span.MsoHyperlink
	{mso-style-priority:99;
	color:#0563C1;
	text-decoration:underline;}
a:visited, span.MsoHyperlinkFollowed
	{mso-style-priority:99;
	color:#954F72;
	text-decoration:underline;}
span.EstiloDeEmail17
	{mso-style-type:personal-compose;
	font-family:""Calibri"",sans-serif;
	color:windowtext;}
.MsoChpDefault
	{mso-style-type:export-only;
	font-family:""Calibri"",sans-serif;
	mso-fareast-language:EN-US;}
@page WordSection1
	{size:612.0pt 792.0pt;
	margin:70.85pt 3.0cm 70.85pt 3.0cm;}
div.WordSection1
	{page:WordSection1;}
--></style><!--[if gte mso 9]><xml>
<o:shapedefaults v:ext=""edit"" spidmax=""1026"" />
</xml><![endif]--><!--[if gte mso 9]><xml>
<o:shapelayout v:ext=""edit"">
<o:idmap v:ext=""edit"" data=""1"" />
</o:shapelayout></xml><![endif]--></head><body lang=PT-BR link=""#0563C1"" vlink=""#954F72""><div class=WordSection1><p class=MsoNormal>Vamos colocar uma imagem supimpa???<o:p></o:p></p><p class=MsoNormal><o:p>&nbsp;</o:p></p><p class=MsoNormal><span style='mso-fareast-language:PT-BR'><img width=248 height=628 id=""Imagem_x0020_1"" src=""cid:image001.png@01D1C727.AF34A550""></span><o:p></o:p></p></div></body></html>";
            var converter = new HtmlToText(html);
            Assert.AreEqual("Vamos colocar uma imagem supimpa???", converter.GetText());
        }
        [TestMethod()]
        public void MustProcessHtmlWithStyleTag()
        {
            var html = @"
            <html xmlns:v=""urn:schemas-microsoft-com:vml"" xmlns:o=""urn:schemas-microsoft-com:office:office"" xmlns:w=""urn:schemas-microsoft-com:office:word"" xmlns:m=""http://schemas.microsoft.com/office/2004/12/omml"" xmlns=""http://www.w3.org/TR/REC-html40"">
                <head>
                    <style>
                        * {behavior:url(#default#VML);}
                    </style>
                </head>
                <body lang=PT-BR link=""#0563C1"" vlink=""#954F72"">
                    <div class=WordSection1>
                        <p class=MsoNormal>Vamos colocar uma imagem supimpa???<o:p></o:p></p>
                        <p class=MsoNormal><o:p>&nbsp;</o:p></p><p class=MsoNormal><span style='mso-fareast-language:PT-BR'>
                        <img width=248 height=628 id=""Imagem_x0020_1"" src=""cid:image001.png@01D1C727.AF34A550""></span><o:p></o:p></p>
                    </div>
                </body>
            </html>";
            var converter = new HtmlToText(html);
            Assert.AreEqual("Vamos colocar uma imagem supimpa???", converter.GetText());
        }
        [TestMethod()]
        public void MustProcessHtmlWithScriptTag()
        {
            var html = @"
            <html xmlns:v=""urn:schemas-microsoft-com:vml"" xmlns:o=""urn:schemas-microsoft-com:office:office"" xmlns:w=""urn:schemas-microsoft-com:office:word"" xmlns:m=""http://schemas.microsoft.com/office/2004/12/omml"" xmlns=""http://www.w3.org/TR/REC-html40"">
                <head>
                    <style>
                        * {behavior:url(#default#VML);}
                    </style>
                </head>
                <body lang=PT-BR link=""#0563C1"" vlink=""#954F72"">
                    <div class=WordSection1>
                        <p class=MsoNormal>Vamos colocar uma imagem supimpa???<o:p></o:p></p>
                        <p class=MsoNormal><o:p>&nbsp;</o:p></p><p class=MsoNormal><span style='mso-fareast-language:PT-BR'>
                        <img width=248 height=628 id=""Imagem_x0020_1"" src=""cid:image001.png@01D1C727.AF34A550""></span><o:p></o:p></p>
                    </div>
                </body>
                <script>
                    function test(){
                        alert('This is a Test');
                    }
                </script>
            </html>";
            var converter = new HtmlToText(html);
            Assert.AreEqual("Vamos colocar uma imagem supimpa???", converter.GetText());
        }
        [TestMethod()]
        [Ignore()]
        public void MustProcessHtmlWithUnicodeChars()
        {
            // TODO: A string criada abaixo não está representando um texto unicode real. Verificar para validar.
            var html = Encoding.Unicode.GetString(Encoding.Unicode.GetBytes(@"
            <html xmlns:v=""urn:schemas-microsoft-com:vml"" xmlns:o=""urn:schemas-microsoft-com:office:office"" xmlns:w=""urn:schemas-microsoft-com:office:word"" xmlns:m=""http://schemas.microsoft.com/office/2004/12/omml"" xmlns=""http://www.w3.org/TR/REC-html40"">
                <head>
                    <style>
                        * {behavior:url(#default#VML);}
                    </style>
                </head>
                <body lang=PT-BR link=""#0563C1"" vlink=""#954F72"">
                    <p class=3DMsoNormal>Esta mensagem foi verificada pelo sistema de antiv\uDCB5s e  acredita-se estar livre de perigo.</p>
                </body>
                <script>
                    function test(){
                        alert('This is a Test');
                    }
                </script>
            </html>"));
            var converter = new HtmlToText(html);
            var result = converter.GetText();
            Assert.AreEqual("Esta mensagem foi verificada pelo sistema de antiv?s e acredita-se estar livre de perigo. Validado com atenção.", result);
        }
    }
}

