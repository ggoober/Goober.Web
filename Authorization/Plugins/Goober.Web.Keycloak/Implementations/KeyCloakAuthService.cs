using Goober.Base.Extensions;
using Goober.Web.Authorization.Abstractions.Responses;
using Goober.Web.Keycloak.Abstractions;
using Goober.Web.Keycloak.Abstractions.DataObjects;
using Goober.Web.Keycloak.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Net;
using System.Security.Claims;

namespace Goober.Web.Keycloak.Implementations
{
    internal class KeyCloakAuthService : IKeycloakAuthenticationProvider, ITokensTranslator
    {
        private static readonly ConcurrentDictionary<string, EndpointsInfo> _realmEndpoints;
        private readonly IServiceProvider _serviceProvider;
        private readonly string _defaultRealm;
        private readonly Uri _defaultUrl;
        private readonly string _secret;
        private readonly string _clientId;
        private readonly HttpClient _client;
        private readonly ILogger _logger;

        static KeyCloakAuthService()
        {
            _realmEndpoints = new ConcurrentDictionary<string, EndpointsInfo>();
        }

        public KeyCloakAuthService(
            IServiceProvider serviceProvider,
            KeycloakAuthenticationOptions configuration,
            ILogger<KeyCloakAuthService> logger
        )
        {
            _serviceProvider = serviceProvider;
            _defaultRealm = configuration.Realm;
            _clientId = configuration.Resource;
            _defaultUrl = new Uri(configuration.AuthServerUrl);
            _secret = configuration.Credentials.GetSecret();
            _client = CreateHttpClient();
            _logger = logger;
        }

        public async Task<string> UserInfoAsync(
            string currentToken,
            string realm,
            CancellationToken cancellationToken = default)
        {
            HttpClient client = _client;
            EndpointsInfo endpointsInfo = GetEndpointsForRealm(realm);
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri(endpointsInfo.UserInfoEndpoint!),
                Method = HttpMethod.Get,
            };

            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
                "Bearer",
                currentToken
            );
            var response = await client.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                throw new ArgumentException(
                    $"Failed to get data about realm \"{realm}\"",
                    nameof(realm)
                );
            }
            else if (response.IsSuccessStatusCode)
            {
                var stringContent = await response.Content.ReadAsStringAsync();
                return stringContent;
            }
            else
            {
                throw new InvalidOperationException(await response.Content.ReadAsStringAsync());
            }
        }

        public async Task<string> IntrospectAsync(
            string currentToken,
            string realm,
            CancellationToken cancellationToken = default)
        {
            string[] parts = currentToken.Split(" ");
            if (parts.Length == 2)
            {
                currentToken = parts[1];
            }
            HttpClient client = CreateHttpClient();
            EndpointsInfo endpointsInfo = GetEndpointsForRealm(realm);
            var content = new FormUrlEncodedContent(
                new[]
                {
                    new KeyValuePair<string, string>("client_id", _clientId),
                    new KeyValuePair<string, string>("token", currentToken),
                    new KeyValuePair<string, string>("client_secret", _secret),
                }
            );

            var response = await client.PostAsync(endpointsInfo.IntrospectionEndpoint, content);
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                throw new ArgumentException(
                    $"Failed to get data about realm \"{realm}\"",
                    nameof(realm)
                );
            }
            else if (response.IsSuccessStatusCode)
            {
                var stringContent = await response.Content.ReadAsStringAsync();
                return stringContent;
            }
            else
            {
                throw new InvalidOperationException(await response.Content.ReadAsStringAsync());
            }
        }

        public async Task<EndpointsInfo> LoadEndpointsForRealmAsync(string realmName)
        {
            HttpClient client = _client;
            HttpRequestMessage request = new HttpRequestMessage()
            {
                RequestUri = new Uri(
                    _defaultUrl,
                    $"realms/{realmName}/.well-known/openid-configuration"
                ),
                Method = HttpMethod.Get,
            };

            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                throw new ArgumentException(
                    $"Failed to get data about realm \"{realmName}\"",
                    nameof(realmName)
                );
            }
            else if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<EndpointsInfo>(
                    await response.Content.ReadAsStringAsync()
                )!;
            }
            else
            {
                throw new InvalidOperationException(await response.Content.ReadAsStringAsync());
            }
        }

        private EndpointsInfo GetEndpointsForRealm(string realmName)
        {
            string key = realmName.ToLower();
            return _realmEndpoints.GetOrAdd(
                key,
                (k) => LoadEndpointsForRealmAsync(realmName).Result
            );
        }

        private async Task<TokenInfo> ConvertToLocalTokenAsync(
            TokenInfo keycloakToken,
            CancellationToken cancellationToken)
        {
            return new TokenInfo()
            {
                ExpiresIn = keycloakToken.ExpiresIn,
                NotBeforePolicy = keycloakToken.NotBeforePolicy,
                RefreshExpiresIn = keycloakToken.RefreshExpiresIn,
                RefreshToken = keycloakToken.RefreshToken,
                Scope = keycloakToken.Scope,
                SessionState = keycloakToken.SessionState,
                TokenType = keycloakToken.TokenType,
                AccessToken = !string.IsNullOrEmpty(keycloakToken.AccessToken) ? await TranslateTokenAsync(
                                keycloakToken.AccessToken,
                                cancellationToken
                              ) : string.Empty
            };
        }

        public async Task<string> TranslateTokenAsync(
            string baseToken,
            CancellationToken cancellationToken = default,
            params Claim[] extraClaims
        )
        {
            IJWTTokenGenerator generator =
                _serviceProvider.GetRequiredService<IJWTTokenGenerator>();
            KeycloakAuthenticationOptions keycloakOptions =
                _serviceProvider.GetRequiredService<KeycloakAuthenticationOptions>();
            var claims = new List<Claim>();
            long? expTime = null;
            ISet<string> requestedClaims = new HashSet<string>(
                keycloakOptions.RequestedClaims?.Select(x => x.ToLower()) ?? Array.Empty<string>()
            );

            var baseTokenClaims = generator.GetTokenClaims(baseToken);
            foreach (Claim claim in baseTokenClaims.Union(extraClaims))
            {
                if (claim.Type == "aud")
                {
                    claims.Add(claim);
                }

                if (claim.Type == "exp" && long.TryParse(claim.Value, out long time))
                {
                    expTime = time;
                }

                if (requestedClaims.Count == 0 || requestedClaims.Contains(claim.Type.ToLower()))
                {
                    claims.Add(
                        new Claim(
                            $"{keycloakOptions.KeycloakClaimsPrefix}{claim.Type}",
                            claim.Value,
                            claim.ValueType,
                            claim.Issuer,
                            claim.OriginalIssuer,
                            claim.Subject
                        )
                    );
                }
            }

            IExternalClaimsReceiver? externalClaimsReceiver =
                _serviceProvider.GetService<IExternalClaimsReceiver>();

            if (externalClaimsReceiver != null)
            {
                var externalClaims = await externalClaimsReceiver.ReceiveExternalClaimsAsync(
                    claims,
                    cancellationToken
                );
                claims.AddRange(externalClaims);
            }

            string token = generator.GenerateJwtToken(
                claims,
                DateTime.UtcNow,
                expTime,
                keycloakOptions.Credentials.GetSecret()
            );
            return token;
        }

        private HttpClient CreateHttpClient()
        {
            IHttpClientFactory httpClientFactory =
                _serviceProvider.GetRequiredService<IHttpClientFactory>();
            return httpClientFactory.CreateClient(nameof(KeyCloakAuthService));
        }

        public ITokenAuthenticationResult Authenticate(
            string username,
            string realm,
            string password
        )
        {
            return AuthenticateAsync(username, realm, password).Result;
        }

        public async Task<ITokenAuthenticationResult> AuthenticateAsync(
            string username,
            string realm,
            string password,
            CancellationToken cancellationToken = default
        )
        {
            if (username == null)
                throw new ArgumentNullException(nameof(username));
            if (realm == null)
                throw new ArgumentNullException(nameof(realm));
            if (password == null)
                throw new ArgumentNullException(nameof(password));

            try
            {
                HttpClient client = _client;
                EndpointsInfo endpointsInfo = GetEndpointsForRealm(realm);
                var content = new FormUrlEncodedContent(
                    new[]
                    {
                        new KeyValuePair<string, string>(
                            "grant_type",
                            EndpointsInfo.TokenEndpointMode.Password
                        ),
                        new KeyValuePair<string, string>("client_id", _clientId),
                        new KeyValuePair<string, string>("username", username),
                        new KeyValuePair<string, string>("password", password),
                        new KeyValuePair<string, string>("client_secret", _secret),
                    }
                );

                HttpResponseMessage response = await client.PostAsync(
                    endpointsInfo.TokenEndpoint,
                    content,
                    cancellationToken
                );
                if (response.IsSuccessStatusCode)
                {
                    TokenInfo keycloakToken = JsonConvert.DeserializeObject<TokenInfo>(
                        await response.Content.ReadAsStringAsync(cancellationToken)
                    )!;
                    return new TokenAuthenticationResult(
                        await ConvertToLocalTokenAsync(keycloakToken, cancellationToken)
                    );
                }
                else
                {
                    throw new InvalidOperationException(
                        await response.Content.ReadAsStringAsync(cancellationToken)
                    );
                }
            }
            catch (Exception ex)
            {
                return new TokenAuthenticationResult(ex);
            }
        }

        public Task<ITokenAuthenticationResult> AuthenticateByTokenAsync(
            string token,
            string realm,
            CancellationToken cancellationToken = default
        )
        {
            if (token == null)
                throw new ArgumentNullException(nameof(token));
            if (realm == null)
                throw new ArgumentNullException(nameof(realm));

            return ExecuteTokenRequestAsync(
                realm: realm,
                authData: new Dictionary<string, string>
                {
                    { "grant_type", EndpointsInfo.TokenEndpointMode.AccessToken },
                    { "client_id", _clientId},
                    { "token", token},
                    { "client_secret", _secret}
                });
        }

        private async Task<ITokenAuthenticationResult> ExecuteTokenRequestAsync(
            string realm,
            IReadOnlyDictionary<string, string> authData,
            CancellationToken cancellationToken = default
        )
        {
            try
            {
                HttpClient client = _client;
                EndpointsInfo endpointsInfo = GetEndpointsForRealm(realm);
                var content = new FormUrlEncodedContent(authData);

                HttpResponseMessage response = await client.PostAsync(
                    endpointsInfo.TokenEndpoint,
                    content,
                    cancellationToken
                );
                if (response.IsSuccessStatusCode)
                {
                    TokenInfo keycloakToken = JsonConvert.DeserializeObject<TokenInfo>(
                        await response.Content.ReadAsStringAsync(cancellationToken)
                    )!;
                    return new TokenAuthenticationResult(
                        await ConvertToLocalTokenAsync(keycloakToken, cancellationToken)
                    );
                }
                else
                {
                    throw new InvalidOperationException(
                        await response.Content.ReadAsStringAsync(cancellationToken)
                    );
                }
            }
            catch (Exception ex)
            {
                return new TokenAuthenticationResult(ex);
            }
        }

        public ITokenAuthenticationResult RefreshToken(string refreshToken, string realm)
        {
            return RefreshTokenAsync(refreshToken, realm).Result;
        }

        public async Task<ITokenAuthenticationResult> RefreshTokenAsync(
            string refreshToken,
            string realm,
            CancellationToken cancellationToken = default
        )
        {
            if (refreshToken == null)
                throw new ArgumentNullException(nameof(refreshToken));
            if (realm == null)
                throw new ArgumentNullException(nameof(realm));

            try
            {
                HttpClient client = _client;
                EndpointsInfo endpointsInfo = GetEndpointsForRealm(realm);
                var content = new FormUrlEncodedContent(
                    new[]
                    {
                        new KeyValuePair<string, string>(
                            "grant_type",
                            EndpointsInfo.TokenEndpointMode.RefreshToken
                        ),
                        new KeyValuePair<string, string>("client_id", _clientId),
                        new KeyValuePair<string, string>("refresh_token", refreshToken),
                        new KeyValuePair<string, string>("client_secret", _secret),
                    }
                );

                var request = new HttpRequestMessage()
                {
                    RequestUri = new Uri(endpointsInfo.TokenEndpoint!),
                    Method = HttpMethod.Post,
                    Content = content
                };

                HttpResponseMessage response = await client.SendAsync(request, cancellationToken);
                if (response.IsSuccessStatusCode)
                {
                    TokenInfo keycloakToken = JsonConvert.DeserializeObject<TokenInfo>(
                        await response.Content.ReadAsStringAsync(cancellationToken)
                    )!;
                    return new TokenAuthenticationResult(
                        await ConvertToLocalTokenAsync(keycloakToken, cancellationToken)
                    );
                }
                else
                {
                    throw new InvalidOperationException(
                        await response.Content.ReadAsStringAsync(cancellationToken)
                    );
                }
            }
            catch (Exception ex)
            {
                return new TokenAuthenticationResult(ex);
            }
        }

        public void Logout(string refreshToken, string realm)
        {
            LogoutAsync(refreshToken, realm).RunSynchronously();
        }

        public async Task LogoutAsync(
            string refreshToken,
            string realm,
            CancellationToken cancellationToken = default
        )
        {
            HttpClient client = _client;
            EndpointsInfo endpointsInfo = GetEndpointsForRealm(realm);
            var content = new FormUrlEncodedContent(
                new[]
                {
                    new KeyValuePair<string, string>("client_id", _clientId),
                    new KeyValuePair<string, string>("refresh_token", refreshToken),
                    new KeyValuePair<string, string>("client_secret", _secret),
                }
            );

            HttpResponseMessage response = await client.PostAsync(
                endpointsInfo.EndSessionEndpoint,
                content,
                cancellationToken
            );
            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidOperationException(
                    await response.Content.ReadAsStringAsync(cancellationToken)
                );
            }
        }

    }
}
