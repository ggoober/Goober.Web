using Goober.Web.IdentityUtils.Converters;
using Goober.Web.IdentityUtils.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace Goober.Web.IdentityUtils.Services
{
    internal class ProductInfoService : IProductInfoService
    {
        private readonly ILogger<ProductInfoService> _logger;
        private readonly ConcurrentDictionary<string, object> _state;
        private readonly string _productName;

        public ProductInfoService(
            ILogger<ProductInfoService> logger,
            ConcurrentDictionary<string, object> state,
            string productName
        )
        {
            _logger = logger;
            _state = state;
            _productName = productName;
        }

        public ProductInfoModel GetProductInfo()
        {
            var productInfo = GetProductInfoInternal();
            productInfo ??= SetProductInfoWithoutLicense();
            var productInfoModel = MapToProductInfoModel(productInfo);

            return productInfoModel;
        }

        private IProductInfo GetProductInfoInternal()
        {
            _state.TryGetValue("product-info", out var value);
            if (value is not string json)
            {
                return null;
            }

            TryDeserialize(json, out var productInfoModel);
            if (productInfoModel is null)
                return null;

            return productInfoModel;
        }

        private IProductInfo SetProductInfoWithoutLicense()
        {
            var productInfoModel = new ProductInfoInternalModel(
                productName: _productName,
                licenseInfo: null
            );

            return productInfoModel;
        }

        private static bool TryDeserialize(
            string json,
            [MaybeNullWhen(false)] out ProductInfoInternalModel productInfoModel
        )
        {
            productInfoModel = null;
            try
            {
                var settings = new JsonSerializerSettings
                {
                    Converters = { new ProductInfoModelJsonConverter() },
                };
                productInfoModel = JsonConvert.DeserializeObject<ProductInfoInternalModel>(json, settings);
            }
            catch
            {
                return false;
            }

            return true;
        }

        private static ProductInfoModel MapToProductInfoModel(IProductInfo productInfo)
        {
            if (productInfo is null)
                return null;

            var productInfoModel = new ProductInfoModel();
            productInfoModel.ProductName = productInfo.ProductName;

            if (productInfo.LicenseInfo is null)
            {
                return productInfoModel;
            }

            productInfoModel.LicenseInfo = new LicenseInfoModel
            {
                Id = productInfo.LicenseInfo.Id,
                Version = productInfo.LicenseInfo.Version,
                ExpireDate = productInfo.LicenseInfo.ExpireDate,
                IssuedAt = productInfo.LicenseInfo.IssuedAt,
                IsPerpetual = productInfo.LicenseInfo.IsPerpetual,
                LicenseAuthor = productInfo.LicenseInfo.LicenseAuthor,
                Customer = productInfo.LicenseInfo.Customer,
                Comment = productInfo.LicenseInfo.Comment,
                FileName = productInfo.LicenseInfo.FileName,
            };

            return productInfoModel;
        }
    }
}
