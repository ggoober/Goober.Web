using Microsoft.AspNetCore.Mvc;
using Goober.Web.Models;

namespace Goober.Web.Controllers.Api
{
    [ApiController]
    public class CookieApiController : ControllerBase
    {
        [HttpPost]
        [Route("/api-base/cookie/set")]
        public SetCookieResponse Set([FromBody]SetCookieRequest request)
        {
            Response.Cookies.Append(key: request.Name, value: request.Value);

            return new SetCookieResponse { IsSuccess = true };
        }
    }
}
