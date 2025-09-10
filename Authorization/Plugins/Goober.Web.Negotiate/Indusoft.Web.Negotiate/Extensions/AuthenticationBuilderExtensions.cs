using Goober.Web.Authorization.Glossaries;
using Goober.Web.Negotiate.Configuration;
using Goober.Web.Negotiate.Glossaries;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Goober.Web.Negotiate.Extensions
{
    /// <summary>
    /// Расширения по работе с <see cref="AuthenticationBuilder"/> и формированию авторизации
    /// </summary>
    public static class AuthenticationBuilderExtensions
    {
        /// <summary>
        /// Добавить аутентификацию по принципу негошиейта
        /// </summary>
        /// <param name="builder">Исходный конфигуратор аутентификации</param>
        /// <param name="configuration">Конфигурация приложения</param>
        /// <param name="negotiateOverwrites">Переопределения настроек протокола</param>
        /// <returns>Объект конфигурирования аутентификации</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static AuthenticationBuilder AddNegotiateAuthentication(
            this AuthenticationBuilder builder,
            IConfiguration configuration,
            Action<Microsoft.AspNetCore.Authentication.Negotiate.NegotiateOptions>? negotiateOverwrites = default)
        {
            if (builder is null)
                throw new ArgumentNullException(nameof(builder));
            if (configuration is null)
                throw new ArgumentNullException(nameof(configuration));

            var options = configuration.GetNegotiateAuthenticationOptions();
            if (options?.Negotiate?.Enabled is true)
            {
                string schemeName = options.Negotiate.GetSchemeName();
                builder = builder.AddNegotiate(schemeName, (o) =>
                {
                    options.Negotiate.ApplyTo(o);
                    negotiateOverwrites?.Invoke(o);
                });
            }

            return builder;
        }

        public static NegotiateConfiguration? GetNegotiateAuthenticationOptions(
            this IConfiguration configuration)
        {
            if (configuration is null)
                throw new ArgumentNullException(nameof(configuration));

            var globalSection = configuration.GetChildren().FirstOrDefault(x => x.Key == ConfigGlossary.AuthSection);
            if (globalSection is null)
                return null;

            var negotiateEnabled = globalSection.GetValue<string>(ConfigGlossary.ProviderSection)?.ToLower() == NegotiateConfigurationGlossary.NegotiateConfigSection.ToLower();
            if (negotiateEnabled is not true)
                return null;

            var negotiateSection = globalSection.GetChildren().FirstOrDefault(x => x.Key == NegotiateConfigurationGlossary.NegotiateConfigSection);
            if (negotiateSection is null)
                return null;

            var options = new NegotiateConfiguration();
            negotiateSection.Bind(options, opt => { opt.BindNonPublicProperties = true; });
            return options;

        }
    }
}
