using Goober.Base.Services;
using Goober.Web.Keycloak.Abstractions;
using Goober.Web.Keycloak.Abstractions.DataObjects;
using Goober.Web.Keycloak.Options.AuthOptions.Models;
using Goober.Web.Keycloak.Options.AuthOptions.Services.Abstractions;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Goober.Web.Keycloak.Options.AuthOptions.Services;

internal class KeycloakTokenService : IKeycloakTokenService
{
    private const string TokenGrantTypeRequest = "grant_type";
    private const string TokenRefreshTokenRequest = "refresh_token";
    private const string TokenClientIdRequest = "client_id";
    private const string TokenClientSecretRequest = "client_secret";

    private readonly ILogger<KeycloakTokenService> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IDateTimeService _dateTimeService;

    public KeycloakTokenService(
        ILogger<KeycloakTokenService> logger,
        IHttpContextAccessor httpContextAccessor,
        IDateTimeService dateTimeService
    )
    {
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
        _dateTimeService = dateTimeService;
    }

    /// <inheritdoc/>
    public TimeSpan? GetAccessTokenLeftTime()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null)
            return null;

        return GetAccessTokenLeftTime(httpContext.User);
    }

    /// <inheritdoc/>
    public TimeSpan? GetAccessTokenLeftTime(ClaimsPrincipal? principal)
    {
        if (principal == null)
            return null;

        var accessToken = principal.FindFirst(KeycloakClaimTypes.AccessToken)?.Value;
        return GetTokenLeftTime(accessToken);
    }

    /// <inheritdoc/>
    public TimeSpan? GetTokenLeftTime(string? token)
    {
        if (string.IsNullOrEmpty(token))
            return null;

        return GetExpirationTimeInternal(token);
    }

    /// <inheritdoc />
    public async Task<RefreshAccessTokenResult> GetAccessTokenByRefreshTokenAsync()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null)
        {
            return new RefreshAccessTokenResult
            {
                ErrorMessage = "Не удалось получить HttpContext."
            };
        }

        var principal = httpContext.User;
        var refreshToken = principal.FindFirst(KeycloakClaimTypes.RefreshToken)?.Value;

        var result = await GetAccessTokenByRefreshTokenInternalAsync(
            principal: principal,
            refreshToken: refreshToken);

        return result;
    }

    /// <inheritdoc />
    public async Task<RefreshAccessTokenResult> GetAccessTokenByRefreshTokenAsync(
        ClaimsPrincipal? principal
    )
    {
        if (principal == null)
        {
            return new RefreshAccessTokenResult
            {
                ErrorMessage = "ClaimsPrincipal не предоставлен."
            };
        }

        var refreshToken = principal.FindFirst(KeycloakClaimTypes.RefreshToken)?.Value;

        var result = await GetAccessTokenByRefreshTokenInternalAsync(
            principal: principal,
            refreshToken: refreshToken);

        return result;
    }

    /// <inheritdoc />
    public async Task<RefreshAccessTokenResult> GetAccessTokenByRefreshTokenAsync(
        string? refreshToken
    )
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null)
        {
            return new RefreshAccessTokenResult
            {
                ErrorMessage = "Не удалось получить HttpContext."
            };
        }

        if (string.IsNullOrEmpty(refreshToken))
        {
            return new RefreshAccessTokenResult
            {
                ErrorMessage = "Refresh token не предоставлен."
            };
        }

        var result = await GetAccessTokenByRefreshTokenInternalAsync(
            principal: httpContext.User,
            refreshToken: refreshToken);

        return result;
    }

    private TimeSpan? GetExpirationTimeInternal(string token)
    {
        if (string.IsNullOrEmpty(token))
            return null;

        JwtSecurityToken jwtAccessToken;
        try
        {
            jwtAccessToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
        }
        catch (Exception)
        {
            return null;
        }

        if (jwtAccessToken.Payload.Exp.HasValue == false)
            return null;
        var exp = jwtAccessToken.Payload.Exp.Value;
        var utcNow = _dateTimeService.GetDateTimeOffsetUtcNow().ToUnixTimeSeconds();
        var accessTokenExp = exp - utcNow;
        var accessTokenExpirationTime = TimeSpan.FromSeconds(accessTokenExp);

        return accessTokenExpirationTime;
    }

    private async Task<RefreshAccessTokenResult> GetAccessTokenByRefreshTokenInternalAsync(
        ClaimsPrincipal principal,
        string? refreshToken
    )
    {
        if (string.IsNullOrEmpty(refreshToken))
        {
            return new RefreshAccessTokenResult
            {
                ErrorMessage = "Refresh token не предоставлен."
            };
        }

        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null)
        {
            var message = "Не удалось получить HttpContext";
            return new RefreshAccessTokenResult
            {
                ErrorMessage = message,
            };
        }

        var openIdConnectOptions = httpContext.RequestServices
            .GetRequiredService<IOptionsSnapshot<OpenIdConnectOptions>>()
            .Get(OpenIdConnectDefaults.AuthenticationScheme);

        var httpClient = new HttpClient();
        var tokenEndpoint = $"{openIdConnectOptions.Authority}/protocol/openid-connect/token";

        var tokenRequestParameters = new Dictionary<string, string>
        {
            { TokenGrantTypeRequest, KeycloakClaimTypes.RefreshToken },
            { TokenRefreshTokenRequest, refreshToken },
            { TokenClientIdRequest, openIdConnectOptions.ClientId },
            { TokenClientSecretRequest, openIdConnectOptions.ClientSecret }
        };

        var requestContent = new FormUrlEncodedContent(tokenRequestParameters);
        HttpResponseMessage tokenResponse;
        try
        {
            tokenResponse = await httpClient.PostAsync(tokenEndpoint, requestContent);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Ошибка при запросе refresh token к {TokenEndpoint}", tokenEndpoint);
            return new RefreshAccessTokenResult
            {
                ErrorMessage = "Ошибка при выполнении HTTP запроса для обновления токена."
            };
        }

        if (tokenResponse.IsSuccessStatusCode == false)
        {
            var message = $"Не удалось получить токен на основании refresh_token. {Environment.NewLine}" +
                $"Http status code: {tokenResponse.StatusCode}. {Environment.NewLine}" +
                $"Refresh token: {refreshToken} {Environment.NewLine}" +
                $"Endpoint: {tokenEndpoint} {Environment.NewLine}" +
                $"{TokenGrantTypeRequest}: {KeycloakClaimTypes.RefreshToken}," +
                $"{TokenRefreshTokenRequest}: {refreshToken}," +
                $"{TokenClientIdRequest}: {openIdConnectOptions.ClientId}," +
                $"{TokenClientSecretRequest}: {openIdConnectOptions.ClientSecret}";

            _logger.LogWarning(message);

            return new RefreshAccessTokenResult
            {
                HttpStatusCode = tokenResponse.StatusCode,
                ErrorMessage = message
            };
        }

        string tokenResponseJson;
        try
        {
            tokenResponseJson = await tokenResponse.Content.ReadAsStringAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при чтении ответа от сервера токенов.");
            return new RefreshAccessTokenResult
            {
                HttpStatusCode = tokenResponse.StatusCode,
                ErrorMessage = "Ошибка при чтении ответа от сервера токенов."
            };
        }

        TokenResponse? newTokenResponse;
        try
        {
            newTokenResponse = JsonConvert.DeserializeObject<TokenResponse>(tokenResponseJson);
        }
        catch (JsonSerializationException ex)
        {
            _logger.LogError(ex, "Ошибка десериализации ответа от сервера токенов: {ResponseJson}", tokenResponseJson);
            return new RefreshAccessTokenResult
            {
                HttpStatusCode = tokenResponse.StatusCode,
                ErrorMessage = "Не удалось десериализовать ответ на запрос об обновлении access token."
            };
        }

        if (string.IsNullOrEmpty(newTokenResponse?.AccessToken))
        {
            var message = "Не удалось получить access token на основании refresh_token. Ответ сервера не содержит access token.";
            _logger.LogWarning(message);

            return new RefreshAccessTokenResult
            {
                HttpStatusCode = tokenResponse.StatusCode,
                ErrorMessage = message
            };
        }

        ITokensTranslator tokensTranslator = httpContext.RequestServices.GetRequiredService<ITokensTranslator>();
        string token = await tokensTranslator.TranslateTokenAsync(newTokenResponse.AccessToken);
        var newClaims = GetKeycloakClaimsWithAddedTokensClaims(
            httpContext: httpContext,
            token: token,
            refreshToken: newTokenResponse.RefreshToken
        );

        var identity = new ClaimsIdentity(principal?.Identity);
        UpdateClaims(identity, newClaims);
        var claimsPrincipal = new ClaimsPrincipal(identity);

        _logger.LogTrace("Response Access token: {Token}", newTokenResponse.AccessToken);
        _logger.LogTrace("Translated Access token: {Token}", token);
        _logger.LogTrace("Refresh token: {Token}", newTokenResponse.RefreshToken);

        var result = new RefreshAccessTokenResult
        {
            AccessToken = token,
            RefreshToken = newTokenResponse.RefreshToken,
            Principal = claimsPrincipal,
            HttpStatusCode = tokenResponse.StatusCode
        };

        return result;
    }

    private static void UpdateClaims(ClaimsIdentity identity, List<Claim> newClaims)
    {
        foreach (var claim in newClaims)
        {
            var existingClaim = identity.FindFirst(claim.Type);
            if (existingClaim != null)
            {
                identity.RemoveClaim(existingClaim);
            }
            identity.AddClaim(claim);
        }
    }

    private static List<Claim> GetKeycloakClaimsWithAddedTokensClaims(
        HttpContext httpContext,
        string token,
        string? refreshToken
    )
    {
        IJWTTokenGenerator tokenGenerator = httpContext.RequestServices.GetRequiredService<IJWTTokenGenerator>();
        var userClaims = tokenGenerator
            .GetTokenClaims(token)
            .ToList();

        if (string.IsNullOrEmpty(token) == false)
        {
            userClaims.Add(new Claim(KeycloakClaimTypes.AccessToken, token));
        }

        if (string.IsNullOrEmpty(refreshToken) == false)
        {
            userClaims.Add(new Claim(KeycloakClaimTypes.RefreshToken, refreshToken));
        }

        return userClaims;
    }
}
