using Goober.WebApi.LoggingMiddleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using System.Globalization;

namespace Goober.WebApi.Extensions
{
    public static class ConfigExtensions
    {
        

        public static void UserApiErrorHandling(this IApplicationBuilder app)
        {
            app.UseMiddleware(typeof(ErrorHandlingMiddleware));
        }
    }
}
