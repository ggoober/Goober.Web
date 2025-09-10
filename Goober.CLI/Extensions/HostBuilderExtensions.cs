using Goober.CLI.Services;
using Goober.CLI.Services.Implementation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Goober.CLI.Extensions
{
    public static class HostBuilderExtensions
    {
        private const string ConfigureServicesMethodName = "ConfigureServices";

        public static IHostBuilder UseStartup<TStartup>(
            this IHostBuilder hostBuilder
        ) where TStartup : class
        {
            hostBuilder.ConfigureServices((context, serviceCollection) =>
            {
                var configServicesMethod = typeof(TStartup)
                    .GetMethod(
                        ConfigureServicesMethodName,
                        new Type[] { typeof(IServiceCollection) }
                     );

                if (configServicesMethod is null)
                    throw new InvalidOperationException(
                        $"Type {typeof(TStartup).FullName} is not contains method " +
                        $"\"{ConfigureServicesMethodName}\" with parameter " +
                        $"\"{typeof(IServiceCollection).FullName}\"");

                TStartup startupInstance = null;

                var logCtor = typeof(TStartup)
                    .GetConstructor(new Type[] { typeof(ILogger<TStartup>) });
                if (logCtor is not null)
                {
                    var provider = serviceCollection.BuildServiceProvider();
                    var logger = provider.GetService<ILogger<TStartup>>();
                    startupInstance = Activator.CreateInstance(typeof(TStartup), logger) as TStartup;
                }

                if (startupInstance is null)
                {
                    var configCtor = typeof(TStartup)
                        .GetConstructor(new Type[] { typeof(IConfiguration) });
                    if (configCtor != null)
                        startupInstance = Activator.CreateInstance(typeof(TStartup), configCtor) as TStartup;
                }

                if (startupInstance is null)
                {
                    startupInstance = Activator.CreateInstance(typeof(TStartup), null) as TStartup;
                }

                configServicesMethod?.Invoke(startupInstance, new object[] { serviceCollection });
            });

            return hostBuilder;
        }

        public static IHostBuilder WithArgs(this IHostBuilder hostBuilder, params string[] args)
        {
            hostBuilder.ConfigureServices((context, serviceCollection) =>
            {
                serviceCollection.AddSingleton<IArgsContextService, ArgsContextService>(x =>
                    new ArgsContextService(args));
            });

            return hostBuilder;
        }

        public static bool RunMain(this IHost host)
        {
            return host.RunMainAsync().GetAwaiter().GetResult();
        }

        public static async Task<bool> RunMainAsync(this IHost host, CancellationToken token = default)
        {
            try
            {
                await host.StartAsync(token).ConfigureAwait(false);

                await host.WaitForShutdownAsync(token).ConfigureAwait(false);

                var mainService = host.Services.GetService<IMainService>();
                var result = mainService?.IsTerminated;

                return result ?? true;
            }
            finally
            {
                if (host is IAsyncDisposable asyncDisposable)
                {
                    await asyncDisposable.DisposeAsync().ConfigureAwait(false);
                }
                else
                {
                    host.Dispose();
                }
            }
        }
    }
}
