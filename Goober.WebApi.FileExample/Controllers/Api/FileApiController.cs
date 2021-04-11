using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Goober.Core.Extensions;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using Goober.Http.Utils;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using Goober.WebApi.FileExample.Models;
using Goober.Http.Services;

namespace Goober.WebApi.FileExample.Controllers.Api
{
    [Route("api/file")]
    [ApiController]
    public class FileApiController : ControllerBase
    {
        private readonly ILogger<FileApiController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpJsonHelperService _httpJsonHelperService;
        private readonly IConfiguration _configuration;

        private const long MaxFileSize = 10L * 1024L * 1024L * 1024L;

        public FileApiController(ILogger<FileApiController> logger,
            IHttpClientFactory httpClientFactory,
            IHttpJsonHelperService httpJsonHelperService,
            IConfiguration configuration)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _httpJsonHelperService = httpJsonHelperService;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("post-file-through-api")]
        [RequestSizeLimit(MaxFileSize)]
        [RequestFormLimits(MultipartBodyLengthLimit = MaxFileSize)]
        public async Task<PostFileResult> PostFileThroughApiAsync([FromHeader] int id, [FromHeader] string name, IFormFile file)
        {
            id.RequiredNotNull(nameof(id));
            name.RequiredNotNull(nameof(name));
            file.RequiredNotNull(nameof(file));

            var contentType = file.ContentType;

            if (contentType.StartsWith("application/x-") == true)
            {
                throw new InvalidOperationException();
            }

            var exampleApiSchemeAndHost = _configuration["Example.Api.SchemeAndHost"];

            var url = HttpUtils.BuildUrl(exampleApiSchemeAndHost, "/api/example/post-file");

            var headerValues = new List<KeyValuePair<string, string>>();
            headerValues.Add(new KeyValuePair<string, string>("id", id.ToString()));
            headerValues.Add(new KeyValuePair<string, string>("name", name));

            var ret = await _httpJsonHelperService.UploadFileAsync<PostFileResult>(
                url: url,
                file: file,
                headerValues: headerValues);

            return ret;
        }
    }
}
