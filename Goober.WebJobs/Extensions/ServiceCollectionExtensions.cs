using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;
using Goober.WebJobs.Api.Extensions;
using Goober.WebJobs.Api.Services;
using Goober.WebJobs.Api.Services.Implementation;
using Goober.WebJobs.Services;
using Goober.WebJobs.Services.Implementation;

namespace Goober.WebJobs.Extensions
{
	public static class ServiceCollectionExtensions
	{
		public static void AddWebJobs<TAssemblyClassName>(this IServiceCollection services)
		{
			var jobTypes = typeof(TAssemblyClassName).Assembly.GetTypes()
				.Where(type => type.IsClass && type.IsSubclassOf(typeof(BaseJob)));

			foreach (var type in jobTypes)
			{
				if (services.Any(x => x.ImplementationType == type
				                      && x.ServiceType == typeof(IHostedService)))
				{
					continue;
				}
				services.Add(new ServiceDescriptor(typeof(IHostedService), type, ServiceLifetime.Singleton));
			}
        }

		public static IServiceCollection ConfigureWebJobs(this IServiceCollection services)
		{
			services.AddWebJobsHttpApi();
			services.AddScoped<ICookieHttpService, CookieHttpService>();
			services.AddSingleton<IClusterInfoVisor, ClusterInfoVisor>();
			return services;
		}
	}
}
