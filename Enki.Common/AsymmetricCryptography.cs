using System;
using System.Security.Cryptography;
using System.Text;

namespace Enki.Common
{
    /// <summary>
    /// Permite criptogradia simétrica com uso de chaves Pública/Privada.
    /// </summary>
    public class AsymmetricCryptography
    {
        private string _privateKey, _publicKey;
        private Encoding encodingToUse = new UTF8Encoding();

        public AsymmetricCryptography()
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider { KeySize = 1024 })
            {
                _publicKey = rsa.ToXmlString(false);
                _privateKey = rsa.ToXmlString(true);
            }
        }

        public AsymmetricCryptography(string privateKey, string publicKey)
        {
            _privateKey = privateKey;
            _publicKey = publicKey;
        }

        public string Encrypt(string message)
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(_publicKey);

                var encodedTextBytes = encodingToUse.GetBytes(message);
                var encryptedBytes = rsa.Encrypt(encodedTextBytes, false);
                var base64EncodedData = Convert.ToBase64String(encryptedBytes);
                return base64EncodedData;
            }
        }

        public string GetPrivateKey()
        {
            return _privateKey;
        }

        public string GetPublicKey()
        {
            return _publicKey;
        }

        public string Decrypt(string base64EncodedData)
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(_privateKey);

                var encryptedBytes = Convert.FromBase64String(base64EncodedData);
                var decryptedBytes = rsa.Decrypt(encryptedBytes, false);
                var decryptedString = encodingToUse.GetString(decryptedBytes);
                return decryptedString;
            }
        }
    }
}