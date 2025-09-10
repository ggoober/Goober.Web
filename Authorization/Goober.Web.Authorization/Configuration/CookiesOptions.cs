using Microsoft.AspNetCore.Authentication.Cookies;

namespace Goober.Web.Authorization.Configuration
{
    public class CookiesOptions
    {
        public bool Enabled { get; set; }
        public string? SchemeName { get; set; }
        public bool? SlidingExpiration { get; set; }
        public string? LoginPath { get; set; }
        public string? LogoutPath { get; set; }
        public string? AccessDeniedPath { get; set; }
        public string? ReturnUrlParameter { get; set; }
        public TimeSpan? ExpireTimeSpan { get; set; }
        public string? ClaimsIssuer { get; set; }
        public string? ForwardDefault { get; set; }
        public string? ForwardAuthenticate { get; set; }
        public string? ForwardChallenge { get; set; }
        public string? ForwardForbid { get; set; }
        public string? ForwardSignIn { get; set; }
        public string? ForwardSignOut { get; set; }
        public CookieBuilderOptions? Cookie { get; set; }

        public void ApplyTo(CookieAuthenticationOptions options)
        {
            if (SlidingExpiration != null)
                options.SlidingExpiration = SlidingExpiration.Value;
            if (LoginPath != null)
                options.LoginPath = LoginPath;
            if (LogoutPath != null)
                options.LogoutPath = LogoutPath;
            if (AccessDeniedPath != null)
                options.AccessDeniedPath = AccessDeniedPath;
            if (ReturnUrlParameter != null)
                options.ReturnUrlParameter = ReturnUrlParameter;
            if (ExpireTimeSpan != null)
                options.ExpireTimeSpan = ExpireTimeSpan.Value;
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
            if (Cookie != null)
                options.Cookie = Cookie.ApplyTo(options.Cookie!);
        }
    }
}
