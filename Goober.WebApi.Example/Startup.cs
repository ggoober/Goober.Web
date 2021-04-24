
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Goober.Core.Extensions;
using Goober.Web.Glossary;

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
                            { "Production",  ConfigGlossary.ProductionConfigApiSchemeAndHost },
                            { "Staging", ConfigGlossary.StagingConfigApiSchemeAndHost },
                            { "Development", "http://localhost:55260/" }
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
