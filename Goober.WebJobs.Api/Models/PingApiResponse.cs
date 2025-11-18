using System.Collections.Generic;

namespace Goober.WebJobs.Api.Models
{
    public class PingApiResponse
    {
        public List<WebJobPingModel> Services { get; set; } = new List<WebJobPingModel>();
    }
}
