using System;
using System.Collections.Generic;
using System.Text;
using Indusoft.DependencyInjection.Extensions;
using Indusoft.WebJobs.Api.Services;
using Indusoft.WebJobs.Api.Services.Implementation;
using Microsoft.Extensions.DependencyInjection;

namespace Indusoft.WebJobs.Api.Extensions
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
