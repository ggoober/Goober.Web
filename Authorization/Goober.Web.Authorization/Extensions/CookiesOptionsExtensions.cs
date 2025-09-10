using Goober.Web.Authorization.Configuration;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Goober.Web.Authorization.Extensions
{
    public static class CookiesOptionsExtensions
    {
        public static string GetSchemeName(
            this CookiesOptions cookiesOptions)
        {
            return !string.IsNullOrWhiteSpace(cookiesOptions.SchemeName) ?
                cookiesOptions.SchemeName :
                CookieAuthenticationDefaults.AuthenticationScheme;
        }
    }
}
