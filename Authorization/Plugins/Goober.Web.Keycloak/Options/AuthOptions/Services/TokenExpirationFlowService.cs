using Goober.Web.Keycloak.Abstractions.DataObjects;
using Goober.Web.Keycloak.Options.AuthOptions.Models;
using Goober.Web.Keycloak.Options.AuthOptions.Services.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Security.Claims;

namespace Goober.Web.Keycloak.Options.AuthOptions.Services;

internal class TokenExpirationFlowService : ITokenExpirationFlowService
{
    private readonly ILogger<TokenExpirationFlowService> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IKeycloakTokenService _keycloakTokenService;

    public TokenExpirationFlowService(
        ILogger<TokenExpirationFlowService> logger,
        IHttpContextAccessor httpContextAccessor,
        IKeycloakTokenService keycloakTokenService
    )
    {
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
        _keycloakTokenService = keycloakTokenService;
    }

    public async Task ExecuteAsync(
        ClaimsPrincipal? principal,
        IndusoftAuthenticationOptions indusoftAuthOptions
    )
    {
        var httpContext = _httpContextAccessor.HttpContext;

        if (indusoftAuthOptions?.Events is null
            || httpContext is null)
            return;

        if (indusoftAuthOptions is { AutoCheckAccessTokenLessThanRemainingTime: false, AutoCheckRefreshTokenLessThanRemainingTime: false })
            return;

        var accessToken = principal?.FindFirst(KeycloakClaimTypes.AccessToken)?.Value;
        var refreshToken = principal?.FindFirst(KeycloakClaimTypes.RefreshToken)?.Value;

        if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(refreshToken))
            return;

        _logger.LogTrace("AccessToken: {AccessToken}", accessToken);
        _logger.LogTrace("RefreshToken: {RefreshToken}", refreshToken);

        var accessTokenLeftTime = _keycloakTokenService.GetTokenLeftTime(accessToken);
        var refreshTokenLeftTime = _keycloakTokenService.GetTokenLeftTime(refreshToken);

        var accessTokenNeedsUpdate = accessTokenLeftTime <= indusoftAuthOptions.AccessTokenRemainingTime && indusoftAuthOptions.AutoCheckAccessTokenLessThanRemainingTime;
        var refreshTokenNeedsUpdate = refreshTokenLeftTime <= indusoftAuthOptions.RefreshTokenRemainingTime && indusoftAuthOptions.AutoCheckRefreshTokenLessThanRemainingTime;

        if (accessTokenNeedsUpdate)
        {
            await AccessTokenExpirationHandle(
                accessToken: accessToken,
                refreshToken: refreshToken,
                httpContext: httpContext,
                principal: principal,
                indusoftAuthOptions: indusoftAuthOptions);
        }

        if (refreshTokenNeedsUpdate)
        {
            await RefreshTokenExpirationHandle(
                accessToken: accessToken,
                refreshToken: refreshToken,
                httpContext: httpContext,
                principal: principal,
                indusoftAuthOptions: indusoftAuthOptions);
        }

        var willUpdateTokens = accessTokenNeedsUpdate && indusoftAuthOptions.AutoSlidingAccessTokenByRefreshToken
            || refreshTokenNeedsUpdate && indusoftAuthOptions.AutoSlidingRefreshToken;
        if (!willUpdateTokens)
            return;

        await RefreshAccessTokenHandle(
            accessToken: accessToken,
            refreshToken: refreshToken,
            httpContext: httpContext,
            principal: principal,
            indusoftAuthOptions: indusoftAuthOptions);
    }

    private async Task RefreshAccessTokenHandle(
        string accessToken,
        string refreshToken,
        HttpContext httpContext,
        ClaimsPrincipal? principal,
        IndusoftAuthenticationOptions indusoftAuthOptions
    )
    {
        var result = await _keycloakTokenService.GetAccessTokenByRefreshTokenAsync(
            principal: principal
        );

        if (result.Success == false)
        {
            await AccessTokenRefreshFailedHandle(
                accessToken: accessToken,
                refreshToken: refreshToken,
                refreshStatusCode: result.HttpStatusCode,
                httpContext: httpContext,
                principal: principal,
                indusoftAuthOptions: indusoftAuthOptions);

            return;
        }

        var context = new AccessTokenRefreshedContext
        {
            HttpContext = httpContext,
            AccessToken = result.AccessToken,
            RefreshToken = result.RefreshToken,
            Principal = result.Principal,
        };

        await indusoftAuthOptions.Events.AccessTokenRefreshed(context);
    }

    private static async Task AccessTokenRefreshFailedHandle(
        string accessToken,
        string refreshToken,
        HttpStatusCode? refreshStatusCode,
        HttpContext httpContext,
        ClaimsPrincipal? principal,
        IndusoftAuthenticationOptions indusoftAuthOptions
    )
    {
        var context = new AccessTokenRefreshFailedContext
        {
            HttpContext = httpContext,
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            RefreshStatusCode = refreshStatusCode,
            Principal = principal
        };

        await indusoftAuthOptions.Events.AccessTokenRefreshFailed(context);
    }

    private static async Task AccessTokenExpirationHandle(
        string accessToken,
        string refreshToken,
        HttpContext httpContext,
        ClaimsPrincipal? principal,
        IndusoftAuthenticationOptions indusoftAuthOptions
    )
    {
        var expirationContext = new AccessTokenExpirationContext
        {
            HttpContext = httpContext,
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            Principal = principal
        };

        await indusoftAuthOptions.Events.AccessTokenExpiration(expirationContext);
    }

    private static async Task RefreshTokenExpirationHandle(
        string accessToken,
        string refreshToken,
        HttpContext httpContext,
        ClaimsPrincipal? principal,
        IndusoftAuthenticationOptions indusoftAuthOptions
    )
    {
        var expirationContext = new RefreshTokenExpirationContext
        {
            HttpContext = httpContext,
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            Principal = principal
        };

        await indusoftAuthOptions.Events.RefreshTokenExpiration(expirationContext);
    }
}
