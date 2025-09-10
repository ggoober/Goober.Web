namespace Goober.Web.Keycloak.Options.AuthOptions.Models;

/// <summary>
/// События возникающих во время аутентификации.
/// </summary>
public class IndusoftAuthenticationEvents
{
    /// <summary>
    /// Вызывается, когда срок действия токена доступа истекает.
    /// </summary>
    /// <remarks>
    /// Позволяет выполнить пользовательскую логику при истечении срока действия токена доступа.
    /// </remarks>
    public Func<AccessTokenExpirationContext, Task> OnAccessTokenExpiration { get; set; } = context => Task.CompletedTask;

    /// <summary>
    /// Вызывается, когда срок действия токена обновления истекает.
    /// </summary>
    /// <remarks>
    /// Позволяет выполнить пользовательскую логику при истечении срока действия токена обновления.
    /// </remarks>
    public Func<RefreshTokenExpirationContext, Task> OnRefreshTokenExpiration { get; set; } = context => Task.CompletedTask;

    /// <summary>
    /// Вызывается после успешного обновления токена доступа.
    /// </summary>
    /// <remarks>
    /// Позволяет выполнить пользовательскую логику после успешного обновления токена доступа.
    /// </remarks>
    public Func<AccessTokenRefreshedContext, Task> OnAccessTokenRefreshed { get; set; } = context => Task.CompletedTask;

    /// <summary>
    /// Вызывается, когда не удалось обновить токен доступа.
    /// </summary>
    /// <remarks>
    /// Позволяет выполнить пользовательскую логику при неудачной попытке обновления токена доступа.
    /// </remarks>
    public Func<AccessTokenRefreshFailedContext, Task> OnAccessTokenRefreshFailed { get; set; } = context => Task.CompletedTask;

    /// <summary>
    /// Вызывает <see cref="OnAccessTokenExpiration"/> делегат.
    /// </summary>
    /// <param name="context">Контекст истечения срока действия токена доступа.</param>
    public virtual Task AccessTokenExpiration(AccessTokenExpirationContext context) => OnAccessTokenExpiration(context);

    /// <summary>
    /// Вызывает <see cref="OnRefreshTokenExpiration"/> делегат.
    /// </summary>
    /// <param name="context">Контекст истечения срока действия токена обновления.</param>
    public virtual Task RefreshTokenExpiration(RefreshTokenExpirationContext context) => OnRefreshTokenExpiration(context);

    /// <summary>
    /// Вызывает <see cref="OnAccessTokenRefreshed"/> делегат.
    /// </summary>
    /// <param name="context">Контекст обновленного токена доступа.</param>
    public virtual Task AccessTokenRefreshed(AccessTokenRefreshedContext context) => OnAccessTokenRefreshed(context);

    /// <summary>
    /// Вызывает <see cref="OnAccessTokenRefreshFailed"/> делегат.
    /// </summary>
    /// <param name="context">Контекст неудачного обновления токена доступа.</param>
    public virtual Task AccessTokenRefreshFailed(AccessTokenRefreshFailedContext context) => OnAccessTokenRefreshFailed(context);
}
