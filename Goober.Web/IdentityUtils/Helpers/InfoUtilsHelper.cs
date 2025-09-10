using Goober.Web.IdentityUtils.Models;
using System;

namespace Goober.Web.IdentityUtils.Helpers
{
    internal class LicenseInformatorFactory
    {
        internal static LicenseInformatorBase CreateInformator(LicenseModel license)
        {
            if (license == null)
                return new LicenseInformatorVer5(license);

            LicenseInformatorBase validator;
            switch (license.Version)
            {
                case 6:
                    validator = new LicenseInformatorVer6(license);
                    break;
                default:
                    validator = new LicenseInformatorVer5(license);
                    break;
            }

            return validator;
        }
    }

    internal abstract class LicenseInformatorBase
    {
        protected LicenseModel License { get; }

        public LicenseInformatorBase(
            LicenseModel license
        )
        {
            License = license;
        }

        internal abstract string GetAboutExpirationDateMessage(DateTime expireDate);
    }

    internal class LicenseInformatorVer5 : LicenseInformatorBase
    {
        public LicenseInformatorVer5(LicenseModel license) : base(license)
        {
        }

        internal override string GetAboutExpirationDateMessage(DateTime expireDate)
        {
            var expirationDate = expireDate.ToString("dd.MM.yyyy HH:mm:ss");
            var message = $"Дата окончания действия лицензии - {expirationDate}";

            return message;
        }
    }

    internal class LicenseInformatorVer6 : LicenseInformatorBase
    {
        public LicenseInformatorVer6(LicenseModel license) : base(license)
        {
        }

        internal override string GetAboutExpirationDateMessage(DateTime expireDate)
        {
            if (License.IsPerpetual == true)
                return "Срок действия лицензии – бессрочно";

            var expirationDate = expireDate.ToString("dd.MM.yyyy HH:mm:ss");
            var message = $"Дата окончания действия лицензии - {expirationDate}";

            return message;
        }
    }
}
