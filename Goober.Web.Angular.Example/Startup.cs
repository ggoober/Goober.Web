using Indusoft.Web.Angular.Example.Services;
using Indusoft.Web.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using Indusoft.DependencyInjection.Extensions;
using Indusoft.Web.GridView;

namespace Indusoft.Web.Angular.Example
{
    public class Startup: BaseStartup
    {
        public Startup()
            : base(configSettings:
                    new BaseStartupConfigSettings
                    {
                        AppSettingsFileName = "./config/appsettings.json",
                        ConfigApiEnvironmentAndHostMappings = new Dictionary<string, string>(),
                        IsAppSettingsFileOptional = false
                    },
                    swaggerSettings: null,
                    memoryCacheSizeLimitInMB: null)
        {
        }

        protected override void ConfigurePipelineAfterMvc(IApplicationBuilder app)
        {
        }

        protected override void ConfigurePipelineAfterRouting(IApplicationBuilder app)
        {
        }

        protected override void ConfigurePipelineBeforeRouting(IApplicationBuilder app)
        {
        }

        protected override void ConfigureServiceCollections(IServiceCollection services)
        {
            services.RegisterAssemblyClasses<DummyForRegisterAssemblyClasses>();
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddGridView();
        }

        protected override void MapControllerRoutes(IEndpointRouteBuilder endpoints)
        {
        }
    }
}
