using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Enki.Common.RegionalUtils {
	public class BrazilianUtils {
		/// <summary>
		/// Valida se uma placa de veículo é válida de acordo com as regras atuais de trânsito.
		/// </summary>
		/// <param name="LicensePlate">Placa a ser validada</param>
		/// <returns>True se for valida e False se for inválida.</returns>
		public static bool ValidLicensePlate(string LicensePlate, bool WithSeparator = false) {
			var pattern = WithSeparator ? "^[A-Za-z]{3}-[0-9]{4}$" : "^[A-Za-z]{3}[0-9]{4}$";
			var regexPlaca = new Regex(pattern);
			if (regexPlaca.IsMatch(LicensePlate)) return true;
			return false;
		}
		public static string FormatCnpj(string cnpj) {
			if (cnpj != null && cnpj != "") {
				cnpj = cnpj.Replace("-", "").Replace("/", "").Replace(".", "");
				return StringUtils.format(cnpj, "##.###.###/####-##");
			} else {
				return "";
			}
		}
		public static string FormatCpf(string cpf) {
			if (cpf != null && cpf != "") {
				cpf = cpf.Replace("-", "").Replace("/", "").Replace(".", "");
				return StringUtils.format(cpf, "###.###.###-##");
			} else {
				return "";
			}
		}
		public static string FormatCep(string Cep) {
			if (Cep != null && Cep != "") {
				Cep = Cep.Replace("-", "");
				return StringUtils.format(Cep, "#####-###");
			} else {
				return "";
			}
		}
		/// <summary>
		/// Efetua a validação do CPF informado.
		/// </summary>
		/// <param name="cpf"></param>
		/// <returns></returns>
		public static bool ValidCpf(String cpf) {
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
		/// Efetua a verificação da estrutura de um CNPJ
		/// </summary>
		/// <param name="cnpj">String do CNPJ</param>
		/// <returns></returns>
		public static bool ValidCnpj(string cnpj) {
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
	}
}
