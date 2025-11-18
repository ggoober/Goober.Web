using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Goober.WebJobs.Api.Enums;
using Goober.WebJobs.Api.Models;

namespace Goober.WebJobs.Helpers
{
	internal class HostAndPortPriorityComparer : IPriorityComparer
	{
		private static readonly Regex HostAndPortRegex = new Regex(@"(?<= *https?:(//|\\\\))(?<host>.+)"
			, RegexOptions.Compiled | RegexOptions.IgnoreCase);

		private readonly BaseJob _ownerJob;

		public Lazy<string> SelfApiSchemeAndHost => new Lazy<string>(() =>
			HostAndPortRegex
				.Match(_ownerJob.SelfApiSchemeAndHost)
				.Groups["host"].Value
				.ToLower());

		public HostAndPortPriorityComparer(BaseJob ownerJob)
		{
			_ownerJob = ownerJob;
		}

		public bool OwnerHasPriority(Dictionary<string, WebJobPingModel> apiPingResult) 
			=> apiPingResult
				.Where(s=>s.Value.State != JobState.Stopped)
				.Select(s => HostAndPortRegex.Match(s.Key).Groups["host"].Value.ToLower())
				.All(s => StringComparer.InvariantCulture.Compare(s, SelfApiSchemeAndHost.Value) < 0);
	}
}