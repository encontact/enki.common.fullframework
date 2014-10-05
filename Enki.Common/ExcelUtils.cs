using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OfficeOpenXml;
using System.Xml;

namespace Enki.Common {
	/// <summary>
	/// Funções utilitárias para excel.
	/// </summary>
	public static class ExcelUtils {
		/// <summary>
		/// Tipo de epoch para cálculo de datas da planilha Excel.
		/// </summary>
		private enum DateEpoch{
			Epoch1900,
			Epoch1904
		}

		/// <summary>
		/// Recupera a data do Excel a partir do número apresentado na coluna.
		/// </summary>
		/// <param name="package">Pacote do Excel aberto para leitura</param>
		/// <param name="longDate">Data recuperada no formato numérico</param>
		/// <returns>Data recuperada ou data atual se não reconhecer.</returns>
		public static DateTime GetDate(ExcelPackage package, long longDate){
			var epoch = GetEpoch(package);
			if (epoch == DateEpoch.Epoch1900) {
				return DateTime.FromOADate(longDate);
			} else if (epoch == DateEpoch.Epoch1904) {
				return new DateTime(1904, 01, 01).AddDays(longDate);
			}
			return DateTime.Now;
		}

		/// <summary>
		/// Recupera a data do Excel a partir da celula do Excel que se espera que seja data.
		/// </summary>
		/// <param name="package">Pacote do excel aberto para leitura</param>
		/// <param name="excelRange">Celula do excel a ter a data recuperada.</param>
		/// <returns>Data e hora reconhecido ou Data atual caso não reconheça a informação.</returns>
		public static DateTime GetDate(ExcelPackage package, ExcelRange excelRange) {
			var format = excelRange.Style.Numberformat.Format;
			if (format == "@") {
				return Convert.ToDateTime(excelRange.Value.ToString());
			} else {
				var epoch = GetEpoch(package);
				var convertedToDateTime = Convert.ToDateTime(excelRange.Value);
				if (epoch == DateEpoch.Epoch1900) {
					return convertedToDateTime;
				} else if (epoch == DateEpoch.Epoch1904) {
					return convertedToDateTime.AddYears(4).AddDays(1);
				}
			}
			return DateTime.Now;
		}

		/// <summary>
		/// Recupera o tipo de epoch configurado na planilha excel.
		/// </summary>
		/// <param name="package"></param>
		/// <returns></returns>
		private static DateEpoch GetEpoch(ExcelPackage package) {
			var returnEpoch = DateEpoch.Epoch1900;
			// Define o tipo de data do Excel
			XmlDocument doc = new XmlDocument();
			doc.LoadXml(package.Workbook.WorkbookXml.InnerXml);
			XmlNodeList elementList = doc.GetElementsByTagName("workbookPr");
			foreach (XmlNode element in elementList) {
				var nameAttribute = element.Attributes["date1904"];
				if (nameAttribute != null && (nameAttribute.Value == "1" || nameAttribute.Value.ToUpper() == "TRUE")) returnEpoch = DateEpoch.Epoch1904;
			}
			return returnEpoch;
		}
	}
}
