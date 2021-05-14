using System.Collections.Generic;

namespace Goober.Web.VueJs.Example.Models.Claims
{
    public class SearchClaimsResponse
    {
        public int FoundCount { get; set; }

        public List<SearchClaimsSingleModel> Claims { get; set; } = new List<SearchClaimsSingleModel>();
    }
}
