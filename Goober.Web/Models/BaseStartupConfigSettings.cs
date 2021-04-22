using System.Collections.Generic;

namespace Goober.Web.Models
{
    public class BaseStartupConfigSettings
    {
        public Dictionary<string, string> ConfigApiEnvironmentAndHostMappings { get; set; } = new Dictionary<string, string>();

        public string AppSettingsFileName { get; set; } = "appsettings.json";

        public string OverrideApplicationName { get; set; }

        public bool IsAppSettingsFileOptional { get; set; } = false;

        public int? CacheRefreshTimeInMinutes { get; set; }
        
        public int? CacheExpirationTimeInMinutes { get; set; }
    }
}
