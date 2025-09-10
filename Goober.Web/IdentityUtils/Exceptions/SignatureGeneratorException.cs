using System;
using System.Runtime.Serialization;

namespace Goober.Web.IdentityUtils.Exceptions
{
    [Serializable]
    public class SignatureGenerationException : Exception
    {
        public SignatureGenerationException()
            : base() { }

        public SignatureGenerationException(string message)
            : base(message) { }

        protected SignatureGenerationException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }

        public SignatureGenerationException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
