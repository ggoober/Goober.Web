using Goober.WebApi.Example.Api.Models;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Goober.WebApi.Example.Api.Services
{
    public interface IExampleHttpService
    {
        Task<PostJsonResponse> PostJsonAsync(PostJsonRequest request, [CallerMemberName] string callerMethodName = null);

        Task<PostJsonResponse> PostJsonExecuteThroughHttpAsync(PostJsonRequest request, [CallerMemberName] string callerMethodName = null);
    }
}