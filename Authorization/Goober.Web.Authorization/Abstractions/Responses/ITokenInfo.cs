namespace Goober.Web.Authorization.Abstractions.Responses
{
    /// <summary>
    /// Информация о сформированном токене
    /// </summary>
    public interface ITokenInfo
    {
        /// <summary>
        /// Сам токен доступа
        /// </summary>
        string AccessToken { get; }
        /// <summary>
        /// Момент истечения времени жизни
        /// </summary>
        int? ExpiresIn { get; }
        /// <summary>
        /// Момент истечения времени жизни токена обновления
        /// </summary>
        int? RefreshExpiresIn { get; }
        /// <summary>
        /// Сам токен обновления, если возможно обновление
        /// </summary>
        string? RefreshToken { get; }
        /// <summary>
        /// Тип токена доступа
        /// </summary>
        string TokenType { get; }
        /// <summary>
        /// Политики токена
        /// </summary>
        int NotBeforePolicy { get; }
        /// <summary>
        /// Состояние сессии токена
        /// </summary>
        Guid? SessionState { get; }
        /// <summary>
        /// Область токена
        /// </summary>
        string Scope { get; }
    }
}
