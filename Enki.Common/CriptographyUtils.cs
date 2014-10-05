using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace Enki.Common {
	public static class CriptographyUtils {
		public static string Md5Encrypt(string phrase) {
			UTF8Encoding encoder = new UTF8Encoding();
			MD5CryptoServiceProvider md5hasher = new MD5CryptoServiceProvider();
			byte[] hashedDataBytes = md5hasher.ComputeHash(encoder.GetBytes(phrase));
			return ByteArrayToString(hashedDataBytes);
		}
		public static string Sha1Encrypt(string phrase) {
			UTF8Encoding encoder = new UTF8Encoding();
			SHA1CryptoServiceProvider sha1hasher = new SHA1CryptoServiceProvider();
			byte[] hashedDataBytes = sha1hasher.ComputeHash(encoder.GetBytes(phrase));
			return ByteArrayToString(hashedDataBytes);
		}
		public static string Sha256Encrypt(string phrase) {
			UTF8Encoding encoder = new UTF8Encoding();
			SHA256Managed sha256hasher = new SHA256Managed();
			byte[] hashedDataBytes = sha256hasher.ComputeHash(encoder.GetBytes(phrase));
			return ByteArrayToString(hashedDataBytes);
		}
		public static string Sha384Encrypt(string phrase) {
			UTF8Encoding encoder = new UTF8Encoding();
			SHA384Managed sha384hasher = new SHA384Managed();
			byte[] hashedDataBytes = sha384hasher.ComputeHash(encoder.GetBytes(phrase));
			return ByteArrayToString(hashedDataBytes);
		}
		public static string Sha512Encrypt(string phrase) {
			UTF8Encoding encoder = new UTF8Encoding();
			SHA512Managed sha512hasher = new SHA512Managed();
			byte[] hashedDataBytes = sha512hasher.ComputeHash(encoder.GetBytes(phrase));
			return ByteArrayToString(hashedDataBytes);
		}
		private static string ByteArrayToString(byte[] inputArray) {
			StringBuilder output = new StringBuilder("");
			for (int i = 0; i < inputArray.Length; i++) {
				output.Append(inputArray[i].ToString("X2"));
			}
			return output.ToString();
		}
		public static string Base64Encode(string data) {
			byte[] encData_byte = new byte[data.Length];
			encData_byte = System.Text.Encoding.UTF8.GetBytes(data);
			string encodedData = Convert.ToBase64String(encData_byte);
			return encodedData;
		}
		public static string Base64Decode(string data) {
			System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
			System.Text.Decoder utf8Decode = encoder.GetDecoder();

			byte[] todecode_byte = Convert.FromBase64String(data);
			int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
			char[] decoded_char = new char[charCount];
			utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
			string result = new String(decoded_char);
			return result;
		}

	}
}
