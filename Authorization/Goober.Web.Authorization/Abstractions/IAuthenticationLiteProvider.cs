using Goober.Web.Authorization.Abstractions.Responses;
using System.Security.Claims;

namespace Goober.Web.Authorization.Abstractions
{
    /// <summary>
    /// Сервис, способный провести базовую аутентификацию пользователя
    /// </summary>
    public interface IAuthenticationLiteProvider : IAuthenticationProvider
    {
        /// <summary>
        /// Провести аутентификацию пользователя во внешней системе
        /// </summary>
        /// <param name="username">Имя проверяемого пользователя</param>
        /// <param name="realm">Домен, где будет осуществлен поиск</param>
        /// <param name="password">Пароль проверяемого пользователя</param>
        /// <returns>Отчет по результатам аутентификации пользователя</returns>
        IClaimsAuthenticationResult Authenticate(string username, string realm, string password);
        /// <summary>
        /// Провести аутентификацию пользователя во внешней системе
        /// </summary>
        /// <param name="username">Имя проверяемого пользователя</param>
        /// <param name="realm">Домен, где будет осуществлен поиск</param>
        /// <param name="password">Пароль проверяемого пользователя</param>
        /// <param name="cancellationToken">Инструмент отмены операции</param>
        /// <returns>Задача, возвращающая отчёт по результатам аутентификации пользователя</returns>
        Task<IClaimsAuthenticationResult> AuthenticateAsync(string username, string realm, string password, CancellationToken cancellationToken = default);
    }
}
