using AutoFixture;
using Goober.Web.VueJs.Example.Models.Claims;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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

            var claims = fixture.Build<SearchClaimsSingleModel>()
                .With(x => x.Name, request.TextQuery + fixture.Create<string>())
                .With(x => x.ScopeId, GetRandomScopeFromListOrGenerate(request.ScopeIds, fixture))
                .CreateMany(random.Next(5, 100));

            return new SearchClaimsResponse
            {
                FoundCount = claims.Count(),
                Claims = claims.Take(request.Count).ToList()
            };
        }

        private int GetRandomScopeFromListOrGenerate(List<int> scopeIds, Fixture fixture)
        {
            if (scopeIds == null || scopeIds.Any() == false)
                return fixture.Create<int>();

            var random = new Random();
            var randomIndex = random.Next(0, scopeIds.Count - 1);

            return scopeIds[randomIndex];
        }
    }
}
