using System;
using System.IO;
using System.Security.Cryptography;

namespace Enki.Common.Cryptography
{
    public class AesSymmetricCryptography
    {
        private string _key, _IV;
        private readonly byte[] SALT = new byte[] { 0x26, 0xdc, 0xff, 0x00, 0xad, 0xed, 0x7a, 0xee, 0xc5, 0xfe, 0x07, 0xaf, 0x4d, 0x08, 0x22, 0x3c };

        public AesSymmetricCryptography(string password = "NoPassWd@01")
        {
            using (var pdb = new Rfc2898DeriveBytes(password, SALT))
            {
                _key = Convert.ToBase64String((pdb.GetBytes(32)));
                _IV = Convert.ToBase64String((pdb.GetBytes(16)));
            }
        }

        public AesSymmetricCryptography(string privateKey, string publicKey)
        {
            _key = privateKey;
            _IV = publicKey;
        }

        public string Encrypt(string message)
        {
            using (var aesAlg = new AesManaged())
            {
                aesAlg.Key = Convert.FromBase64String(_key);
                aesAlg.IV = Convert.FromBase64String(_IV);

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(message);
                        }
                        return Convert.ToBase64String(msEncrypt.ToArray());
                    }
                }
            }
        }

        public string GetKey()
        {
            return _key;
        }

        public string GetIV()
        {
            return _IV;
        }

        public string Decrypt(string base64EncodedData)
        {
            string plaintext = null;
            using (var aesAlg = new AesManaged())
            {
                aesAlg.Key = Convert.FromBase64String(_key);
                aesAlg.IV = Convert.FromBase64String(_IV);
                
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                using (var msDecrypt = new MemoryStream(Convert.FromBase64String(base64EncodedData)))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var srDecrypt = new StreamReader(csDecrypt))
                        {
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }

            }

            return plaintext;
        }
    }
}
