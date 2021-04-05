using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Goober.Core.Extensions;
using Goober.Http.Extensions;

namespace Goober.WebApi.Example
{
    public class Startup : Goober.WebApi.BaseStartup
    {
        public Startup(IConfiguration config) : base(config)
        {
            SwaggerXmlCommentsFileNameList.Add("WebApi.Example.xml");
            UseSwaggerHideDocsFilter = true;
        }

        protected override void ConfigurePipelineAfterExceptionsHandling(IApplicationBuilder app)
        {
        }

        protected override void ConfigurePipelineAfterMvc(IApplicationBuilder app)
        {
        }

        protected override void ConfigureServiceCollections(IServiceCollection services)
        {
            services.AddGooberHttpServices();
            services.RegisterAssemblyClasses<Startup>();
        }
    }
}
