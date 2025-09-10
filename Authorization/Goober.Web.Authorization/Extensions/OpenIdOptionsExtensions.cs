using Goober.Web.Authorization.Configuration;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

namespace Goober.Web.Authorization.Extensions
{
    public static class OpenIdOptionsExtensions
    {
        public static string GetSchemeName(
            this OpenIdOptions openIdOptions)
        {
            return !string.IsNullOrWhiteSpace(openIdOptions.SchemeName) ?
                openIdOptions.SchemeName :
                OpenIdConnectDefaults.AuthenticationScheme;
        }
    }
}
