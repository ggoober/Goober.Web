using Goober.Core.Extensions;
using Goober.Http.Extensions;
using Goober.WebApp.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using System.Linq;

namespace Goober.WebApp
{
    public abstract class GooberStartup
    {
        #region props

        protected IConfiguration Configuration { get; private set; }

        protected List<string> SwaggerXmlCommentsFileNameList { get; set; } = new List<string>();

        protected OpenApiInfo SwaggerInfo { get; set; }

        #endregion

        #region ctor

        public GooberStartup(IConfiguration config)
        {
            Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
        }

        #endregion

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGooberDateTimeService();
            services.AddGooberCaching();
            services.AddGooberHttpHelper();

            services.AddSingleton(Configuration);

            if (SwaggerXmlCommentsFileNameList != null && SwaggerXmlCommentsFileNameList.Any())
            {
                services.AddSwaggerGenWithXmlDocs(SwaggerXmlCommentsFileNameList, SwaggerInfo);
            }
            else
            {
                services.AddSwaggerGenWithDocs(SwaggerInfo);
            }

            services.AddControllersWithViews();

            ConfigureServiceCollections(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseGooberLoggingVariables();

            app.UseRequestLocalizationByDefault();

            app.UseGooberExceptionsHandling();

            ConfigurePipelineAfterExceptionsHandling(app);

            app.UseSwagger();

            app.UseSwaggerUIWithDocs();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(ConfigureRoutes);

            ConfigurePipelineAfterMvc(app);
        }

        protected abstract void ConfigureServiceCollections(IServiceCollection services);

        protected abstract void ConfigurePipelineAfterExceptionsHandling(IApplicationBuilder app);

        protected abstract void ConfigurePipelineAfterMvc(IApplicationBuilder app);

        protected abstract void ConfigureRoutes(IEndpointRouteBuilder routes);
    }
}
