using System.Security.Claims;
using Goober.Base.Extensions;
using Goober.Web.Authorization.Abstractions;
using Goober.Web.Authorization.Abstractions.Responses;
using Goober.Web.LDAP.Abstractions.Models;
using Goober.Web.LDAP.Abstractions.Models.Responses;
using Goober.Web.LDAP.Configuration;
using Goober.Web.LDAP.Enums;
using Goober.Web.LDAP.Extensions;
using Goober.Web.LDAP.Glossaries;
using Novell.Directory.Ldap;

namespace Goober.Web.LDAP.Implementations
{
    internal class LdapService : IAuthenticationLiteProvider
    {
        private readonly LdapConfiguration _ldapConfig;

        public LdapService(LdapConfiguration ldapConfiguration)
        {
            _ldapConfig = ldapConfiguration;
        }

        public IClaimsAuthenticationResult Authenticate(string username, string realm, string password)
        {
            if (username == null)
                throw new ArgumentNullException(nameof(username));
            if (realm == null)
                throw new ArgumentNullException(nameof(realm));
            if (password == null)
                throw new ArgumentNullException(nameof(password));

            return AuthenticateInternal(username, realm, password);
        }

        public Task<IClaimsAuthenticationResult> AuthenticateAsync(string username, string realm, string password, CancellationToken cancellationToken = default)
        {
            if (username == null)
                throw new ArgumentNullException(nameof(username));
            if (realm == null)
                throw new ArgumentNullException(nameof(realm));
            if (password == null)
                throw new ArgumentNullException(nameof(password));

            return Task.Run<IClaimsAuthenticationResult>(() => AuthenticateInternal(username, realm, password, cancellationToken), cancellationToken);
        }

        private IClaimsAuthenticationResult AuthenticateInternal(string username, string realm, string password, CancellationToken cancellationToken = default)
        {
            try
            {
                var userNameWithoutDomain = GetNameWithoutDomainComponent(username);
                cancellationToken.ThrowIfCancellationRequested();
                using (var ldapConnection = new LdapConnection())
                {
                    ldapConnection.Connect(_ldapConfig.Address, _ldapConfig.Port);

                    cancellationToken.ThrowIfCancellationRequested();
                    if (TrySearchUser(ldapConnection, _ldapConfig, userNameWithoutDomain, cancellationToken, out var user) == false || user == null)
                        throw new ArgumentException($"Authentication failed for user '{username}' on LDAP server {_ldapConfig.Address}:{_ldapConfig.Port}.");

                    if (user.Enabled == false)
                    {
                        throw new ArgumentException($"ser account '{username}' is inactive");
                    }

                    ldapConnection.Bind(LdapConnection.LdapV3, user.DistinguishedName, password);
                    cancellationToken.ThrowIfCancellationRequested();

                    var authenticated = ldapConnection.Bound;

                    return new ClaimsAuthenticationResult(new ClaimsPrincipal(new ClaimsIdentity(BuildClaims(username, password))));
                }
            }
            catch (Exception ex)
            {
                return new ClaimsAuthenticationResult(ex);
            }
        }
        private List<Claim> BuildClaims(string userName, string password)
        {
            var claims = new List<Claim>()
            {
                new Claim("UserName", userName),
                new Claim("Password", password.EncryptString())
            };
            return claims;
        }
        private bool TrySearchUser(
            LdapConnection ldapConnection,
            LdapConfiguration ldapConfig,
            string userName,
            CancellationToken cancellationTokem,
            out ADUser? user)
        {
            user = null;

            var bindDistinguishedName = BuildDistinguishedName(ldapConfig.UserName, ldapConfig.UsersCommonName, ldapConfig.DomainComponent);
            cancellationTokem.ThrowIfCancellationRequested();
            ldapConnection.Bind(LdapConnection.LdapV3, bindDistinguishedName, ldapConfig.GetPassword());
            cancellationTokem.ThrowIfCancellationRequested();

            if (ldapConnection.Bound == false)
            {
                throw new ArgumentException($"Authentication failed for user '{ldapConfig.UserName}' on LDAP server {ldapConfig.Address}:{ldapConfig.Port}.");
            };

            var filter = (ldapConfig.FilterSearchUser ?? LdapFilterDefaultGlossary.FilterSearchUser)
                .Replace(LdapFilterParamGlossary.UserName, userName, StringComparison.OrdinalIgnoreCase);

            cancellationTokem.ThrowIfCancellationRequested();
            var searchQueue = ldapConnection.Search(
                @base: ldapConfig.DomainComponent,
                scope: LdapConnection.ScopeSub,
                filter: filter,
                attrs: new string[] { LdapAttributeGlossary.UserAccountControl, LdapAttributeGlossary.SAMAccountName },
                typesOnly: false,
                queue: null
            );

            LdapMessage message;

            while ((message = searchQueue.GetResponse()) != null)
            {
                cancellationTokem.ThrowIfCancellationRequested();
                if (message is LdapSearchResult searchResult)
                {
                    var set = searchResult?.Entry?.GetAttributeSet() as Dictionary<string, LdapAttribute>;

                    if (set == null)
                    {
                        throw new ArgumentException($"Can't find LDAP attributes for user '{userName}'");
                    }

                    TryGetSAMAccountName(set, out var sMAccountName);
                    TryGetUserAccountEnabled(set, out var userAccountEnabled);

                    user = new ADUser()
                    {
                        Name = sMAccountName,
                        DistinguishedName = searchResult!.Entry.Dn,
                        Enabled = userAccountEnabled
                    };

                    return true;
                }
            }

            return false;
        }

        static string BuildDistinguishedName(string userName, string usersCommonName, string domainComponent)
            => $"CN={userName},{usersCommonName},{domainComponent}";

        private static string GetNameWithoutDomainComponent(string userNameWithDc)
        {
            var userName = userNameWithDc.Split('\\');
            if (userName.Length > 1)
                return userName[1];

            userName = userNameWithDc.Split('@');
            if (userName.Length > 1)
                return userName[0];

            return userNameWithDc;
        }

        private bool TryGetSAMAccountName(Dictionary<string, LdapAttribute> set, out string? sAMAccountName)
        {
            sAMAccountName = null;

            if (set.TryGetValue(LdapAttributeGlossary.SAMAccountName, out var attrSAMAccountName) == false)
            {
                throw new ArgumentException($"Could not find LDAP attribute {LdapAttributeGlossary.SAMAccountName}");
            };

            sAMAccountName = attrSAMAccountName.StringValue;

            return true;
        }

        private bool TryGetUserAccountEnabled(Dictionary<string, LdapAttribute> set, out bool userAccountEnabled)
        {
            userAccountEnabled = false;

            if (set.TryGetValue(LdapAttributeGlossary.UserAccountControl, out var attrUserAccountControl) == false)
            {
                throw new ArgumentException($"Could not find LDAP attribute {LdapAttributeGlossary.UserAccountControl}");
            };

            if (Enum.TryParse<LdapUserAccountControl>(attrUserAccountControl.StringValue, out var userAccountControl) == false)
            {
                throw new ArgumentException($"Failed to read value for LDAP attribute {LdapAttributeGlossary.UserAccountControl}");
            };

            userAccountEnabled = (userAccountControl & LdapUserAccountControl.ACCOUNTDISABLE) == 0;

            return true;
        }
    }
}
