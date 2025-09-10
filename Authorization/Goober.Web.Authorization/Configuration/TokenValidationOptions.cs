using Microsoft.IdentityModel.Tokens;

namespace Goober.Web.Authorization.Configuration
{
    public class TokenValidationOptions
    {
        public string? AuthenticationType { get; set; }
        public TimeSpan? ClockSkew { get; set; }
        public string? DebugId { get; set; }
        public bool? IgnoreTrailingSlashWhenValidatingAudience { get; set; }
        public bool? IncludeTokenOnFailedValidation { get; set; }
        public bool? LogTokenId { get; set; }
        public bool? LogValidationExceptions { get; set; }
        public string? NameClaimType { get; set; }
        public IDictionary<string, object>? PropertyBag { get; set; }
        public bool? RefreshBeforeValidation { get; set; }
        public bool? RequireAudience { get; set; }
        public bool? RequireExpirationTime { get; set; }
        public bool? RequireSignedTokens { get; set; }
        public string? RoleClaimType { get; set; }
        public bool? SaveSigninToken { get; set; }
        public bool? TryAllIssuerSigningKeys { get; set; }
        public bool? ValidateActor { get; set; }
        public bool? ValidateAudience { get; set; }
        public bool? ValidateIssuer { get; set; }
        public bool? ValidateWithLKG { get; set; }
        public bool? ValidateIssuerSigningKey { get; set; }
        public bool? ValidateLifetime { get; set; }
        public bool? ValidateSignatureLast { get; set; }
        public bool? ValidateTokenReplay { get; set; }
        public IEnumerable<string>? ValidAlgorithms { get; set; }
        public string? ValidAudience { get; set; }
        public IEnumerable<string>? ValidAudiences { get; set; }
        public string? ValidIssuer { get; set; }
        public IEnumerable<string>? ValidIssuers { get; set; }
        public IEnumerable<string>? ValidTypes { get; set; }
        public TokenValidationParameters ApplyTo(TokenValidationParameters? options)
        {
            if (options == null)
            {
                options = new TokenValidationParameters();
            }

            if (AuthenticationType != null)
                options.AuthenticationType = AuthenticationType;
            if (ClockSkew != null)
                options.ClockSkew = ClockSkew.Value;
            if (DebugId != null)
                options.DebugId = DebugId;
            if (IgnoreTrailingSlashWhenValidatingAudience != null)
                options.IgnoreTrailingSlashWhenValidatingAudience = IgnoreTrailingSlashWhenValidatingAudience.Value;
            if (IncludeTokenOnFailedValidation != null)
                options.IncludeTokenOnFailedValidation = IncludeTokenOnFailedValidation.Value;
            if (LogTokenId != null)
                options.LogTokenId = LogTokenId.Value;
            if (LogValidationExceptions != null)
                options.LogValidationExceptions = LogValidationExceptions.Value;
            if (NameClaimType != null)
                options.NameClaimType = NameClaimType;
            if (PropertyBag != null)
                options.PropertyBag = PropertyBag;
            if (RefreshBeforeValidation != null)
                options.RefreshBeforeValidation = RefreshBeforeValidation.Value;
            if (RequireAudience != null)
                options.RequireAudience = RequireAudience.Value;
            if (RequireExpirationTime != null)
                options.RequireExpirationTime = RequireExpirationTime.Value;
            if (RequireSignedTokens != null)
                options.RequireSignedTokens = RequireSignedTokens.Value;
            if (RoleClaimType != null)
                options.RoleClaimType = RoleClaimType;
            if (SaveSigninToken != null)
                options.SaveSigninToken = SaveSigninToken.Value;
            if (TryAllIssuerSigningKeys != null)
                options.TryAllIssuerSigningKeys = TryAllIssuerSigningKeys.Value;
            if (ValidateActor != null)
                options.ValidateActor = ValidateActor.Value;
            if (ValidateAudience != null)
                options.ValidateAudience = ValidateAudience.Value;
            if (ValidateIssuer != null)
                options.ValidateIssuer = ValidateIssuer.Value;
            if (ValidateWithLKG != null)
                options.ValidateWithLKG = ValidateWithLKG.Value;
            if (ValidateIssuerSigningKey != null)
                options.ValidateIssuerSigningKey = ValidateIssuerSigningKey.Value;
            if (ValidateLifetime != null)
                options.ValidateLifetime = ValidateLifetime.Value;
            if (ValidateSignatureLast != null)
                options.ValidateSignatureLast = ValidateSignatureLast.Value;
            if (ValidateTokenReplay != null)
                options.ValidateTokenReplay = ValidateTokenReplay.Value;
            if (ValidAlgorithms != null)
                options.ValidAlgorithms = ValidAlgorithms;
            if (ValidAudience != null)
                options.ValidAudience = ValidAudience;
            if (ValidAudiences != null)
                options.ValidAudiences = ValidAudiences;
            if (ValidIssuer != null)
                options.ValidIssuer = ValidIssuer;
            if (ValidIssuers != null)
                options.ValidIssuers = ValidIssuers;
            if (ValidTypes != null)
                options.ValidTypes = ValidTypes;
            return options;
        }
    }
}
