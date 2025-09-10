using Microsoft.AspNetCore.Authentication.OpenIdConnect;

namespace Goober.Web.Authorization.Configuration
{
    public class OpenIdOptions
    {
        public bool Enabled { get; set; }
        public string? SchemeName { get; set; }
        public string? Authority { get; set; }
        public string? ClientId { get; set; }
        public string? ClientSecret { get; set; }
        public bool? GetClaimsFromUserInfoEndpoint { get; set; }
        public bool? RequireHttpsMetadata { get; set; }
        public string? MetadataAddress { get; set; }
        public TimeSpan? MaxAge { get; set; }
        public string? SignedOutCallbackPath { get; set; }
        public string? SignedOutRedirectUri { get; set; }
        public bool? RefreshOnIssuerKeyNotFound { get; set; }
        public OpenIdConnectRedirectBehavior? AuthenticationMethod { get; set; }
        public string? Resource { get; set; }
        public string? ResponseMode { get; set; }
        public string? ResponseType { get; set; }
        public string? Prompt { get; set; }
        public string? RemoteSignOutPath { get; set; }
        public string? SignOutScheme { get; set; }
        public bool? UseTokenLifetime { get; set; }
        public bool? SkipUnrecognizedRequests { get; set; }
        public bool? DisableTelemetry { get; set; }
        public bool? UsePkce { get; set; }
        public TimeSpan? AutomaticRefreshInterval { get; set; }
        public TimeSpan? RefreshInterval { get; set; }
        public bool? MapInboundClaims { get; set; }
        public TimeSpan? BackchannelTimeout { get; set; }
        public string? CallbackPath { get; set; }
        public string? AccessDeniedPath { get; set; }
        public string? ReturnUrlParameter { get; set; }
        public string? SignInScheme { get; set; }
        public TimeSpan? RemoteAuthenticationTimeout { get; set; }
        public bool? SaveTokens { get; set; }
        public string? ClaimsIssuer { get; set; }
        public string? ForwardDefault { get; set; }
        public string? ForwardAuthenticate { get; set; } = null;
        public string? ForwardChallenge { get; set; }
        public string? ForwardForbid { get; set; }
        public string? ForwardSignIn { get; set; }
        public string? ForwardSignOut { get; set; }
        public TokenValidationOptions? TokenValidationParameters { get; set; }
        public OpenIdConnectOptions? Configuration { get; set; }
        public CookieBuilderOptions? NonceCookie { get; set; }
        public CookieBuilderOptions? CorrelationCookie { get; set; }
        public void ApplyTo(Microsoft.AspNetCore.Authentication.OpenIdConnect.OpenIdConnectOptions options)
        {
            if (Authority != null)
                options.Authority = Authority;
            if (ClientId != null)
                options.ClientId = ClientId;
            if (ClientSecret != null)
                options.ClientSecret = ClientSecret;
            if (GetClaimsFromUserInfoEndpoint != null)
                options.GetClaimsFromUserInfoEndpoint = GetClaimsFromUserInfoEndpoint.Value;
            if (RequireHttpsMetadata != null)
                options.RequireHttpsMetadata = RequireHttpsMetadata.Value;
            if (MetadataAddress != null)
                options.MetadataAddress = MetadataAddress;
            if (MaxAge != null)
                options.MaxAge = MaxAge.Value;
            if (SignedOutCallbackPath != null)
                options.SignedOutCallbackPath = SignedOutCallbackPath;
            if (SignedOutRedirectUri != null)
                options.SignedOutRedirectUri = SignedOutRedirectUri;
            if (RefreshOnIssuerKeyNotFound != null)
                options.RefreshOnIssuerKeyNotFound = RefreshOnIssuerKeyNotFound.Value;
            if (AuthenticationMethod != null)
                options.AuthenticationMethod = AuthenticationMethod.Value;
            if (Resource != null)
                options.Resource = Resource;
            if (ResponseMode != null)
                options.ResponseMode = ResponseMode;
            if (ResponseType != null)
                options.ResponseType = ResponseType;
            if (Prompt != null)
                options.Prompt = Prompt;
            if (RemoteSignOutPath != null)
                options.RemoteSignOutPath = RemoteSignOutPath;
            if (SignOutScheme != null)
                options.SignOutScheme = SignOutScheme;
            if (UseTokenLifetime != null)
                options.UseTokenLifetime = UseTokenLifetime.Value;
            if (SkipUnrecognizedRequests != null)
                options.SkipUnrecognizedRequests = SkipUnrecognizedRequests.Value;
            if (DisableTelemetry != null)
                options.DisableTelemetry = DisableTelemetry.Value;
            if (UsePkce != null)
                options.UsePkce = UsePkce.Value;
            if (AutomaticRefreshInterval != null)
                options.AutomaticRefreshInterval = AutomaticRefreshInterval.Value;
            if (RefreshInterval != null)
                options.RefreshInterval = RefreshInterval.Value;
            if (MapInboundClaims != null)
                options.MapInboundClaims = MapInboundClaims.Value;
            if (BackchannelTimeout != null)
                options.BackchannelTimeout = BackchannelTimeout.Value;
            if (CallbackPath != null)
                options.CallbackPath = CallbackPath;
            if (AccessDeniedPath != null)
                options.AccessDeniedPath = AccessDeniedPath;
            if (ReturnUrlParameter != null)
                options.ReturnUrlParameter = ReturnUrlParameter;
            if (SignInScheme != null)
                options.SignInScheme = SignInScheme;
            if (RemoteAuthenticationTimeout != null)
                options.RemoteAuthenticationTimeout = RemoteAuthenticationTimeout.Value;
            if (SaveTokens != null)
                options.SaveTokens = SaveTokens.Value;
            if (ClaimsIssuer != null)
                options.ClaimsIssuer = ClaimsIssuer;
            if (ForwardDefault != null)
                options.ForwardDefault = ForwardDefault;
            if (ForwardAuthenticate != null)
                options.ForwardAuthenticate = ForwardAuthenticate;
            if (ForwardChallenge != null) options.ForwardChallenge = ForwardChallenge;
            if (ForwardForbid != null)
                options.ForwardForbid = ForwardForbid;
            if (ForwardSignIn != null)
                options.ForwardSignIn = ForwardSignIn;
            if (ForwardSignOut != null)
                options.ForwardSignOut = ForwardSignOut;
            if (TokenValidationParameters != null)
                options.TokenValidationParameters = TokenValidationParameters.ApplyTo(options.TokenValidationParameters);
            if (Configuration != null)
                options.Configuration = Configuration.ApplyTo(options.Configuration);
            if (NonceCookie != null)
                options.NonceCookie = NonceCookie.ApplyTo(options.NonceCookie);
            if (CorrelationCookie != null)
                options.CorrelationCookie = CorrelationCookie.ApplyTo(options.CorrelationCookie);
        }
    }
}
