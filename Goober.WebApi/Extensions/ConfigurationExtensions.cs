using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace Goober.WebApi.Extensions
{
    public static class ConfigurationExtensions
    {
        public static void UseGooberLoggingVariables(this IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.UseMiddleware(typeof(Goober.WebApi.LoggingMiddleware.LoggingMiddleware));
        }

        public static void UseGooberExceptionsHandling(this IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.UseMiddleware(typeof(Goober.WebApi.LoggingMiddleware.ErrorHandlingMiddleware));
        }

        public static IWebHostBuilder UseGooberLogging(this IWebHostBuilder webHostBuilder)
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
    }
}
