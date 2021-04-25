using Goober.WebApi.Example.Models;
using Goober.Core.Extensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Goober.WebApi.Example.Services;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Goober.Core.Attributes;

namespace Goober.WebApi.Example.Controllers.Api
{
    [Route("api/example")]
    [ApiController]
    public class ExampleApiController : ControllerBase
    {
        private readonly ILogger<ExampleApiController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IExampleHttpService _exampleHttpService;
        private readonly IWebHostEnvironment _appEnvironment;

        private const long MaxFileSize = 10L * 1024L * 1024L * 1024L;

        public ExampleApiController(ILogger<ExampleApiController> logger,
            IConfiguration configuration,
            IExampleHttpService exampleHttpService,
            IWebHostEnvironment appEnvironment)
        {
            _logger = logger;
            _configuration = configuration;
            _exampleHttpService = exampleHttpService;
            _appEnvironment = appEnvironment;
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
        public GetResponse Get([FromQuery] int intValue,
            [FromQuery] float floatValue,
            [FromQuery] string stringValue,
            [FromQuery] DateTime dateValue,
            [FromQuery] ExampleEnum enumValue,
            [FromQuery] List<int> intList)
        {
            _logger.LogError("query string readed");

            return new GetResponse
            {
                IntValue = intValue,
                FloatValue = floatValue,
                StringValue = stringValue,
                DateValue = dateValue,
                EnumValue = enumValue,
                IntList = intList
            };
        }

        /// <summary>
        /// Post json api method
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("post-json")]
        public PostJsonResponse PostJson([FromBody] PostJsonRequest request)
        {
            _logger.LogError("json readed");

            return new PostJsonResponse { Message = $"post-json {request.Serialize()}" };
        }

        /// <summary>
        /// Post json api method
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("post-json-through-http")]
        public async Task<PostJsonResponse> PostJsonExecuteThroughHttpAsync([FromBody] PostJsonRequest request)
        {
            request.RequiredNotNull(nameof(request));
            request.RequiredNotNull(() => request.StringValue);
            request.RequiredArgumentEnumIsDefinedValue(() => request.EnumValue);
            request.RequiredArgumentNotDefaultValue(() => request.IntValue);
            request.RequiredArgumentListNotEmpty(() => request.IntList);
            request.RequiredArgumentNotDefaultValue(() => request.FloatValue);

            _logger.LogError("json readed");

            return await _exampleHttpService.PostJsonAsync(request);
        }

        /// <summary>
        /// Post json api method
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("post-json-through-http-twice")]
        public async Task<PostJsonResponse> PostJsonExecuteThroughHttpTwiceAsync([FromBody] PostJsonRequest request)
        {
            _logger.LogError("json readed");

            return await _exampleHttpService.PostJsonExecuteThroughHttpAsync(request);
        }

        [HttpPost]
        [Route("post-form")]
        public string PostForm([FromForm] int id, [FromForm] string name, [FromForm] DateTime date)
        {
            name.RequiredArgumentNotNull(nameof(name));

            _logger.LogError("form readed");

            return $"{new { id, name, date }.Serialize()}";
        }

        [HttpPost]
        [Route("post-file")]
        [RequestSizeLimit(MaxFileSize)]
        [RequestFormLimits(MultipartBodyLengthLimit = MaxFileSize)]
        public async Task<PostFileResult> PostFileAsync([FromHeader] int id, [FromHeader] string name, IFormFile file)
        {
            id.RequiredNotNull(nameof(id));
            name.RequiredNotNull(nameof(name));
            file.RequiredNotNull(nameof(file));

            var contentType = file.ContentType;

            if (contentType.StartsWith("application/x-") == true)
            {
                throw new InvalidOperationException("Can't upload exe files");
            }

            string path = Path.Combine("files", file.FileName);
            using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            _logger.LogError("file saved");

            return new PostFileResult { FileName = file.FileName };
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
        public void ThrowException([FromBody] LogRequest request)
        {
            request.RequiredArgumentNotNull(nameof(request));

            throw new InvalidOperationException(request.LogMessage);
        }

        public class Div
        {
            public string Text { get; set; }

            public string Span { get; set; }
        }
        public class Body
        {
            public Div Div { get; set; }

            public List<Div> Divs { get; set; } = new List<Div>();
        }

        public class Doc
        {
            public Body Body { get; set; }
        }

        [HttpGet]
        [Route("get-configs")]
        public string GetConfigs()
        {
            var section = _configuration.GetSection("Doc");

            var ret = section?.Get<Doc>().Serialize();

            return ret;
        }
    }
}
