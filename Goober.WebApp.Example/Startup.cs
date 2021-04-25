using Goober.Web;
using Goober.Web.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Goober.WebApp.Example
{
    public class Startup : BaseApiStartup
    {
        public Startup()
        {
        }

        public Startup(BaseStartupSwaggerSettings swaggerSettings = null, BaseStartupConfigSettings configSettings = null, int? memoryCacheSizeLimitInMB = null) : base(swaggerSettings, configSettings, memoryCacheSizeLimitInMB)
        {
        }

        protected override void ConfigurePipelineAfterExceptionsHandling(IApplicationBuilder app)
        {
            throw new NotImplementedException();
        }

        protected override void ConfigurePipelineAfterMvc(IApplicationBuilder app)
        {
            throw new NotImplementedException();
        }

        protected override void ConfigureRoutes(IRouteBuilder routes)
        {
            throw new NotImplementedException();
        }

        protected override void ConfigureServiceCollections(IServiceCollection services)
        {
            throw new NotImplementedException();
        }
    }
}
