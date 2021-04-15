using System.Collections.Generic;

namespace Goober.WebApi.Models
{
    public class BaseStartupConfigSettings
    {
        public Dictionary<string, string> ConfigApiEnvironmentAndHostMappings { get; set; } = new Dictionary<string, string>();

        public string AppSettingsFileName { get; set; } = "appsettings.json";

        public bool IsAppSettingsFileOptional { get; set; } = false;
    }
}
