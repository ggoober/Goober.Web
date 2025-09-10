using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goober.CLI.Extensions
{
    public static class ConfigurationExtensions
    {
        public static IHostBuilder UseIndusoftCliLogging(this IHostBuilder webHostBuilder)
        {
            return webHostBuilder
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(LogLevel.Trace);
                }
                )
                .UseNLog();
        }
    }
}
