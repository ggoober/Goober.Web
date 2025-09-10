using System;

namespace Goober.Web.IdentityUtils.Models
{
    public class LicenseInfoModel
    {
        public Guid Id { get; set; }
        public int Version { get; set; }
        public string Customer { get; set; }
        public DateTime IssuedAt { get; set; }
        public DateTime ExpireDate { get; set; }
        public bool IsPerpetual { get; set; }
        public string LicenseAuthor { get; set; }
        public string FileName { get; set; }
        public string Comment { get; set; }
    }
}
