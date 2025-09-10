using System.Collections.Generic;

namespace Indusoft.Web.VueJs.Example.Models.Claims
{
    public class SearchClaimsResponse
    {
        public int FoundCount { get; set; }

        public List<SearchClaimsSingleModel> Claims { get; set; } = new List<SearchClaimsSingleModel>();
    }
}
