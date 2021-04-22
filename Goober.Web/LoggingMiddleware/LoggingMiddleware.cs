using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Goober.Core.Extensions;

namespace Goober.Web.LoggingMiddleware
{
    public class LoggingMiddleware
    {
        public static int MaxContentLength { get; set; } = 1024 * 30;

        private readonly RequestDelegate _next;

        private const string CallSequenceIdKey = "g-callsec-id";

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

            context.Items[CallSequenceIdKey] = GetCallSequenceIdFromRequestHeaderOrGenerateNew(context);

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

            var sb = new StringBuilder();

            if (request.Form.Files.Count > 0)
            {
                var fileNames = request.Form.Files.Select(x => x.FileName).ToList();
                sb.AppendLine($"files: {string.Join(";", fileNames)}");

                return sb.ToString();
            }

            if (request.ContentLength > MaxContentLength)
                return $"RequestForm content length > {MaxContentLength}";

            request.EnableBuffering();

            foreach (var iFormItem in request.Form)
            {
                sb.AppendLine($"{iFormItem.Key}:{iFormItem.Value}");
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
            var readBodyResult = await request.Body.ReadStreamWithMaxSizeRetrictionAsync(Encoding.UTF8, maxSize: MaxContentLength);
            if (readBodyResult.IsReadToTheEnd == false)
            {
                readBodyResult.StringResult.AppendLine();
                readBodyResult.StringResult.AppendLine($"<<< NOT END, request body is greter than {MaxContentLength}.");
            }
            requestBody = readBodyResult.StringResult.ToString();

            request.Body.Position = 0;
            
            return requestBody;
        }

        private string GetCallSequenceIdFromRequestHeaderOrGenerateNew(HttpContext httpContext)
        {
            var request = httpContext.Request;

            if (request == null || request.Headers.ContainsKey(CallSequenceIdKey) == false)
            {
                return Guid.NewGuid().ToString();
            }

            var ret = request.Headers[CallSequenceIdKey];

            return ret;
        }
    }
}
