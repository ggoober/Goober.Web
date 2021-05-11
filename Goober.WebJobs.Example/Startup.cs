using Goober.WebJobs.Extensions;
using Goober.Web;
using Goober.Web.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Goober.Core.Extensions;
using Microsoft.AspNetCore.Routing;

namespace Goober.WebJobs.Example
{
    public class Startup : BaseStartup
    {
        public Startup()
            : base(configSettings:
                    new BaseStartupConfigSettings
                    {
                        ConfigApiEnvironmentAndHostMappings = null,
                        IsAppSettingsFileOptional = false
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
            services.AddWebJobs<Goober.WebJobs.Example.Startup>();
            services.RegisterAssemblyClasses<Goober.WebJobs.Example.Startup>();
        }

        protected override void MapControllerRoutes(IEndpointRouteBuilder endpoints)
        {
        }
    }
}
