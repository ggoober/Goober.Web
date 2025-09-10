using Goober.Web.Authorization.Abstractions.Responses;

namespace Goober.Web.Authorization.Abstractions
{
    /// <summary>
    /// Сервис, способный провести аутентификацию пользователя и предоставить по её результатам токены доступа
    /// </summary>
    public interface IAuthenticationTokensProvider : IAuthenticationProvider
    {
        /// <summary>
        /// Провести аутентификацию пользователя во внешней системе
        /// </summary>
        /// <param name="username">Имя проверяемого пользователя</param>
        /// <param name="realm">Домен, где будет осуществлен поиск</param>
        /// <param name="password">Пароль проверяемого пользователя</param>
        /// <returns>Отчет по результатам аутентификации пользователя</returns>
        ITokenAuthenticationResult Authenticate(string username, string realm, string password);
        /// <summary>
        /// Провести аутентификацию пользователя во внешней системе
        /// </summary>
        /// <param name="username">Имя проверяемого пользователя</param>
        /// <param name="realm">Домен, где будет осуществлен поиск</param>
        /// <param name="password">Пароль проверяемого пользователя</param>
        /// <param name="cancellationToken">Инструмент отмены операции</param>
        /// <returns>Задача, возвращающая отчет по результатам аутентификации пользователя</returns>
        Task<ITokenAuthenticationResult> AuthenticateAsync(string username, string realm, string password, CancellationToken cancellationToken = default);
        /// <summary>
        /// Выполнить актуализакцию токена доступа
        /// </summary>
        /// <param name="refreshToken">Актуальный токен обновления для сессии</param>
        /// <param name="realm">Домен, в котором был авутентифицирован пользователь сессии</param>
        /// <returns>Отчет по результатам обновления с данными об актуальных токенах</returns>
        ITokenAuthenticationResult RefreshToken(string refreshToken, string realm);
        /// <summary>
        /// Выполнить актуализакцию токена доступа
        /// </summary>
        /// <param name="refreshToken">Актуальный токен обновления для сессии</param>
        /// <param name="realm">Домен, в котором был авутентифицирован пользователь сессии</param>
        /// <param name="cancellationToken">Инструмент отмены операции</param>
        /// <returns>Задача, возвращающая отчет по результатам обновления с данными об актуальных токенах</returns>
        Task<ITokenAuthenticationResult> RefreshTokenAsync(string refreshToken, string realm, CancellationToken cancellationToken = default);
        /// <summary>
        /// Завершить сессию пользователя
        /// </summary>
        /// <param name="refreshToken">Актуальный токен обновления для сессии</param>
        /// <param name="realm">Домен, в котором был авутентифицирован пользователь сессии</param>
        void Logout(string refreshToken, string realm);
        /// <summary>
        /// Завершить сессию пользователя
        /// </summary>
        /// <param name="refreshToken">Актуальный токен обновления для сессии</param>
        /// <param name="realm">Домен, в котором был авутентифицирован пользователь сессии</param>
        /// <param name="cancellationToken">Инструмент отмены операции</param>
        /// <returns>Задача, закрывающая сессию</returns>
        Task LogoutAsync(string refreshToken, string realm, CancellationToken cancellationToken = default);
    }
}