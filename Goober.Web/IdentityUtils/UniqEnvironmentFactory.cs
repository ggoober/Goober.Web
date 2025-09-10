using Goober.Web.IdentityUtils.Generators;
using Goober.Web.IdentityUtils.Services;
using System;
using System.Runtime.InteropServices;

namespace Goober.Web.IdentityUtils
{
    /// <summary>
    /// Создает уникальную подпись лицензии
    /// </summary>
    internal sealed class UniqEnvironmentFactory
    {
        /// <summary>
        /// Генерирует строку для сервера приложений, основанную на уникальных параметрах окружения
        /// </summary>
        /// <returns>Уникальная строка окружения</returns>
        internal static string Create()
        {
            LicenseSignatureGenerator generator = CreateLicenseSignatureGenerator();
            return generator.GetSignature();
        }

        private static LicenseSignatureGenerator CreateLicenseSignatureGenerator()
        {
            SingnatureCryptoServiceBase cryptoService = new SingnatureCryptoService();

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return new WindowsLicenseSignatureGenerator(cryptoService);
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return new UnixLicenseSignatureGenerator(cryptoService);
            }

            throw new NotSupportedException("Hardware parameters for current platform does not implemented.");
        }
    }
}
