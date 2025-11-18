using System.Collections.Generic;
using Goober.WebJobs.Api.Models;

namespace Goober.WebJobs.Helpers
{
	internal interface IPriorityComparer
	{
		bool OwnerHasPriority(Dictionary<string, WebJobPingModel> apiPingResult);
	}
}