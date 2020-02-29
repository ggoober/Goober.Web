using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace Goober.WebApi.LoggingMiddleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate next;

        public LoggingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            string headers = string.Empty;
            foreach (var key in context.Request.Headers.Keys)
                headers += key + "=" + context.Request.Headers[key] + Environment.NewLine;

            context.Items["CONTEXT_USER_IDENTITY_NAME"] = context.User?.Identity?.Name;
            context.Items["REQUEST_HEADERS"] = headers;
            context.Items["REQUEST_BODY"] = context?.Request?.Body?.ToString();

            await next(context);
        }
    }
}
