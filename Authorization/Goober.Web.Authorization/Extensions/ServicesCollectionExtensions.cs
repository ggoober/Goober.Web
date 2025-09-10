using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace Goober.Web.Authorization.Extensions
{
    /// <summary>
    /// Расширения, регистрирующие внутренние сервисы, связанные с авторизацией
    /// </summary>
    public static class ServicesCollectionExtensions
    {
        /// <summary>
        /// Добавить базовые сервисы авторизации в проект
        /// </summary>
        /// <param name="services">Коллекция сервисов, где произойдет регистрация</param>
        /// <returns>Инструмент, используемый для последующей настройки авторизации и аутентификаци</returns>
        /// <exception cref="ArgumentNullException">Отсутствие одного из параметров</exception>
        public static AuthenticationBuilder AddIndusoftAuthorization(
            this IServiceCollection services
            )
        {
            if (services == null) 
                throw new ArgumentNullException(nameof(services));

            return services.AddAuthentication();
        }

        /// <summary>
        /// Добавить базовые сервисы авторизации в проект
        /// </summary>
        /// <param name="services">Коллекция сервисов, где произойдет регистрация</param>
        /// <returns>Инструмент, используемый для последующей настройки авторизации и аутентификаци</returns>
        /// <exception cref="ArgumentNullException">Отсутствие одного из параметров</exception>
        public static AuthenticationBuilder AddIndusoftAuthorization(this IServiceCollection services, string defaultScheme)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));
            if (defaultScheme == null)
                throw new ArgumentNullException(nameof(defaultScheme));

            return services.AddAuthentication(defaultScheme);
        }
    }
}
