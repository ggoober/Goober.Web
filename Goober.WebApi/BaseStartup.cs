using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json.Serialization;
using Goober.Core.Extensions;
using Goober.WebApi.Extensions;
using Goober.WebApi.ModelBinder;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace Goober.WebApi
{
    public abstract class BaseStartup
    {
        #region props

        protected IConfiguration Configuration { get; private set; }

        protected List<string> SwaggerXmlCommentsFileNameList { get; set; } = new List<string>();

        protected OpenApiInfo SwaggerInfo { get; set; }

        protected bool UseSwaggerHideDocsFilter { get; set; }

        #endregion

        #region ctor

        public BaseStartup(IConfiguration config)
        {
            Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true)
                .Build();
        }

        #endregion

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGooberDateTimeService();
            services.AddGooberCaching();
            services.AddSingleton(Configuration);

            if (SwaggerXmlCommentsFileNameList != null && SwaggerXmlCommentsFileNameList.Any())
            {
                services.AddSwaggerGenWithXmlDocs(SwaggerXmlCommentsFileNameList, UseSwaggerHideDocsFilter, SwaggerInfo);
            }
            else
            {
                services.AddSwaggerGenWithDocs(UseSwaggerHideDocsFilter, SwaggerInfo);
            }

            services
                .AddControllers(o =>
                {
                    o.ModelBinderProviders.Insert(0, new DateCultureIsoModelBinderProvider());
                })
                .AddJsonOptions(o =>
                {
                    o.JsonSerializerOptions.IgnoreReadOnlyProperties = true;
                    o.JsonSerializerOptions.PropertyNameCaseInsensitive = false;
                    o.JsonSerializerOptions.IgnoreNullValues = true;
                    o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });


            ConfigureServiceCollections(services);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseGooberLoggingVariables();

            app.UseGooberExceptionsHandling();

            app.UseRequestLocalizationByDefault();

            ConfigurePipelineAfterExceptionsHandling(app);

            app.UseSwagger();

            app.UseSwaggerUIWithDocs();

            app.UseRouting();

            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            ConfigurePipelineAfterMvc(app);
        }

        protected abstract void ConfigureServiceCollections(IServiceCollection services);

        protected abstract void ConfigurePipelineAfterExceptionsHandling(IApplicationBuilder app);

        protected abstract void ConfigurePipelineAfterMvc(IApplicationBuilder app);
    }
}
