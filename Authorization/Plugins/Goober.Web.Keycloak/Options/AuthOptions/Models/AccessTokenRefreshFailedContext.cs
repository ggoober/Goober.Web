using Microsoft.AspNetCore.Http;
using System.Net;
using System.Security.Claims;

namespace Goober.Web.Keycloak.Options.AuthOptions.Models;

/// <summary>
/// Контекст неудачного обновления токена доступа.
/// </summary>
public class AccessTokenRefreshFailedContext
{
    /// <summary>
    /// <see cref="HttpContext"/> текущего запроса.
    /// </summary>
    public HttpContext HttpContext { get; set; } = null!;

    /// <summary>
    /// Токен доступа, обновление которого не удалось.
    /// </summary>
    public string AccessToken { get; set; } = string.Empty;

    /// <summary>
    /// Токен обновления (refresh token), использованный при попытке обновления токена доступа.
    /// Может быть null или пустым, если токен обновления недоступен.
    /// </summary>
    public string? RefreshToken { get; set; } = string.Empty;

    /// <summary>
    /// <see cref="HttpStatusCode"/> ответа от сервера обновления токенов, если запрос на обновление был выполнен.
    /// Может быть null, если запрос на обновление не был выполнен или произошла ошибка на стороне клиента.
    /// </summary>
    public HttpStatusCode? RefreshStatusCode { get; set; }

    /// <summary>
    /// <see cref="ClaimsPrincipal"/>, связанный с контекстом аутентификации.
    /// Может быть null, если аутентификация не была выполнена или не удалось определить пользователя.
    /// </summary>
    public virtual ClaimsPrincipal? Principal { get; set; }
}