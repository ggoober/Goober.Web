using System.Collections.Generic;

namespace Goober.WebJobs.Parameters
{
    public class WebJobsParameters
    {
        public List<SingleJobParameters> Jobs { get; set; } = new List<SingleJobParameters>();
    }
}
