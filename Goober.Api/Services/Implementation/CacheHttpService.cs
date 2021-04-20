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
    class CacheHttpService : BaseHttpService, ICacheHttpService
    {
        public CacheHttpService(
            IConfiguration configuration,
            IHttpJsonHelperService httpJsonHelperService,
            IHttpContextAccessor httpContextAccessor,
            IHostEnvironment hostEnvironment)
            : base(configuration, httpJsonHelperService, httpContextAccessor, hostEnvironment)
        {
        }

        protected override string ApiSchemeAndHostConfigKey { get; set; }

        public async Task<GetCachedEntriesResponse> GetCachedEntriesAsync(
            string targetSchemeAndHost,
            AuthenticationHeaderValue authenticationHeaderValue = null,
            [CallerMemberName] string callerMemberName = null)
            => await ExecuteGetAsync<GetCachedEntriesResponse>(path: "/api/cache/get-entries",
                            queryParameters: null,
                            authenticationHeaderValue: authenticationHeaderValue,
                            overrieApiSchemeAndHost: targetSchemeAndHost,
                            callerMemberName: callerMemberName);

    }
}
