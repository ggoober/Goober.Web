using Goober.Base.Extensions;
using Goober.Web.LDAP.Configuration;

namespace Goober.Web.LDAP.Extensions
{
    public static class LdapConfigurationExtensions
    {
        internal static string? GetPassword(
            this LdapConfiguration ldapOptions)
        {
            if (!string.IsNullOrWhiteSpace(ldapOptions.PasswordEncrypted) && string.IsNullOrWhiteSpace(ldapOptions.Password))
            {
                return CryptoExtensions.DecryptString(ldapOptions.PasswordEncrypted);
            }
            return ldapOptions.Password;
        }
    }
}
