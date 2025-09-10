using Goober.Web.IdentityUtils.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Goober.Web.IdentityUtils.Converters
{
    internal class LicenseModelJsonConverter : JsonConverter<LicenseModel>
    {
        public override LicenseModel ReadJson(JsonReader reader, Type objectType, LicenseModel existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            JObject jobject = JObject.Load(reader);
            if (jobject is null)
                return null;

            var model = new LicenseModel();
            model.Version = jobject.GetValueOrDefault("Version", 5, StringComparison.OrdinalIgnoreCase);
            model.Sign = jobject.GetValueOrDefault("Sign", string.Empty, StringComparison.OrdinalIgnoreCase);
            model.ProductName = jobject.GetValueOrDefault("ProductName", string.Empty, StringComparison.OrdinalIgnoreCase);
            model.Customer = jobject.GetValueOrDefault("Customer", string.Empty, StringComparison.OrdinalIgnoreCase);
            model.FileName = jobject.GetValueOrDefault("FileName", string.Empty, StringComparison.OrdinalIgnoreCase);
            model.LicDate = jobject.GetValueOrDefault("LicDate", DateTime.MinValue, StringComparison.OrdinalIgnoreCase);
            model.LicWDate = jobject.GetValueOrDefault("LicWDate", DateTime.MinValue, StringComparison.OrdinalIgnoreCase);
            model.IsPerpetual = jobject.GetValueOrDefault("IsPerpetual", false, StringComparison.OrdinalIgnoreCase);
            model.LicenseAuthor = jobject.GetValueOrDefault("LicenseAuthor", string.Empty, StringComparison.OrdinalIgnoreCase);
            model.Comment = jobject.GetValueOrDefault("Comment", string.Empty, StringComparison.OrdinalIgnoreCase);

            var guidString = jobject.GetValue("LicGuid", StringComparison.OrdinalIgnoreCase).Value<string>();
            model.LicGuid = Guid.TryParse(guidString, out var guid)
                ? guid
                : Guid.Empty;

            return model;
        }

        public override void WriteJson(JsonWriter writer, LicenseModel value, JsonSerializer serializer)
        {
            if (value is null)
                return;

            JObject jobject = new JObject();
            jobject[nameof(value.Version)] = value.Version;
            jobject[nameof(value.Customer)] = value.Customer;
            jobject[nameof(value.ProductName)] = value.ProductName;
            jobject[nameof(value.FileName)] = value.FileName;
            jobject[nameof(value.LicDate)] = value.LicDate;
            jobject[nameof(value.LicWDate)] = value.LicWDate;
            jobject[nameof(value.LicGuid)] = value.LicGuid;
            jobject[nameof(value.Comment)] = value.Comment;
            jobject[nameof(value.Sign)] = value.Sign;
            jobject[nameof(value.IsPerpetual)] = value.IsPerpetual;
            jobject[nameof(value.LicenseAuthor)] = value.LicenseAuthor;

            jobject.WriteTo(writer);
        }
    }
}
