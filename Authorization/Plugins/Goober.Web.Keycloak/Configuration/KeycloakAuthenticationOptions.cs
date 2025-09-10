using Goober.Web.Authorization.Configuration;
using Goober.Web.Authorization.Glossaries;
using Microsoft.Extensions.Configuration;

namespace Goober.Web.Keycloak.Configuration
{
    public class KeycloakAuthenticationOptions : KeycloakInstallationOptions
    {
        public const string GlobalAuthSection = ConfigGlossary.AuthSection;
        public const string Section = ConfigurationConstants.ConfigurationPrefix;

        [ConfigurationKeyName("keycloack-claims-prefix")]
        public string? KeycloakClaimsPrefix { get; set; }
        [ConfigurationKeyName("requested-claims")]
        public string[]? RequestedClaims { get; set; }
        [ConfigurationKeyName("access-token-check-remaining-time")]
        public TimeSpan? AccessTokenCheckRemainingTime { get; set; }
        [ConfigurationKeyName("refresh-token-check-remaining-time")]
        public TimeSpan? RefreshTokenCheckRemainingTime { get; set; }
        [ConfigurationKeyName("Cookie")]
        public CookiesOptions? CookiesOptions { get; set; }
        [ConfigurationKeyName("JwtBearer")]
        public JWTOptions? JWTOptions { get; set; }
        [ConfigurationKeyName("OpenId")]
        public OpenIdOptions? OpenIdOptions { get; set; }
    }
}
