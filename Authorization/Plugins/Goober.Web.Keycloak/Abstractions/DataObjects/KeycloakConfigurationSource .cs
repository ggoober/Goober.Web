using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace Goober.Web.Keycloak.Abstractions.DataObjects
{
    internal class KeycloakConfigurationSource : JsonConfigurationSource
    {
        public override IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            EnsureDefaults(builder);
            return new KeycloakConfigurationProvider(this);
        }
    }
}
