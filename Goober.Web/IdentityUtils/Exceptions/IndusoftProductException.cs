using System;
using System.Runtime.Serialization;

namespace Goober.Web.IdentityUtils.Exceptions
{
    [Serializable]
    public class IndusoftProductException : Exception
    {
        public IndusoftProductException()
            : base() { }

        public IndusoftProductException(string message)
            : base(message) { }

        protected IndusoftProductException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }

        public IndusoftProductException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
