using Goober.WebApi.Example.Models;
using Goober.Core.Extensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace Goober.WebApi.Example.Controllers.Api
{
    [Route("api/example")]
    [ApiController]
    public class ExampleApiController : ControllerBase
    {
        public ExampleApiController()
        {
        }

        /// <summary>
        /// Get api method
        /// </summary>
        /// <param name="intValue"></param>
        /// <param name="floatValue"></param>
        /// <param name="stringValue"></param>
        /// <param name="dateValue"></param>
        /// <param name="enumValue"></param>
        /// <param name="intList"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("get")]
        public string Get([FromQuery] int intValue,
            [FromQuery] float floatValue,
            [FromQuery] string stringValue,
            [FromQuery] DateTime dateValue,
            [FromQuery] ExampleEnum enumValue,
            [FromQuery] List<int> intList)
        {
            return $"get-string result {new { intValue, floatValue, stringValue, dateValue, enumValue, intList }.Serialize()}";
        }

        /// <summary>
        /// Post json api method
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
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

        [HttpPost]
        [Route("post-file")]
        public async Task<FileResult> PostBytesAsync([FromHeader] int id, [FromHeader] string name, IFormFile file)
        {
            id.RequiredNotNull(nameof(id));
            name.RequiredNotNull(nameof(name));
            file.RequiredNotNull(nameof(file));

            if (file.ContentType.StartsWith("application/x-") == true)
            {
                throw new InvalidOperationException();
            }

            var stream = file.OpenReadStream();
            var contentType = file.ContentType;

            return new FileStreamResult(fileStream: stream, contentType: contentType)
            {
                FileDownloadName = name
            };
        }
    }
}
