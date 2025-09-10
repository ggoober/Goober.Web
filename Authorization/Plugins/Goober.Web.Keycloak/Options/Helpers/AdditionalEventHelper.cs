using Goober.Web.Keycloak.Configuration;
using Goober.Web.Keycloak.Options.AuthOptions.Models;
using Goober.Web.Keycloak.Options.AuthOptions.Services;
using Goober.Web.Keycloak.Options.AuthOptions.Services.Abstractions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;

namespace Goober.Web.Keycloak.Options.Helpers;

internal class AdditionalEventHelper
{
    internal static IndusoftAuthenticationOptions ApplyIndusoftAuthOptions(
        IServiceCollection services,
        KeycloakAuthenticationOptions keycloakOptions,
        Action<IndusoftAuthenticationOptions>? configureOptions
    )
    {
        var options = new IndusoftAuthenticationOptions();

        if (keycloakOptions.AccessTokenCheckRemainingTime is not null)
            options.AccessTokenRemainingTime = keycloakOptions.AccessTokenCheckRemainingTime.Value;
        if (keycloakOptions.RefreshTokenCheckRemainingTime is not null)
            options.RefreshTokenRemainingTime = keycloakOptions.RefreshTokenCheckRemainingTime.Value;

        configureOptions?.Invoke(options);

        services.AddScoped<ITokenExpirationFlowService, TokenExpirationFlowService>();
        services.AddScoped<IKeycloakTokenService, KeycloakTokenService>();

        return options;
    }

    internal static void ApplyAddintionEvents(
        CookieAuthenticationOptions options,
        IndusoftAuthenticationOptions indusoftAuthOptions
    )
    {
        var originalOnValidatePrincipal = options.Events?.OnValidatePrincipal;

        options.Events ??= new();
        options.Events.OnValidatePrincipal = async context =>
        {
            if (originalOnValidatePrincipal != null)
            {
                await originalOnValidatePrincipal(context);
            }

            var tokenExpirationFlowService = context.HttpContext.RequestServices
                .GetRequiredService<ITokenExpirationFlowService>();

            await tokenExpirationFlowService.ExecuteAsync(
                principal: context.Principal,
                indusoftAuthOptions: indusoftAuthOptions);
        };
    }


    internal static void ApplyAddintionEvents(
        JwtBearerOptions options,
        IndusoftAuthenticationOptions indusoftAuthOptions
    )
    {
        var originalOnTokenValidated = options.Events?.OnTokenValidated;

        options.Events ??= new();
        options.Events.OnTokenValidated = async context =>
        {
            if (originalOnTokenValidated != null)
            {
                await originalOnTokenValidated(context);
            }

            var accessTokenExprationFlowService = context.HttpContext.RequestServices
                .GetRequiredService<ITokenExpirationFlowService>();

            await accessTokenExprationFlowService.ExecuteAsync(
                principal: context.Principal,
                indusoftAuthOptions: indusoftAuthOptions);
        };
    }
}
