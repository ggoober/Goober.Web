using System;

namespace Goober.Web.IdentityUtils.Models
{
    internal class LicenseInfoInternalModel : ILicenseInfo
    {
        public Guid Id { get; }
        public int Version { get; }
        public string Customer { get; }
        public DateTime IssuedAt { get; }
        public DateTime ExpireDate { get; }
        public bool IsPerpetual { get; }
        public string LicenseAuthor { get; }
        public string FileName { get; }
        public string Comment { get; }

        public LicenseInfoInternalModel(
            Guid id,
            int version,
            string customer,
            DateTime issuedAt,
            DateTime expireDate,
            string licenseAuthor,
            string fileName,
            string comment,
            bool isPerpetual = false
        )
        {
            Id = id;
            Version = version;
            Customer = customer;
            IssuedAt = issuedAt;
            ExpireDate = expireDate;
            LicenseAuthor = licenseAuthor;
            FileName = fileName;
            Comment = comment;
            IsPerpetual = isPerpetual;
        }
    }
}
