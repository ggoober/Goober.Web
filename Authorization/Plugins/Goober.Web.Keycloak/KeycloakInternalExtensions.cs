using Goober.Web.Authorization.Abstractions;
using Goober.Web.Authorization.Extensions;
using Goober.Web.Keycloak.Abstractions;
using Goober.Web.Keycloak.Abstractions.DataObjects;
using Goober.Web.Keycloak.Configuration;
using Goober.Web.Keycloak.Implementations;
using Goober.Web.Keycloak.Options.AuthOptions.Models;
using Goober.Web.Keycloak.Options.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Security.Claims;
using OpenIdConnectOptions = Microsoft.AspNetCore.Authentication.OpenIdConnect.OpenIdConnectOptions;
using TokenValidationParameters = Microsoft.IdentityModel.Tokens.TokenValidationParameters;

namespace Goober.Web.Keycloak
{
    internal static class KeycloakInternalExtensions
    {
        private const string roleClaimType = "role";
        /// <summary>
        /// Adds keycloak authentication services.
        /// </summary>
        public static AuthenticationBuilder AddKeycloakAuthentication(
            this AuthenticationBuilder builder,
            KeycloakAuthenticationOptions keycloakOptions,
            Action<JwtBearerOptions>? jwtOverwrites = default,
            Action<CookieAuthenticationOptions>? cookiesOverwrites = default,
            Action<OpenIdConnectOptions>? openidOverwrites = default,
            Action<IndusoftAuthenticationOptions>? iAuthOptions = default
        )
        {
            IServiceCollection services = builder.Services;
            services.AddScoped<KeyCloakAuthService>();
            services.AddScoped<IKeycloakAuthenticationProvider>((sp) => sp.GetRequiredService<KeyCloakAuthService>());
            services.AddScoped<IAuthenticationTokensProvider>((sp) => sp.GetRequiredService<KeyCloakAuthService>());
            services.AddScoped<ITokensTranslator>((sp) => sp.GetRequiredService<KeyCloakAuthService>());
            var indusoftAuthOptions = AdditionalEventHelper.ApplyIndusoftAuthOptions(services, keycloakOptions, iAuthOptions);

            services.AddSingleton(keycloakOptions);
            services.AddScoped<IJWTTokenGenerator, JWTTokenGenerator>();
            // options.Resource == Audience
            services.AddTransient<IClaimsTransformation>(_ =>
                new KeycloakRolesClaimsTransformation(
                    roleClaimType,
                    keycloakOptions.RolesSource,
                    keycloakOptions.Resource));
            AuthenticationBuilder authBuilder = builder;

            if (keycloakOptions.JWTOptions?.Enabled is true)
            {
                authBuilder = authBuilder.AddJwtBearer(keycloakOptions.JWTOptions.GetSchemeName(), opts =>
                {
                    var encrtyptionKey = keycloakOptions.Credentials.GetEncryptionKey();
                    var validationParameters = new TokenValidationParameters
                    {
                        ClockSkew = keycloakOptions.TokenClockSkew,
                        ValidateAudience = keycloakOptions.VerifyTokenAudience ?? true,
                        ValidateIssuer = true,
                        NameClaimType = keycloakOptions.JWTOptions?.TokenValidationParameters?.NameClaimType ?? $"{keycloakOptions.KeycloakClaimsPrefix}preferred_username",
                        RoleClaimType = $"{keycloakOptions.KeycloakClaimsPrefix}{roleClaimType}",
                        IssuerSigningKey = JWTTokenGenerator.GetSecurityKey(encrtyptionKey),
                        ValidIssuer = keycloakOptions.Credentials.Issuer
                    };

                    var sslRequired = string.IsNullOrWhiteSpace(keycloakOptions.SslRequired)
                        || keycloakOptions.SslRequired
                            .Equals("external", StringComparison.OrdinalIgnoreCase);

                    opts.Authority = keycloakOptions.KeycloakUrlRealm;
                    opts.Audience = keycloakOptions.Resource;
                    opts.TokenValidationParameters = validationParameters;
                    opts.RequireHttpsMetadata = sslRequired;
                    opts.SaveToken = true;
                    keycloakOptions.JWTOptions?.ApplyTo(opts);
                    jwtOverwrites?.Invoke(opts);
                    AdditionalEventHelper.ApplyAddintionEvents(opts, indusoftAuthOptions);
                });
            }

            if (keycloakOptions.CookiesOptions?.Enabled is true)
            {
                authBuilder = authBuilder.AddCookie(keycloakOptions.CookiesOptions.GetSchemeName(), (cOpts) =>
                {
                    cOpts.LoginPath = new PathString("/login");
                    cOpts.LogoutPath = new PathString("/logout");
                    cOpts.AccessDeniedPath = new PathString("/login");
                    cOpts.Cookie.Path = "/";
                    cOpts.Cookie.SameSite = SameSiteMode.Lax;
                    cOpts.Cookie.IsEssential = true;
                    cOpts.ExpireTimeSpan = TimeSpan.FromMinutes(120);
                    cOpts.SlidingExpiration = true;
                    keycloakOptions.CookiesOptions?.ApplyTo(cOpts);
                    cookiesOverwrites?.Invoke(cOpts);
                    AdditionalEventHelper.ApplyAddintionEvents(cOpts, indusoftAuthOptions);
                });
            }

            if (keycloakOptions.OpenIdOptions?.Enabled is true)
            {
                string openidname = keycloakOptions.OpenIdOptions.GetSchemeName();
                authBuilder = authBuilder.AddOpenIdConnect(openidname, (oidOpts) =>
                {
                    if (keycloakOptions.CookiesOptions?.Enabled is true)
                    {
                        oidOpts.SignInScheme = keycloakOptions.CookiesOptions.GetSchemeName();
                    }

                    oidOpts.Authority = keycloakOptions.KeycloakUrlRealm;
                    var sslRequired = string.IsNullOrWhiteSpace(keycloakOptions.SslRequired)
                        || keycloakOptions.SslRequired
                            .Equals("external", StringComparison.OrdinalIgnoreCase);
                    oidOpts.RequireHttpsMetadata = sslRequired;
                    oidOpts.SignedOutRedirectUri = keycloakOptions.SignedOutRedirectUri;
                    oidOpts.ClientId = keycloakOptions.Resource;
                    oidOpts.ClientSecret = keycloakOptions.Credentials.GetSecret(); ;
                    oidOpts.ResponseType = OpenIdConnectResponseType.CodeIdTokenToken;
                    oidOpts.GetClaimsFromUserInfoEndpoint = true;
                    oidOpts.SaveTokens = true;
                    oidOpts.Scope.Add("openid");
                    oidOpts.Events.OnSignedOutCallbackRedirect += context =>
                    {
                        context.Response.Redirect(context.Options.SignedOutRedirectUri);
                        context.HandleResponse();

                        return Task.CompletedTask;
                    };
                    oidOpts.Events.OnTokenResponseReceived += async context =>
                    {
                        TokenResponseReceivedContext c = context;
                        IJWTTokenGenerator tokenGenerator = c.HttpContext.RequestServices.GetRequiredService<IJWTTokenGenerator>();
                        ITokensTranslator tokensTranslator = c.HttpContext.RequestServices.GetRequiredService<ITokensTranslator>();

                        string token = await tokensTranslator.TranslateTokenAsync(c.TokenEndpointResponse.AccessToken);
                        var userClaims = tokenGenerator.GetTokenClaims(token).ToList();
                        if (string.IsNullOrEmpty(token) == false)
                        {
                            userClaims.Add(new Claim(KeycloakClaimTypes.AccessToken, token));
                        }

                        var refreshToken = c.TokenEndpointResponse?.RefreshToken;
                        if (string.IsNullOrEmpty(refreshToken) == false)
                        {
                            userClaims.Add(new Claim(KeycloakClaimTypes.RefreshToken, refreshToken));
                        }

                        var identity = new ClaimsIdentity(
                            claims: userClaims,
                            authenticationType: openidname,
                            nameType: keycloakOptions.OpenIdOptions?.TokenValidationParameters?.NameClaimType ?? oidOpts?.TokenValidationParameters?.NameClaimType,
                            roleType: keycloakOptions.OpenIdOptions?.TokenValidationParameters?.RoleClaimType ?? oidOpts?.TokenValidationParameters?.RoleClaimType);
                        c.Principal = new ClaimsPrincipal(identity);
                    };

                    keycloakOptions.OpenIdOptions?.ApplyTo(oidOpts);
                    openidOverwrites?.Invoke(oidOpts);
                });
            }

            return authBuilder;
        }

        public static AuthenticationBuilder AddKeycloakAuthentication(
            this AuthenticationBuilder builder,
            IConfiguration configuration,
            Action<JwtBearerOptions>? jwtOverwrites = default,
            Action<CookieAuthenticationOptions>? cookiesOverwrites = default,
            Action<OpenIdConnectOptions>? openidOverwrites = default,
            Action<IndusoftAuthenticationOptions>? iAuthOptions = default
        )
        {
            IServiceCollection services = builder.Services;

            var options = configuration.GetKeycloakAuthenticationOptions();
            if (options is not null)
            {
                return builder.AddKeycloakAuthentication(
                    keycloakOptions: options,
                    jwtOverwrites: jwtOverwrites,
                    cookiesOverwrites: cookiesOverwrites,
                    openidOverwrites: openidOverwrites,
                    iAuthOptions: iAuthOptions);
            }
            return builder;
        }
    }
}
