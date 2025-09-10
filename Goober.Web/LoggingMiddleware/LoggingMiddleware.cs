using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Goober.Base.Extensions;

namespace Goober.Web.LoggingMiddleware
{
    public class LoggingMiddleware
    {
		public static int MaxContentLength { get; set; } = 50 * 1024 * 1024;

        private readonly RequestDelegate _next;

        private const string CallSequenceIdKey = "i-callsec-id";

		private const string ContextRequestBody = "CONTEXT_REQUEST_BODY";
		private const string ContextRequestForm = "CONTEXT_REQUEST_FORM";

        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task Invoke(HttpContext context)
        {
            var request = context?.Request;
            if (request == null)
                return;
            
            context.Items[CallSequenceIdKey] = GetCallSequenceIdFromRequestHeaderOrGenerateNew(context);

			request.EnableBuffering();

			context.Items[ContextRequestForm] = GetRequestForm(request);

			var requestBodyDelegate = context.RequestServices.GetService<LoggingRequestBodyAbstractDelegate>();
			if (requestBodyDelegate != null)
			{
				requestBodyDelegate.MaxContentLength = MaxContentLength;
				requestBodyDelegate.HttpContext = context;
				requestBodyDelegate.RequestBodyBytes = await GetRequestBodyBytesAsync(request);

				context.Items[ContextRequestBody] = requestBodyDelegate;
			}
			else
            {
				context.Items[ContextRequestBody] = await GetRequestBodyStringAsync(request);
            }

            await _next(context);
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

		private async Task<string> GetRequestBodyStringAsync(HttpRequest request)
        {
            if (request.Method.ToUpper() == "GET")
                return null;

            if (request.ContentLength > MaxContentLength)
                return $"RequestBody content length > {MaxContentLength}";

            string requestBody;
            
			request.Body.Position = 0;

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

		private async Task<byte[]> GetRequestBodyBytesAsync(HttpRequest request)
        {
			if (request.Method.ToUpper() == "GET")
				return null;

			if (request.ContentLength > MaxContentLength)
				return Encoding.UTF8.GetBytes($"RequestBody content length > {MaxContentLength}");

			request.Body.Position = 0;
			var readBodyResult = await request.Body.ReadStreamBytesWithMaxSizeRetrictionAsync(maxSize: MaxContentLength);
			request.Body.Position = 0;

			if (readBodyResult.IsReadToTheEnd == true)
            {
				return readBodyResult.Bytes;
            }

			var str = Encoding.UTF8.GetString(readBodyResult.Bytes);
			var retString = new StringBuilder(str);
			retString.AppendLine();
			retString.AppendLine($"<<< NOT END, request body is greter than {MaxContentLength}.");

			return Encoding.UTF8.GetBytes(retString.ToString());
		}

		private string GetRequestForm(HttpRequest request)
		{
			if (request.Method == "GET")
				return null;

			if (request.ContentType == null
				|| request.ContentType.Contains("form-data") == false)
				return null;

			var sb = new StringBuilder();

			if (request.Form.Files?.Count > 0)
			{
				var fileNames = request.Form.Files.Select(x => x.FileName).ToList();
				sb.AppendLine($"files: {string.Join(";", fileNames)}");

				return sb.ToString();
			}

			if (request.ContentLength > MaxContentLength)
				return $"RequestForm content length > {MaxContentLength}";

			request.EnableBuffering();

			request.Body.Position = 0;

			foreach (var iFormItem in request.Form)
			{
				sb.AppendLine($"{iFormItem.Key}:{iFormItem.Value}");
			}

			request.Body.Position = 0;

			return sb.ToString();
        }
    }
}
