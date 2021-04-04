﻿using Goober.Core.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Goober.WebApi.LoggingMiddleware
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
                var message = $"Exception: {ex.Message}";
                if (ex.Message != originalExc?.Message)
                {
                    message = $"Exception: {ex.Message}, BaseExceptionMessage: {originalExc.Message}";
                }
                _logger.LogError(exception: ex, message: message);
                
                await SetErrorResponseAsync(context, ex.Message);
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
