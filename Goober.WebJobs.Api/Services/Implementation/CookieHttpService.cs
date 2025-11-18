using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Goober.Http;
using Goober.Http.Services;
using Goober.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Goober.WebJobs.Api.Services.Implementation
{
	public class CookieHttpService: BaseHttpService, ICookieHttpService
	{
		public CookieHttpService(IConfiguration configuration, IHttpJsonHelperService httpJsonHelperService, IHttpContextAccessor httpContextAccessor) : base(configuration, httpJsonHelperService, httpContextAccessor)
		{
		}

		protected override string ApiSchemeAndHostConfigKey { get; set; }

		public async Task<SetCookieResponse> SetAsync(string apiSchemeAndHost, SetCookieRequest request, int timeoutMilliseconds = 12000, [CallerMemberName] string callerName = null)
			=> await ExecutePostAsync<SetCookieResponse, SetCookieRequest>(
				path: "/api-base/cookie/set",
				request: request,
				callerMemberName: callerName,
				timeoutInMilliseconds: timeoutMilliseconds,
				overrieApiSchemeAndHost: apiSchemeAndHost);
	}

}
