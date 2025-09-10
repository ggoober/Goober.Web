using Newtonsoft.Json;

namespace Goober.Web.Keycloak.Abstractions.DataObjects
{
    public class EndpointsInfo
    {
        [JsonProperty("authorization_endpoint")]
        public string? AuthorizationEndpoint { get; set; }
        [JsonProperty("token_endpoint")]
        public string? TokenEndpoint { get; set; }
        [JsonProperty("introspection_endpoint")]
        public string? IntrospectionEndpoint { get; set; }
        [JsonProperty("userinfo_endpoint")]
        public string? UserInfoEndpoint { get; set; }
        [JsonProperty("end_session_endpoint")]
        public string? EndSessionEndpoint { get; set; }

        public class TokenEndpointMode
        {
            public const string Password = "password";
            public const string AccessToken = "access_token";
            public const string RefreshToken = "refresh_token";
        }
    }
}
