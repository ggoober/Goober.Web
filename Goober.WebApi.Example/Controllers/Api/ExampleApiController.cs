using Goober.WebApi.Example.Models;
using Goober.Core.Extensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Goober.CommonModels;
using Microsoft.Extensions.Logging;

namespace Goober.WebApi.Example.Controllers.Api
{
    [Route("api/example")]
    [ApiController]
    public class ExampleApiController : ControllerBase
    {
        private readonly ILogger<ExampleApiController> _logger;

        public ExampleApiController(ILogger<ExampleApiController> logger)
        {
            _logger = logger;
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
            _logger.LogError("query string readed");

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
            _logger.LogError("json readed");

            return $"post-json {request.Serialize()}";
        }

        [HttpPost]
        [Route("post-form")]
        public string PostForm([FromForm] int id, [FromForm] string name, [FromForm]DateTime date)
        {
            name.RequiredArgumentNotNull(nameof(name));

            _logger.LogError("form readed");

            return $"post-form {new { id, name }.Serialize()}";
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

            _logger.LogError("file readed");

            return new FileStreamResult(fileStream: stream, contentType: contentType)
            {
                FileDownloadName = name
            };
        }

        /// <summary>
        /// hidden method
        /// </summary>
        [HttpGet]
        [Route("hidden")]
        [SwaggerHideInDocsAttribute("test")]
        public void HiddenMethod()
        {
            _logger.LogError("hidden method executed");
        }

        [HttpPost]
        [Route("throw-exception")]
        public void Throwxception([FromBody]LogRequest request)
        {
            request.RequiredArgumentNotNull(nameof(request));

            throw new InvalidOperationException(request.LogMessage);
        }
    }
}
