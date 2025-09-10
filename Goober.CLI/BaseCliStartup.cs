using Goober.Base.Extensions;
using Goober.Caching;
using Goober.CLI.Abstractions.Glossary;
using Goober.CLI.Abstractions.Models;
using Goober.CLI.Services;
using Goober.CLI.Services.Implementation;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace Goober.CLI
{
    public abstract class BaseCliStartup
    {
        private readonly long? _memoryCacheSizeLimitInBytes = null;

        private BaseCLIStartupConfigSettings _configSettings { get; set; } = new BaseCLIStartupConfigSettings
        {
            AppSettingsFileName = ConfigGlossary.AppSettingsFileName,
            IsAppSettingsFileOptional = false,
            CacheExpirationTimeInMinutes = null,
            CacheRefreshTimeInMinutes = 5,
            ConfigApiEnvironmentAndHostMappings = ConfigGlossary.ConfigApiEnvironmentAndHostMappings
        };

        public BaseCliStartup()
        {
        }

        public BaseCliStartup(
            BaseCLIStartupConfigSettings? configSettings = null,
            int? memoryCacheSizeLimitInMB = null
        )
        {
            if (configSettings != null)
            {
                _configSettings = configSettings;
            }

            _memoryCacheSizeLimitInBytes = memoryCacheSizeLimitInMB * 1024;
        }

        public IConfiguration Configuration { get; private set; }

        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureBaseService(services);
            ConfigureConfiguration(services);
            ConfigureServiceCollections(services);
            ConfigureMainService(services);
        }

        public virtual void Configure(IApplicationBuilder app)
        {
        }

        protected virtual void ConfigureBaseService(IServiceCollection services)
        {
            services.AddIndusoftDateTimeService();
            services.AddIndusoftCaching(memoryCacheSizeLimitInBytes: _memoryCacheSizeLimitInBytes);
        }

        [MemberNotNull(nameof(Configuration))]
        protected virtual void ConfigureConfiguration(IServiceCollection services)
        {
            Configuration = GenerateConfiguration(
                serviceCollection: services,
                appSettingsFileName: _configSettings?.AppSettingsFileName ?? ConfigGlossary.AppSettingsFileName,
                isAppSettingsFileOptional: _configSettings?.IsAppSettingsFileOptional ?? ConfigGlossary.IsAppSettingsFileOptional
            );

            services.AddSingleton(Configuration);
        }

        protected virtual void ConfigureMainService(IServiceCollection services)
        {
            services.AddSingleton<IMainService, MainService>(x => new MainService(MainAsync));
            services.AddScoped<IHostedService, AfterStartHostService>();
        }

        protected abstract void ConfigureServiceCollections(IServiceCollection services);
        protected abstract Task<bool> MainAsync(string[] args, IServiceProvider serviceProvider, CancellationToken cancellationToken);

        private static IConfiguration GenerateConfiguration(IServiceCollection serviceCollection,
                string appSettingsFileName,
                bool isAppSettingsFileOptional
        )
        {
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();

            configurationBuilder = configurationBuilder
                .AddJsonFile(appSettingsFileName, optional: isAppSettingsFileOptional)
                .AddEnvironmentVariables()
                .AddCommandLine(Environment.GetCommandLineArgs());

            return configurationBuilder.Build();
        }
    }
}
