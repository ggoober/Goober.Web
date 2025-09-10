using System.Collections.Generic;

namespace Indusoft.WebJobs.Parameters
{
	public class WebJobsParameters : JobParametersBase
	{
        public List<SingleJobParameters> Jobs { get; set; } = new List<SingleJobParameters>();
	}
}
