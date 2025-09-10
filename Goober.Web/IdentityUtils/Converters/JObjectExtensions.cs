using Newtonsoft.Json.Linq;
using System;

namespace Goober.Web.IdentityUtils.Converters
{
    public static class JObjectExtensions
    {
        public static TValue GetValueOrDefault<TValue>(
            this JObject jobject,
            string key,
            TValue defaultValue,
            StringComparison stringComparison = StringComparison.Ordinal
        )
        {
            if (jobject == null)
                return defaultValue;

            var existValue = jobject.TryGetValue(key, stringComparison, out var value);
            if (existValue)
            {
                TValue result = value.Value<TValue>() ?? defaultValue;
                return result;
            }

            return defaultValue;
        }
    }
}
