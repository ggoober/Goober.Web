using Goober.Web.IdentityUtils.Converters;
using Goober.Web.IdentityUtils.Exceptions;
using Goober.Web.IdentityUtils.Helpers;
using Goober.Web.IdentityUtils.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Goober.Web.IdentityUtils.Services
{
    internal class IDSProdService : IHostedService, IDisposable
    {
        private readonly int _periodCheckFromHours = 24;
        private readonly DateTime _scheduledTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 12, 0, 0);
        private readonly IDictionary<string, object> _appBaseConfig;
        private readonly IHostApplicationLifetime _hostApplicationLifetime;
        private readonly ILogger _logger;
        private readonly object _state;
        private readonly Timer _timer;
        private DateTime _expirationDate;

        public DateTime ExpirationDate => _expirationDate;

        public IDSProdService(
            DateTime expirationDate,
            IDictionary<string, object> appBaseConfig,
            IHostApplicationLifetime hostApplicationLifetime,
            ILogger<IndusoftProductException> logger,
            object state
        )
        {
            _expirationDate = expirationDate;
            _appBaseConfig = appBaseConfig;
            _hostApplicationLifetime = hostApplicationLifetime;
            _logger = logger;
            _state = state;
            TimeSpan dueTime = GetTimeUntil(_scheduledTime);
            _timer = new Timer(LicenseValidation, null, dueTime, TimeSpan.FromHours(_periodCheckFromHours));

            LicenseValidation(null);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

        private void AboutExpirationDate(DateTime expireDate)
        {
            var expirationDate = expireDate.ToString("dd.MM.yyyy HH:mm:ss");
            _logger?.LogInformation("Дата окончания действия лицензии - {ExpirationDate}", expirationDate);
        }

        private static TimeSpan GetTimeUntil(DateTime scheduledTime)
        {
            if (scheduledTime < DateTime.Now)
            {
                scheduledTime = scheduledTime.AddDays(1);
            }

            TimeSpan timeUntilScheduled = scheduledTime - DateTime.Now;

            return timeUntilScheduled;
        }

        private void LicenseValidation(object state)
        {
            _appBaseConfig.TryGetValue("id", out object productId);
            var product = IdentityUtilsHelper.GetIndusoftProduct(productId);
            if (product == null)
                return;

            try
            {
                var isLoadedLicense = TryLoadLicenseFromFile(product, out var license);
                LicenseValidationHandler(isLoadedLicense);

                var isValidLicense = ValidateLicenseAndGetExpireDate(license, product, out DateTime? expireDate);
                LicenseValidationHandler(isValidLicense);

                SetAppBaseConfig(expireDate);

                _expirationDate = expireDate.Value;

                var informator = LicenseInformatorFactory.CreateInformator(license);
                if (informator != null)
                {
                    var message = informator.GetAboutExpirationDateMessage(expireDate.Value);
                    _logger?.LogInformation(message);
                }
                else
                {
                    AboutExpirationDate(expireDate.Value);
                }

                SetProductInfo(product, license);
            }
            catch (IndusoftProductException ex)
            {
                _logger?.LogError(ex.Message);
                _hostApplicationLifetime.StopApplication();
            }
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

#if (DEBUG)
            _appBaseConfig.TryGetValue("signkey", out var configKey);
            if (configKey is string signKey && !string.IsNullOrEmpty(signKey))
            {
                keyStr = signKey;
            }
#endif
            bool isVerify = false;
            try
            {
                using (var rsa = new RSACryptoServiceProvider())
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

        private void SetProductInfo(IndusoftProduct product, LicenseModel licenseModel)
        {
            if (_state is not ConcurrentDictionary<string, object> dic)
                return;

            var settings = new JsonSerializerSettings
            {
                Converters = { new OuterLicenseModelJsonConverter() },
            };

            string json = null;
            try
            {
                json = JsonConvert.SerializeObject(licenseModel, settings);
            }
            catch
            {
                return;
            }

            if (json != null)
            {
                dic.AddOrUpdate("product-info", json, (key, oldValue) =>
                {
                    return json;
                });
            }
        }

        [Obsolete]
        private void SetAppBaseConfig(DateTime? expireDate)
        {
            if (_appBaseConfig != null)
                _appBaseConfig["ExpireDate"] = expireDate;
        }
    }
}
