using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Indusoft.Web.VueJs.Example
{
    public class Startup : BaseStartup
    {
        public Startup()
            : base(
                configSettings:
                    new Indusoft.Web.Models.BaseStartupConfigSettings
                    {
                        ConfigApiEnvironmentAndHostMappings = new System.Collections.Generic.Dictionary<string, string>()
                    })
        {
        }

        protected override void ConfigurePipelineAfterRouting(IApplicationBuilder app)
        {
        }

        protected override void ConfigurePipelineAfterMvc(IApplicationBuilder app)
        {
            
        }

        protected override void ConfigureServiceCollections(IServiceCollection services)
        {
        }

        protected override void MapControllerRoutes(IEndpointRouteBuilder endpoints)
        {
        }

        protected override void ConfigurePipelineBeforeRouting(IApplicationBuilder app)
        {
            app.UsePathBase(new PathString("/example-section"));
            app.UseStaticFiles("/example-section");
        }
    }
}
