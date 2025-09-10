using Goober.Base.Extensions;
using System;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Goober.Web.Negotiate.Configuration
{
    public class LDAPOptions
    {
        public bool Enabled { get; set; }
        public bool? EnableLdapClaimResolution { get; set; }
        public string? Domain { get; set; }
        public string? MachineAccountName { get; set; }
        public string? MachineAccountPassword { get; set; }
        public string? EncryptedMachineAccountPassword { get; set; }
        public bool? IgnoreNestedGroups { get; set; }
        public TimeSpan? ClaimsCacheSlidingExpiration { get; set; }
        public TimeSpan? ClaimsCacheAbsoluteExpiration { get; set; } 
        public int? ClaimsCacheSize { get; set; }
        public LDAPConnection? Connection { get; set; }
        public void ApplyTo(Microsoft.AspNetCore.Authentication.Negotiate.NegotiateOptions options)
        {
            if (!Enabled)
            {
                return;
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                options.EnableLdap(c =>
                {
                    if (EnableLdapClaimResolution != null)
                        c.EnableLdapClaimResolution = EnableLdapClaimResolution.Value;
                    if (Domain != null)
                        c.Domain = Domain;
                    if (MachineAccountName != null)
                        c.MachineAccountName = MachineAccountName;
                    if (EncryptedMachineAccountPassword != null)
                        c.MachineAccountPassword = CryptoExtensions.DecryptString(EncryptedMachineAccountPassword);
                    if (MachineAccountPassword != null)
                        c.MachineAccountPassword = MachineAccountPassword;
                    if (IgnoreNestedGroups != null)
                        c.IgnoreNestedGroups = IgnoreNestedGroups.Value;
                    if (ClaimsCacheSlidingExpiration != null)
                        c.ClaimsCacheSlidingExpiration = ClaimsCacheSlidingExpiration.Value;
                    if (ClaimsCacheAbsoluteExpiration != null)
                        c.ClaimsCacheAbsoluteExpiration = ClaimsCacheAbsoluteExpiration.Value;
                    if (ClaimsCacheSize != null)
                        c.ClaimsCacheSize = ClaimsCacheSize.Value;
                    if (Connection != null)
                        Connection.ApplyTo(c);
                });
            }
        }
    }
}
