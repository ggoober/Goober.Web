
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Goober.Core.Extensions;
using Goober.Web.Glossary;
using Microsoft.AspNetCore.Routing;

namespace Goober.WebApi.Example
{
    public class Startup : Goober.Web.BaseStartup
    {
        public Startup() 
            :
            base(
                configSettings:
                    new Goober.Web.Models.BaseStartupConfigSettings
                    {
                        ConfigApiEnvironmentAndHostMappings = null
                    })
        {
           
        }
        
        protected override void ConfigurePipelineAfterExceptionsHandling(IApplicationBuilder app)
        {
        }

        protected override void ConfigurePipelineAfterMvc(IApplicationBuilder app)
        {
        }

        protected override void ConfigureServiceCollections(IServiceCollection services)
        {
            services.RegisterAssemblyClasses<Startup>();
        }

        protected override void MapControllerRoutes(IEndpointRouteBuilder endpoints)
        {
        }
    }
}
