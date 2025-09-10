using Goober.Base.Extensions;
using Microsoft.AspNetCore.Authentication.Negotiate;
using System.DirectoryServices.Protocols;
using System.Net;

namespace Goober.Web.Negotiate.Configuration
{
    public class LDAPConnection
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string EncryptedPassword { get; set; }
        public string ServerAdress { get; set; }
        public void ApplyTo(LdapSettings ldapSettings)
        {
            var ldapConnection = new LdapConnection(
               new LdapDirectoryIdentifier(ServerAdress),
               new NetworkCredential(UserName, !string.IsNullOrWhiteSpace(EncryptedPassword) ? CryptoExtensions.DecryptString(EncryptedPassword) : Password), AuthType.Basic);
            ldapConnection.SessionOptions.ReferralChasing = ReferralChasingOptions.None;
            ldapSettings.LdapConnection = ldapConnection;
        }
    }
}
