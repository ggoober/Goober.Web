using System.Collections.Generic;

namespace Goober.Web.Glossary
{
    public class ConfigGlossary
    {
        public static string DevelopmentConfigApiSchemeAndHost { get; set; } = "http://config-api-dev.domain.local";

        public static string StagingConfigApiSchemeAndHost { get; set; } = "http://config-api-dev.domain.local";

        public static string ProductionConfigApiSchemeAndHost { get; set; } = "http://config-api.domain.local";

        public static Dictionary<string, string> ConfigApiEnvironmentAndHostMappings { get; set; } = 
            new Dictionary<string, string> {
                { "Production", ProductionConfigApiSchemeAndHost },
                { "Development", DevelopmentConfigApiSchemeAndHost },
                { "Staging", StagingConfigApiSchemeAndHost }
            };

        public static int DefaultCacheExpirationTimeInMinutes { get; set; } = 15;

        public static int DefaultCacheRefreshTimeInMinutes { get; set; } = 5;
    }
}
