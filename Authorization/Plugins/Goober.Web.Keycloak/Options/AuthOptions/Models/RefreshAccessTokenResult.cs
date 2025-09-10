using System.Net;
using System.Security.Claims;

namespace Goober.Web.Keycloak.Options.AuthOptions.Models;

/// <summary>
/// Представляет результат попытки обновления токена доступа.
/// </summary>
public class RefreshAccessTokenResult
{
    /// <summary>
    /// Указывает, было ли обновление токена доступа успешным.
    /// </summary>
    /// <remarks>
    /// Возвращает true, если <see cref="AccessToken"/> не null, иначе false.
    /// </remarks>
    public bool Success => AccessToken != null;

    /// <summary>
    /// Новый токен доступа, полученный в результате обновления.
    /// </summary>
    /// <remarks>
    /// Null, если обновление токена доступа не удалось.
    /// </remarks>
    public string? AccessToken { get; set; }

    /// <summary>
    /// Новый токен обновления (refresh token), полученный в результате обновления.
    /// </summary>
    /// <remarks>
    /// Может быть null, если сервер обновления токенов не вернул новый токен обновления или если обновление не удалось.
    /// </remarks>
    public string? RefreshToken { get; set; }

    /// <summary>
    /// <see cref="ClaimsPrincipal"/>, связанный с обновленным токеном доступа.
    /// </summary>
    /// <remarks>
    /// Может быть null, если обновление токена доступа не удалось или если информация о пользователе не была получена при обновлении.
    /// </remarks>
    public ClaimsPrincipal? Principal { get; set; }

    /// <summary>
    /// <see cref="HttpStatusCode"/> ответа от сервера обновления токенов.
    /// </summary>
    /// <remarks>
    /// Может быть null, если запрос на обновление не был выполнен или произошла ошибка на стороне клиента.
    /// </remarks>
    public HttpStatusCode? HttpStatusCode { get; set; }

    /// <summary>
    /// Сообщение об ошибке, если обновление токена доступа не удалось.
    /// </summary>
    /// <remarks>
    /// Null или пустая строка, если обновление токена доступа было успешным.
    /// </remarks>
    public string? ErrorMessage { get; set; }
}
