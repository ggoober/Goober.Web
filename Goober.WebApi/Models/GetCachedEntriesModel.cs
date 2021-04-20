using System;

namespace Goober.WebApi.Models
{
    public class GetCachedEntriesModel
    {
        public string CacheKey { get; set; }

        public DateTime? NextRefreshDateTime { get; set; }

        public int? RefreshTimeInMinutes { get; set; }

        public DateTime? LastRefreshDateTime { get; set; }

        public DateTime? ExpirationDateTime { get; set; }

        public int? ExpirationTimeInMinutes { get; set; }

        public DateTime? LastAccessDateTime { get; set; }

        public DateTime RowCreatedDateTime { get; set; }

        public bool IsEmpty { get; set; }
    }
}
