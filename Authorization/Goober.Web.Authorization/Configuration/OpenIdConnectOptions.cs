using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace Goober.Web.Authorization.Configuration
{
    public class OpenIdConnectOptions
    {
        public IDictionary<string, object>? AdditionalData { get; set; }
        public ICollection<string>? AcrValuesSupported { get; set; }
        public string? AuthorizationEndpoint { get; set; }
        public string? CheckSessionIframe { get; set; }
        public ICollection<string>? ClaimsSupported { get; set; }
        public ICollection<string>? ClaimsLocalesSupported { get; set; }
        public bool? ClaimsParameterSupported { get; set; }
        public ICollection<string>? ClaimTypesSupported { get; set; }
        public ICollection<string>? DisplayValuesSupported { get; set; }
        public string? EndSessionEndpoint { get; set; }
        public string? FrontchannelLogoutSessionSupported { get; set; }
        public string? FrontchannelLogoutSupported { get; set; }
        public ICollection<string>? GrantTypesSupported { get; set; }
        public bool? HttpLogoutSupported { get; set; }
        public ICollection<string>? IdTokenEncryptionAlgValuesSupported { get; set; }
        public ICollection<string>? IdTokenEncryptionEncValuesSupported { get; set; }
        public ICollection<string>? IdTokenSigningAlgValuesSupported { get; set; }
        public string? IntrospectionEndpoint { get; set; }
        public ICollection<string>? IntrospectionEndpointAuthMethodsSupported { get; set; }
        public ICollection<string>? IntrospectionEndpointAuthSigningAlgValuesSupported { get; set; }
        public string? Issuer { get; set; }
        public string? JwksUri { get; set; }
        public bool? LogoutSessionSupported { get; set; }
        public string? OpPolicyUri { get; set; }
        public string? OpTosUri { get; set; }
        public string? RegistrationEndpoint { get; set; }
        public ICollection<string>? RequestObjectEncryptionAlgValuesSupported { get; set; }
        public ICollection<string>? RequestObjectEncryptionEncValuesSupported { get; set; }
        public ICollection<string>? RequestObjectSigningAlgValuesSupported { get; set; }
        public bool? RequestParameterSupported { get; set; }
        public bool? RequestUriParameterSupported { get; set; }
        public bool? RequireRequestUriRegistration { get; set; }
        public ICollection<string>? ResponseModesSupported { get; set; }
        public ICollection<string>? ResponseTypesSupported { get; set; }
        public string? ServiceDocumentation { get; set; }
        public ICollection<string>? ScopesSupported { get; set; }
        public ICollection<string>? SubjectTypesSupported { get; set; }
        public string? TokenEndpoint { get; set; }
        public string? ActiveTokenEndpoint { get; set; }
        public ICollection<string>? TokenEndpointAuthMethodsSupported { get; set; }
        public ICollection<string>? TokenEndpointAuthSigningAlgValuesSupported { get; set; }
        public ICollection<string>? UILocalesSupported { get; set; }
        public string? UserInfoEndpoint { get; set; }
        public ICollection<string>? UserInfoEndpointEncryptionAlgValuesSupported { get; set; }
        public ICollection<string>? UserInfoEndpointEncryptionEncValuesSupported { get; set; }
        public ICollection<string>? UserInfoEndpointSigningAlgValuesSupported { get; set; }

        public OpenIdConnectConfiguration ApplyTo(OpenIdConnectConfiguration? options)
        {
            if (options == null)
            {
                options = new OpenIdConnectConfiguration();
            }

            if (AdditionalData != null)
                ApplyTo(options.AdditionalData, AdditionalData);
            if (AcrValuesSupported != null)
                ApplyTo(options.AcrValuesSupported, AcrValuesSupported);
            if (AuthorizationEndpoint != null)
                options.AuthorizationEndpoint = AuthorizationEndpoint;
            if (CheckSessionIframe != null)
                options.CheckSessionIframe = CheckSessionIframe;
            if (ClaimsSupported != null)
                ApplyTo(options.ClaimsSupported, ClaimsSupported);
            if (ClaimsLocalesSupported != null)
                ApplyTo(options.ClaimsLocalesSupported, ClaimsLocalesSupported);
            if (ClaimsParameterSupported != null)
                options.ClaimsParameterSupported = ClaimsParameterSupported.Value;
            if (ClaimTypesSupported != null)
                ApplyTo(options.ClaimTypesSupported, ClaimTypesSupported);
            if (DisplayValuesSupported != null)
                ApplyTo(options.DisplayValuesSupported, DisplayValuesSupported);
            if (EndSessionEndpoint != null)
                options.EndSessionEndpoint = EndSessionEndpoint;
            if (FrontchannelLogoutSessionSupported != null)
                options.FrontchannelLogoutSessionSupported = FrontchannelLogoutSessionSupported;
            if (FrontchannelLogoutSupported != null)
                options.FrontchannelLogoutSupported = FrontchannelLogoutSupported;
            if (GrantTypesSupported != null)
                ApplyTo(options.GrantTypesSupported, GrantTypesSupported);
            if (HttpLogoutSupported != null)
                options.HttpLogoutSupported = HttpLogoutSupported.Value;
            if (IdTokenEncryptionAlgValuesSupported != null)
                ApplyTo(options.IdTokenEncryptionAlgValuesSupported, IdTokenEncryptionAlgValuesSupported);
            if (IdTokenEncryptionEncValuesSupported != null)
                ApplyTo(options.IdTokenEncryptionEncValuesSupported, IdTokenEncryptionEncValuesSupported);
            if (IdTokenSigningAlgValuesSupported != null)
                ApplyTo(options.IdTokenSigningAlgValuesSupported, IdTokenSigningAlgValuesSupported);
            if (IntrospectionEndpoint != null)
                options.IntrospectionEndpoint = IntrospectionEndpoint;
            if (IntrospectionEndpointAuthMethodsSupported != null)
                ApplyTo(options.IntrospectionEndpointAuthMethodsSupported, IntrospectionEndpointAuthMethodsSupported);
            if (IntrospectionEndpointAuthSigningAlgValuesSupported != null)
                ApplyTo(options.IntrospectionEndpointAuthSigningAlgValuesSupported, IntrospectionEndpointAuthSigningAlgValuesSupported);
            if (Issuer != null)
                options.Issuer = Issuer;
            if (JwksUri != null)
                options.JwksUri = JwksUri;
            if (LogoutSessionSupported != null)
                options.LogoutSessionSupported = LogoutSessionSupported.Value;
            if (OpPolicyUri != null)
                options.OpPolicyUri = OpPolicyUri;
            if (OpTosUri != null)
                options.OpTosUri = OpTosUri;
            if (RegistrationEndpoint != null)
                options.RegistrationEndpoint = RegistrationEndpoint;
            if (RequestObjectEncryptionAlgValuesSupported != null)
                ApplyTo(options.RequestObjectEncryptionAlgValuesSupported, RequestObjectEncryptionAlgValuesSupported);
            if (RequestObjectEncryptionEncValuesSupported != null)
                ApplyTo(options.RequestObjectEncryptionEncValuesSupported, RequestObjectEncryptionEncValuesSupported);
            if (RequestObjectSigningAlgValuesSupported != null)
                ApplyTo(options.RequestObjectSigningAlgValuesSupported, RequestObjectSigningAlgValuesSupported);
            if (RequestParameterSupported != null)
                options.RequestParameterSupported = RequestParameterSupported.Value;
            if (RequestUriParameterSupported != null)
                options.RequestUriParameterSupported = RequestUriParameterSupported.Value;
            if (RequireRequestUriRegistration != null)
                options.RequireRequestUriRegistration = RequireRequestUriRegistration.Value;
            if (ResponseModesSupported != null)
                ApplyTo(options.ResponseModesSupported, ResponseModesSupported);
            if (ResponseTypesSupported != null)
                ApplyTo(options.ResponseTypesSupported, ResponseTypesSupported);
            if (ServiceDocumentation != null)
                options.ServiceDocumentation = ServiceDocumentation;
            if (ScopesSupported != null)
                ApplyTo(options.ScopesSupported, ScopesSupported);
            if (SubjectTypesSupported != null)
                ApplyTo(options.SubjectTypesSupported, SubjectTypesSupported);
            if (TokenEndpoint != null)
                options.TokenEndpoint = TokenEndpoint;
            if (ActiveTokenEndpoint != null)
                options.ActiveTokenEndpoint = ActiveTokenEndpoint;
            if (TokenEndpointAuthMethodsSupported != null)
                ApplyTo(options.TokenEndpointAuthMethodsSupported, TokenEndpointAuthMethodsSupported);
            if (TokenEndpointAuthSigningAlgValuesSupported != null)
                ApplyTo(options.TokenEndpointAuthSigningAlgValuesSupported, TokenEndpointAuthSigningAlgValuesSupported);
            if (UILocalesSupported != null)
                ApplyTo(options.UILocalesSupported, UILocalesSupported);
            if (UserInfoEndpoint != null)
                options.UserInfoEndpoint = UserInfoEndpoint;
            if (UserInfoEndpointEncryptionAlgValuesSupported != null)
                ApplyTo(options.UserInfoEndpointEncryptionAlgValuesSupported, UserInfoEndpointEncryptionAlgValuesSupported);
            if (UserInfoEndpointEncryptionEncValuesSupported != null)
                ApplyTo(options.UserInfoEndpointEncryptionEncValuesSupported, UserInfoEndpointEncryptionEncValuesSupported);
            if (UserInfoEndpointSigningAlgValuesSupported != null)
                ApplyTo(options.UserInfoEndpointSigningAlgValuesSupported, UserInfoEndpointSigningAlgValuesSupported);
            return options;
        }

        private static ICollection<T> ApplyTo<T>(ICollection<T> baseCollection, ICollection<T> newCollection)
        {
            baseCollection.Clear();
            foreach (T t in newCollection)
            {
                baseCollection.Add(t);
            }

            return baseCollection;
        }
    }
}
