using Goober.Web.Authorization.Abstractions.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goober.Web.Keycloak.Abstractions.DataObjects
{
    internal class TokenAuthenticationResult : ITokenAuthenticationResult
    {
        public bool Successfully { get; }

        public Exception? Exception { get; }

        public ITokenInfo? UserData { get; }

        internal TokenAuthenticationResult(ITokenInfo tokenInfo)
        {
            if (tokenInfo == null) 
                throw new ArgumentNullException(nameof(tokenInfo));
            UserData = tokenInfo;
            Successfully = true;
        }

        internal TokenAuthenticationResult(Exception? exception)
        {
            Exception = exception;
            Successfully = false;
        }
    }
}
