using System.Security.Claims;

namespace Goober.Web.Authorization.Abstractions.Responses
{
    /// <summary>
    /// Ответ на попытку аутентификации пользователя
    /// </summary>
    /// <typeparam name="TUserData">Формат полученных в ходе аутентификации данных пользователя</typeparam>
    public interface IAuthenticationResult<TUserData>
    {
        /// <summary>
        /// Была ли аутентификация успешной
        /// </summary>
        bool Successfully { get; }
        /// <summary>
        /// Ошибка, возникшая при аутентификации в случае провала
        /// </summary>
        Exception? Exception { get; }
        /// <summary>
        /// Данные пользователя, есль аутентификация прошла успешно
        /// </summary>
        TUserData? UserData { get; }
    }
}
