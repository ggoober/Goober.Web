using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Goober.WebJobs.Api.Models;

namespace Goober.WebJobs.Api.Services
{
	public interface IWebJobsHttpService
	{
		Task<PingApiResponse> PingAsync(string apiSchemeAndHost, int timeoutMilliseconds = 12000, [CallerMemberName] string callerName = null);
		Task<StartJobResponse> StartJobAsync(string apiSchemeAndHost, StartJobRequest request, int timeoutMilliseconds = 12000, [CallerMemberName] string callerName = null);
		Task<StopJobResponse> StopJobAsync(string apiSchemeAndHost, StopJobRequest request, int timeoutMilliseconds = 12000, [CallerMemberName] string callerName = null);
	}
	
}
