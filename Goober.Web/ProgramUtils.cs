using Goober.Web.Extensions;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Hosting.WindowsServices;
using System.IO;
using Microsoft.Extensions.Hosting.WindowsServices;

namespace Goober.Web
{
    public static class ProgramUtils
    {
        public static string ApplicationName { get; private set; }

        public static string AssemblyVersion { get; private set; }

        static ProgramUtils()
        {
            ApplicationName = Assembly.GetEntryAssembly()?.GetName().Name ?? string.Empty;
            AssemblyVersion = Assembly.GetEntryAssembly()?.GetName().Version?.ToString() ?? string.Empty;
        }

        public static void RunWebhost<TStartup>(
            string[] args,
            string nlogConfigFileName = "nlog.config",
            Action<WebHostBuilderContext, IConfigurationBuilder> configDelegate = null,
            Action<KestrelServerOptions> configureKestrelDelegate = null)
            where TStartup : class
        {
            var logger = NLog.Web.NLogBuilder.ConfigureNLog(nlogConfigFileName).GetCurrentClassLogger();
            try
            {
                logger.Debug("Init main");
                var webHost = BuildWebHost<TStartup>(
                    args: args,
                    configDelegate: configDelegate,
                    configureKestrelDelegate: configureKestrelDelegate);

                if (WindowsServiceHelpers.IsWindowsService())
                {
                    logger.Debug("Run as windows service");
                    //При запуске системной службы она по умолчанию наследует все атрибуты Service Control Manager,
                    //включая рабочую директорию, которую изменить внешними настройками нельзя
                    Directory.SetCurrentDirectory(AppContext.BaseDirectory);
                    webHost.RunAsService();
                }
                else
                {
                    logger.Debug("Run as web application");
                    webHost.Run();
            }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Stopped program because of exception");
                throw;
            }
            finally
            {
                NLog.LogManager.Shutdown();
            }
        }

        public static async Task RunWebhostAsync<TStartup>(
            string[] args,
            string nlogConfigFileName = "nlog.config",
            Action<WebHostBuilderContext, IConfigurationBuilder> configDelegate = null,
            Action<KestrelServerOptions> configureKestrelDelegate = null)
            where TStartup : class
        {
            var logger = NLog.Web.NLogBuilder.ConfigureNLog(nlogConfigFileName).GetCurrentClassLogger();
            try
            {
                logger.Debug("Init main");
                var webHost = BuildWebHost<TStartup>(
                    args: args,
                    configDelegate: configDelegate,
                    configureKestrelDelegate: configureKestrelDelegate);

                if (WindowsServiceHelpers.IsWindowsService())
                {
                    logger.Debug("Run as windows service");
                    //При запуске системной службы она по умолчанию наследует все атрибуты Service Control Manager,
                    //включая рабочую директорию, которую изменить внешними настройками нельзя
                    Directory.SetCurrentDirectory(AppContext.BaseDirectory);
                    webHost.RunAsService();
                }
                else
                {
                    logger.Debug("Run as web application");
                await webHost.RunAsync();
            }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Stopped program because of exception");
                throw;
            }
            finally
            {
                NLog.LogManager.Shutdown();
            }
        }

        public static IWebHost BuildWebHost<TStartup>(
            string[] args,
            Action<WebHostBuilderContext, IConfigurationBuilder> configDelegate = null,
            Action<KestrelServerOptions> configureKestrelDelegate = null) where TStartup : class
        {

            var webHost = WebHost.CreateDefaultBuilder(args)
                .UseWindowsService()
                .UseIndusoftLogging()
                .ConfigureKestrel(options =>
                {
                    configureKestrelDelegate?.Invoke(options);
                })
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    configDelegate?.Invoke(hostingContext, config);
                })
                .UseStartup<TStartup>()
                .Build();
            return webHost;
        }
    }
}
