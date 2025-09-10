using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
using Indusoft.WebJobs.Api.Models;

namespace Indusoft.WebJobs.Services
{
	public interface IClusterInfoVisor
	{
		int LifetimeResultInMilliseconds { get; set; }
		Task<List<KeyValuePair<string, PingApiResponse>>> GetApiPingResultsAsync(List<string> apiSchemeAndHosts, CancellationToken cancellationToken, int? pingTimeout, bool isForceUpdateResult = false);
		Task<List<KeyValuePair<string, PingReply>>> GetHostPingResultsAsync(List<string> apiSchemeAndHosts, CancellationToken cancellationToken, int? pingTimeout, bool isForceUpdateResult = false);
	}
}