using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Goober.WebJobs.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddWebJobs<TAssemblyClassName>(this IServiceCollection services)
        {
            var assembly = typeof(TAssemblyClassName).Assembly;

            var jobsTypes = new List<Type>();

            foreach (var assemblyDefinedType in assembly.GetTypes())
            {
                if (assemblyDefinedType.IsClass == false)
                {
                    continue;
                }

                if (ContainsTypeInBaseTypes(type: assemblyDefinedType,
                    searchingType: typeof(BaseJob)) == false)
                {
                    continue;
                }

                jobsTypes.Add(assemblyDefinedType);
            }

            foreach (var iJobType in jobsTypes)
            {
                if (services.Any(x => x.ImplementationType == iJobType
                        && x.ServiceType == typeof(IHostedService)) == true)
                {
                    continue;
                }

                services.Add(new ServiceDescriptor(typeof(IHostedService), iJobType, ServiceLifetime.Singleton));
            }
        }

        private static bool ContainsTypeInBaseTypes(Type type, Type searchingType)
        {
            var baseType = type;

            var maxIterations = 10;
            var currentIteration = 0;

            while (baseType != null)
            {
                if (baseType == searchingType)
                    return true;

                currentIteration++;
                if (currentIteration > maxIterations)
                {
                    throw new InvalidOperationException($"currentIteration > {maxIterations}");
                }

                baseType = baseType.BaseType;
            }

            return false;
        }
    }
}
