using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goober.Web.LDAP.Abstractions.Models
{
    internal class ADUser
    {
        public string? Name { get; set; }
        public string DistinguishedName { get; set; } = string.Empty;
        public bool Enabled { get; set; }
    }
}
