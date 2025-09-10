namespace Goober.Web.Negotiate.Configuration
{
    public class NegotiateOptions
    {
        public bool Enabled { get; set; }
        public string? SchemeName { get; set; }
        public bool? PersistKerberosCredentials { get; set; }
        public bool? PersistNtlmCredentials { get; set; }
        public string? ClaimsIssuer { get; set; }
        public string? ForwardDefault { get; set; }
        public string? ForwardAuthenticate { get; set; }
        public string? ForwardChallenge { get; set; }
        public string? ForwardForbid { get; set; }
        public string? ForwardSignIn { get; set; }
        public string? ForwardSignOut { get; set; }
        public LDAPOptions? LDAP { get; set; }

        public void ApplyTo(Microsoft.AspNetCore.Authentication.Negotiate.NegotiateOptions options)
        {
            if (PersistKerberosCredentials != null)
                options.PersistKerberosCredentials = PersistKerberosCredentials.Value;
            if (PersistNtlmCredentials != null)
                options.PersistNtlmCredentials = PersistNtlmCredentials.Value;
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
            if (LDAP != null) LDAP.ApplyTo(options);
        }
    }
}
