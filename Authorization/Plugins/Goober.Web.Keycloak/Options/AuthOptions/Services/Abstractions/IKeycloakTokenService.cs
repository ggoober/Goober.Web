using Goober.Web.Keycloak.Options.AuthOptions.Models;
using System.Security.Claims;

namespace Goober.Web.Keycloak.Options.AuthOptions.Services.Abstractions;

/// <summary>
/// Сервис для работы с токенами keycloak
/// </summary>
public interface IKeycloakTokenService
{
    /// <summary>
    /// Получить время до истечения access token из HttpContext.User.
    /// </summary>
    /// <returns>TimeSpan? - время до истечения, null если не удалось определить.</returns>
    TimeSpan? GetAccessTokenLeftTime();

    /// <summary>
    /// Получить время до истечения токена.
    /// </summary>
    /// <param name="token">JWT в виде строки.</param>
    /// <returns>TimeSpan? - время до истечения, null если не удалось определить.</returns>
    TimeSpan? GetTokenLeftTime(string? token);

    /// <summary>
    /// Получить время до истечения access token из ClaimsPrincipal.
    /// </summary>
    /// <param name="principal">ClaimsPrincipal, содержащий claim с access token.</param>
    /// <returns>TimeSpan? - время до истечения, null если не удалось определить.</returns>
    TimeSpan? GetAccessTokenLeftTime(ClaimsPrincipal? principal);

    /// <summary>
    /// Получить access token из HttpContext.User.
    /// </summary>
    /// <returns>Goober.Web.Keycloak.Options.AuthOptions.Models.RefreshAccessTokenResult</returns>
    Task<RefreshAccessTokenResult> GetAccessTokenByRefreshTokenAsync();

    /// <summary>
    /// Получить access token на основании refresh token.
    /// </summary>
    /// <param name="refreshToken"></param>
    /// <returns>Goober.Web.Keycloak.Options.AuthOptions.Models.RefreshAccessTokenResult</returns>
    Task<RefreshAccessTokenResult> GetAccessTokenByRefreshTokenAsync(string? refreshToken);

    /// <summary>
    /// Получить access token из ClaimsPrincipal.
    /// </summary>
    /// <param name="principal">ClaimsPrincipal, содержащий claim с refresh token.</param>
    /// <returns>Goober.Web.Keycloak.Options.AuthOptions.Models.RefreshAccessTokenResult</returns>
    Task<RefreshAccessTokenResult> GetAccessTokenByRefreshTokenAsync(ClaimsPrincipal? principal);
}
