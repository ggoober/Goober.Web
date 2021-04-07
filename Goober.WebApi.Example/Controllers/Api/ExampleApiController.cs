﻿using Goober.WebApi.Example.Models;
using Goober.Core.Extensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Goober.CommonModels;
using Microsoft.Extensions.Logging;
using Goober.WebApi.Example.Services;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace Goober.WebApi.Example.Controllers.Api
{
    [Route("api/example")]
    [ApiController]
    public class ExampleApiController : ControllerBase
    {
        private readonly ILogger<ExampleApiController> _logger;
        private readonly IExampleHttpService _exampleHttpService;
        private readonly IWebHostEnvironment _appEnvironment;

        private const long MaxFileSize = 10L * 1024L * 1024L * 1024L;

        public ExampleApiController(ILogger<ExampleApiController> logger,
            IExampleHttpService exampleHttpService,
            IWebHostEnvironment appEnvironment)
        {
            _logger = logger;
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
        public string PostForm([FromForm] int id, [FromForm] string name, [FromForm]DateTime date)
        {
            name.RequiredArgumentNotNull(nameof(name));

            _logger.LogError("form readed");

            return $"post-form {new { id, name }.Serialize()}";
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
                throw new InvalidOperationException();
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
        public void Throwxception([FromBody]LogRequest request)
        {
            request.RequiredArgumentNotNull(nameof(request));

            throw new InvalidOperationException(request.LogMessage);
        }
    }
}
