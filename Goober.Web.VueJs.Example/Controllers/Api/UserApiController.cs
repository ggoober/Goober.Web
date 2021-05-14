using AutoFixture;
using Goober.Web.VueJs.Example.Models.User;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
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

        [Route("api/user/search")]
        [HttpPost]
        public async Task<SearchUsersResponse> SearchAsync([FromBody] SearchUsersRequest request)
        {
            var fixture = new Fixture();
            var random = new Random();

            var ret = new SearchUsersResponse();
            
            var allUsers = fixture.CreateMany<SearchUsersSingleModel>(random.Next(5, 100));
            ret.Users = allUsers.Take(request.Count).ToList();
            ret.Count = allUsers.Count();

            return ret;
        }
    }
}
