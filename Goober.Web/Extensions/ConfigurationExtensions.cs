using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting.WindowsServices;
using Microsoft.Extensions.Logging;
using NLog.Web;
using Goober.Web.Glossary;

namespace Goober.Web.Extensions
{
    public static class ConfigurationExtensions
    {
        public static void UseIndusoftLoggingVariables(this IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.UseMiddleware(typeof(Goober.Web.LoggingMiddleware.LoggingMiddleware));
        }

        public static void UseIndusoftExceptionsHandling(this IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.UseMiddleware(typeof(Goober.Web.LoggingMiddleware.ErrorHandlingMiddleware));
        }

        public static IWebHostBuilder UseIndusoftLogging(this IWebHostBuilder webHostBuilder)
        {
            return webHostBuilder
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(LogLevel.Trace);
                }
                )
                .UseNLog();
        }
        public static IWebHostBuilder UseWindowsService(this IWebHostBuilder webHostBuilder)
        {
            if (WindowsServiceHelpers.IsWindowsService())
            {
                webHostBuilder.UseContentRoot(AppContext.BaseDirectory);
            }
            return webHostBuilder;
        }

        public static bool GetUseSwagger(this IConfiguration config, bool defaultValue) => config.GetValue(ConfigKeysGlossary.EnableSwagger, defaultValue);
    }
}
