using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Goober.Core.Extensions;
using System.Reflection;

namespace Goober.WebApi.LoggingMiddleware
{
    public class LoggingMiddleware
    {
        public static int MaxContentLength { get; set; } = 1024 * 5;

        private readonly RequestDelegate _next;

        private string _assemblyName;

        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task Invoke(HttpContext context)
        {
            var request = context?.Request;
            if (request == null)
                return;

            context.Items["APPLICATION"] = ProgramUtils.ApplicationName;

            var requestForm = GetRequestForm(request);
            context.Items["CONTEXT_REQUEST_FORM"] = requestForm;

            string requestBody = null;
            if (string.IsNullOrEmpty(requestForm) == true)
            {
                requestBody = await GetRequestBodyAsync(request);
            }
            context.Items["CONTEXT_REQUEST_BODY"] = requestBody;

            await _next(context);
        }

        private string GetRequestForm(HttpRequest request)
        {
            if (request.Method == "GET")
                return null;

            if (request.ContentType.Contains("form-data") == false)
                return null;

            if (request.ContentLength > MaxContentLength)
                return $"RequestForm content length > {MaxContentLength}";

            request.EnableBuffering();

            var sb = new StringBuilder();

            foreach (var iFormItem in request.Form)
            {
                sb.AppendLine($"{iFormItem.Key}:{iFormItem.Value}");
            }

            if (request.Form.Files.Count > 0)
            {
                var fileNames = request.Form.Files.Select(x => x.FileName).ToList();
                sb.AppendLine($"files: {string.Join(";", fileNames)}");
            }

            request.Body.Position = 0;

            return sb.ToString();
        }

        private async Task<string> GetRequestBodyAsync(HttpRequest request)
        {
            if (request.Method.ToUpper() == "GET")
                return null;

            if (request.ContentLength > MaxContentLength)
                return $"RequestBody content length > {MaxContentLength}";

            string requestBody;
            
            request.EnableBuffering();
            requestBody = await request.Body.ReadWithMaxSizeLimitsAsync(Encoding.UTF8, maxSize: MaxContentLength);
            request.Body.Position = 0;
            
            return requestBody;
        }
    }
}
