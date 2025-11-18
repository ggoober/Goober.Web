using System.Collections.Generic;

namespace Goober.WebJobs.Parameters
{
	public class WebJobsParameters : JobParametersBase
	{
        public List<SingleJobParameters> Jobs { get; set; } = new List<SingleJobParameters>();
	}
}
