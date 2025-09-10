using Goober.Web.Authorization.Abstractions.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Goober.Web.LDAP.Abstractions.Models.Responses
{
    internal class ClaimsAuthenticationResult : IClaimsAuthenticationResult
    {
        public bool Successfully { get; }

        public Exception? Exception { get; }

        public ClaimsPrincipal? UserData { get; }

        internal ClaimsAuthenticationResult(ClaimsPrincipal claimsPrincipal)
        {
            if (claimsPrincipal == null)
                throw new ArgumentNullException(nameof(claimsPrincipal));
            UserData = claimsPrincipal;
            Successfully = true;
        }

        internal ClaimsAuthenticationResult(Exception? exception)
        {
            Exception = exception;
            Successfully = false;
        }
    }
}
