using Goober.WebApi.Example.Models;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Goober.WebApi.Example.Services
{
    public interface IExampleHttpService
    {
        Task<PostJsonResponse> PostJsonAsync(PostJsonRequest request, [CallerMemberName] string callerMethodName = null);

        Task<PostJsonResponse> PostJsonExecuteThroughHttpAsync(PostJsonRequest request, [CallerMemberName] string callerMethodName = null);
    }
}