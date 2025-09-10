using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.DependencyInjection;

namespace Goober.Web.Extensions
{
    public static class ForwardedHeadersExtensions
    {
        public static IServiceCollection ConfigureForwardedHeaders(
            this IServiceCollection services
        )
        {
            services.Configure<ForwardedHeadersOptions>(
                options =>
                {
                    options.ForwardedHeaders =
                       ForwardedHeaders.XForwardedFor
                       | ForwardedHeaders.XForwardedHost
                       | ForwardedHeaders.XForwardedProto;

                    options.KnownNetworks.Clear();
                    options.KnownProxies.Clear();
                }
            );
            return services;
        }
    }
}
