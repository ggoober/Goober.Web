using Goober.Web.Authorization.Abstractions.Responses;
using Newtonsoft.Json;

namespace Goober.Web.Keycloak.Abstractions.DataObjects
{
    internal class TokenInfo : ITokenInfo
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; } = string.Empty;
        [JsonProperty("expires_in")]
        public int? ExpiresIn { get; set; }
        [JsonProperty("refresh_expires_in")]
        public int? RefreshExpiresIn { get; set; }
        [JsonProperty("refresh_token")]
        public string? RefreshToken { get; set; }
        [JsonProperty("token_type")]
        public string TokenType { get; set; } = string.Empty;
        [JsonProperty("not-before-policy")]
        public int NotBeforePolicy { get; set; }
        [JsonProperty("e87c789c-945f-4afd-8ec5-13f2a5d05751")]
        public Guid? SessionState { get; set; }
        [JsonProperty("profile email")]
        public string Scope { get; set; } = string.Empty;
    }
}
