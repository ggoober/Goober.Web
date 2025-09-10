using Goober.Web.IdentityUtils.Exceptions;
using Goober.Web.IdentityUtils.Services;

namespace Goober.Web.IdentityUtils.Generators
{
    internal abstract class LicenseSignatureGenerator
    {
        internal static readonly string SignatureSeparator = "#";
        private readonly SingnatureCryptoServiceBase _cryptoService;

        internal LicenseSignatureGenerator(SingnatureCryptoServiceBase cryptoService)
        {
            _cryptoService = cryptoService;
        }

        internal string GetSignature()
        {
            string uniqueString;
            try
            {
                uniqueString = Generate();
            }
            catch (SignatureGenerationException)
            {
                throw new SignatureGenerationException("Не доступа к необходимым параметрам для генерации сигнатуры");
            }

            if (string.IsNullOrEmpty(uniqueString))
                return null;

            var cipherSignature = _cryptoService.EncryptSignature(uniqueString);

            return cipherSignature;
        }

        protected internal abstract string Generate();
    }
}
