namespace Goober.Web.IdentityUtils.Models
{
    internal enum LicenseValidationResult
    {
        Success,
        NoFoundLicenseFile,
        NoParsedLicense,
        NoCorrectSignature,
        NoDeserializedLicense,
        NoCorrectEnvironmentSignature,
        NoValidProduct,
        NoValidDate,
        Unknown
    }
}
