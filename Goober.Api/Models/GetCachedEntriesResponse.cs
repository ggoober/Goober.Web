using System.Collections.Generic;

namespace Goober.Api.Models
{
    public class GetCachedEntriesResponse
    {
        public List<GetCachedEntriesModel> CachedEntries { get; set; } = new List<GetCachedEntriesModel>();
    }
}
