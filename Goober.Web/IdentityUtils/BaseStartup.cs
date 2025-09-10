using Goober.Web.IdentityUtils;
using Goober.Web.IdentityUtils.Converters;
using Goober.Web.IdentityUtils.Exceptions;
using Goober.Web.IdentityUtils.Helpers;
using Goober.Web.IdentityUtils.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Goober.Web
{
    public abstract partial class BaseStartup
    {
        private DateTime? _expireDate;
        protected readonly Dictionary<string, object> ApplicationBaseConfiguration = new Dictionary<string, object>();

        /// <summary>
        /// Only for test (Configuration == 'DEBUG')
        /// </summary>
        internal string SignKey { get; set; }

        private void LicenseValidation()
        {
            ApplicationBaseConfiguration.TryGetValue("id", out object productId);
            var product = IdentityUtilsHelper.GetIndusoftProduct(productId);
            if (product == null || IsDisableValidation(product))
                return;

            var isLoadedLicense = TryLoadLicenseFromFile(product, out var license);
            LicenseValidationHandler(isLoadedLicense);

            var isValidLicense = ValidateLicenseAndGetExpireDate(license, product, out _expireDate);
            LicenseValidationHandler(isValidLicense);

        }

        private bool IsDisableValidation(IndusoftProduct product)
        {
            var productId = product != null
                ? product.Id
                : Guid.Empty;

            try
            {
                var existValue = TryGetValueFromApplication(out var value);
                if (!existValue)
                    return false;

                var isVerifyGuidData = VerifyGuidData(value, productId);
                return isVerifyGuidData;
            }
            catch
            {
                return false;
            }
        }

        private bool TryGetValueFromApplication(out string value)
        {
            value = null;
            if (ApplicationBaseConfiguration.Count < 3)
            {
                return false;
            }

            var entryPointAssamblyName = System.Reflection.Assembly.GetEntryAssembly()?.GetName()?.Name;
            var chunks = Chanking(entryPointAssamblyName);
            var chunkHashs = GetHashForChanks(chunks);

            var existValues = false;
            string resultValues = string.Empty;
            foreach (var chunkHash in chunkHashs)
            {
                if (!ApplicationBaseConfiguration.TryGetValue(chunkHash, out var obj))
                {
                    existValues = false;
                    break;
                }

                if (obj is string valueString)
                {
                    resultValues += valueString;
                    existValues = true;
                }
            }

            if (existValues)
                value = resultValues;

            return existValues;
        }

        private static string[] Chanking(string input)
        {
            var chunkCount = 3;
            var chunkSize = input.Length / chunkCount;
            var chunks = new string[chunkCount];
            for (int i = 0; i < chunkCount; i++)
            {
                var startIndex = chunkSize * i;
                var length = i == 2
                    ? input.Length - startIndex
                    : chunkSize;

                chunks[i] = input.Substring(startIndex, length);
            }

            return chunks;
        }

        private static string[] GetHashForChanks(string[] chunks)
        {
            var resultChunks = new string[chunks.Length];
            var count = chunks.Length - 1;
            for (int i = 0; i <= count; i++)
            {
                var result = chunks[i];
                for (int j = count; j >= 0; j--)
                {
                    if (j == i)
                        continue;

                    result += chunks[j];
                }

                var sha256 = SHA256.Create();
                byte[] bytes1 = Encoding.UTF8.GetBytes(result);
                byte[] hash = sha256.ComputeHash(bytes1);
                string key = Convert.ToBase64String(hash);
                resultChunks[i] = key;
            }

            return resultChunks;
        }

        private static bool VerifyGuidData(string value, Guid productGuid)
        {
            var guidStr = productGuid.ToString().ToLower();
            var guid = Encoding.UTF8.GetBytes(guidStr);
            var sign = Convert.FromBase64String(value);

            bool verifyData = false;
            var publicKey = "<RSAKeyValue><Modulus>s8rAINdIqqoG22uI7xcqP4TvS/qaKfwp6HVGFZhxaFy/ZI2nvjoQCsLL6eqGPRgOUuaOiq5tmx36lPNzMSjykddo2dCkzOGFGww0Gp73MPIwX5APDf5CBrXXe6oY9RmY6JfIKCsARqxhs91OOyXJ5b1qLpaRHeteGgp8dWTiiPE=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
            using (var rsa = new RSACryptoServiceProvider(1024))
            {
                rsa.FromXmlString(publicKey);
                verifyData = rsa.VerifyData(guid, sign, HashAlgorithmName.SHA512, RSASignaturePadding.Pkcs1);
            }

            return verifyData;
        }

        private LicenseValidationResult ValidateLicenseAndGetExpireDate(LicenseModel license, IndusoftProduct product, out DateTime? expireDate)
        {
            expireDate = null;
            if (license == null || product == null)
                return LicenseValidationResult.Success;

            expireDate = license.LicWDate;

            var environmentSignature = UniqEnvironmentFactory.Create();
            if (license.Sign != environmentSignature)
                return LicenseValidationResult.NoCorrectEnvironmentSignature;

            var isProductNameValid = license.ProductName == product.Name;
            var isLicGuidValid = license.LicGuid == product.Id;

            if (!isProductNameValid || !isLicGuidValid)
                return LicenseValidationResult.NoValidProduct;

            var isDateValid = expireDate > DateTime.Now;
            if (!isDateValid)
                return LicenseValidationResult.NoValidDate;

            return LicenseValidationResult.Success;
        }

        private void LicenseValidationHandler(LicenseValidationResult result)
        {
            if (result == LicenseValidationResult.Success)
                return;

            string message = IdentityUtilsHelper.GetValidationMessageBy(result);
            if (message == null)
                return;

            throw new IndusoftProductException(message);
        }

        private LicenseValidationResult TryLoadLicenseFromFile(IndusoftProduct product, out LicenseModel licenseModel)
        {
            licenseModel = null;

            if (product == null)
                return LicenseValidationResult.Success;

            var licenseFileFolder = AppDomain.CurrentDomain.BaseDirectory;
            var licenseFileName = product.FileName;
            var licenseFullPath = Path.Combine(licenseFileFolder, licenseFileName);

            if (!File.Exists(licenseFullPath))
            {
                return LicenseValidationResult.NoFoundLicenseFile;
            }

            string licenseFileContent;
            using (var licFile = new StreamReader(licenseFullPath))
            {
                licenseFileContent = licFile.ReadToEnd();
            }

            var isParsedLicense = TryParseLicense(licenseFileContent, out var licenseBytes, out var signatureBytes);
            if (!isParsedLicense)
                return LicenseValidationResult.NoParsedLicense;

            var isVerificatedSignature = VerifySignature(licenseBytes, signatureBytes);
            if (!isVerificatedSignature)
                return LicenseValidationResult.NoCorrectSignature;

            var isDeserializeLicense = TryDeserializeLicense(licenseBytes, out var license);
            if (!isDeserializeLicense)
                return LicenseValidationResult.NoDeserializedLicense;

            licenseModel = license;
            return LicenseValidationResult.Success;
        }

        private bool TryDeserializeLicense(byte[] licenseBytes, out LicenseModel license)
        {
            license = null;
            try
            {
                var json = Encoding.UTF8.GetString(licenseBytes);
                var settings = new JsonSerializerSettings
                {
                    Converters = { new LicenseModelJsonConverter() },
                };
                license = JsonConvert.DeserializeObject<LicenseModel>(json, settings);
            }
            catch
            {
                return false;
            }

            return true;
        }

        private bool TryParseLicense(
            string licenseFileContent,
            out byte[] linceseBytes,
            out byte[] signatureBytes
        )
        {
            var licenseParts = 2;
            var licenseSplitter = '.';

            linceseBytes = Array.Empty<byte>();
            signatureBytes = Array.Empty<byte>();

            var license = licenseFileContent.Split(licenseSplitter);
            if (license.Length != licenseParts
                || license.Any(x => string.IsNullOrWhiteSpace(x)))
                return false;

            try
            {
                linceseBytes = Convert.FromBase64String(license[0]);
                signatureBytes = Convert.FromBase64String(license[1]);
            }
            catch
            {
                return false;
            }

            return true;
        }

        private bool VerifySignature(byte[] data, byte[] signature)
        {
            var exponent = "AQAB";
            var keyStr = "6yA91N8CT+/119SffY3mmZJKIyO9Xh40YLVh0d8aONc4y8LX0GIhyc8+NjCLm5GswjXjdzMfhfMwVA5AWGILzyRdKfm7lMdrjkDYcJKAL2PHdzHmg6xVdF4vwqP4FS1JSy8PDcpCY2yjDQr1RQl5ExgJ6mo/XDwUkqPIBrSzUTgudM8UGCoDbXONK45tf5nnWDy7f701U+VhTw1YuJcUeoDxPOQUSkj3a+uDQOiKCaXCmUteUgN8vSFs6Gp6Ogu6mwoS8ASTwqinw2wh40wV97jP/nD4ruJPsRjUZe2x4UdA1zqzFPuei6LZKDuQxgOScZU6LhJsbMbuaRsRlgMQV1GRRFQVhIb9nc+pRaUY/X7hh6MeqRI5xMqBQRYcPlqWheQerKa63Hrc7+HQTQSu3PLVXGqXT6J1C4WtgCg/5FgD5kkqGbz2rJicJJdytYcK6OVV1rdwW/ORlucHMFlEcPtZR42HPcCNwo1VBdXcSXCCUAq5f8s3w3IQZ+fmq+DlLaHLpqrw7Q2yp3kXMjv5NhCTifD9m4p6NPDCfpXBtgL44Vqto4CkDxao+j5av35jMF4nlK+kt0AtHpRDxpnJwPo1omttJvy+2SyeMCx0lSzW9L0NRxqG6eufF5+Pl6cjZODWYCdPMs6ZPhHSIL68NX8+ri3xkwwm6fS43VLZDIM=";
#if(DEBUG)
            if (!string.IsNullOrEmpty(SignKey))
            {
                keyStr = SignKey;
                ApplicationBaseConfiguration.Add("signkey", SignKey);
            }
#endif
            bool isVerify = false;
            try
            {
                using (var rsa = RSA.Create())
                {
                    rsa.FromXmlString($"<RSAKeyValue><Modulus>{keyStr}</Modulus><Exponent>{exponent}</Exponent></RSAKeyValue>");
                    isVerify = rsa.VerifyData(data, signature, HashAlgorithmName.SHA512, RSASignaturePadding.Pkcs1);
                }
            }
            catch
            {
                isVerify = false;
            }

            return isVerify;
        }
    }
}
