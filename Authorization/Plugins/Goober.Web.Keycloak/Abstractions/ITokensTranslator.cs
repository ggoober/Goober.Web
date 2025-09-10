using Goober.Web.Keycloak.Abstractions.DataObjects;
using System.Security.Claims;

namespace Goober.Web.Keycloak.Abstractions
{
    /// <summary>
    /// Вспомогательный сервис, получающий данные о конечных точках сервиса по выдаче токенов и занимающийся их транслированием
    /// </summary>
    internal interface ITokensTranslator
    {
        /// <summary>
        /// Получение всех точек доступа для указанного домена
        /// </summary>
        /// <param name="realm">Домен, чьи точки доступа будут получены</param>
        /// <returns>Задача, возвращающая все доступные точки доступа для домена</returns>
        Task<EndpointsInfo> LoadEndpointsForRealmAsync(string realm);
        /// <summary>
        /// Сформировать внутренний токен доступа на базе кейклоковского
        /// </summary>
        /// <param name="token">Исходный кейклоковский токен</param>
        /// <param name="cancellationToken">Инструмент отмены операции</param>
        /// <param name="extraClaims">Дополнительные клеймы, что будут включены в токен</param>
        /// <returns>Сформированный внутренний токен</returns>
        Task<string> TranslateTokenAsync(string token, CancellationToken cancellationToken = default, params Claim[] extraClaims);
    }
}
