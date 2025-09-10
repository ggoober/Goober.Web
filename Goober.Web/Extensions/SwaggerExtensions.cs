using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.IO;

namespace Goober.Web.Extensions
{
    public static class SwaggerExtensions
    {
        public static void AddSwaggerGenWithDocs(this IServiceCollection service, bool useHideDocsFilter = false, OpenApiInfo info = null, Action<SwaggerGenOptions> optionsAction = null)
        {
            service.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", info ?? new OpenApiInfo { Title = "Web API", Version = "v1" });

                if (useHideDocsFilter == true)
                {
                    c.DocumentFilter<SwaggerHideInDocsFilter>();
                }

                if (optionsAction != null)
                {
                    optionsAction(c);
                }
            });
        }

        public static void AddSwaggerGenWithXmlDocs(this IServiceCollection service, IList<string> xmlDocFileNameList, bool useHideDocsFilter = false, OpenApiInfo info = null, Action<SwaggerGenOptions> optionsAction = null)
        {
            service.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", info ?? new OpenApiInfo { Title = "Web API", Version = "v1" });

                if (useHideDocsFilter == true)
                {
                    c.DocumentFilter<SwaggerHideInDocsFilter>();
                }

                var applicationPath = PlatformServices.Default.Application.ApplicationBasePath;
                foreach (var iFileName in xmlDocFileNameList)
                {
                    var xmlPath = Path.Combine(applicationPath, iFileName);
                    c.IncludeXmlComments(xmlPath);
                }

                if (optionsAction != null)
                {
                    optionsAction(c);
                }
            });
        }

        public static void UseSwaggerUIWithDocs(this IApplicationBuilder app, string basePath = "")
        {
            var normalizedBasePath = NormalizeBasePath(basePath);

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"{normalizedBasePath}swagger/v1/swagger.json", "Web API v1");
                c.RoutePrefix = $"{normalizedBasePath.TrimStart('/')}swagger";
            });
        }

        /// <summary>
        /// Включает и настраивает Swagger с учетом базового пути приложения.
        /// Устанавливает маршрут для Swagger JSON, учитывая указанный базовый путь, 
        /// и инициализирует Swagger UI с документацией по этому же пути.
        /// </summary>
        /// <param name="app">Экземпляр IApplicationBuilder для конфигурирования конвейера приложения.</param>
        /// <param name="basePath">Базовый путь приложения (по умолчанию — пустая строка).</param>
        public static void UseSwaggerWithBasePath(this IApplicationBuilder app, string basePath = "")
        {
            var normalizedBasePath = NormalizeBasePath(basePath);

            app.UseSwagger(c =>
            {
                c.RouteTemplate = $"{normalizedBasePath}swagger/{{documentName}}/swagger.json";
            });

            app.UseSwaggerUIWithDocs(normalizedBasePath);
        }

        private static string NormalizeBasePath(string basePath)
        {
            var trimmed = (string.IsNullOrWhiteSpace(basePath) ? "" : basePath).Trim('/');
            return trimmed.Length == 0 ? "/" : $"/{trimmed}/";
        }
    }
}
