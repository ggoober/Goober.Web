using Goober.Web.Keycloak.Abstractions;
using Goober.Web.Keycloak.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Goober.Web.Keycloak.Implementations
{
    internal class JWTTokenGenerator : IJWTTokenGenerator
    {
        private readonly KeycloakAuthenticationOptions _options;
        public JWTTokenGenerator(KeycloakAuthenticationOptions options)
        {
            _options = options;
        }

        public string GenerateJwtToken(IEnumerable<Claim> claims, DateTime currentDate, long? expTime, string secretKey)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Issuer = _options.Credentials.Issuer,
                Expires = expTime.HasValue ? DateTimeOffset.FromUnixTimeSeconds(expTime.Value).UtcDateTime : currentDate.AddSeconds(1200),
                SigningCredentials = new SigningCredentials(
                    GetSecurityKey(_options.Credentials.GetEncryptionKey()),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public static SecurityKey GetSecurityKey(string signature)
        {
            var encryptionKey = Encoding.UTF8.GetBytes(signature);
            byte[] ecKey = new byte[256 / 8];
            Array.Copy(encryptionKey, ecKey, 256 / 8);

            return new SymmetricSecurityKey(ecKey);
        }

        public IEnumerable<Claim> GetTokenClaims(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var securityToken = tokenHandler.ReadToken(token);
            if (securityToken is JwtSecurityToken jwtSecurityToken)
            {
                return jwtSecurityToken.Claims;
            }

            return Array.Empty<Claim>();
        }
    }
}
