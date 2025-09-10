using Goober.Web.IdentityUtils.Exceptions;
using Goober.Web.IdentityUtils.Services;
using System;
using System.Collections.Generic;

namespace Goober.Web.IdentityUtils.Generators
{
    internal class UnixLicenseSignatureGenerator : LicenseSignatureGenerator
    {
        private readonly string _etcOSReleasePath = "/etc/os-release";
        private readonly string _usrLibOSReleasePath = "/usr/lib/os-release";

        public UnixLicenseSignatureGenerator(SingnatureCryptoServiceBase cryptoService)
            : base(cryptoService)
        {
        }

        /// <inheritdoc/>
        protected internal sealed override string Generate()
        {
            var result = GetOSReleaseSignature();
            return result;
        }

        /// <summary>
        /// Get signature OsName/OsID OsVersion/OsVersionID OsReleaseFile creation time (from /etc/os-release or /usr/lib/os-release).
        /// </summary>
        /// <returns>System.String</returns>
        /// <exception cref="InvalidOperationException"></exception>
        private string GetOSReleaseSignature()
        {
            var invalidMessages = new List<string>();

            string signature = GetFileSiganture(_etcOSReleasePath, invalidMessages);
            if (signature != null)
                return signature;

            signature = GetFileSiganture(_usrLibOSReleasePath, invalidMessages);
            if (signature != null)
                return signature;

            var message = string.Join(", ", invalidMessages);
            var invalidMessage = !string.IsNullOrEmpty(message)
                ? message
                : $"Contents of files not available: {_etcOSReleasePath}, {_usrLibOSReleasePath}";

            throw new SignatureGenerationException(invalidMessage);
        }

        private static string GetFileSiganture(string path, List<string> invalidMessages)
        {
            FileSignatureGeneratorBase signatureGenerator = new OSReleaseSignatureGenerator(path, SignatureSeparator);
            var signiture = signatureGenerator.Generate();
            if (signiture != null)
                return signiture;

            if (!signatureGenerator.IsValid && signatureGenerator.InvalidMessage != null)
            {
                invalidMessages.Add(signatureGenerator.InvalidMessage);
            }

            return null;
        }
    }
}
