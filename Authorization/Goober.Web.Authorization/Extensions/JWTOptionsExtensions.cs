using Goober.Web.Authorization.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Goober.Web.Authorization.Extensions
{
    public static class JWTOptionsExtensions
    {
        public static string GetSchemeName(
            this JWTOptions jwtOptions)
        {
            return !string.IsNullOrWhiteSpace(jwtOptions.SchemeName) ?
                jwtOptions.SchemeName :
                JwtBearerDefaults.AuthenticationScheme;
        }
    }
}
