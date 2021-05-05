using System.Collections.Generic;

namespace Goober.Web.Glossary
{
    public class ConfigGlossary
    {
        public static string DevelopmentConfigApiSchemeAndHost = "http://config-api-dev.domain.local";

        public static string StagingConfigApiSchemeAndHost = "http://config-api-dev.domain.local";

        public static string ProductionConfigApiSchemeAndHost = "http://config-api.domain.local";

        public static Dictionary<string, string> ConfigApiEnvironmentAndHostMappings = 
            new Dictionary<string, string> {
                { "Production", ProductionConfigApiSchemeAndHost },
                { "Development", DevelopmentConfigApiSchemeAndHost },
                { "Staging", StagingConfigApiSchemeAndHost }
            };
    }
}
