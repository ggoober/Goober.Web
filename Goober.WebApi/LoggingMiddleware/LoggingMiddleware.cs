using Microsoft.AspNetCore.Http;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goober.WebApi.LoggingMiddleware
{
    public class LoggingMiddleware
    {
        public static int MaxContentLength { get; set; } = 1024 * 5;

        private readonly RequestDelegate next;

        public LoggingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            //var requestHeaders = GetRequestHeadersStr(context);

            var request = context?.Request;
            if (request == null)
                return;

            var requestForm = await GetRequestFormAsync(request);
            context.Items["CONTEXT_REQUEST_FORM"] = requestForm;

            string requestBody = null;
            if (string.IsNullOrEmpty(requestForm) == true)
            {
                requestBody = await GetRequestBodyAsync(request);
            }
            context.Items["CONTEXT_REQUEST_BODY"] = requestBody;

            await next(context);
        }

        private static async Task<string> GetRequestFormAsync(HttpRequest request)
        {
            if (request.Method == "GET")
                return null;

            if (request.ContentType.Contains("form-data") == false)
                return null;

            if (request.ContentLength > MaxContentLength)
                return $"ContentForm content length > {MaxContentLength}";

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

        private static async Task<string> GetRequestBodyAsync(HttpRequest request)
        {
            if (request.Method.ToUpper() == "GET")
                return null;

            if (request.ContentLength > MaxContentLength)
                return $"ContentForm content length > {MaxContentLength}";

            string requestBody;
            
            request.EnableBuffering();
            requestBody = await ReadRequestBodyAsync(request);
            request.Body.Position = 0;
            
            return requestBody;
        }

        private async static Task<string> ReadRequestBodyAsync(HttpRequest request, int maxReadSize = 1024 * 5)
        {
            string requestBody;

            using (StreamReader reader = new StreamReader(stream: request.Body,
                detectEncodingFromByteOrderMarks: true,
                leaveOpen: true))
            {
                requestBody = await reader.ReadToEndAsync();
            }

            return requestBody;
        }
    }
}
