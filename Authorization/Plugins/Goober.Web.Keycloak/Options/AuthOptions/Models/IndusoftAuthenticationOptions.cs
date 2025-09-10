namespace Goober.Web.Keycloak.Options.AuthOptions.Models;


/// <summary>
/// Настройки аутентификации.
/// Варианты поведения при различных комбинациях настроек обновления токенов перечислены в <a href="https://tfs.indusoft.ru/tfs/InduSoft/Infrastructure/_wiki/wikis/Infrastructure.wiki?wikiVersion=GBwikiMaster&pagePath=%2FInfrastructure%2FIndusoft.Web%2FIndusoft.Web.Authorization%2FIndusoft.Web.Keycloak%2FKeycloak%20Token%20Refresh">Wiki</a>
/// </summary>
public class IndusoftAuthenticationOptions
{
    private static readonly TimeSpan _defaultAccessTokenRemainingTime = new TimeSpan(0, 1, 0);
    private static readonly TimeSpan _defaultRefreshTokenRemainingTime = new TimeSpan(0, 10, 0);

    /// <summary>
    /// Определяет, следует ли автоматически проверять, что токен доступа истекает раньше,
    /// чем указанное <see cref="AccessTokenRemainingTime"/> время.
    /// </summary>
    public bool AutoCheckAccessTokenLessThanRemainingTime { get; set; } = false;

    /// <summary>
    /// Определяет, следует ли автоматически проверять, что токен обновления истекает раньше,
    /// чем указанное <see cref="RefreshTokenRemainingTime"/> время.
    /// </summary>
    public bool AutoCheckRefreshTokenLessThanRemainingTime { get; set; } = false;

    /// <summary>
    /// Время, за которое до истечения срока действия токена доступа необходимо выполнить автоматическое обновление.
    /// </summary>
    /// <remarks>
    /// Используется для автоматической проверки и обновления токена доступа,
    /// если <see cref="AutoCheckAccessTokenLessThanRemainingTime"/> установлено в true.
    /// Значение по умолчанию: 1 минута.
    /// </remarks>
    public TimeSpan AccessTokenRemainingTime { get; set; } = _defaultAccessTokenRemainingTime;

    /// <summary>
    /// Время, за которое до истечения срока действия токена обновления необходимо выполнить автоматическое обновление.
    /// </summary>
    /// <remarks>
    /// Используется для автоматической проверки и обновления токена обновления,
    /// если <see cref="AutoCheckRefreshTokenLessThanRemainingTime"/> установлено в true.
    /// Значение по умолчанию: 10 минут.
    /// </remarks>
    public TimeSpan RefreshTokenRemainingTime { get; set; } = _defaultRefreshTokenRemainingTime;

    /// <summary>
    /// Определяет, следует ли автоматически обновлять токен доступа с использованием токена обновления (refresh token)
    /// при приближении срока его действия.
    /// </summary>
    public bool AutoSlidingAccessTokenByRefreshToken { get; set; } = false;

    /// <summary>
    /// Определяет, следует ли автоматически обновлять токен обновления при приближении срока его действия.
    /// </summary>
    public bool AutoSlidingRefreshToken { get; set; } = false;

    /// <summary>
    /// События аутентификации, позволяющие настроить поведение аутентификации.
    /// </summary>
    public IndusoftAuthenticationEvents Events { get; set; } = new();
}
