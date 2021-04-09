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

namespace Goober.WebApi.FileExample.Controllers.Api
{
    [Route("api/file")]
    [ApiController]
    public class FileApiController : ControllerBase
    {
        private readonly ILogger<FileApiController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        private const long MaxFileSize = 10L * 1024L * 1024L * 1024L;

        public FileApiController(ILogger<FileApiController> logger,
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
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

            var stream = file.OpenReadStream();

            using (var httpClient = _httpClientFactory.CreateClient())
            {
                httpClient.Timeout = TimeSpan.FromMilliseconds(120000);

                var httpRequest = HttpUtils.GenerateHttpRequestMessage(
                    requestUri: url,
                    httpMethodType: HttpMethod.Post,
                    authenticationHeaderValue: null,
                    headerValues: null,
                    responseMediaTypes: new List<string> { "application/json" });


                var streamContent = new StreamContent(stream);
                streamContent.Headers.Add("Content-Type", file.ContentType);

                var formData = new MultipartFormDataContent();
                formData.Add(streamContent, "file", file.FileName);

                httpRequest.Content = formData;

                foreach (var iCustomHeader in headerValues)
                {
                    httpRequest.Content.Headers.Add(iCustomHeader.Key, iCustomHeader.Value);
                }

                var httpResponse = await httpClient.SendAsync(httpRequest);

                var ret = await httpResponse.Content.ReadAsStringAsync();

                _logger.LogError("file sended");

                return ret.Deserialize<PostFileResult>();
            }
        }
    }
}
