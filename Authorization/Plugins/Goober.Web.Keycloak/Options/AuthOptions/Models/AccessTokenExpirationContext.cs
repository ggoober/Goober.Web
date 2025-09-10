using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Goober.Web.Keycloak.Options.AuthOptions.Models;

/// <summary>
/// Контекст истечения срока действия токена доступа.
/// </summary>
public class AccessTokenExpirationContext
{
    /// <summary>
    /// <see cref="HttpContext"/> текущего запроса.
    /// </summary>
    public HttpContext HttpContext { get; set; } = null!;

    /// <summary>
    /// Токен доступа.
    /// </summary>
    public string AccessToken { get; set; } = string.Empty;

    /// <summary>
    /// Токен обновления (refresh token).
    /// </summary>
    public string RefreshToken { get; set; } = string.Empty;

    /// <summary>
    /// <see cref="ClaimsPrincipal"/>, связанный с контекстом аутентификации.
    /// Может быть null, если аутентификация не была выполнена или не удалось определить пользователя.
    /// </summary>
    public virtual ClaimsPrincipal? Principal { get; set; }

}
