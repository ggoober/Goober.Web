using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Goober.Web.VueJs.Example
{
    public class Startup : BaseStartup
    {
        public Startup()
            : base(
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
        }

        protected override void MapControllerRoutes(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapControllerRoute("default",
                                     "{controller=Home}/{action=Index}");
        }
    }
}
