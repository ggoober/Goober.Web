using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Goober.Core.Extensions;
using Goober.Http.Extensions;
using System;
using System.Net.Http;

namespace Goober.WebApi.Example
{
    public class Startup : Goober.WebApi.BaseStartup
    {
        public Startup(IServiceProvider serviceProvider) 
            : 
            base(swaggerSettings: null,
                configSettings: 
                    new WebApi.Models.BaseStartupConfigSettings { 
                        ConfigApiEnvironmentAndHostMappings = new System.Collections.Generic.Dictionary<string, string> { 
                            { "Production", "http://localhost:55260/" } 
                        }
                    },
                memoryCacheSizeLimitInMB: null)
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
            //services.Configure<Controllers.Api.ExampleApiController.Doc>(Configuration.GetSection("doc"));
        }
    }
}
