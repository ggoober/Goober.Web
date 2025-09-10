namespace Goober.Web.IdentityUtils.Models
{
    internal interface IProductInfo
    {
        public string ProductName { get; }
        public ILicenseInfo LicenseInfo { get; }
    }
}
