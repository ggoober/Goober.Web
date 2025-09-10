using Goober.Web.Negotiate.Configuration;
using NegotiateDefaults = Microsoft.AspNetCore.Authentication.Negotiate.NegotiateDefaults;

namespace Goober.Web.Negotiate.Extensions
{
    public static class NegotiateOptionsExtensions
    {
        public static string GetSchemeName(
            this NegotiateOptions negotiateOptions)
        {
            return !string.IsNullOrWhiteSpace(negotiateOptions.SchemeName) ?
                negotiateOptions.SchemeName :
                NegotiateDefaults.AuthenticationScheme;
        }
    }
}
