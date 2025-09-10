using System.Collections.Generic;

namespace Indusoft.Web.VueJs.Example.Models.Claims
{
    public class SearchClaimsRequest
    {
        public string TextQuery { get; set; }

        public int Count { get; set; }

        public List<int> ScopeIds { get; set; } = new List<int>();
    }
}
