using System.Linq;
using System.Text.Json.Serialization;
using Goober.Core.Extensions;
using Goober.WebApi.Extensions;
using Goober.WebApi.ModelBinder;
using Goober.Config.Api;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Goober.WebApi.Models;
using Goober.Http.Extensions;

namespace Goober.WebApi
{
    public abstract class BaseStartup
    {
        #region props

        protected IConfiguration Configuration { get; private set; }

        private BaseStartupSwaggerSettings SwaggerSettings { get; set; } = new BaseStartupSwaggerSettings {
            UseHideInDocsFilter = false
        };

        private BaseStartupConfigSettings ConfigSettings { get; set; } = new BaseStartupConfigSettings 
        { 
            AppSettingsFileName = "appsettings.json", 
            IsAppSettingsFileOptional = false
        };

        #endregion

        #region ctor

        public BaseStartup(IConfiguration notUsingConfiguration)
        {
            Init(swaggerSettings: null, configSettings: null);
        }

        public BaseStartup(BaseStartupSwaggerSettings swaggerSettings = null, 
            BaseStartupConfigSettings configSettings = null)
        {
            Init(swaggerSettings, configSettings);
        }

        private IConfigurationBuilder Init(BaseStartupSwaggerSettings swaggerSettings, BaseStartupConfigSettings configSettings)
        {
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();

            if (configSettings != null)
            {
                ConfigSettings = configSettings;
            }

            if (swaggerSettings != null)
            {
                SwaggerSettings = swaggerSettings;
            }

            if (ConfigSettings != null)
            {
                if (ConfigSettings.ConfigApiEnvironmentAndHostMappings != null
                    && ConfigSettings.ConfigApiEnvironmentAndHostMappings.Any() == true)
                {
                    configurationBuilder = configurationBuilder.AddApiConfiguration(environmentConfigApiSchemeAndHosts: ConfigSettings.ConfigApiEnvironmentAndHostMappings);
                }

                if (string.IsNullOrEmpty(ConfigSettings.AppSettingsFileName) == false)
                {
                    configurationBuilder = configurationBuilder.AddJsonFile(ConfigSettings.AppSettingsFileName, optional: ConfigSettings.IsAppSettingsFileOptional);
                }
            }
            else
            {
                configurationBuilder = configurationBuilder.AddJsonFile("appsettings.json", optional: false);
            }

            Configuration = configurationBuilder.Build();
            return configurationBuilder;
        }

        #endregion

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGooberDateTimeService();
            services.AddGooberCaching();
            services.AddSingleton(Configuration);
            services.AddGooberHttpServices(); ;

            if (SwaggerSettings != null)
            {
                if (SwaggerSettings.XmlCommentsFileNameList != null 
                    && SwaggerSettings.XmlCommentsFileNameList.Any() == true)
                {
                    services.AddSwaggerGenWithXmlDocs(SwaggerSettings.XmlCommentsFileNameList, SwaggerSettings.UseHideInDocsFilter, SwaggerSettings.OpenApiInfo);
                }
                else
                {
                    services.AddSwaggerGenWithDocs(SwaggerSettings.UseHideInDocsFilter, SwaggerSettings.OpenApiInfo);
                }
            }

            services
                .AddControllers(o =>
                {
                    o.ModelBinderProviders.Insert(0, new DateCultureIsoModelBinderProvider());
                })
                .AddJsonOptions(o =>
                {
                    o.JsonSerializerOptions.IgnoreReadOnlyProperties = true;
                    o.JsonSerializerOptions.PropertyNameCaseInsensitive = false;
                    o.JsonSerializerOptions.IgnoreNullValues = true;
                    o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

            ConfigureServiceCollections(services);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseGooberExceptionsHandling();

            app.UseGooberLoggingVariables();

            app.UseRequestLocalizationByDefault();

            ConfigurePipelineAfterExceptionsHandling(app);

            app.UseSwagger();

            app.UseSwaggerUIWithDocs();

            app.UseRouting();

            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            ConfigurePipelineAfterMvc(app);
        }

        protected abstract void ConfigureServiceCollections(IServiceCollection services);

        protected abstract void ConfigurePipelineAfterExceptionsHandling(IApplicationBuilder app);

        protected abstract void ConfigurePipelineAfterMvc(IApplicationBuilder app);
    }
}
