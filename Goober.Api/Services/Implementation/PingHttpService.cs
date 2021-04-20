using Goober.Api.Models;
using Goober.Http;
using Goober.Http.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Goober.Api.Services.Implementation
{
    class PingHttpService : BaseHttpService, IPingHttpService
    {
        public PingHttpService(IConfiguration configuration,
            IHttpJsonHelperService httpJsonHelperService,
            IHttpContextAccessor httpContextAccessor,
            IHostEnvironment hostEnvironment)
            : base(configuration, httpJsonHelperService, httpContextAccessor, hostEnvironment)
        {
        }

        protected override string ApiSchemeAndHostConfigKey { get; set; }

        public async Task<GetPingResponse> GetPingAsync(
            string targetSchemeAndHost,
            AuthenticationHeaderValue authenticationHeaderValue = null,
            [CallerMemberName] string callerMemberName = null)
            => await ExecuteGetAsync<GetPingResponse>(path: "/api/ping/get",
                            queryParameters: null,
                            authenticationHeaderValue: authenticationHeaderValue,
                            overrieApiSchemeAndHost: targetSchemeAndHost,
                            callerMemberName: callerMemberName);
    }
}
