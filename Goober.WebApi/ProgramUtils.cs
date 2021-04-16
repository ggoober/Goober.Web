using Goober.WebApi.Extensions;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Reflection;

namespace Goober.WebApi
{
    public static class ProgramUtils
    {
        public static string ApplicationName
        {
            get; private set;
        }

        public static string AssemblyVersion
        {
            get; private set;
        }

        public static void RunWebhost<TStartup>(string[] args, string nlogConfigFileName = "nlog.config")
            where TStartup : class
        {
            ApplicationName = Assembly.GetEntryAssembly().GetName().Name;

            AssemblyVersion = Assembly.GetEntryAssembly().GetName().Version.ToString();

            var logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            try
            {
                logger.Debug("init main");
                var webHost = WebHost.CreateDefaultBuilder(args)
                             .UseGooberLogging()
                             .UseStartup<TStartup>()
                             .Build();

                webHost.Run();
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
    }
}
