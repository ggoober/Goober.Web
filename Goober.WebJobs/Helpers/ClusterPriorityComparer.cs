using System.Collections.Generic;
using System.Linq;
using Goober.WebJobs.Api.Enums;
using Goober.WebJobs.Api.Models;

namespace Goober.WebJobs.Helpers
{
	internal class ClusterPriorityComparer : IPriorityComparer
	{
		private readonly BaseJob _ownerJob;

		public ClusterPriorityComparer(BaseJob ownerJob)
		{
			_ownerJob = ownerJob;
		}

		public bool OwnerHasPriority(Dictionary<string, WebJobPingModel> apiPingResult)
			=> apiPingResult
				.Where(s => s.Value.State != JobState.Stopped)
				.All(s => s.Value.ClusterNodePriority < _ownerJob.ClusterNodePriority);
	}
}