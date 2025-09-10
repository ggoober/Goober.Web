using Goober.Web.Authorization.Abstractions;

namespace Goober.Web.Keycloak.Abstractions
{
    /// <summary>
    /// Сервис, ответственный за авторизацию пользователя
    /// </summary>
    public interface IKeycloakAuthenticationProvider : IAuthenticationTokensProvider
    {
        /// <summary>
        /// Запросить данные о пользователе
        /// </summary>
        /// <param name="currentToken">Текущий токен, используемый для авторизации</param>
        /// <param name="realm">Домен, к которому будет обращение</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>JSON данные о пользователе</returns>
        Task<string> UserInfoAsync(string currentToken, string realm, CancellationToken cancellationToken = default);
        /// <summary>
        /// Запрос данных об указанном токене
        /// </summary>
        /// <param name="currentToken">Токен для анализа</param>
        /// <param name="realm">Домен, к которому будет обращение</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>JSON данные о токене</returns>
        Task<string> IntrospectAsync(string currentToken, string realm, CancellationToken cancellationToken = default);
    }
}
