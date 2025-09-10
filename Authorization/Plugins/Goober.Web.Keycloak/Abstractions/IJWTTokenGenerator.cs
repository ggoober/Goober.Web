using System.Security.Claims;

namespace Goober.Web.Keycloak.Abstractions
{
    /// <summary>
    /// Сервис, используемый для генерации внутренних токенов
    /// </summary>
    public interface IJWTTokenGenerator
    {
        /// <summary>
        /// Сформировать новый токен доступа
        /// </summary>
        /// <param name="claims">Клеймы, что будут включены в токен</param>
        /// <param name="currentDate">Момент формирования нового токена</param>
        /// <param name="expTime">Момент истечения актуальности токена кейклога</param>
        /// <param name="secretKey">Ключ формирования токена</param>
        /// <returns>Сформированный внутренний токен</returns>
        public string GenerateJwtToken(IEnumerable<Claim> claims, DateTime currentDate, long? expTime, string secretKey);
        /// <summary>
        /// Вытянуть из внутреннего токена все его клеймы
        /// </summary>
        /// <param name="token">Анализируемый токен</param>
        /// <returns>Перечень хранимых в токене клеймов</returns>
        public IEnumerable<Claim> GetTokenClaims(string token);
    }
}
