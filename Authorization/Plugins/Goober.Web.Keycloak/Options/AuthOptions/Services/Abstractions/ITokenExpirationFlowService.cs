using Goober.Web.Keycloak.Options.AuthOptions.Models;
using System.Security.Claims;

namespace Goober.Web.Keycloak.Options.AuthOptions.Services.Abstractions;

internal interface ITokenExpirationFlowService
{
    /// <summary>
    /// Запустить поток контроля времени жизни токена доступа
    /// </summary>
    /// <param name="principal"></param>
    /// <param name="indusoftAuthOptions"></param>
    /// <returns></returns>
    Task ExecuteAsync(
        ClaimsPrincipal? principal,
        IndusoftAuthenticationOptions indusoftAuthOptions
    );
}
