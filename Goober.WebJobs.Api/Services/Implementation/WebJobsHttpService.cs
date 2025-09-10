using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Indusoft.DependencyInjection.Attributes;
using Indusoft.Http;
using Indusoft.Http.Services;
using Indusoft.WebJobs.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Indusoft.WebJobs.Api.Services.Implementation
{
	[ExportService(typeof(IWebJobsHttpService), ServiceLifetime.Transient)]
	class WebJobsHttpServiсe : BaseHttpService, IWebJobsHttpService
	{
		public WebJobsHttpServiсe(IConfiguration configuration, IHttpJsonHelperService httpJsonHelperService, IHttpContextAccessor httpContextAccessor) : base(configuration, httpJsonHelperService, httpContextAccessor)
		{
		}

		protected override string ApiSchemeAndHostConfigKey { get; set; }

		public async Task<PingApiResponse> PingAsync(string apiSchemeAndHost, int timeoutMilliseconds = 12000, [CallerMemberName] string callerName = null) 
			=> await ExecuteGetAsync<PingApiResponse>(
				path: "api/job/ping",
				queryParameters: null,
				callerMemberName: callerName, 
				timeoutMiliseconds: timeoutMilliseconds,
				overrieApiSchemeAndHost: apiSchemeAndHost);

		public async Task<StartJobResponse> StartJobAsync(string apiSchemeAndHost, StartJobRequest request, int timeoutMilliseconds = 12000, [CallerMemberName] string callerName = null)
			=> await ExecutePostAsync<StartJobResponse, StartJobRequest>(
				path: "api/job/start",
				request: request,
				callerMemberName: callerName,
				timeoutInMilliseconds: timeoutMilliseconds,
				overrieApiSchemeAndHost: apiSchemeAndHost);

		public async Task<StopJobResponse> StopJobAsync(string apiSchemeAndHost, StopJobRequest request, int timeoutMilliseconds = 12000, [CallerMemberName] string callerName = null)
			=> await ExecutePostAsync<StopJobResponse, StopJobRequest>(
				path: "api/job/stop",
				request: request,
				callerMemberName: callerName,
				timeoutInMilliseconds: timeoutMilliseconds,
				overrieApiSchemeAndHost: apiSchemeAndHost);


	}
}