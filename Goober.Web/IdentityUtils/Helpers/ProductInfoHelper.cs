using Goober.Web.IdentityUtils.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Goober.Web.IdentityUtils.Helpers
{
    internal static class ProductInfoHelper
    {
        internal static object SetProductInfoService(
            IServiceCollection services,
            IDictionary<string, object> appBaseConfig
        )
        {
            var state = new ConcurrentDictionary<string, object>();
            string productName = GetProductName(appBaseConfig);

            services.AddScoped<IProductInfoService>(provider =>
            {
                var logger = provider.GetService<ILogger<ProductInfoService>>();
                return new ProductInfoService(
                    logger: logger,
                    state: state,
                    productName: productName
                );
            });

            return state;
        }

        private static string GetProductName(IDictionary<string, object> appBaseConfig)
        {
            if (appBaseConfig is null)
                return null;

            appBaseConfig.TryGetValue("id", out object productId);
            var product = IdentityUtilsHelper.GetIndusoftProduct(productId);
            var productName = product?.Name;

            return productName;
        }
    }
}
