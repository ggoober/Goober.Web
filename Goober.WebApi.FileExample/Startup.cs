using Goober.Web;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Goober.WebApi.FileExample
{
    public class Startup : BaseStartup
    {
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
        }
    }
}
