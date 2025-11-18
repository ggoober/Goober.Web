using System;
using System.Collections.Generic;
using System.Text;
using Goober.DependencyInjection.Extensions;
using Goober.WebJobs.Api.Services;
using Goober.WebJobs.Api.Services.Implementation;
using Microsoft.Extensions.DependencyInjection;

namespace Goober.WebJobs.Api.Extensions
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddWebJobsHttpApi(this IServiceCollection services)
		{
			services.RegisterAssemblyClasses<IWebJobsHttpService>();
			return services;
		}
	}
}
