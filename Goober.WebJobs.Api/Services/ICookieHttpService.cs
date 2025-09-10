using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Indusoft.Web.Models;

namespace Indusoft.WebJobs.Api.Services
{
	public interface ICookieHttpService
	{
		Task<SetCookieResponse> SetAsync(string apiSchemeAndHost, SetCookieRequest request, int timeoutMilliseconds = 12000, [CallerMemberName] string callerName = null);
	}
}