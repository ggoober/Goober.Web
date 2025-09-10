using System.IO;

namespace Goober.Web.IdentityUtils.Generators
{
    internal abstract class FileSignatureGeneratorBase
    {
        private const int _maxFileSizeInBytes = 1024;
        protected internal readonly string filePath;
        protected internal readonly string signatureSeparator;
        protected internal readonly FileInfo fileInfo;

        internal bool IsValid => Validate();
        internal string InvalidMessage { get; set; }

        internal FileSignatureGeneratorBase(string filePath, string separator)
        {
            this.filePath = filePath;
            signatureSeparator = separator;
            fileInfo = new FileInfo(filePath);
        }

        internal string Generate()
        {
            Validate();
            if (!IsValid)
            {
                return null;
            }

            var signature = GetSignature();

            return signature;
        }

        protected internal virtual bool Validate()
        {
            if (fileInfo is null || !fileInfo.Exists)
            {
                InvalidMessage = $"File ({filePath}) not found";
                return false;
            }

            if (fileInfo.Length > _maxFileSizeInBytes)
            {
                InvalidMessage = $"File ({filePath}) size ({fileInfo.Length} bytes) is too large";
                return false;
            }

            return true;
        }

        protected internal abstract string GetSignature();
    }
}
