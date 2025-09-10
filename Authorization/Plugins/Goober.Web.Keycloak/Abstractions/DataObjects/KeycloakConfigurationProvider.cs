using Goober.Web.Keycloak.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System.Globalization;
using System.Text;

namespace Goober.Web.Keycloak.Abstractions.DataObjects
{
    internal class KeycloakConfigurationProvider : JsonConfigurationProvider
    {
        private const char KeycloakPropertyDelimiter = '-';
        private const char NestedConfigurationDelimiter = ':';
        private const int Utf8LowerCaseDistant = 32;

        public KeycloakConfigurationProvider(KeycloakConfigurationSource source) : base(source)
        {

        }

        public override void Load(Stream stream)
        {
            base.Load(stream);
            Data = Data.ToDictionary(
                x => NormalizeKey(x.Key),
                x => x.Value,
                StringComparer.OrdinalIgnoreCase);
        }

        private string NormalizeKey(string key)
        {
            var sections = key
                .ToUpper(CultureInfo.InvariantCulture)
                .Split(NestedConfigurationDelimiter);
            StringBuilder stringBuilder = new StringBuilder();

            foreach (var section in sections)
            {
                if (stringBuilder.Length != 0)
                {
                    stringBuilder.Append(NestedConfigurationDelimiter);
                }

                foreach (var x in section.Split(KeycloakPropertyDelimiter))
                {
                    for (var i = 0; i < x.Length; i++)
                    {
                        var @char = x[i];
                        if (i == 0)
                        {
                            stringBuilder.Append(@char);
                        }
                        else
                        {
                            stringBuilder.Append((char)(@char + Utf8LowerCaseDistant));
                        }
                    }
                }
            }

            var result = ConfigurationConstants.ConfigurationPrefix + NestedConfigurationDelimiter +
                         stringBuilder.ToString();

            return result;
        }
    }

}