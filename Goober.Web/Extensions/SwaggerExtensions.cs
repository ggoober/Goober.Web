﻿using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using System.IO;

namespace Goober.Web.Extensions
{
    static class SwaggerExtensions
    {
        public static void AddSwaggerGenWithDocs(this IServiceCollection service, bool useHideDocsFilter = false, OpenApiInfo info = null)
        {
            service.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", info ?? new OpenApiInfo { Title = "Web API", Version = "v1" });

                if (useHideDocsFilter == true)
                {
                    c.DocumentFilter<SwaggerHideInDocsFilter>();
                }
            });
        }

        public static void AddSwaggerGenWithXmlDocs(this IServiceCollection service, IList<string> xmlDocFileNameList, bool useHideDocsFilter = false, OpenApiInfo info = null)
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
            });
        }

        public static void UseSwaggerUIWithDocs(this IApplicationBuilder app)
        {
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", $"Web API v1");
            });
        }
    }
}
