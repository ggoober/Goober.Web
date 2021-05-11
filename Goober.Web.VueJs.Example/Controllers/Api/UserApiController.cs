using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Goober.Web.VueJs.Example.Controllers.Api
{
    [ApiController]
    public class UserApiController : ControllerBase
    {
        [Route("/api/user/get")]
        [HttpGet]
        public async Task<string> GetAsync([FromQuery] int id)
        {
            return id.ToString();
        }
    }
}
