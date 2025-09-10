using System.Threading.Tasks;
using Indusoft.Web;
using Microsoft.Extensions.Configuration;

namespace Indusoft.WebJobs.Example
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await ProgramUtils.RunWebhostAsync<Startup>(
	            args: args, 
	            configDelegate: (context, config) => config.AddEnvironmentVariables(), 
	            nlogConfigFileName: "config/webjobs-example-node-nlog.config");
        }
    }
}
