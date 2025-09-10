using Goober.CLI.Abstractions.Glossary;
using System.Collections.Generic;

namespace Goober.CLI.Abstractions.Models
{
    public class BaseCLIStartupConfigSettings
    {
        public Dictionary<string, string> ConfigApiEnvironmentAndHostMappings { get; set; } = new Dictionary<string, string>();

        public string AppSettingsFileName { get; set; } = ConfigGlossary.AppSettingsFileName;

        public string OverrideApplicationName { get; set; }

        public bool IsAppSettingsFileOptional { get; set; } = ConfigGlossary.IsAppSettingsFileOptional;

        public int? CacheRefreshTimeInMinutes { get; set; }

        public int? CacheExpirationTimeInMinutes { get; set; }
    }
}
