
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Goober.Core.Extensions;

namespace Goober.WebApi.Example
{
    public class Startup : Goober.Web.BaseApiStartup
    {
        public Startup() 
            : 
            base(
                configSettings: 
                    new Goober.Web.Models.BaseStartupConfigSettings { 
                        ConfigApiEnvironmentAndHostMappings = new System.Collections.Generic.Dictionary<string, string> { 
                            { "Production", "http://localhost:55260/" } 
                        }
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
    }
}
