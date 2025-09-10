using System;

namespace Goober.Web.IdentityUtils.Models
{
    internal class LicenseModel
    {
        internal int Version { get; set; }
        internal string Sign { get; set; }
        internal string ProductName { get; set; }
        internal string Customer { get; set; }
        internal string FileName { get; set; }
        internal Guid LicGuid { get; set; }
        internal DateTime LicDate { get; set; }
        internal DateTime LicWDate { get; set; }
        internal bool IsPerpetual { get; set; }
        internal string LicenseAuthor { get; set; }
        internal string Comment { get; set; }
    }
}
