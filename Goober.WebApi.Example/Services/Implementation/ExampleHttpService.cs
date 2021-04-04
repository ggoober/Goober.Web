using Goober.Http;
using Goober.Http.Services;
using Goober.WebApi.Example.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Goober.WebApi.Example.Services.Implementation
{
    class ExampleHttpService : BaseHttpService, IExampleHttpService
    {
        protected override string ApiSchemeAndHostConfigKey { get; set; } = "Example.Api.SchemeAndHost";

        public ExampleHttpService(IConfiguration configuration, 
            IHttpJsonHelperService httpJsonHelperService, 
            IHttpContextAccessor httpContextAccessor,
            IHostEnvironment hostEnvironment) 
            : base(configuration, httpJsonHelperService, httpContextAccessor, hostEnvironment)
        {
        }

        public async Task<PostJsonResponse> PostJsonAsync(PostJsonRequest request, [CallerMemberName] string callerMethodName = null)
            => await ExecutePostAsync<PostJsonResponse, PostJsonRequest>("api/example/post-json", request, callerMethodName: callerMethodName);

        public async Task<PostJsonResponse> PostJsonExecuteThroughHttpAsync(PostJsonRequest request, [CallerMemberName] string callerMethodName = null)
            => await ExecutePostAsync<PostJsonResponse, PostJsonRequest>("api/example/post-json-through-http", request, callerMethodName: callerMethodName);
    }
}
