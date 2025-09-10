namespace Goober.Web.IdentityUtils.Models
{
    internal class ProductInfoInternalModel : IProductInfo
    {
        public string ProductName { get; }
        public ILicenseInfo LicenseInfo { get; }

        public ProductInfoInternalModel(
            string productName,
            ILicenseInfo licenseInfo
        )
        {
            ProductName = productName;
            LicenseInfo = licenseInfo;
        }
    }
}
