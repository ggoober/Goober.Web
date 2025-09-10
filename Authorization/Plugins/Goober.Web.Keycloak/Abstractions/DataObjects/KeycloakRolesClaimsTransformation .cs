using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Text.Json;

namespace Goober.Web.Keycloak.Abstractions.DataObjects
{
    internal class KeycloakRolesClaimsTransformation : IClaimsTransformation
    {
        private readonly string roleClaimType;
        private readonly RolesClaimTransformationSource roleSource;
        private readonly string audience;

        public KeycloakRolesClaimsTransformation(
            string roleClaimType,
            RolesClaimTransformationSource roleSource,
            string audience)
        {
            this.roleClaimType = roleClaimType;
            this.roleSource = roleSource;
            this.audience = audience;
        }

        public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            var result = principal.Clone();
            if (result.Identity is not ClaimsIdentity identity)
            {
                return Task.FromResult(result);
            }

            if (roleSource == RolesClaimTransformationSource.ResourceAccess)
            {
                var resourceAccessValue = principal.FindFirst("resource_access")?.Value;
                if (string.IsNullOrWhiteSpace(resourceAccessValue))
                {
                    return Task.FromResult(result);
                }

                using var resourceAccess = JsonDocument.Parse(resourceAccessValue);
                var containsAudienceRoles = resourceAccess
                    .RootElement
                    .TryGetProperty(audience, out var rolesElement);

                if (!containsAudienceRoles)
                {
                    return Task.FromResult(result);
                }

                var clientRoles = rolesElement.GetProperty("roles");

                foreach (var role in clientRoles.EnumerateArray())
                {
                    var value = role.GetString();

                    var matchingClaim = identity.Claims.FirstOrDefault(claim =>
                        claim.Type.Equals(roleClaimType, StringComparison.InvariantCultureIgnoreCase) &&
                        claim.Value.Equals(value, StringComparison.InvariantCultureIgnoreCase));

                    if (matchingClaim is null && !string.IsNullOrWhiteSpace(value))
                    {
                        identity.AddClaim(new Claim(roleClaimType, value));
                    }
                }

                return Task.FromResult(result);
            }

            if (roleSource == RolesClaimTransformationSource.Realm)
            {
                var realmAccessValue = principal.FindFirst("realm_access")?.Value;
                if (string.IsNullOrWhiteSpace(realmAccessValue))
                {
                    return Task.FromResult(result);
                }

                using var realmAccess = JsonDocument.Parse(realmAccessValue);

                var containsRoles = realmAccess
                    .RootElement
                    .TryGetProperty("roles", out var rolesElement);

                if (containsRoles)
                {
                    foreach (var role in rolesElement.EnumerateArray())
                    {
                        var value = role.GetString();

                        var matchingClaim = identity.Claims.FirstOrDefault(claim =>
                            claim.Type.Equals(roleClaimType, StringComparison.InvariantCultureIgnoreCase) &&
                            claim.Value.Equals(value, StringComparison.InvariantCultureIgnoreCase));

                        if (matchingClaim is null && !string.IsNullOrWhiteSpace(value))
                        {
                            identity.AddClaim(new Claim(roleClaimType, value));
                        }
                    }

                    return Task.FromResult(result);
                }
            }

            return Task.FromResult(result);
        }
    }
}
