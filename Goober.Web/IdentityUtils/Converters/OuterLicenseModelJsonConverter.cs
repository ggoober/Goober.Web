using Goober.Web.IdentityUtils.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Goober.Web.IdentityUtils.Converters
{
    internal class OuterLicenseModelJsonConverter : JsonConverter<LicenseModel>
    {
        public override LicenseModel ReadJson(
            JsonReader reader,
            Type objectType,
            LicenseModel existingValue,
            bool hasExistingValue,
            JsonSerializer serializer
        )
        {
            JObject jobject = JObject.Load(reader);
            if (jobject is null)
                return null;

            JObject licJson = jobject.GetValueOrDefault<JObject>(
                key: "lic",
                defaultValue: null,
                stringComparison: StringComparison.OrdinalIgnoreCase);
            var licenseInfoModel = ReadLicenseInfoJson(licJson);

            string productName = jobject.GetValueOrDefault(
                key: "productName",
                defaultValue: string.Empty,
                stringComparison: StringComparison.OrdinalIgnoreCase);

            var model = new LicenseModel();
            model.ProductName = productName;

            if (licenseInfoModel is not null)
            {
                model.Version = licenseInfoModel.Version;
                model.Customer = licenseInfoModel.Customer;
                model.FileName = licenseInfoModel.FileName;
                model.LicDate = licenseInfoModel.IssuedAt;
                model.LicWDate = licenseInfoModel.ExpireDate;
                model.IsPerpetual = licenseInfoModel.IsPerpetual;
                model.LicenseAuthor = licenseInfoModel.LicenseAuthor;
                model.Comment = licenseInfoModel.Comment;
                model.LicGuid = licenseInfoModel.Id;
            }

            return model;
        }

        public override void WriteJson(JsonWriter writer, LicenseModel value, JsonSerializer serializer)
        {
            if (value is null)
                return;

            JObject lic = new JObject();
            lic["id"] = value.LicGuid;
            lic["version"] = value.Version;
            lic["customer"] = value.Customer;
            lic["issuedAt"] = value.LicDate;
            lic["exp"] = value.LicWDate;
            lic["isPerpetual"] = value.IsPerpetual;
            lic["licenseAuthor"] = value.LicenseAuthor;
            lic["fileName"] = value.FileName;
            lic["comment"] = value.Comment;

            JObject product = new JObject();
            product["productName"] = value.ProductName;
            product["lic"] = lic;

            product.WriteTo(writer);
        }

        private static LicenseInfoInternalModel ReadLicenseInfoJson(JObject jobject)
        {
            if (jobject is null)
                return null;

            var version = jobject.GetValueOrDefault(
                key: "version",
                defaultValue: 5,
                stringComparison: StringComparison.OrdinalIgnoreCase);

            var customer = jobject.GetValueOrDefault(
                key: "customer",
                defaultValue: string.Empty,
                stringComparison: StringComparison.OrdinalIgnoreCase);

            var issuedAt = jobject.GetValueOrDefault(
                key: "issuedAt",
                defaultValue: DateTime.MinValue,
                stringComparison: StringComparison.OrdinalIgnoreCase);

            var expireDate = jobject.GetValueOrDefault(
                key: "exp",
                defaultValue: DateTime.MinValue,
                stringComparison: StringComparison.OrdinalIgnoreCase);

            var isPerpetual = jobject.GetValueOrDefault(
                key: "isPerpetual",
                defaultValue: false,
                stringComparison: StringComparison.OrdinalIgnoreCase);

            var licenseAuthor = jobject.GetValueOrDefault(
                key: "licenseAuthor",
                defaultValue: string.Empty,
                stringComparison: StringComparison.OrdinalIgnoreCase);

            var fileName = jobject.GetValueOrDefault(
                key: "fileName",
                defaultValue: string.Empty,
                stringComparison: StringComparison.OrdinalIgnoreCase);

            var comment = jobject.GetValueOrDefault(
                key: "comment",
                defaultValue: string.Empty,
                stringComparison: StringComparison.OrdinalIgnoreCase);

            var guidString = jobject.GetValue("id", StringComparison.OrdinalIgnoreCase).Value<string>();
            var id = Guid.TryParse(guidString, out var guid)
                ? guid
                : Guid.Empty;

            var licenseInfoModel = new LicenseInfoInternalModel(
                id: id,
                version: version,
                customer: customer,
                issuedAt: issuedAt,
                expireDate: expireDate,
                licenseAuthor: licenseAuthor,
                fileName: fileName,
                comment: comment,
                isPerpetual: isPerpetual
            );

            return licenseInfoModel;
        }
    }
}
