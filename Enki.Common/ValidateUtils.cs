using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
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
		public static bool ValidaCpf(String cpf) {
			try {
				//Remove tudo o que não é digito;
				cpf = cpf.Replace(".", "").Replace("-", "").Replace("/", "").Replace("\\", "");
				if (cpf.Length != 11 || cpf == "00000000000" || cpf == "11111111111" || cpf == "22222222222" || cpf == "33333333333" || cpf == "44444444444" || cpf == "55555555555" || cpf == "66666666666" || cpf == "77777777777" || cpf == "88888888888" || cpf == "99999999999") {
					return false;
				}
				// Se não for numérico o valor resultante então não é um CNPJ
				Int64 outInteger = 0;
				if (!Int64.TryParse(cpf, out outInteger)) return false;

				int add = 0;
				for (int i = 0; i < 9; i++)
					add += Int32.Parse(cpf[i].ToString()) * (10 - i);

				int rev = 11 - (add % 11);
				if (rev == 10 || rev == 11)
					rev = 0;
				if (rev != Int32.Parse(cpf[9].ToString()))
					return false;
				add = 0;
				for (int i = 0; i < 10; i++)
					add += Int32.Parse(cpf[i].ToString()) * (11 - i);
				rev = 11 - (add % 11);
				if (rev == 10 || rev == 11)
					rev = 0;
				if (rev != Int32.Parse(cpf[10].ToString()))
					return false;

				return true;
			} catch {
				return false;
			}
		}
		/// <summary>
		/// Verifica se um  e-mail tem a estrutura valida.
		/// </summary>
		/// <param name="inputEmail"></param>
		/// <returns></returns>
		public static bool ValidaEmail(string inputEmail) {
			inputEmail = inputEmail == null ? "" : inputEmail;
			string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
				  @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
				  @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
			Regex re = new Regex(strRegex);
			if (re.IsMatch(inputEmail))
				return (true);
			else
				return (false);
		}
		/// <summary>
		/// Efetua a verificação da estrutura de um CNPJ
		/// </summary>
		/// <param name="cnpj">String do CNPJ</param>
		/// <returns></returns>
		public static bool ValidaCnpj(string cnpj) {
			int[] multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
			int[] multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

			int soma;
			int resto;
			string digito;
			string tempCnpj;

			cnpj = cnpj.Trim();
			cnpj = cnpj.Replace(".", "").Replace("-", "").Replace("/", "");
			// Se não for numérico o valor resultante então não é um CNPJ
			Int64 outInteger = 0;
			if (!Int64.TryParse(cnpj, out outInteger)) return false;

			if (cnpj.Length != 14)
				return false;

			tempCnpj = cnpj.Substring(0, 12);
			soma = 0;

			for (int i = 0; i < 12; i++)
				soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];

			resto = (soma % 11);
			if (resto < 2)
				resto = 0;
			else
				resto = 11 - resto;

			digito = resto.ToString();
			tempCnpj = tempCnpj + digito;
			soma = 0;

			for (int i = 0; i < 13; i++)
				soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];

			resto = (soma % 11);
			if (resto < 2)
				resto = 0;
			else
				resto = 11 - resto;

			digito = digito + resto.ToString();

			return cnpj.EndsWith(digito);
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
