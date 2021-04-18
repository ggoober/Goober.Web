using System.Linq;
using System.Text.Json.Serialization;
using Goober.Core.Extensions;
using Goober.WebApi.Extensions;
using Goober.WebApi.ModelBinder;
using Goober.Config.Api;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Goober.WebApi.Models;
using Goober.Http.Extensions;
using System;

namespace Goober.WebApi
{
    public abstract class BaseStartup
    {
        #region props

        protected IConfiguration Configuration { get; private set; }

        private BaseStartupSwaggerSettings _swaggerSettings { get; set; } = new BaseStartupSwaggerSettings {
            UseHideInDocsFilter = false
        };

        private BaseStartupConfigSettings _configSettings { get; set; } = new BaseStartupConfigSettings 
        { 
            AppSettingsFileName = "appsettings.json", 
            IsAppSettingsFileOptional = false
        };

        private long? _memoryCacheSizeLimitInBytes = null;

        #endregion

        #region ctor

        public BaseStartup()
        { 
        }

        public BaseStartup(BaseStartupSwaggerSettings swaggerSettings, BaseStartupConfigSettings configSettings, int? memoryCacheSizeLimitInMB)
        {
            if (swaggerSettings != null)
            {
                _swaggerSettings = swaggerSettings;
            }

            if (configSettings != null)
            {
                _configSettings = configSettings;
            }

            _memoryCacheSizeLimitInBytes = memoryCacheSizeLimitInMB * 1024;
        }

        #endregion

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGooberDateTimeService();
            services.AddGooberCaching(memoryCacheSizeLimitInBytes: _memoryCacheSizeLimitInBytes);
            services.AddGooberHttp();

            var serviceProvider = services.BuildServiceProvider();
            Configuration = GenerateConfiguration(configSettings: _configSettings, serviceProvider: serviceProvider);

            services.AddSingleton(Configuration);

            if (_swaggerSettings != null)
            {
                ConfigureSwagger(services);
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

        private void ConfigureSwagger(IServiceCollection services)
        {
            if (_swaggerSettings.XmlCommentsFileNameList != null
                                && _swaggerSettings.XmlCommentsFileNameList.Any() == true)
            {
                services.AddSwaggerGenWithXmlDocs(_swaggerSettings.XmlCommentsFileNameList, _swaggerSettings.UseHideInDocsFilter, _swaggerSettings.OpenApiInfo);
            }
            else
            {
                services.AddSwaggerGenWithDocs(_swaggerSettings.UseHideInDocsFilter, _swaggerSettings.OpenApiInfo);
            }
        }

        public void Configure(IApplicationBuilder app)
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

        private static IConfiguration GenerateConfiguration(BaseStartupConfigSettings configSettings,
            IServiceProvider serviceProvider)
        {
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();

            if (configSettings != null)
            {
                if (configSettings.ConfigApiEnvironmentAndHostMappings != null
                    && configSettings.ConfigApiEnvironmentAndHostMappings.Any() == true)
                {
                    configurationBuilder = configurationBuilder.AddApiConfiguration(
                        environmentConfigApiSchemeAndHosts: configSettings.ConfigApiEnvironmentAndHostMappings,
                        serviceProvider: serviceProvider);
                }

                if (string.IsNullOrEmpty(configSettings.AppSettingsFileName) == false)
                {
                    configurationBuilder = configurationBuilder.AddJsonFile(configSettings.AppSettingsFileName, optional: configSettings.IsAppSettingsFileOptional);
                }
            }
            else
            {
                configurationBuilder = configurationBuilder.AddJsonFile("appsettings.json", optional: false);
            }

            return configurationBuilder.Build();
        }
    }
}
