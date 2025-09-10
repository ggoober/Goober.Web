using Anemonis.AspNetCore.RequestDecompression;
using Goober.Base.Extensions;
using Goober.Caching;
using Goober.Http;
using Goober.Http.Glossary;
using Goober.Http.Models;
using Goober.Http.Models.Parameters;
using Goober.Web.Extensions;
using Goober.Web.Glossary;
using Goober.Web.IdentityUtils.Exceptions;
using Goober.Web.ModelBinder;
using Goober.Web.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace Goober.Web
{
    public abstract partial class BaseStartup
    {
        #region props

        protected IConfiguration Configuration { get; private set; }

        /// <summary>
        /// Базовый путь.
        /// </summary>
        protected virtual string BasePath { get; private set; } = "";

        private BaseStartupSwaggerSettings _swaggerSettings { get; set; }

        private long? _memoryCacheSizeLimitInBytes = null;

        #endregion

        #region ctor

        public BaseStartup()
        {
        }

        public BaseStartup(
            BaseStartupSwaggerSettings swaggerSettings = null,
            int? memoryCacheSizeLimitInMB = null)
        {
            _swaggerSettings = swaggerSettings ?? new BaseStartupSwaggerSettings();

            _memoryCacheSizeLimitInBytes = memoryCacheSizeLimitInMB * 1024;
        }

        #endregion

        public virtual void ConfigureServices(IServiceCollection services)
        {
            ConfigureBeforeBaseService(services);

            VerifyLicense(services);
            services.AddIndusoftExpirationDate(_expireDate, ApplicationBaseConfiguration);

            ConfigureBaseService(services);

            ConfigureConfiguration(services);

            ConfigureIndusoftHttp(services);

            ConfigureSwagger(services);

            ConfigureModelBindings(services);

            ConfigureServiceCollections(services);

            var baseAuthPassword = Configuration.GetDecryptedString(CryptoExtensions.EncryptedPasswordConfigKey);

            Goober.Base.Attributes.SwaggerHideInDocsAttribute.DefaultPassword = baseAuthPassword;

            Goober.Web.Filters.BasicAuthAttribute.DefaultPassword = baseAuthPassword;

            ManageResponseCompression(services);

            ManageRequestDecompression(services);
        }

        private void ManageRequestDecompression(IServiceCollection services)
        {
            var needToAddRequestDecompression = Configuration.NeedToAddRequestDecompression();
            if (needToAddRequestDecompression)
            {
                services.AddRequestDecompression(o =>
                {
                    o.Providers.Add<DeflateDecompressionProvider>();
                    o.Providers.Add<GzipDecompressionProvider>();
                    o.Providers.Add<BrotliDecompressionProvider>();
                });
            }
        }

        private void ManageResponseCompression(IServiceCollection services)
        {
            var needToAddResponseCompression = Configuration.NeedToAddResponseCompression();
            if (needToAddResponseCompression == false)
                return;

            var enableForHttps = Configuration.NeedToUseResponseHttpsCompression();
            services.AddResponseCompression(options => options.EnableForHttps = enableForHttps);
        }

        protected virtual void ConfigureModelBindings(IServiceCollection services)
        {
            services
                .AddControllersWithViews(o =>
                {
                    o.ModelBinderProviders.Insert(0, new DateCultureIsoModelBinderProvider());
                })
                .AddJsonOptions(o =>
                {
                    o.JsonSerializerOptions.IgnoreReadOnlyProperties = true;
                    o.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                    o.JsonSerializerOptions.IgnoreNullValues = true;
                    o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });
        }

        protected virtual void ConfigureConfiguration(IServiceCollection services)
        {
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();

            configurationBuilder = configurationBuilder
                .AddJsonFile("appsettings.json", optional: false)
                .AddEnvironmentVariables()
                .AddCommandLine(Environment.GetCommandLineArgs());

            Configuration = configurationBuilder.Build();

            services.AddSingleton(Configuration);
        }

        protected virtual void ConfigureIndusoftHttp(IServiceCollection services)
        {
            var authorizationFunctions = GetAuthenticateFunctions();

            if ((authorizationFunctions?.Count ?? 0) == 0)
            {
                services.AddSingleton(typeof(IList<AuthenticationEndPointModel>), new List<AuthenticationEndPointModel>());

                return;
            }

            var authorizationEndPoints = GetAuthorizationEndPointsWithCheckForDuplicates();
            var configAuthorizations = authorizationEndPoints.ToDictionary(x => x.Alias.ToLowerAndTrimSafety());

            var resultAuthorizationEndpoints = GetIndusoftAuthorizationEndpoints(authorizationFunctions, configAuthorizations);

            services.AddSingleton(typeof(IList<AuthenticationEndPointModel>), resultAuthorizationEndpoints);
        }

        protected virtual Dictionary<string, EndpointRequestOptions> GetAuthenticateFunctions()
        {
            return new Dictionary<string, EndpointRequestOptions>();
        }

        protected virtual void ConfigureBeforeBaseService(IServiceCollection services)
        {
        }

        /// <summary>
        /// DateTimeSerivce, Caching, Http
        /// </summary>
        /// <param name="services"></param>
        protected virtual void ConfigureBaseService(IServiceCollection services)
        {
            services.AddIndusoftDateTimeService();
            services.AddIndusoftCaching(memoryCacheSizeLimitInBytes: _memoryCacheSizeLimitInBytes);
            services.AddIndusoftHttp();
        }

        protected virtual void ConfigureSwagger(IServiceCollection services)
        {
            if (_swaggerSettings.XmlCommentsFileNameList != null
                                && _swaggerSettings.XmlCommentsFileNameList.Any() == true)
            {
                services.AddSwaggerGenWithXmlDocs(xmlDocFileNameList: _swaggerSettings.XmlCommentsFileNameList,
                useHideDocsFilter: _swaggerSettings.UseHideInDocsFilter,
                info: _swaggerSettings.OpenApiInfo,
                optionsAction: ConfigureSwaggerOptions);
            }
            else
            {
                services.AddSwaggerGenWithDocs(useHideDocsFilter: _swaggerSettings.UseHideInDocsFilter,
                    info: _swaggerSettings.OpenApiInfo,
                    optionsAction: ConfigureSwaggerOptions);
            }
        }

        public virtual void Configure(IApplicationBuilder app)
        {
            var addResponseCompression = Configuration.NeedToAddResponseCompression();
            if (addResponseCompression)
            {
                app.UseResponseCompression();
            }

            var needToAddRequestDecompression = Configuration.NeedToAddRequestDecompression();
            if (needToAddRequestDecompression)
            {
                app.UseRequestDecompression();
            }

            ConfigureStartPipeline(app);

            ConfigurePipelineBeforeRouting(app);

            ConfigureRouting(app);

            ConfigurePipelineAfterRouting(app);

            ConfigureMvcPipeline(app);

            ConfigurePipelineAfterMvc(app);
        }

        protected virtual void ConfigureMvcPipeline(IApplicationBuilder app)
        {
            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}");

                MapControllerRoutes(endpoints);

                endpoints.MapControllers();
            });
        }

        protected virtual void ConfigureRouting(IApplicationBuilder app)
        {
            app.UseRouting();
        }

        protected virtual void ConfigureStartPipeline(IApplicationBuilder app)
        {
            app.UseIndusoftExceptionsHandling();

            app.UseIndusoftLoggingVariables();

            app.UseRequestLocalizationByDefault();

            app.UseSwaggerWithBasePath(BasePath);
        }

        protected abstract void ConfigurePipelineBeforeRouting(IApplicationBuilder app);

        protected abstract void MapControllerRoutes(IEndpointRouteBuilder endpoints);

        protected abstract void ConfigureServiceCollections(IServiceCollection services);

        protected abstract void ConfigurePipelineAfterRouting(IApplicationBuilder app);

        protected abstract void ConfigurePipelineAfterMvc(IApplicationBuilder app);

        protected virtual void ConfigureSwaggerOptions(SwaggerGenOptions swaggerOptions)
        {
            //do nothing
        }

        private void VerifyLicense(IServiceCollection services)
        {
            try
            {
                LicenseValidation();
            }
            catch (IndusoftProductException exception)
            {
                var provider = services.BuildServiceProvider();
                var logger = provider.GetService<ILogger<IndusoftProductException>>();
                logger?.LogError(exception.Message);

                throw exception;
            }
        }

        private static List<AuthenticationEndPointModel> GetIndusoftAuthorizationEndpoints(
                Dictionary<string, EndpointRequestOptions> authorizationFunctions,
                Dictionary<string, IndusoftAuthorizationEndPointParameters> configAuthorizations)
        {
            var resultAuthorizationEndpoints = new List<AuthenticationEndPointModel>();

            var missingConfigs = new List<string>();

            foreach (var iAuthFunction in authorizationFunctions)
            {
                var authAlias = iAuthFunction.Key.ToLowerAndTrimSafety();

                if (configAuthorizations.TryGetValue(authAlias, out var configAuth) == false)
                {
                    missingConfigs.Add(authAlias);
                    continue;
                }

                var newRecord = new AuthenticationEndPointModel
                {
                    AuthenticateAsync = iAuthFunction.Value?.AuthenticateAsync,
                    EndPointAlias = authAlias,
                    EndPointSchemeAndHost = configAuth.SchemeAndHost,
                    BeforeHttpRequestSendAsync = iAuthFunction.Value?.BeforeHttpRequestSendAsync,
                    AfterHttpResponseRecivedAsync = iAuthFunction.Value?.AfterHttpResponseRecivedAsync,
                    RetryCount = configAuth.RetryCount,
                    RetryPeriod = configAuth.RetryPeriod
                };

                resultAuthorizationEndpoints.Add(newRecord);
            }

            if (missingConfigs.Count > 0)
            {
                throw new InvalidOperationException($"Can't find authorization endpoint configs for alias: {string.Join(", ", missingConfigs)}");
            }

            return resultAuthorizationEndpoints;
        }

        List<IndusoftAuthorizationEndPointParameters> GetAuthorizationEndPointsWithCheckForDuplicates()
        {
            var httpParameters = Configuration.GetSettingValueByKeyOrDefault<IndusoftHttpParameters>(ConfigurationStatic.IndusoftHttpConfigSectionKey, null);
            if (httpParameters is null)
                return new();

            var duplicates = httpParameters.AuthorizationEndPoints
                                           .GroupBy(p => p.Alias)
                                           .Where(g => g.Count() > 1)
                                           .Select(g => g.Key);

            if (duplicates.Any())
                throw new InvalidOperationException($"Имена псевдонимов '{string.Join("', '", duplicates)}' не уникальны. Проверьте содержимое секции '{ConfigurationStatic.IndusoftHttpConfigSectionKey}' файла конфигурации.");

            return httpParameters.AuthorizationEndPoints;
        }
    }
}
