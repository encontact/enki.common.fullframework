using Enki.Common.RegionalUtils;
using System;
using System.Text.RegularExpressions;

namespace Enki.Common {
    /// <summary>
    /// Classe de validações da Enki Consultoria e Desenvolvimento.
    /// <link>www.enkiconsultoria.com.br</link>
    /// </summary>
    public static class ValidateUtils {
		/// <summary>
		/// Efetua a validação do CPF informado.
		/// </summary>
		/// <param name="cpf"></param>
		/// <returns></returns>
		[Obsolete("Será descontinuado por ser específico. Utilizar BrazilianUtils.ValidCpf")]
		public static bool ValidaCpf(string cpf) {
			return BrazilianUtils.ValidCpf(cpf);
		}
		/// <summary>
		/// Verifica se um  e-mail tem a estrutura valida.
		/// </summary>
		/// <param name="inputEmail"></param>
		/// <returns></returns>
		public static bool ValidaEmail(string inputEmail) {
			inputEmail = inputEmail == null ? "" : inputEmail;
			string strRegex = @"^[\w\+\=\-\.]+@[a-zA-Z0-9]{1}[a-zA-Z0-9\-]*?[a-zA-Z0-9]{1,}(\.{1}[a-zA-Z0-9]{2,})+?$|^[^<>]*?<\s*[\w\+\=\-\.]+@[a-zA-Z0-9]{1}[a-zA-Z0-9\-]*?[a-zA-Z0-9]{1,}(\.{1}[a-zA-Z0-9]{2,})+?\s*>$";
			var re = new Regex(strRegex, RegexOptions.IgnoreCase);
			return re.IsMatch(inputEmail);
		}
		/// <summary>
		/// Efetua a verificação da estrutura de um CNPJ
		/// </summary>
		/// <param name="cnpj">String do CNPJ</param>
		/// <returns></returns>
		[Obsolete("Será descontinuado por ser específico. Utilizar BrazilianUtils.ValidCnpj")]
		public static bool ValidaCnpj(string cnpj) {
			return BrazilianUtils.ValidCnpj(cnpj);
		}
		/// <summary>
		/// Verifica se o texto informado é formado apenas por números inteiros.
		/// </summary>
		/// <param name="inputNumber">String que se quer validar</param>
		/// <returns>True se é inteiro e false se não é</returns>
		public static bool ValidaNumeroInteiro(string inputNumber) {
			inputNumber = inputNumber == null ? "" : inputNumber;
			if (new Regex("[^0-9]").IsMatch(inputNumber)) {
				return true;
			} else {
				return false;
			}

		}
	}
}
