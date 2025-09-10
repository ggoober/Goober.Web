using System;

namespace Goober.Web.IdentityUtils.Models
{
    internal interface ILicenseInfo
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
    }
}
