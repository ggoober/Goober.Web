using System.Collections.Generic;
using Indusoft.WebJobs.Api.Models;

namespace Indusoft.WebJobs.Helpers
{
	internal interface IPriorityComparer
	{
		bool OwnerHasPriority(Dictionary<string, WebJobPingModel> apiPingResult);
	}
}