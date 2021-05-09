using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Goober.Web.VueJs.Example.Controllers.Api
{
    [ApiController]
    public class EntityApiController : ControllerBase
    {
        [Route("/api/entity/get")]
        [HttpGet]
        public async Task<string> GetAsync([FromQuery] int id)
        {
            return id.ToString();
        }
    }
}
