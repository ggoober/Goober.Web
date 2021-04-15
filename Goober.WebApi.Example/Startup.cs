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
        public Startup(IConfiguration config, IServiceProvider serviceProvider) 
            : 
            base(swaggerSettings: null,
                configSettings: 
                    new WebApi.Models.BaseStartupConfigSettings { 
                        ConfigApiEnvironmentAndHostMappings = new System.Collections.Generic.Dictionary<string, string> { 
                            { "Production", "http://localhost:55260/" } 
                        }
                    }
                )
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddHttpClient();
            var sp = serviceCollection.BuildServiceProvider();
            var factory = sp.GetRequiredService<IHttpClientFactory>();
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
            services.Configure<Controllers.Api.ExampleApiController.Doc>(Configuration.GetSection("doc"));
        }
    }
}
