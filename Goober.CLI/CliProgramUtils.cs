using Goober.CLI.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Reflection;

namespace Goober.CLI;

public static class CliProgramUtils
{
    public static string ApplicationName { get; private set; }
    public static string AssemblyVersion { get; private set; }

    public static bool RunCliHost<TStartup>(
        string[] args,
        string nlogConfigFileName = "nlog.config",
        Action<HostBuilderContext, IConfigurationBuilder>? configDelegate = null
    ) where TStartup : class
    {
        ApplicationName = Assembly.GetEntryAssembly()?.GetName().Name ?? string.Empty;
        AssemblyVersion = Assembly.GetEntryAssembly()?.GetName().Version?.ToString() ?? string.Empty;

        var logger = NLog.Web.NLogBuilder
            .ConfigureNLog(nlogConfigFileName)
            .GetCurrentClassLogger();

        bool isTerminatedCommand = false;
        try
        {
            var host = Host.CreateDefaultBuilder()
                .UseIndusoftCliLogging()
                .ConfigureAppConfiguration((context, builder) =>
                    configDelegate?.Invoke(context, builder)
                )
                .UseStartup<TStartup>()
                .WithArgs(args)
                .Build();

            isTerminatedCommand = host.RunMain();
        }
        catch (Exception ex)
        {
            logger.Error(ex, "Stopped cli program because of exception");
            throw;
        }
        finally
        {
            NLog.LogManager.Shutdown();
        }

        return isTerminatedCommand;
    }
}
