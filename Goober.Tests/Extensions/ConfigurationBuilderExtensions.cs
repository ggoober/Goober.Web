using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System;
using System.Linq;

namespace Goober.Tests.Extensions
{
    public static class ConfigurationBuilderExtensions
    {
        public static IConfigurationBuilder RemoveSources<TSource>(
            this IConfigurationBuilder configurationBuilder,
            bool includeInheritedTypes = false)
            where TSource : class, IConfigurationSource
        {
            Func<IConfigurationSource, bool> sourceTypeFilter = source =>
                includeInheritedTypes ? typeof(TSource).IsAssignableFrom(source.GetType()) :
                                         typeof(TSource) == source.GetType();

            var fileConfigBuilders = configurationBuilder.Sources
                            .Where(sourceTypeFilter)
                            .ToList();

            foreach (var fileConfigBuilder in fileConfigBuilders)
                configurationBuilder.Sources.Remove(fileConfigBuilder);

            return configurationBuilder;
        }

        public static IConfigurationBuilder RemoveFileSources(
            this IConfigurationBuilder configurationBuilder)
        {
            return configurationBuilder.RemoveSources<FileConfigurationSource>(includeInheritedTypes: true);
        }

        public static IConfigurationBuilder RemoveJsonFileSources(
            this IConfigurationBuilder configurationBuilder)
        {
            return configurationBuilder.RemoveSources<JsonConfigurationSource>(includeInheritedTypes: true);
        }
    }
}
