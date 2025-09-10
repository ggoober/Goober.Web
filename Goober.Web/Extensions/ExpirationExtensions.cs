using Goober.Web.IdentityUtils.Exceptions;
using Goober.Web.IdentityUtils.Helpers;
using Goober.Web.IdentityUtils.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace Goober.Web.Extensions
{
    public static class ExpirationExtensions
    {
        public static IServiceCollection AddIndusoftExpirationDate(
            this IServiceCollection services,
            DateTime? expirationDate,
            IDictionary<string, object> appBaseConfig
        )
        {
            var state = ProductInfoHelper.SetProductInfoService(services, appBaseConfig);

            if (!expirationDate.HasValue)
                return services;

            services.AddSingleton<IHostedService>(provider =>
            {
                var hostAppLifetime = provider.GetService<IHostApplicationLifetime>();
                var logger = provider.GetService<ILogger<IndusoftProductException>>();
                return new IDSProdService(
                    expirationDate: expirationDate.Value,
                    appBaseConfig: appBaseConfig,
                    hostApplicationLifetime: hostAppLifetime,
                    logger: logger,
                    state: state);
            });

            return services;
        }
    }
}
