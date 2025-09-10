using System.Security.Claims;

namespace Goober.Web.Authorization.Abstractions.Responses
{
    /// <summary>
    /// Результат аутентификации пользователя с данными, выраженными через <see cref="ClaimsPrincipal"/>
    /// </summary>
    public interface IClaimsAuthenticationResult : IAuthenticationResult<ClaimsPrincipal>
    {
    }
}
