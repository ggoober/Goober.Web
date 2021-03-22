using Goober.WebApi.Example.Models;
using Goober.Core.Extensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

namespace Goober.WebApi.Example.Controllers.Api
{
    [Route("api/example")]
    [ApiController]
    public class ExampleApiController : ControllerBase
    {
        public ExampleApiController()
        {
        }

        [HttpGet]
        [Route("get-strinng")]
        public string GetString([FromQuery] int intValue,
            [FromQuery] float floatValue,
            [FromQuery] string stringValue,
            [FromQuery] DateTime dateValue,
            [FromQuery] ExampleEnum enumValue,
            [FromQuery] List<int> intList)
        {
            return $"get-string result {new { intValue, floatValue, stringValue, dateValue, enumValue, intList }.Serialize()}";
        }

        [HttpPost]
        [Route("post-json")]
        public string PostJson([FromBody] PostJsonRequest request)
        {
                //if (request.Params == null || request.Params.Any() == false)
                //{
                //    request.Params = new System.Collections.Generic.Dictionary<int, string>();
                //    request.Params.Add(1, "1str");
                //    request.Params.Add(2, "2str");
                //}
            return $"post-json {request.Serialize()}";
        }
    }
}
