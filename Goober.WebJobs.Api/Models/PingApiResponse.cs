using System.Collections.Generic;

namespace Indusoft.WebJobs.Api.Models
{
    public class PingApiResponse
    {
        public List<WebJobPingModel> Services { get; set; } = new List<WebJobPingModel>();
    }
}
