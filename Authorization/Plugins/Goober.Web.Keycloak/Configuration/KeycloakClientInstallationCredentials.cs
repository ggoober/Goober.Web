using Microsoft.Extensions.Configuration;

namespace Goober.Web.Keycloak.Configuration
{
    public class KeycloakClientInstallationCredentials
    {
        [ConfigurationKeyName("secret")]
        public string Secret { get; set; } = string.Empty;
        [ConfigurationKeyName("encryption-key")]
        public string EncryptionKey { get; set; } = string.Empty;
        [ConfigurationKeyName("encrypted-secret")]
        public string SecuredSecret { get; set; } = string.Empty;
        [ConfigurationKeyName("encrypted-encryption-key")]
        public string SecuredEncryptionKey { get; set; } = string.Empty;
        [ConfigurationKeyName("issuer")]
        public string Issuer { get; set; } = string.Empty;
    }
}
