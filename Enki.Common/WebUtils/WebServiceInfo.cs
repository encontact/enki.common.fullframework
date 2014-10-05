using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.Web.Services.Description;
using System.Net;

namespace Enki.Common {
	public class WebServiceInfo {
		WebMethodInfoCollection _webMethods = new WebMethodInfoCollection();
		Uri _url;
		static Dictionary<string, WebServiceInfo> _webServiceInfos = new Dictionary<string, WebServiceInfo>();

		/// <summary>
		/// Constructor creates the web service info from the given url.
		/// </summary>
		/// <param name="url">
		private WebServiceInfo(Uri url) {
			if (url == null)
				throw new ArgumentNullException("url");

			_url = url;
			_webMethods = GetWebServiceDescription(url);
		}

		/// <summary>
		/// Factory method for WebServiceInfo. Maintains a hashtable WebServiceInfo objects
		/// keyed by url in order to cache previously accessed wsdl files.
		/// </summary>
		/// <param name="url">
		/// <returns></returns>
		public static WebServiceInfo OpenWsdl(Uri url) {
			WebServiceInfo webServiceInfo;
			if (!_webServiceInfos.TryGetValue(url.ToString(), out webServiceInfo)) {
				webServiceInfo = new WebServiceInfo(url);
				_webServiceInfos.Add(url.ToString(), webServiceInfo);
			}
			return webServiceInfo;
		}

		/// <summary>
		/// Convenience overload that takes a string url
		/// </summary>
		/// <param name="url">
		/// <returns></returns>
		public static WebServiceInfo OpenWsdl(string url) {
			Uri uri = new Uri(url);
			return OpenWsdl(uri);
		}

		/// <summary>
		/// Load the WSDL file from the given url.
		/// Use the ServiceDescription class to walk the wsdl and create the WebServiceInfo
		/// instance.
		/// </summary>
		/// <param name="url">
		private WebMethodInfoCollection GetWebServiceDescription(Uri url) {
			var uriBuilder = new UriBuilder(url);
			uriBuilder.Query = "WSDL";

			var webMethodInfos = new WebMethodInfoCollection();

			var webRequest = (HttpWebRequest)WebRequest.Create(uriBuilder.Uri);
			webRequest.ContentType = "text/xml;charset=\"utf-8\"";
			webRequest.Method = "GET";
			webRequest.Accept = "text/xml";

			ServiceDescription serviceDescription;

			using (System.Net.WebResponse response = webRequest.GetResponse())
			using (System.IO.Stream stream = response.GetResponseStream()) {
				serviceDescription = ServiceDescription.Read(stream);
			}

			foreach (PortType portType in serviceDescription.PortTypes) {
				foreach (Operation operation in portType.Operations) {
					if (webMethodInfos.Contains(operation.Name)) continue;

					var operationName = operation.Name;
					var inputMessageName = operation.Messages.Input.Message.Name;
					var outputMessageName = operation.Messages.Output.Message.Name;

					// get the message part
					var inputMessagePartName = serviceDescription.Messages[inputMessageName].Parts[0].Element.Name;
					var outputMessagePartName = serviceDescription.Messages[outputMessageName].Parts[0].Element.Name;

					// get the parameter name and type
					var inputParameters = GetParameters(serviceDescription, inputMessagePartName);
					var outputParameters = GetParameters(serviceDescription, outputMessagePartName);

					var webMethodInfo = new WebMethodInfo(operation.Name, inputParameters, outputParameters);
					webMethodInfos.Add(webMethodInfo);
				}
			}

			return webMethodInfos;
		}

		/// <summary>
		/// Walk the schema definition to find the parameters of the given message.
		/// </summary>
		/// <param name="serviceDescription">
		/// <param name="messagePartName">
		/// <returns></returns>
		private static Parameter[] GetParameters(ServiceDescription serviceDescription, string messagePartName) {
			List<Parameter> parameters = new List<Parameter>();

			Types types = serviceDescription.Types;
			System.Xml.Schema.XmlSchema xmlSchema = types.Schemas[0];

			foreach (object item in xmlSchema.Items) {
				System.Xml.Schema.XmlSchemaElement schemaElement = item as System.Xml.Schema.XmlSchemaElement;
				if (schemaElement != null) {
					if (schemaElement.Name == messagePartName) {
						System.Xml.Schema.XmlSchemaType schemaType = schemaElement.SchemaType;
						System.Xml.Schema.XmlSchemaComplexType complexType = schemaType as System.Xml.Schema.XmlSchemaComplexType;
						if (complexType != null) {
							System.Xml.Schema.XmlSchemaParticle particle = complexType.Particle;
							System.Xml.Schema.XmlSchemaSequence sequence = particle as System.Xml.Schema.XmlSchemaSequence;
							if (sequence != null) {
								foreach (System.Xml.Schema.XmlSchemaElement childElement in sequence.Items) {
									string parameterName = childElement.Name;
									string parameterType = childElement.SchemaTypeName.Name;
									parameters.Add(new Parameter(parameterName, parameterType));
								}
							}
						}
					}
				}
			}
			return parameters.ToArray();
		}

		/// <summary>
		/// WebMethodInfo
		/// </summary>
		public WebMethodInfoCollection WebMethods {
			get { return _webMethods; }
		}

		/// <summary>
		/// Url
		/// </summary>
		public Uri Url {
			get { return _url; }
			set { _url = value; }
		}
	}
}
