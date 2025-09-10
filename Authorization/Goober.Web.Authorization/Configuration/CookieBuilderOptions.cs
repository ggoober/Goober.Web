using Microsoft.AspNetCore.Http;

namespace Goober.Web.Authorization.Configuration
{
    public class CookieBuilderOptions
    {
        public string? Name { get; set; }
        public string? Path { get; set; }
        public string? Domain { get; set; }
        public bool? HttpOnly { get; set; }
        public SameSiteMode? SameSite { get; set; }
        public CookieSecurePolicy? SecurePolicy { get; set; }
        public TimeSpan? Expiration { get; set; }
        public TimeSpan? MaxAge { get; set; }
        public bool? IsEssential { get; set; }

        public CookieBuilder ApplyTo(CookieBuilder options)
        {
            if (options == null)
            {
                options = new CookieBuilder();
            }

            if (Name != null) options.Name = Name;
            if (Path != null) options.Path = Path;
            if (Domain != null) options.Domain = Domain;
            if (HttpOnly != null) options.HttpOnly = HttpOnly.Value;
            if (SameSite != null) options.SameSite = SameSite.Value;
            if (SecurePolicy != null) options.SecurePolicy = SecurePolicy.Value;
            if (Expiration != null) options.Expiration = Expiration;
            if (MaxAge != null) options.MaxAge = MaxAge;
            if (IsEssential != null) options.IsEssential = IsEssential.Value;
            return options;
        }
    }
}
