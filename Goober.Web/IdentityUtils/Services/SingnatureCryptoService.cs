using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Goober.Web.IdentityUtils.Services
{
    internal sealed class SingnatureCryptoService : SingnatureCryptoServiceBase
    {
        private static readonly string _key = "ac84e648ecca23a42cca45e432e80311";
        private static readonly string _salt = "f2n1g3m0ec0";

        internal override string EncryptSignature(string plainSignature)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(_key);
            byte[] saltBytes = Encoding.UTF8.GetBytes(_salt);
            byte[] buffer;

            using (var aes = new AesManaged())
            {
                aes.KeySize = 256;
                aes.BlockSize = 128;
                aes.Padding = PaddingMode.PKCS7;
                aes.Mode = CipherMode.CBC;

                var keyDerivationFunction = new Rfc2898DeriveBytes(keyBytes, saltBytes, 1000);
                aes.Key = keyDerivationFunction.GetBytes(aes.KeySize / 8);
                aes.IV = keyDerivationFunction.GetBytes(aes.BlockSize / 8);

                ICryptoTransform cryptoTransform = aes.CreateEncryptor(aes.Key, aes.IV);
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                        {
                            streamWriter.Write(plainSignature);
                        }
                    }

                    buffer = memoryStream.ToArray();
                }
            }

            var cipherSignature = Convert.ToBase64String(buffer);

            return cipherSignature;
        }

        internal override string DecryptSignature(string cipherSignature)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(_key);
            byte[] saltBytes = Encoding.UTF8.GetBytes(_salt);
            byte[] buffer;

            using (var aes = new AesManaged())
            {
                aes.KeySize = 256;
                aes.BlockSize = 128;
                aes.Padding = PaddingMode.PKCS7;
                aes.Mode = CipherMode.CBC;

                var keyDerivationFunction = new Rfc2898DeriveBytes(keyBytes, saltBytes, 1000);
                aes.Key = keyDerivationFunction.GetBytes(aes.KeySize / 8);
                aes.IV = keyDerivationFunction.GetBytes(aes.BlockSize / 8);

                byte[] encryptedDataBytes = Convert.FromBase64String(cipherSignature);
                ICryptoTransform cryptoTransform = aes.CreateDecryptor(aes.Key, aes.IV);
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(encryptedDataBytes, 0, encryptedDataBytes.Length);
                    }

                    buffer = memoryStream.ToArray();
                }
            }

            var plainSignature = Encoding.UTF8.GetString(buffer);

            return plainSignature;
        }
    }
}
