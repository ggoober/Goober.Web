using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Goober.Web.Authorization.Configuration
{
    public class JWTOptions
    {
        public bool Enabled { get; set; }
        public string? SchemeName { get; set; }
        public bool? RequireHttpsMetadata { get; set; }
        public string? MetadataAddress { get; set; }
        public string? Authority { get; set; }
        public string? Audience { get; set; }
        public string? Challenge { get; set; }
        public TimeSpan? BackchannelTimeout { get; set; }
        public bool? RefreshOnIssuerKeyNotFound { get; set; }
        public bool? SaveToken { get; set; }
        public bool? IncludeErrorDetails { get; set; }
        public bool? MapInboundClaims { get; set; }
        public TimeSpan? AutomaticRefreshInterval { get; set; }
        public TimeSpan? RefreshInterval { get; set; }
        public string? ClaimsIssuer { get; set; }
        public string? ForwardDefault { get; set; }
        public string? ForwardAuthenticate { get; set; }
        public string? ForwardChallenge { get; set; }
        public string? ForwardForbid { get; set; }
        public string? ForwardSignIn { get; set; }
        public string? ForwardSignOut { get; set; }
        public OpenIdConnectOptions? Configuration { get; set; }
        public TokenValidationOptions? TokenValidationParameters { get; set; }
        public void ApplyTo(JwtBearerOptions options)
        {
            if (RequireHttpsMetadata != null)
                options.RequireHttpsMetadata = RequireHttpsMetadata.Value;
            if (MetadataAddress != null)
                options.MetadataAddress = MetadataAddress;
            if (Authority != null)
                options.Authority = Authority;
            if (Audience != null)
                options.Audience = Audience;
            if (Challenge != null)
                options.Challenge = Challenge;
            if (BackchannelTimeout != null)
                options.BackchannelTimeout = BackchannelTimeout.Value;
            if (RefreshOnIssuerKeyNotFound != null)
                options.RefreshOnIssuerKeyNotFound = RefreshOnIssuerKeyNotFound.Value;
            if (SaveToken != null)
                options.SaveToken = SaveToken.Value;
            if (IncludeErrorDetails != null)
                options.IncludeErrorDetails = IncludeErrorDetails.Value;
            if (MapInboundClaims != null)
                options.MapInboundClaims = MapInboundClaims.Value;
            if (AutomaticRefreshInterval != null)
                options.AutomaticRefreshInterval = AutomaticRefreshInterval.Value;
            if (RefreshInterval != null)
                options.RefreshInterval = RefreshInterval.Value;
            if (ClaimsIssuer != null)
                options.ClaimsIssuer = ClaimsIssuer;
            if (ForwardDefault != null)
                options.ForwardDefault = ForwardDefault;
            if (ForwardAuthenticate != null)
                options.ForwardAuthenticate = ForwardAuthenticate;
            if (ForwardChallenge != null)
                options.ForwardChallenge = ForwardChallenge;
            if (ForwardForbid != null)
                options.ForwardForbid = ForwardForbid;
            if (ForwardSignIn != null)
                options.ForwardSignIn = ForwardSignIn;
            if (ForwardSignOut != null)
                options.ForwardSignOut = ForwardSignOut;
            if (Configuration != null)
                options.Configuration = Configuration.ApplyTo(options.Configuration!);
            if (TokenValidationParameters != null)
                options.TokenValidationParameters = TokenValidationParameters.ApplyTo(options.TokenValidationParameters);
        }
    }
}
