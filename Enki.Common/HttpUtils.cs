using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace Enki.Common {
	public static class HttpUtils {
		/// <summary>
		/// Recupera uma URL através de chamada no método GET, recuperando seu conteúdo exato em texto.
		/// </summary>
		/// <param name="url">Url a ser carregada</param>
		/// <param name="proxy">Opcional, proxy de acesso</param>
		/// <param name="proxyPort">Opcional, porta do proxy</param>
		/// <param name="headers">Opcional, cabeçalhos a serem enviados na chamada</param>
		/// <param name="timeOutSeconds">Opcional, segundos para timeout da chamada</param>
		/// <returns>String com o conteúdo recuperado.</returns>
		public static string GetUrl(string url, string proxy = "noproxy", int proxyPort = 80, List<string> headers = null, int timeOutSeconds = 100){
			var wr = CreateRequest(url);

			// Define o timeout da chamada
			wr.Timeout = timeOutSeconds * 1000;

			// Se informado, acrescenta proxy
			if (proxy != "noproxy") {
				var myProxy = new WebProxy(proxy, proxyPort) { BypassProxyOnLocal = true };
				wr.Proxy = myProxy;
			} else {
				wr.Proxy = WebRequest.DefaultWebProxy;
			}

			// Se informado, acrescenta Headers HTTP como parâmetro.
			if (headers != null && headers.Count > 0) {
				var headersHttp = wr.Headers;
				foreach (var head in headers) {
					headersHttp.Add(head);
				}
			}

			using (var response = GetResponse(wr)) {
				using (var stream = response.GetResponseStream()) {
					using (var sr = new StreamReader(stream, Encoding.GetEncoding(response.CharacterSet))) {
						return sr.ReadToEnd();
					}
				}
			}
		}
		/// <summary>
		/// Recupera uma página Web, tratando os links de CSS e Imagem para que apontem para o site de origem.
		/// </summary>
		/// <param name="url">Url a ser carregada</param>
		/// <param name="proxy">Opcional, proxy de acesso</param>
		/// <param name="proxyPort">Opcional, porta do proxy</param>
		/// <param name="headers">Opcional, cabeçalhos a serem enviados na chamada</param>
		/// <param name="timeOutSeconds">Opcional, segundos para timeout da chamada</param>
		/// <returns>Html da página com ajustes nos endereços e CSS e Imagens.</returns>
		public static string GetWebPage(string url, string proxy = "noproxy", int proxyPort = 80, List<string> headers = null, int timeOutSeconds = 100) {
			var wr = CreateRequest(url);
			wr.Headers.Add("Accept-Charset", "utf-8");
			// Define o timeout da chamada
			wr.Timeout = timeOutSeconds * 1000;

			// Se informado, acrescenta proxy
			if (proxy != "noproxy") {
				var myProxy = new WebProxy(proxy, proxyPort) { BypassProxyOnLocal = true };
				wr.Proxy = myProxy;
			} else {
				wr.Proxy = WebRequest.DefaultWebProxy;
			}

			// Se informado, acrescenta Headers HTTP como parâmetro.
			if (headers != null && headers.Count > 0) {
				var headersHttp = wr.Headers;
				foreach (var head in headers) {
					headersHttp.Add(head);
				}
			}

			var htmlCode = "";
			using (var response = GetResponse(wr)) {
				using (var stream = response.GetResponseStream()) {
					using (var sr = new StreamReader(stream, Encoding.UTF8)) {
						 htmlCode = sr.ReadToEnd();
					}
				}
			}

			// Recupera a URI do site para substituição.
			var uri = new Uri(url);
			var baseSubstitution = "";
			if (htmlCode.Contains("<base href=")) {
				var startIndex = htmlCode.IndexOf("<base href=") + 12;
				var endIndex = htmlCode.IndexOf(">", startIndex) -2;
				baseSubstitution = htmlCode.Substring(startIndex, endIndex-startIndex);
			} else {
				baseSubstitution = uri.Scheme + "://" + uri.Host;
			}
			for (int i = 0; i < uri.Segments.Length; i++) {
				var endUri = "";
				for (int j = 0; j < uri.Segments.Length; j++) {
					if (j <= i) endUri += uri.Segments[j];
				}
				var repeatCount = uri.Segments.Length - (2 + i);
				if(repeatCount > 0){
					var stringToReplace = String.Concat(Enumerable.Repeat("../", repeatCount));
					htmlCode = htmlCode.Replace(stringToReplace, baseSubstitution + (endUri != "/" ? "/" + endUri : endUri));
				} else if (repeatCount == 0) {
					// Ajusta Urls que não começam com http, com base da url da url acessada.
					htmlCode = Regex.Replace(htmlCode, "((?<=href=\")[^http](.*?)+?(?=\"))", baseSubstitution + "/$&", RegexOptions.IgnoreCase | RegexOptions.Multiline);
					htmlCode = Regex.Replace(htmlCode, "((?<=src=\")[^http](.*?)+?(?=\"))", baseSubstitution + "/$&", RegexOptions.IgnoreCase | RegexOptions.Multiline);
				}
			}
			return htmlCode;
		}
		public static string PostUrl(string url, List<string> parameters, List<string> headers = null, string proxy = "noproxy", int proxyPort = 80, int timeOutSeconds = 100) {
			var wr = CreateRequest(url);
			wr.Method = "POST";

			// Define o timeout da chamada
			wr.Timeout = timeOutSeconds * 1000;

			// Set the encoding type
			wr.ContentType = "application/x-www-form-urlencoded";

			// Se informado, acrescenta proxy
			if (proxy != "noproxy") {
				var myProxy = new WebProxy(proxy, proxyPort) { BypassProxyOnLocal = true };
				wr.Proxy = myProxy;
			} else {
				wr.Proxy = WebRequest.DefaultWebProxy;
			}

			// Se informado, acrescenta Headers HTTP como parâmetro.
			if (headers != null && headers.Count > 0) {
				var headersHttp = wr.Headers;
				foreach (var head in headers) {
					headersHttp.Add(head);
				}
			}

			// Build a string containing all the parameters
			var formParameters = String.Join("&", parameters);
			wr.ContentLength = Encoding.UTF8.GetBytes(formParameters).Length;

			using (var stream = wr.GetRequestStream()) {
				using (var sw = new StreamWriter(stream)) {
					sw.Write(formParameters);
					sw.Flush();
					sw.Close();
				}
			}
			using (var response = GetResponse(wr)) {
				using (var stream = response.GetResponseStream()) {
					using (var sr = new StreamReader(stream, Encoding.GetEncoding(response.CharacterSet))) {
						return sr.ReadToEnd();
					}
				}
			}
		}
		public static Dictionary<string,string> GetResponseHeadersByGet(string url) {
			var wr = CreateRequest(url);

			using (var response = GetResponse(wr)) {
				var ret = new Dictionary<string, string>();
				foreach (var key in response.Headers.AllKeys) ret.Add(key, response.Headers[key]);
				return ret;
			}
		}
		public static HttpWebRequest CreateRequest(string url) {
			return (HttpWebRequest)HttpWebRequest.Create(url);
		}
		public static HttpWebResponse GetResponse(HttpWebRequest request) {
			return (HttpWebResponse)request.GetResponse();
		}
        /// <summary>
        /// Recupera o conteúdo de uma URL em forma de um binário.
        /// Util para download de arquivos ou imagens de uma URL.
        /// </summary>
        /// <param name="url">Url do arquivo ou imagem. Ex: http://www.google.com/images/logos/ps_logo2.png </param>
        /// <returns>Array de bytes do arquivo.</returns>
        public static byte[] GetBytes(string url) {
            return new WebClient().DownloadData(url);
        }
    }
}
