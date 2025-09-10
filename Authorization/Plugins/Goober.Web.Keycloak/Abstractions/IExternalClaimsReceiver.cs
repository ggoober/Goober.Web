using System.Security.Claims;

namespace Goober.Web.Keycloak.Abstractions
{
    /// <summary>
    /// Инструмент, который будет дергаться для получения внешних клеймов при формировании токена
    /// </summary>
    public interface IExternalClaimsReceiver
    {
        /// <summary>
        /// Получение внешних клеймов для добавления во внутренний токен
        /// </summary>
        /// <param name="claims">Уже имеющиеся кеймы в токене</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Задача, возвращающая набор клеймов, которые следует еще дополнительно добавить в токен</returns>
        public Task<IEnumerable<Claim>> ReceiveExternalClaimsAsync(
            IEnumerable<Claim> claims,
            CancellationToken cancellationToken = default);
    }
}
