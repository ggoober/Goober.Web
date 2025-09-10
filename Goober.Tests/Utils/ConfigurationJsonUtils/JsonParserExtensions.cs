using Goober.Tests.Utils.StreamUtils;
using System.Collections.Generic;

namespace Goober.Tests.Utils.ConfigurationJsonUtils
{
    public static class JsonParserExtensions
    {
        public static Dictionary<string, string> ParseFromJsonString(this string jsonString)
        {
            var stream = StreamHelper.StringToStream(jsonString);
            var result = JsonConfigurationToDictionaryParser.Parse(stream);
            return result;
        }
    }
}
