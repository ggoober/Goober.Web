namespace Goober.Web.Authorization.Abstractions.Responses
{
    /// <summary>
    /// Результат аутентификации пользователя с данными, переданными через токен-доступа
    /// </summary>
    public interface ITokenAuthenticationResult : IAuthenticationResult<ITokenInfo>
    {
    }
}
