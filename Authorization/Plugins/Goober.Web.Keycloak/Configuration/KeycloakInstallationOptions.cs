using Goober.Web.Keycloak.Abstractions.DataObjects;
using Microsoft.Extensions.Configuration;

namespace Goober.Web.Keycloak.Configuration
{
    public class KeycloakInstallationOptions
    {
        private string? authServerUrl;
        private bool? verifyTokenAudience;
        private TimeSpan? tokenClockSkew;
        private string? sslRequired;

        [ConfigurationKeyName("auth-server-url")]
        public string AuthServerUrl
        {
            get => authServerUrl ?? AuthServerUrl2;
            set => authServerUrl = value;
        }


        [ConfigurationKeyName("signedout-redirecturi")]
        public string SignedOutRedirectUri { get; set; } = string.Empty;

        [ConfigurationKeyName("AuthServerUrl")]
        private string AuthServerUrl2 { get; set; } = default!;

        public string Realm { get; set; } = string.Empty;

        public string Resource { get; set; } = string.Empty;

        [ConfigurationKeyName("verify-token-audience")]
        public bool? VerifyTokenAudience
        {
            get => verifyTokenAudience ?? VerifyTokenAudience2;
            set => verifyTokenAudience = value;
        }
        [ConfigurationKeyName("VerifyTokenAudience")]
        private bool? VerifyTokenAudience2 { get; set; }

        public KeycloakClientInstallationCredentials Credentials { get; set; } = new();

        [ConfigurationKeyName("token-clock-skew")]
        public TimeSpan TokenClockSkew
        {
            get => tokenClockSkew ?? TokenClockSkew2 ?? TimeSpan.Zero;
            set => tokenClockSkew = value;
        }
        [ConfigurationKeyName("TokenClockSkew")]
        private TimeSpan? TokenClockSkew2 { get; set; }

        [ConfigurationKeyName("ssl-required")]
        public string SslRequired
        {
            get => sslRequired ?? SslRequired2;
            set => sslRequired = value;
        }
        [ConfigurationKeyName("SslRequired")]
        private string SslRequired2 { get; set; } = default!;

        public string KeycloakUrlRealm => $"{NormalizeUrl(AuthServerUrl)}/realms/{Realm}";

        private static string NormalizeUrl(string url)
        {
            var urlNormalized = !url.EndsWith('/') ? url : url.TrimEnd('/');

            return urlNormalized;
        }

        [ConfigurationKeyName("RolesSource")]
        public RolesClaimTransformationSource RolesSource { get; set; } = RolesClaimTransformationSource.ResourceAccess;
    }
}
