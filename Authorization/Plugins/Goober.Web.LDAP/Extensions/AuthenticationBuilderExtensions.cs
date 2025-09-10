using Goober.Web.Authorization.Abstractions;
using Goober.Web.Authorization.Extensions;
using Goober.Web.LDAP.Implementations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Goober.Web.LDAP.Extensions
{
    /// <summary>
    /// Расширения по работе с <see cref="AuthenticationBuilder"/> и вормированию авторизации
    /// </summary>
    public static class AuthenticationBuilderExtensions
    {
        /// <summary>
        /// Добавить аутентификацию по LDAP. Если не выбрана как провайдер, или в конфиге отсутствует блок LDAP, будет проигнорировано
        /// </summary>
        /// <param name="builder">Исходный строитель аутентификации</param>
        /// <param name="configuration">Конфигурация приложения</param>
        /// <param name="cookiesOverwrites">Переопределения настроек куков</param>
        /// <returns>Объект конфигурирования аутентификации</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static AuthenticationBuilder AddLDAPAuthentication(this AuthenticationBuilder builder, IConfiguration configuration, Action<CookieAuthenticationOptions>? cookiesOverwrites = default)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            var ldapOptions = configuration.GetLDAPAuthenticationOptions();
            if (ldapOptions?.CookiesOptions?.Enabled is true)
            {
                builder.Services.AddSingleton(ldapOptions);
                builder = builder.AddCookie(ldapOptions.CookiesOptions.GetSchemeName(), o =>
                {
                    ldapOptions.CookiesOptions.ApplyTo(o);
                    cookiesOverwrites?.Invoke(o);
                });
                builder.Services.AddScoped<IAuthenticationLiteProvider, LdapService>();
            }

            return builder;

        }

    }
}
