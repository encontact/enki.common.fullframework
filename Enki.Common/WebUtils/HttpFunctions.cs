using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Web;
using System.Collections;

namespace Enki.Common {
	/// <summary>
	/// Responsável por manter interfaces de acesso a endereços HTTP externos.
	/// </summary>
	public static class HttpFunctions {
		/// <summary>
		/// Container de cookies para permitir o recebimento de cookies caso necessário.
		/// </summary>
		private static readonly CookieContainer Cookies = new CookieContainer();

		/// <summary>
		/// Recupera o texto retornado a partir da chamada de uma URL, ideal para chamadas a API Rest por método get.
		/// </summary>
		/// <param name="url">URl a ser acessada</param>
		/// <param name="proxy">Opcional, Nome do Proxy. Padrão: noproxy</param>
		/// <param name="proxyPort">Opcional, Porta do Proxy. Padrão: 80</param>
		/// <param name="headers">Opcional, Lista de variáveis de Header a serem utilizados. Padrão: null. Exemplo: "chave:valor;chave:valor" ou seja "Accept-Language:en;q=0.8" </param>
		/// <param name="timeOutSeconds">Opcional, Timeout em segundos a ser utilizado para aguardar o retorno da chamada. Padrão 100 segundos.</param>
		/// <returns>Texto retornado pela URL</returns>
		public static String getUrl(String url, String proxy = "noproxy", int proxyPort = 80, List<string> headers = null, int timeOutSeconds = 100) {
			var wrGeturl = WebRequest.Create(url);

			// Define timeout para a chamada
			wrGeturl.Timeout = timeOutSeconds * 1000;

			// Se informado, acrescenta Headers HTTP como parâmetro.
			if (headers != null && headers.Count > 0) {
				var headersHttp = wrGeturl.Headers;
				foreach (var head in headers) {
					headersHttp.Add(head);
				}
			}

			// Se informado, acrescenta proxy
			if (proxy != "noproxy") {
				var myProxy = new WebProxy(proxy, proxyPort) { BypassProxyOnLocal = true };
				wrGeturl.Proxy = myProxy;
			} else {
				wrGeturl.Proxy = WebRequest.DefaultWebProxy;
			}

			// Efetua chamada e recupera retorno
			var objStream = wrGeturl.GetResponse().GetResponseStream();
			if (objStream != null) {
				var objReader = new StreamReader(objStream);

				var sLine = "";
				var i = 0;

				var result = "";
				while (sLine != null) {
					i++;
					sLine = objReader.ReadLine();
					if (sLine != null)
						result += sLine;
				}

				return result;
			} else {
				throw new Exception("Invalid response from call.");
			}
		}

		/// <summary>
		/// Recupera o resultado de uma chamada http no método POST
		/// </summary>
		/// <param name="url">Endereço web da chamada a ser efetuada.</param>
		/// <param name="theQueryData">Parametros informados para a requisição. Exemplo: "chave=valor" ou seja "Accept-Language=en"</param>
		/// <param name="headers">Opcional, Lista de variáveis de Header a serem utilizados. Padrão: null. Exemplo: "chave:valor;chave:valor" ou seja "Accept-Language:en;q=0.8" </param>
		/// <param name="proxy">Proxy utilizado na chamada</param>
		/// <param name="proxyPort">Porta do proxy utilizado na chamada</param>
		/// <param name="timeOutSeconds">Opcional, Timeout em segundos a ser utilizado para aguardar o retorno da chamada. Padrão 100 segundos.</param>
		/// <returns>Texto retornado pela chamada</returns>
		public static String postUrl(String url, ArrayList theQueryData, List<string> headers = null, String proxy = "noproxy", int proxyPort = 80, int timeOutSeconds = 100) {
			var theRequest = (HttpWebRequest)WebRequest.Create(url);
			theRequest.Method = "POST";

			// Define o timeout da chamada
			theRequest.Timeout = timeOutSeconds * 1000;

			// Set the encoding type
			theRequest.ContentType = "application/x-www-form-urlencoded";

			// Se informado, acrescenta proxy
			if (proxy != "noproxy") {
				var myProxy = new WebProxy(proxy, proxyPort) { BypassProxyOnLocal = true };
				theRequest.Proxy = myProxy;
			} else {
				theRequest.Proxy = WebRequest.DefaultWebProxy;
			}

			// Se informado, acrescenta Headers HTTP como parâmetro.
			if (headers != null && headers.Count > 0) {
				var headersHttp = theRequest.Headers;
				foreach (var head in headers) {
					headersHttp.Add(head);
				}
			}

			// Build a string containing all the parameters
			string parameters = String.Join("&", (String[])theQueryData.ToArray(typeof(string)));
			Encoding encoding = new UTF8Encoding();
			byte[] byteLen = encoding.GetBytes(parameters);
			theRequest.ContentLength = byteLen.Length;

			// We write the parameters into the request
			if (Cookies.Count > 0) {
				theRequest.CookieContainer = Cookies;
			} else {
				theRequest.CookieContainer = new CookieContainer();
			}
			var sw = new StreamWriter(theRequest.GetRequestStream());
			sw.Write(parameters);
			sw.Flush();
			sw.Close();

			// Execute the query
			var theResponse = (HttpWebResponse)theRequest.GetResponse();
			if (theResponse.Cookies.Count > 0) {
				Cookies.Add(theResponse.Cookies);
			}
			var sr = new StreamReader(theResponse.GetResponseStream());
			return sr.ReadToEnd();
		}

		/// <summary>
		/// Recupera o resultado de uma chamada http no método POST, retornando uma string Json.
		/// NOTA: O retorno deste JSON sempre será um texto incluso num atributo "d", seguindo regra de segurança da Microsoft para impedir
		/// execução direta de javascript através do retorno JSON. (Mais em: http://encosia.com/a-breaking-change-between-versions-of-aspnet-ajax/)
		/// </summary>
		/// <param name="url">Endereço web da chamada a ser efetuada.</param>
		/// <param name="jsonParameters">Parametros em formato Json. Ex: {'nome':'teste', 'id':'15'}</param>
		/// <param name="proxy">Proxy utilizado na chamada</param>
		/// <param name="proxyPort">Porta do proxy utilizado na chamada</param>
		/// <param name="timeOutSeconds">Opcional, Timeout em segundos a ser utilizado para aguardar o retorno da chamada. Padrão 100 segundos.</param>
		/// <returns>Texto Json retornado pela chamada</returns>
		public static String postJsonUrl(String url, string jsonParameters, String proxy = "noproxy", int proxyPort = 80, int timeOutSeconds = 100) {
			// Monta cabeçalho da chamada Post para Json.
			var request = (HttpWebRequest)WebRequest.Create(url);
			request.ContentType = "application/json";
			request.Accept = "application/json, text/javascript, */*";
			request.Method = "POST";

			// Define o timeout da chamada
			request.Timeout = timeOutSeconds * 1000;

			// Se informado, acrescenta proxy
			if (proxy != "noproxy") {
				var myProxy = new WebProxy(proxy, proxyPort) { BypassProxyOnLocal = true };
				request.Proxy = myProxy;
			} else {
				request.Proxy = WebRequest.DefaultWebProxy;
			}

			// Acrescenta parametros informados.
			using (var writer = new StreamWriter(request.GetRequestStream())) {
				// Escreve os parâmetros do Webservice, Ex: "{id : 'test'}"
				writer.Write(jsonParameters);
			}

			// Efetua chamada
			var response = request.GetResponse();
			var stream = response.GetResponseStream();

			// verifica o charsert retornado
			//se nao encontrar define utf-8 como padrao
			string charset = "utf-8";
			int charsetStart = response.ContentType.IndexOf("charset=");
			if (charsetStart != -1) {
				charsetStart += 8;
				var charsetEnd = response.ContentType.IndexOfAny(new[] { ' ', '\"', ';' }, charsetStart);
				charsetEnd = (charsetEnd == -1) ? response.ContentType.Length : charsetEnd;
				charset = response.ContentType.Substring(charsetStart, charsetEnd - charsetStart);

			}

			// Recupera resultado
			var encode = Encoding.GetEncoding(charset);
			var json = "";
			using (var reader = new StreamReader(stream, encode)) {
				while (!reader.EndOfStream) {
					json += reader.ReadLine();
				}
			}

			// Verifica se o JSON está encapsulado dentro de um atributo "d", se não estiver inclui.
			// Atributo existente nos WebServices C# desde o framework 3.5 (Mais em: http://encosia.com/a-breaking-change-between-versions-of-aspnet-ajax/)
			if (json != "") {
				var parsedResult = Newtonsoft.Json.Linq.JObject.Parse(json);
				var hasDProperty = false;
				foreach (var property in parsedResult.Properties()) {
					if (property.Name == "d") {
						hasDProperty = true;
					}
				}
				// Se não encontrou propriedade "d".
				if (hasDProperty == false) {
					parsedResult = new Newtonsoft.Json.Linq.JObject { { "d", json } };
					json = Newtonsoft.Json.JsonConvert.SerializeObject(parsedResult);
				}
			}

			// Retorna string json.
			return json;
		}

		/// <summary>
		/// Retorna uma chamada HTTP GET completa através de um objeto HttpWebResponse
		/// </summary>
		/// <param name="url">URl a ser acessada</param>
		/// <param name="proxyName">Opcional, Nome do Proxy. Padrão: noproxy</param>
		/// <param name="proxyPort">Opcional, Porta do Proxy. Padrão: 80</param>
		/// <param name="headers">Opcional, Lista de variáveis de Header a serem utilizados na chamada. Padrão: null. Exemplo: "chave:valor;chave:valor" ou seja "Accept-Language:en;q=0.8" </param>
		/// <param name="timeOutSeconds">Opcional, Timeout em segundos a ser utilizado para aguardar o retorno da chamada. Padrão 100 segundos.</param>
		/// <returns>Objeto com todas as informações da chamada GET</returns>
		public static HttpWebResponse getResponseHeadersByGet(String url, String proxyName = "noproxy", int proxyPort = 80, List<string> headers = null, int timeOutSeconds = 100) {
			var wrGeturl = WebRequest.Create(url);

			// Define o timeout da chamada
			wrGeturl.Timeout = timeOutSeconds * 1000;

			// Se informado, acrescenta Headers HTTP como parâmetro.
			if (headers != null && headers.Count > 0) {
				var headersHttp = wrGeturl.Headers;
				foreach (var head in headers) {
					headersHttp.Add(head);
				}
			}

			// Se informado, acrescenta proxy
			if (proxyName != "noproxy") {
				var myProxy = new WebProxy(proxyName, proxyPort) { BypassProxyOnLocal = true };
				wrGeturl.Proxy = myProxy;
			} else {
				wrGeturl.Proxy = WebRequest.DefaultWebProxy;
			}

			// Efetua chamada e recupera retorno
			return (HttpWebResponse)wrGeturl.GetResponse();
		}

	}
}
