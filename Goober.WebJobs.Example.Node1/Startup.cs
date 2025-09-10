using Indusoft.DependencyInjection.Extensions;
using Indusoft.Web;
using Indusoft.Web.Models;
using Indusoft.WebJobs.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using Indusoft.DependencyInjection;
using Indusoft.WebJobs.Api.Services;

namespace Indusoft.WebJobs.Example
{
	public class Startup : BaseStartup
    {
        public Startup()
	        : base(
		        configSettings: new BaseStartupConfigSettings
		        {
			        AppSettingsFileName = "./config/webjobs-example-node1-appsettings.json",
			        ConfigApiEnvironmentAndHostMappings = new Dictionary<string, string>(),
			        IsAppSettingsFileOptional = false
		        },
		        swaggerSettings: null,
		        memoryCacheSizeLimitInMB: null)
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
	        services.ConfigureWebJobs();
	        services.AddWebJobs<Startup>();
            services.RegisterAssemblyClasses<Startup>();
            //services.FinishConfigure();
            //var s = ServiceProviderStatic.GetRequiredService<IWebJobsHttpService>();
        }

        protected override void ConfigurePipelineBeforeRouting(IApplicationBuilder app)
        {
	        
        }

        protected override void MapControllerRoutes(IEndpointRouteBuilder endpoints)
        {
        }
    }
}
