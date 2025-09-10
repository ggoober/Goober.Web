using Goober.Web.Authorization.Glossaries;
using Goober.Web.LDAP.Configuration;
using Goober.Web.LDAP.Glossaries;
using Microsoft.Extensions.Configuration;

namespace Goober.Web.LDAP.Extensions
{
    public static class ConfigurationExtensions
    {
        public static LdapConfiguration? GetLDAPAuthenticationOptions(
            this IConfiguration configuration)
        {
            if (configuration is null)
                throw new ArgumentNullException(nameof(configuration));

            IConfigurationSection? globalSection = configuration.GetChildren().FirstOrDefault(x => x.Key == ConfigGlossary.AuthSection);
            if (globalSection is null)
                return null;

            var ldapEnabled = globalSection.GetValue<string>(ConfigGlossary.ProviderSection)?.ToLower() == LDAPConfigurationGlossary.LDAPConfigSection.ToLower();
            if (ldapEnabled is not true)
                return null;

            var ldapSection = globalSection.GetChildren().FirstOrDefault(x => x.Key == LDAPConfigurationGlossary.LDAPConfigSection);
            if (ldapSection is null)
                return null;

            var options = new LdapConfiguration();
            ldapSection.Bind(options, opt => { opt.BindNonPublicProperties = true; });
            return options;

        }
    }
}
