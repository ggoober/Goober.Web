using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using System.Globalization;

namespace Goober.WebApi.Extensions
{
    public static class LocalizationExtensions
    {
        public static void UseRequestLocalizationByDefault(this IApplicationBuilder app)
        {
            var supportedCultures = new[] {
                new CultureInfo("en"),
                new CultureInfo("ru"),
            };

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("ru"),
                // Formatting numbers, dates, etc.
                SupportedCultures = supportedCultures,
                // UI strings that we have localized.
                SupportedUICultures = supportedCultures
            });
        }
    }
}
