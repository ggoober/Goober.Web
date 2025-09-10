using Goober.Base.Extensions;
using Goober.Web.Authorization.Glossaries;
using Goober.Web.Keycloak.Configuration;
using Goober.Web.Keycloak.Options.AuthOptions.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Goober.Web.Keycloak
{
    /// <summary>
    /// Расширения <see cref="IServiceCollection"/>, регистрирующие сервисы по работе с Keycloak
    /// </summary>
    public static class KeycloakExtensions
    {
        /// <summary>
        /// Добавить аутентификацию посредствам Keycloak. Будет выполнен только, если в конфиге присутствует секция Keycloak, и провайдером указан Keycloak
        /// </summary>
        /// <param name="builder">Текущая конфигурация аутентификации</param>
        /// <param name="configuration">Конфигурация приложения</param>
        /// <param name="jwtOverwrites">Переопределения настроек токенов</param>
        /// <param name="cookiesOverwrites">Переопределения настроек куков</param>
        /// <param name="openidOverwrites">Переопределения настроек OpenId</param>
        /// <param name="iAuthOptions">Переопределения настроек IndusoftAuth</param>
        /// <returns>Объект конфигурирования аутентификации</returns>
        public static AuthenticationBuilder AddKeycloakAuthentication(
            this AuthenticationBuilder builder,
            IConfiguration configuration,
            Action<JwtBearerOptions>? jwtOverwrites = default,
            Action<CookieAuthenticationOptions>? cookiesOverwrites = default,
            Action<OpenIdConnectOptions>? openidOverwrites = default,
            Action<IndusoftAuthenticationOptions>? iAuthOptions = default)
        {
            return KeycloakInternalExtensions.AddKeycloakAuthentication(
                builder: builder,
                configuration: configuration,
                jwtOverwrites: jwtOverwrites,
                cookiesOverwrites: cookiesOverwrites,
                openidOverwrites: openidOverwrites,
                iAuthOptions: iAuthOptions);
        }

        public static string GetEncryptionKey(
            this KeycloakClientInstallationCredentials clientCredentials)
        {
            if (string.IsNullOrWhiteSpace(clientCredentials.SecuredEncryptionKey))
            {
                return clientCredentials.EncryptionKey;
            }
            else
            {
                return CryptoExtensions.DecryptString(clientCredentials.SecuredEncryptionKey);
            }
        }

        public  static string GetSecret(
            this KeycloakClientInstallationCredentials clientCredentials)
        {
            if (string.IsNullOrWhiteSpace(clientCredentials.SecuredSecret))
            {
                return clientCredentials.Secret;
            }
            else
            {
                return CryptoExtensions.DecryptString(clientCredentials.SecuredSecret);
            }
        }

        public static KeycloakAuthenticationOptions? GetKeycloakAuthenticationOptions(
            this IConfiguration configuration)
        {
            IConfiguration? globalSection = configuration.GetChildren().FirstOrDefault(x => x.Key == KeycloakAuthenticationOptions.GlobalAuthSection);
            if (globalSection is null)
                return null;

            var keyckoakEnabled = globalSection.GetValue<string>(ConfigGlossary.ProviderSection)?.ToLower() == KeycloakAuthenticationOptions.Section.ToLower();
            if (keyckoakEnabled is not true)
                return null;

            var keycloakSection = globalSection.GetChildren().FirstOrDefault(x => x.Key == KeycloakAuthenticationOptions.Section);
            if (keycloakSection is null)
                return null;

            var options = new KeycloakAuthenticationOptions();
            keycloakSection.Bind(options, opt => { opt.BindNonPublicProperties = true; });
            return options;
        }
    }
}
