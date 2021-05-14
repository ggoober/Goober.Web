using AutoFixture;
using Goober.Web.VueJs.Example.Models.Claims;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Goober.Web.VueJs.Example.Controllers.Api
{
    [ApiController]
    public class ClaimApiController : ControllerBase
    {
        [Route("api/claim/search")]
        [HttpPost]
        public async Task<SearchClaimsResponse> SearchAsync([FromBody] SearchClaimsRequest request)
        {
            var fixture = new Fixture();
            var random = new Random();

            var claims = fixture.CreateMany<SearchClaimsSingleModel>(random.Next(5, 100));

            return new SearchClaimsResponse {
                FoundCount = claims.Count(),
                Claims = claims.Take(request.Count).ToList()
            };
        }
    }
}
