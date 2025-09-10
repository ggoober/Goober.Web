using Newtonsoft.Json;

namespace Goober.Web.Keycloak.Options.AuthOptions.Models;

public class TokenResponse
{
    [JsonProperty("access_token")]
    public string? AccessToken { get; set; }

    [JsonProperty("refresh_token")]
    public string? RefreshToken { get; set; }

    [JsonProperty("id_token")]
    public string? IdToken { get; set; }

    [JsonProperty("expires_in")]
    public int? ExpiresIn { get; set; }

    [JsonProperty("refresh_expires_in")]
    public int? RefreshExpiresIn { get; set; }

    [JsonProperty("token_type")]
    public string? TokenType { get; set; }

    [JsonProperty("scope")]
    public string? Scope { get; set; }
}