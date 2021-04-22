using Goober.Core.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Goober.Web.LoggingMiddleware
{
    public class ErrorHandlingMiddleware
    {
        private const string ApplicationJsonContentType = "application/json";
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = loggerFactory?.CreateLogger<ErrorHandlingMiddleware>() ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var originalExc = ex.GetBaseException();
                var message = $"{ex.GetType().Name}: {ex.Message}";
                if (ex.Message != originalExc?.Message)
                {
                    message = $"{ex.GetType().Name}: {ex.Message}, Base {originalExc.GetType().Name}: {originalExc.Message}";
                }

                context.Items["CONTEXT_RESPONSE_STATUSCODE"] = (int)HttpStatusCode.InternalServerError;

                _logger.LogError(exception: ex, message: message);
                
                await SetErrorResponseAsync(context, message);
            }
        }

        private static async Task SetErrorResponseAsync(HttpContext context, string message)
        {
            var result = new { error = message }.Serialize();
            context.Response.ContentType = ApplicationJsonContentType;
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await context.Response.WriteAsync(result);
        }
    }
}
