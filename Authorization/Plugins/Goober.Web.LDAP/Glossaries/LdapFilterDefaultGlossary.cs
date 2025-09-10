using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goober.Web.LDAP.Glossaries
{
    public static class LdapFilterDefaultGlossary
    {
        public const string FilterSearchUser = $"(&(objectCategory=person)(|(sAMAccountName={LdapFilterParamGlossary.UserName})(userPrincipalName={LdapFilterParamGlossary.UserName})))";
    }
}
