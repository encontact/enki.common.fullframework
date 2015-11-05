using System;
using System.Security.Cryptography;
using System.Text;

namespace Enki.Common
{
    public static class CriptographyUtils
    {
        public static string Md5Encrypt(string phrase)
        {
            var encoder = new UTF8Encoding();
            using (var md5hasher = new MD5CryptoServiceProvider())
            {
                var hashedDataBytes = md5hasher.ComputeHash(encoder.GetBytes(phrase));
                return ByteArrayToString(hashedDataBytes);
            }
        }

        public static string Sha1Encrypt(string phrase)
        {
            var encoder = new UTF8Encoding();
            using (var sha1hasher = new SHA1CryptoServiceProvider())
            {
                var hashedDataBytes = sha1hasher.ComputeHash(encoder.GetBytes(phrase));
                return ByteArrayToString(hashedDataBytes);
            }
        }

        public static string Sha256Encrypt(string phrase)
        {
            var encoder = new UTF8Encoding();
            using (var sha256hasher = new SHA256Managed())
            {
                var hashedDataBytes = sha256hasher.ComputeHash(encoder.GetBytes(phrase));
                return ByteArrayToString(hashedDataBytes);
            }
        }

        public static string Sha384Encrypt(string phrase)
        {
            var encoder = new UTF8Encoding();
            using (var sha384hasher = new SHA384Managed())
            {
                var hashedDataBytes = sha384hasher.ComputeHash(encoder.GetBytes(phrase));
                return ByteArrayToString(hashedDataBytes);
            }
        }

        public static string Sha512Encrypt(string phrase)
        {
            var encoder = new UTF8Encoding();
            using (var sha512hasher = new SHA512Managed())
            {
                var hashedDataBytes = sha512hasher.ComputeHash(encoder.GetBytes(phrase));
                return ByteArrayToString(hashedDataBytes);
            }
        }

        private static string ByteArrayToString(byte[] inputArray)
        {
            var output = new StringBuilder("");
            for (int i = 0; i < inputArray.Length; i++)
            {
                output.Append(inputArray[i].ToString("X2"));
            }
            return output.ToString();
        }

        public static string Base64Encode(string data)
        {
            var encData_byte = new byte[data.Length];
            encData_byte = Encoding.UTF8.GetBytes(data);
            var encodedData = Convert.ToBase64String(encData_byte);
            return encodedData;
        }

        public static string Base64Decode(string data)
        {
            var encoder = new UTF8Encoding();
            var utf8Decode = encoder.GetDecoder();

            var todecode_byte = Convert.FromBase64String(data);
            var charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
            var decoded_char = new char[charCount];
            utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
            var result = new string(decoded_char);
            return result;
        }
    }
}